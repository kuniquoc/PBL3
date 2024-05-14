import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { Button, DropdownSelect, Pagination, SearchBox } from '../../components'
import DesPreviewCard, { DPCLoading } from './DesPreviewCard'
import axios from 'axios'
import { useToast } from '../../hook/useToast'

const filterOptions = [
	{
		name: 'Location',
		options: [
			'All',
			'Hải Châu',
			'Cẩm Lệ',
			'Thanh Khê',
			'Liên Chiểu',
			'Ngũ Hành Sơn',
			'Sơn Trà',
			'Hòa Vang',
			'Hoàng Sa',
		],
	},
	{
		name: 'Type',
		options: ['All', 'Nature', 'Culture', 'Entertainment', 'Accommodation'],
	},
	{
		name: 'Others',
		options: ['Hot destination', 'New destination', 'Recommended'],
	},
]

type DestinationType = {
	id: number
	name: string
	address: string
	openingTime: string
	closingTime: string
	cost: number
	rating: number
	tags: string[]
	thumbnail: string
	pin: string
	favorite: boolean
}

const initFilter = {
	location: 0,
	type: 0,
	price: {
		min: -1,
		max: -1,
	},
	rating: {
		min: -1,
		max: -1,
	},
	others: -1,
}

const sortOptions = ['Sort by', 'Price', 'Rating']

const DestinationPage: React.FC = () => {
	const cardPerPage = 12
	const [destinations, setDestinations] = useState<DestinationType[]>()
	const [searchValue, setSearchValue] = useState('')
	const [filter, setFilter] = useState(initFilter)
	const [sort, setSort] = useState(0)
	const [currentPage, setCurrentPage] = useState(1)
	const navigate = useNavigate()
	const toast = useToast()
	document.title = 'Destinations | Da Nang Explore'

	useEffect(() => {
		getDestinations(currentPage)
	}, [currentPage])

	const getDestinations = async (page: number) => {
		try {
			setDestinations(undefined)
			const response = await axios.get(`/api/destination/page-${page}.json`)
			// simulate delay
			await new Promise((resolve) => setTimeout(resolve, 2000))
			setDestinations(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}
	return (
		<div className="mx-auto flex min-h-screen justify-center pb-6 pt-[64px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="flex h-full w-full justify-start gap-5 ">
				{/* <div className="w-[260px] "></div> */}
				<div className="mt-8 h-auto w-[260px]">
					{filterOptions[0] && (
						<div className="mb-4 flex w-full flex-col gap-2">
							<label className="text-sm font-semibold" htmlFor={`filter_0`}>
								{filterOptions[0].name}
							</label>
							<DropdownSelect
								id={`filter_0`}
								className="w-full"
								options={filterOptions[0].options}
								value={filter.location}
								onChange={(event) => {
									setFilter({
										...filter,
										location: Number(event.target.value),
									})
								}}
							/>
						</div>
					)}

					{filterOptions[1] && (
						<div className="mb-4 flex w-full flex-col gap-2">
							<label className="text-sm font-semibold" htmlFor={`filter_1`}>
								{filterOptions[1].name}
							</label>
							<DropdownSelect
								id={`filter_1`}
								className="w-full"
								options={filterOptions[1].options}
								value={filter.type}
								onChange={(event) => {
									setFilter({
										...filter,
										type: Number(event.target.value),
									})
								}}
							/>
						</div>
					)}

					<div className="mb-4 flex flex-col gap-2">
						<label className="text-sm font-semibold">Price</label>
						<div className="mb-3 flex w-full items-center justify-start gap-2 text-sm text-txtCol-2">
							<p>From $</p>
							<input
								className="w-[60px]"
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
					<div className="mb-4 flex flex-col gap-2">
						<label className="text-sm font-semibold"> Rating </label>
						<div className="mb-3 flex w-full items-center justify-start gap-2 text-sm text-txtCol-2">
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
								className="w-[60px] border-borderCol-1"
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
					<div className="mb-8 flex flex-col gap-2">
						<label className="text-sm font-semibold">Others</label>
						{filterOptions[2]?.options?.map((item, index) => (
							<div
								key={index}
								className="flex w-full items-center justify-start gap-2 text-sm text-txtCol-2"
							>
								<input
									type="checkbox"
									id={`others_${index}`}
									name="others_filter"
									checked={filter.others === index}
									onChange={() => {
										if (filter.others === index) {
											setFilter({
												...filter,
												others: -1,
											})
										} else
											setFilter({
												...filter,
												others: index,
											})
									}}
								/>
								<label htmlFor={`others_${index}`}>{item}</label>
							</div>
						))}
					</div>
					<div className="relative flex items-center justify-between gap-5">
						<Button
							className="text-bold h-8 w-20 border-2 border-tertiary-1 text-tertiary-1"
							onClick={() => {
								setFilter(initFilter)
								console.log(filter)
							}}
						>
							Reset
						</Button>
						<Button
							className="flex-1 bg-primary-2 text-white"
							onClick={() => {
								console.log(filter)
								toast.info('Filter applied', 'Check console for filter data')
							}}
						>
							Apply
						</Button>
					</div>
				</div>

				<div className="h-full flex-1">
					<div className="mb-5 flex w-full items-center justify-between">
						<SearchBox
							className="h-9 w-[220px]"
							onChangeValue={(event) => setSearchValue(event.target.value)}
							onClickSearch={() => console.log(searchValue)}
						/>
						<DropdownSelect
							id={`sort`}
							className="h-9 w-[120px]"
							options={sortOptions}
							value={sort}
							onChange={(event) => {
								setSort(Number(event.target.value))
							}}
						/>
					</div>
					<div className="flex flex-wrap justify-between gap-y-5">
						{destinations
							? destinations.map((des, index) => (
									<DesPreviewCard
										key={index}
										thumbnail={(des as any).thumbnail}
										name={des.name}
										location={des.address}
										rating={des.rating}
										cost={des.cost}
										openTime={des.openingTime}
										closeTime={des.closingTime}
										tags={des.tags}
										pin={des.pin}
										favorite={des.favorite}
										onVisit={() => navigate(`/destination/${des.id}`)}
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
		</div>
	)
}

export default DestinationPage
