import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { Button, DropdownSelect, Pagination, SearchBox } from '../../components'
import { PiSlidersFill } from 'react-icons/pi'
import DesPreviewCard, { DPCLoading } from './DesPreviewCard'
import axios from 'axios'
import { useToast } from '../../hook/useToast'
import { DesListItemProps } from '../../types/destination'
import { SortTypeButton, ToggleButton } from '../../components/Buttons'
import { twMerge } from 'tailwind-merge'
import { AnimatePresence, motion } from 'framer-motion'

const locations = [
	'All',
	'Hải Châu',
	'Cẩm Lệ',
	'Thanh Khê',
	'Liên Chiểu',
	'Ngũ Hành Sơn',
	'Sơn Trà',
	'Hòa Vang',
	'Hoàng Sa',
]

const sortBy = [
	{
		value: 'created_at',
		label: 'Release date',
	},
	{
		value: 'name',
		label: 'Name',
	},
	{
		value: 'rating',
		label: 'Avg. Rating',
	},
	{
		value: 'cost',
		label: 'Avg. Cost',
	},
]

const initFilter = {
	location: 0,
	price: {
		min: -1,
		max: -1,
	},
	rating: {
		min: -1,
		max: -1,
	},
}
const DestinationPage: React.FC = () => {
	const cardPerPage = 12
	const [destinations, setDestinations] = useState<DesListItemProps[]>()
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [filter, setFilter] = useState(initFilter)
	const [currentPage, setCurrentPage] = useState(1)
	const [isFilterOpen, setIsFilterOpen] = useState(false)

	const navigate = useNavigate()
	const toast = useToast()
	document.title = 'Destinations | Da Nang Explore'

	useEffect(() => {
		getDestinations(currentPage)
	}, [currentPage])

	const getDestinations = async (page: number) => {
		try {
			setDestinations(undefined)
			const response = await axios.get(`/api/destination/list-${page}.json`)
			// simulate delay
			await new Promise((resolve) => setTimeout(resolve, 2000))
			setDestinations(response.data.data.items)
		} catch (error: any) {
			toast.error('Failed to fetch destinations', error.message)
		}
	}

	const handleSearch = async () => {
		try {
			// setDestinations(undefined)
			// const response = await axios.get(`/api/destination/search.json?query=${searchValue}`)
			// await new Promise((resolve) => setTimeout(resolve, 2000))
			// setDestinations(response.data.data.items)

			console.log('searchValue', searchValue, 'sort', sort)
		} catch (error: any) {
			toast.error('Failed to fetch destinations', error.message)
		}
	}

	return (
		<div className="mx-auto flex min-h-screen justify-center pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="h-full w-full">
				<div className="mb-5 flex w-full items-center justify-between">
					<SearchBox
						className="h-9 w-[300px]"
						onChangeValue={(event) => setSearchValue(event.target.value)}
						onClickSearch={handleSearch}
					/>
					<div className="item-center relative flex gap-4">
						<ToggleButton
							id="open-filter"
							onClick={() => setIsFilterOpen(!isFilterOpen)}
							className="h-9 w-[92px] border-2"
							text="Filter"
							toggledText="Close"
							initToggled={isFilterOpen}
							btnColor="#76C893"
							icon={<PiSlidersFill className="text-xl" />}
						></ToggleButton>
						<DropdownSelect
							id="sort-by"
							className="h-9 w-[140px]"
							options={sortBy.map((item) => item.label)}
							value={sort.by}
							onChange={(event) => {
								setSort({
									...sort,
									by: Number(event.target.value),
								})
							}}
						/>
						<SortTypeButton
							id="sort-type"
							className="h-9 w-9"
							value={sort.type}
							onClick={() => {
								setSort({
									...sort,
									type: sort.type === 'asc' ? 'desc' : 'asc',
								})
							}}
						/>
						<AnimatePresence>
							{isFilterOpen && (
								<motion.div
									className="absolute -left-4 top-0 z-[5] w-[300px]"
									initial={{ opacity: 0, x: '-100%' }}
									animate={{ opacity: 1, x: '-105%' }}
									exit={{ opacity: 0, x: '-100%' }}
								>
									<DestinationFilter
										filter={filter}
										setFilter={setFilter}
										className={isFilterOpen ? 'block' : 'hidden'}
										onSubmit={() => {
											console.log('filter', filter)
											setIsFilterOpen(false)
											toast.success(
												'Filter applied',
												'Filter applied successfully',
											)
										}}
										onReset={() => {
											setFilter(initFilter)
											setIsFilterOpen(false)
											toast.success('Filter reset', 'Filter reset successfully')
										}}
									/>
								</motion.div>
							)}
						</AnimatePresence>
					</div>
				</div>
				<div className="flex flex-wrap justify-between gap-y-6">
					{destinations
						? destinations.map((des, index) => (
								<DesPreviewCard
									key={index}
									onVisit={() => navigate(`/destination/${des.id}`)}
									{...des}
								/>
							))
						: Array.from({ length: cardPerPage }).map((_, index) => (
								<DPCLoading key={index} />
							))}
					<Pagination
						className="mt-2 w-full justify-center"
						numbOfPages={8}
						currentPage={currentPage}
						setCurrentPage={(numb) => {
							setCurrentPage(numb)
							console.log(numb)
						}}
					/>
				</div>
			</div>
		</div>
	)
}

