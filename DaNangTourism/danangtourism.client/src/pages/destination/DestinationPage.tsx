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
import noItemImg from '../../assets/no-item.png'

const locations = [
	'All',
	'Hai Chau',
	'Cam Le',
	'Thanh Khe',
	'Lien Chieu',
	'Ngu Hanh Son',
	'Son Tra',
	'Hoa Vang',
	'Hoang Sa',
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
	const [numbOfPages, setNumbOfPages] = useState(1)
	const [destinations, setDestinations] = useState<DesListItemProps[]>()
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [filter, setFilter] = useState(initFilter)
	const [currentPage, setCurrentPage] = useState(1)
	const [isFilterOpen, setIsFilterOpen] = useState(false)
	const [loading, setLoading] = useState(false)

	const navigate = useNavigate()
	const toast = useToast()
	document.title = 'Destinations | Danang Tourism'

	useEffect(() => {
		getDestinations()
	}, [currentPage, sort])

	const getDestinations = async () => {
		setLoading(true)
		try {
			let params = {}
			params = { ...params, page: currentPage, limit: cardPerPage }
			if (searchValue) {
				params = { ...params, search: searchValue }
			}
			if (sort.by) {
				params = { ...params, sortBy: sortBy[sort.by].value }
			}
			if (sort.type) {
				params = { ...params, sortType: sort.type }
			}
			if (filter.location !== 0) {
				params = {
					...params,
					location: encodeURIComponent(locations[filter.location]),
				}
			}
			if (filter.price.min !== -1) {
				params = { ...params, costFrom: filter.price.min }
			}
			if (filter.price.max !== -1) {
				params = { ...params, costTo: filter.price.max }
			}
			if (filter.rating.min !== -1) {
				params = { ...params, ratingFrom: filter.rating.min }
			}
			if (filter.rating.max !== -1) {
				params = { ...params, ratingTo: filter.rating.max }
			}
			const response = await axios.get('/api/destination/list', {
				params: params,
			})
			const data = response.data.data
			setDestinations(data.items)
			setCurrentPage(data.page)
			setNumbOfPages(Math.ceil(data.total / cardPerPage))
		} catch (error: any) {
			if (error.response.status === 404) {
				setDestinations(undefined)
			} else {
				console.log(error)
				toast.error('Failed to fetch destinations', error.message)
			}
		}
		setLoading(false)
	}

	const getAllDestinations = async () => {
		setLoading(true)
		try {
			const response = await axios.get('/api/destination/list', {
				params: { page: currentPage, limit: cardPerPage },
			})
			const data = response.data.data
			setDestinations(data.items)
			setCurrentPage(data.page)
			setNumbOfPages(Math.ceil(data.total / cardPerPage))
		} catch (error: any) {
			toast.error('Failed to fetch destinations', error.message)
			setDestinations(undefined)
		}
		setLoading(false)
	}

	return (
		<div className="mx-auto flex min-h-screen justify-center pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="h-full w-full">
				<div className="mb-5 flex w-full items-center justify-between">
					<SearchBox
						className="h-9 w-[300px]"
						onChangeValue={(event) => setSearchValue(event.target.value)}
						onClickSearch={getDestinations}
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
											setIsFilterOpen(false)
											getDestinations()
										}}
										onReset={() => {
											setFilter(initFilter)
											setIsFilterOpen(false)
											getAllDestinations()
										}}
									/>
								</motion.div>
							)}
						</AnimatePresence>
					</div>
				</div>
				<div className="flex flex-wrap justify-around gap-y-6">
					{destinations ? (
						destinations.map((des, index) => (
							<DesPreviewCard
								key={index}
								onVisit={() => navigate(`/destination/${des.id}`)}
								{...des}
							/>
						))
					) : loading ? (
						Array.from({ length: cardPerPage }).map((_, index) => (
							<DPCLoading key={index} />
						))
					) : (
						<div className="flex h-[480px] w-full flex-col items-center justify-center gap-5">
							<img className="h-[320px]" src={noItemImg} alt="No item found" />
							<p className="text-3xl font-semibold tracking-wide text-txtCol-3">
								No destinations found
							</p>
						</div>
					)}
					{destinations && (
						<Pagination
							className="mt-2 w-full justify-center"
							numbOfPages={numbOfPages}
							currentPage={currentPage}
							setCurrentPage={(numb) => {
								setCurrentPage(numb)
								console.log(numb)
							}}
						/>
					)}
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
