import { twMerge } from 'tailwind-merge'
import {
	Button,
	DropdownSelect,
	SearchBox,
	ToggleButton,
} from '../../components'
import { useEffect, useState } from 'react'
import axios from 'axios'

const SortOptions = [
	'Sort by',
	'Rating Ascending',
	'Rating Descending',
	'Price Ascending',
	'Price Descending',
]

type DestinationType = {
	id: number
	name: string
	address: string
	openingTime: string
	closingTime: string
	cost: number
	rating: number
	favorite: boolean
}

const AddDestinationModal: React.FC<{
	className?: string
	onCancel: () => void
}> = ({ className = '', onCancel }) => {
	const [searchValue, setSearchValue] = useState('')
	const [sortIndex, setSortIndex] = useState(0)
	const [isFavorite, setIsFavorite] = useState(false)
	const [destinations, setDestinations] = useState<DestinationType[]>([])

	const getDestinations = async () => {
		try {
			const response = await axios.get('/api/destination/page-1.json')
			setDestinations(response.data.data)
			console.log(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}

	const [scheduleDestination, setScheduleDestination] = useState({
		destination_id: 0,
		date: '',
		arrival_time: '',
		departure_time: '',
	})

	const handleAddDestination = () => {
		if (scheduleDestination.destination_id === 0) {
			window.alert('Please select a destination')
			return
		}

		if (
			scheduleDestination.date === '' ||
			scheduleDestination.arrival_time === '' ||
			scheduleDestination.departure_time === ''
		) {
			window.alert('Please fill in all fields')
			return
		}

		if (
			scheduleDestination.arrival_time >= scheduleDestination.departure_time
		) {
			window.alert('Arrival time must be before departure time')
			return
		}
		// send data to server
		window.location.reload()
		console.log('Add destination:', scheduleDestination)
	}

	useEffect(() => {
		getDestinations()
	}, [])
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
							id={''}
							className="h-8 w-[172px]"
							options={SortOptions}
							value={sortIndex}
							onChange={(event) => {
								setSortIndex(Number(event.target.value))
							}}
						></DropdownSelect>
					</div>
				</div>
				<div className="w-full rounded border border-borderCol-1 py-2 pr-1">
					<div className="flex w-full items-center gap-2 pr-3 text-center text-sm font-semibold">
						<div className="w-[212px]">Destination name</div>
						<div className="flex-1">Address</div>
						<div className="w-[92px]">Open time</div>
						<div className="w-[92px]">Close time</div>
						<div className="w-[84px]">Avg. cost</div>
						<div className="w-[84px]">Avg. rating</div>
					</div>
					<div className="h-[400px] overflow-y-auto pt-2">
						{destinations?.map((destination, index) => (
							<div
								key={destination.id}
								className={twMerge(
									`flex w-full items-center gap-2 py-2 pl-2 text-center text-sm ${index % 2 === 0 ? 'bg-[#0000000d]' : 'bg-white'} ${scheduleDestination.destination_id === destination.id && 'bg-[#2898c82a] font-semibold'}`,
								)}
								onClick={() => {
									setScheduleDestination({
										...scheduleDestination,
										destination_id: destination.id,
									})
								}}
							>
								<a
									className={`line-clamp-1 w-[212px] text-left hover:text-primary-1 hover:underline`}
									title={destination.name}
									href={`/destination/${destination.id}`}
									target="_blank"
								>
									{destination.name}
								</a>
								<div
									className="line-clamp-1 flex-1 text-left"
									title={destination.address}
								>
									{destination.address}
								</div>
								<div className="w-[92px]">{destination.openingTime}</div>
								<div className="w-[92px]">{destination.closingTime}</div>
								<div className="w-[84px]">${destination.cost}</div>
								<div className="w-[84px]">{destination.rating}</div>
							</div>
						))}
					</div>
				</div>
				<div className="flex w-full justify-between">
					<div className="flex items-center gap-3 text-sm">
						<div className="font-semibold">Date</div>
						<input
							className="h-8 bg-bgCol-1"
							type="date"
							value={scheduleDestination.date}
							onChange={(event) =>
								setScheduleDestination({
									...scheduleDestination,
									date: event.target.value,
								})
							}
						/>
						<div className="ml-2 font-semibold">Arrival time</div>
						<input
							className="h-8 bg-bgCol-1"
							type="time"
							value={scheduleDestination.arrival_time}
							onChange={(event) =>
								setScheduleDestination({
									...scheduleDestination,
									arrival_time: event.target.value,
								})
							}
						/>
						<div className="ml-2 font-semibold">Departure time</div>
						<input
							className="h-8 bg-bgCol-1"
							type="time"
							value={scheduleDestination.departure_time}
							onChange={(event) =>
								setScheduleDestination({
									...scheduleDestination,
									departure_time: event.target.value,
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