const DestinationFilter: React.FC<{
	className?: string
	filter: typeof initFilter
	setFilter: (filter: typeof initFilter) => void
	onSubmit: () => void
	onReset: () => void
}> = ({ className, filter, setFilter, onSubmit, onReset }) => {
	return (
		<div
			className={twMerge(
				'shadow-modal rounded-lg border border-borderCol-1 bg-bgCol-3 p-5',
				className,
			)}
		>
			<div className="mb-4 flex w-full flex-col gap-2">
				<label className="text-sm font-semibold" htmlFor="location-filter">
					Locations
				</label>
				<DropdownSelect
					id="location-filter"
					className="w-full"
					options={locations}
					value={filter.location}
					onChange={(event) => {
						setFilter({
							...filter,
							location: Number(event.target.value),
						})
					}}
				/>
			</div>
			<div className="mb-2 flex flex-col gap-2">
				<label className="text-sm font-semibold">Price</label>
				<div className="mb-2 flex w-full items-center justify-start gap-2 text-sm text-txtCol-2">
					<p>From $</p>
					<input
						className="w-[80px]"
						type="number"
						placeholder="0"
						min={0}
						max={filter.price.max === -1 ? 1000000 : filter.price.max}
						value={filter.price.min === -1 ? '' : filter.price.min}
						onChange={(event) => {
							setFilter({
								...filter,
								price: {
									...filter.price,
									min: Number(event.target.value),
								},
							})
						}}
					/>
					<p>to $</p>
					<input
						className="w-[80px]"
						type="number"
						placeholder="100"
						min={filter.price.min === -1 ? 0 : filter.price.min}
						value={filter.price.max === -1 ? '' : filter.price.max}
						onChange={(event) => {
							setFilter({
								...filter,
								price: {
									...filter.price,
									max: Number(event.target.value),
								},
							})
						}}
					/>
				</div>
			</div>
			<div className="mb-3 flex flex-col gap-2">
				<label className="text-sm font-semibold"> Rating </label>
				<div className="mb-2 flex w-full items-center justify-start gap-2 text-sm text-txtCol-2">
					<p>From</p>
					<input
						className="w-[52px] border-borderCol-1"
						type="number"
						placeholder="0"
						min={0}
						max={filter.rating.max === -1 ? 5 : filter.rating.max}
						value={filter.rating.min === -1 ? '' : filter.rating.min}
						onChange={(event) => {
							setFilter({
								...filter,
								rating: {
									...filter.rating,
									min: Number(event.target.value),
								},
							})
						}}
					/>
					<p>stars to</p>
					<input
						className="w-[52px] border-borderCol-1"
						type="number"
						placeholder="5"
						min={filter.rating.min === -1 ? 0 : filter.rating.min}
						max={5}
						value={filter.rating.max === -1 ? '' : filter.rating.max}
						onChange={(event) => {
							setFilter({
								...filter,
								rating: {
									...filter.rating,
									max: Number(event.target.value),
								},
							})
						}}
					/>
					<p>stars</p>
				</div>
			</div>
			<div className="relative flex items-center justify-between gap-5">
				<Button
					className="text-bold h-8 w-20 border-2 border-tertiary-1 text-tertiary-1"
					onClick={() => {
						onReset()
					}}
				>
					Reset
				</Button>
				<Button
					className="flex-1 bg-primary-2 text-white"
					onClick={() => {
						onSubmit()
					}}
				>
					Apply
				</Button>
			</div>
		</div>
	)
}

export default DestinationPage
