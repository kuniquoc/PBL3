import { twMerge } from 'tailwind-merge'
import {
	Button,
	DropdownSelect,
	Pagination,
	SearchBox,
	SortTypeButton,
	ToggleButton,
} from '../../components'
import { useEffect, useState } from 'react'
import axios from 'axios'
import { SimpleDesProps } from '../../types/destination'
import { useToast } from '../../hook/useToast'

const sortBy = [
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

const AddDestinationModal: React.FC<{
	className?: string
	onCancel: () => void
	scheduleId: number
	onSubmitted: () => void
}> = ({ className = '', onCancel, scheduleId, onSubmitted }) => {
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'asc',
	})
	const [isFavorite, setIsFavorite] = useState<boolean>(false)
	const [destinations, setDestinations] = useState<SimpleDesProps[]>()
	const limit = 9
	const [currentPage, setCurrentPage] = useState(1)
	const [total, setTotal] = useState(0)
	const toast = useToast()

	const getDestinations = async () => {
		try {
			const response = await axios.get('/api/destination/list', {
				params: {
					page: currentPage,
					limit,
					sortBy: sortBy[sort.by].value,
					sortType: sort.type,
					...(isFavorite ? { isFavorite: true } : {}),
					...(searchValue !== '' ? { search: searchValue } : {}),
				},
			})
			const data = response.data.data
			setDestinations(data.items)
			setTotal(data.total)
		} catch (error) {
			console.error(error)
		}
	}

	const [sDestination, setSDestination] = useState({
		destinationId: 0,
		date: '',
		arrivalTime: '',
		leaveTime: '',
		budget: 0,
		note: '',
	})

	const validate = () => {
		if (
			sDestination.date === '' ||
			sDestination.arrivalTime === '' ||
			sDestination.leaveTime === ''
		) {
			toast.error('Empty field', 'Please fill in all required fields')
			return false
		}

		if (sDestination.leaveTime <= sDestination.arrivalTime) {
			toast.error(
				'Invalid time',
				'Departure time must be later than arrival time',
			)
			return false
		}

		return true
	}

	const handleAddDestination = async () => {
		if (!validate()) return
		try {
			await axios.post('/api/schedule/addDestination', {
				scheduleId,
				...sDestination,
			})
			toast.success(
				'Add destination success',
				'Destination has been added to schedule',
			)
			onSubmitted()
		} catch (error) {
			console.error(error)
			toast.error('Add destination failed', 'Please try again later')
		}
	}

	useEffect(() => {
		getDestinations()
	}, [searchValue, sort, isFavorite])
	return (
		<div
			className={twMerge(
				`flex items-center justify-center bg-[#0000004b] ${className}`,
			)}
		>
			<div className="flex w-[1000px] flex-col items-center gap-4 rounded-xl bg-white p-5">
				<h3 className="text-xl font-semibold">Add destination to schedule</h3>
				<div className="flex w-full items-center justify-between">
					<SearchBox
						className="h-8 w-[200px]"
						onChangeValue={(event) => {
							setSearchValue(event.target.value)
						}}
						onClickSearch={() => {
							console.log('Searching for:', searchValue)
						}}
					/>

					<div className="flex items-center gap-4">
						<ToggleButton
							onClick={() => {
								setIsFavorite(!isFavorite)
							}}
							text="Favorite"
							toggledText="All"
							initToggled={true}
							btnColor={'#64B8DC'}
							className="h-8 w-[80px]"
						></ToggleButton>
						<DropdownSelect
							id="sort-by"
							className="h-8 w-[140px]"
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
							className="h-8 w-8"
							value={sort.type}
							onClick={() => {
								setSort({
									...sort,
									type: sort.type === 'asc' ? 'desc' : 'asc',
								})
							}}
						/>
					</div>
				</div>
				<table className="w-full border-spacing-2 rounded border border-borderCol-1 py-2 pr-1">
					<thead className="border-b border-borderCol-1">
						<tr className="h-8 text-center text-sm [&_*]:font-semibold">
							<th className="w-[212px] pl-2">Destination name</th>
							<th>Address</th>
							<th className="w-[92px]">Open time</th>
							<th className="w-[92px]">Close time</th>
							<th className="w-[84px]">Avg. cost</th>
							<th className="w-[84px] pr-2">Avg. rating</th>
						</tr>
					</thead>
					<tbody className="overflow-y-auto pt-2 [&>*:nth-child(odd)]:bg-gray-100 hover:[&_tr]:bg-[#64ccdc3f]">
						{destinations?.map((destination) => (
							<tr
								key={destination.id}
								className={twMerge(
									`h-10 text-center text-sm ${sDestination.destinationId === destination.id && 'bg-[#2898c82a] font-semibold'}`,
								)}
								onClick={() => {
									setSDestination({
										...sDestination,
										destinationId: destination.id,
									})
								}}
							>
								<td className="pl-2 ">
									<a
										className={`line-clamp-1 text-left hover:text-primary-1 hover:underline`}
										title={destination.name}
										href={`/destination/${destination.id}`}
										target="_blank"
									>
										{destination.name}
									</a>
								</td>
								<td>
									<a className="line-clamp-1 text-left">
										{destination.address}
									</a>
								</td>
								<td>{destination.openTime}</td>
								<td>{destination.closeTime}</td>
								<td>${destination.cost}</td>
								<td className="pr-2">{destination.rating.toFixed(2)}</td>
							</tr>
						))}
						<tr
							style={
								destinations && {
									height: `${(limit - destinations.length) * 40}px`,
								}
							}
						></tr>
					</tbody>
				</table>
				<div className="flex w-full justify-between">
					<div className="flex h-8 items-center rounded border border-borderCol-1 px-3 text-txtCol-2">
						{(currentPage - 1) * limit + 1}
						{' - '}
						{currentPage * limit > total ? total : currentPage * limit}
						{' of '}
						{total}
					</div>
					<Pagination
						numbOfPages={Math.ceil(total / limit)}
						currentPage={currentPage}
						setCurrentPage={setCurrentPage}
					/>
				</div>
				<div className="flex w-full justify-between">
					<div className="flex items-center gap-3 text-sm">
						<div className="font-semibold">Date</div>
						<input
							className="h-8 bg-bgCol-1"
							type="date"
							value={sDestination.date}
							onChange={(event) =>
								setSDestination({
									...sDestination,
									date: event.target.value,
								})
							}
						/>
						<div className="ml-2 font-semibold">Arrival time</div>
						<input
							className="h-8 bg-bgCol-1"
							type="time"
							value={sDestination.arrivalTime}
							onChange={(event) =>
								setSDestination({
									...sDestination,
									arrivalTime: event.target.value,
								})
							}
						/>
						<div className="ml-2 font-semibold">Departure time</div>
						<input
							className="h-8 bg-bgCol-1"
							type="time"
							value={sDestination.leaveTime}
							onChange={(event) =>
								setSDestination({
									...sDestination,
									leaveTime: event.target.value,
								})
							}
						/>
					</div>
					<div className="flex items-center gap-5">
						<Button
							className="h-8 w-[100px] border-[2px] border-tertiary-1 text-tertiary-1 hover:bg-[#e75b5125]"
							onClick={onCancel}
						>
							Cancel
						</Button>
						<Button
							className="h-8 w-[100px] bg-primary-2 text-white hover:bg-primary-1"
							onClick={handleAddDestination}
						>
							Add
						</Button>
					</div>
				</div>
			</div>
		</div>
	)
}

export default AddDestinationModal
