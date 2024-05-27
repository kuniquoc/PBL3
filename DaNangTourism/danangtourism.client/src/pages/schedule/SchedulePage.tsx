import { useEffect, useState } from 'react'
import { Button, DropdownSelect, Pagination, SearchBox } from '../../components'
import { PiCalendarPlusBold } from 'react-icons/pi'
import {
	MyScheduleItemProps,
	PublicScheduleItemProps,
	ScheduleStatus,
} from '../../types/schedule'
import axios from 'axios'
import MyScheduleItem from './MyScheduleItem'
import PublicScheduleItem from './PublicScheduleItem'
import LoadingScheduleItem from './LoadingScheduleItem'
import SetupModal from './SetupModal'

const SortOptions = [
	'Sort by',
	'Last updated',
	'Start date (ascending)',
	'Start date (descending)',
]

const SchedulePage: React.FC = () => {
	document.title = 'Schedules | Da Nang Explore'
	const [tabIndex, setTabIndex] = useState(0)
	const [mySchedules, setMySchedules] = useState<MyScheduleItemProps[]>()
	const [publicSchedules, setPublicSchedules] =
		useState<PublicScheduleItemProps[]>()
	const StatusArray = ScheduleStatus.map((item) => item.status)
	const [statusIndex, setStatusIndex] = useState(0)
	const [sortIndex, setSortIndex] = useState(0)
	const [searchValue, setSearchValue] = useState('')
	const [loading, setLoading] = useState(true)
	const [isNewScheduleModalOpen, setIsNewScheduleModalOpen] = useState(false)
	const numbOfPages = 5
	const [currentPage, setCurrentPage] = useState(1)

	const getMySchedules = async (page: number) => {
		try {
			setLoading(true)
			setMySchedules(undefined)
			const response = await axios.get(
				`/api/schedule/my-schedules-${page}.json`,
			)
			await new Promise((resolve) => setTimeout(resolve, 1500))
			if (response.data.status === 200) {
				setMySchedules(response.data.data.items)
				setLoading(false)
			}
		} catch (error) {
			console.error(error)
		}
	}

	const getPublicSchedules = async (page: number) => {
		try {
			setLoading(true)
			setPublicSchedules(undefined)
			const response = await axios.get(
				`/api/schedule/public-schedules-${page}.json`,
			)
			await new Promise((resolve) => setTimeout(resolve, 3000))
			if (response.data.status === 200) {
				setPublicSchedules(response.data.data.items)
				setLoading(false)
			}
			console.log(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}

	useEffect(() => {
		if (tabIndex === 0) getMySchedules(currentPage)
		else getPublicSchedules(currentPage)
	}, [tabIndex, currentPage])
	return (
		<div className="mx-auto flex min-h-screen justify-center gap-4 pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="flex w-full items-start justify-center gap-4">
				<div className="flex w-[200px] flex-col gap-3 pt-[52px]">
					<button
						className={`h-10 w-full rounded border px-4 text-left text-sm font-semibold ${tabIndex == 0 ? 'border-borderCol-1 bg-white' : ' border-transparent hover:bg-[#0000000e]'} transition-all`}
						onClick={() => setTabIndex(0)}
					>
						Your travel schedules
					</button>
					<button
						className={`h-10 w-full rounded  border px-4 text-left text-sm font-semibold ${tabIndex == 1 ? 'border-borderCol-1 bg-white' : 'border-transparent text-txtCol-2 transition-all hover:bg-[#0000000e]'}`}
						onClick={() => setTabIndex(1)}
					>
						Explore everyone's
					</button>
				</div>
				<div className="flex-1">
					<div className="flex w-full items-center justify-between">
						<div className="flex gap-4">
							<DropdownSelect
								id="schedule-sort"
								className="h-9 w-[200px]"
								title="sort-blog"
								options={SortOptions}
								value={sortIndex}
								onChange={(event) => {
									setSortIndex(Number(event.target.value))
								}}
							/>
							<DropdownSelect
								id="schedule-status"
								className="h-9 w-[120px]"
								title="status-blog"
								options={StatusArray}
								value={statusIndex}
								onChange={(event) => {
									setStatusIndex(Number(event.target.value))
								}}
							/>
						</div>
						<div className="flex gap-4">
							<SearchBox
								className="h-9 w-[200px]"
								onChangeValue={(event) => {
									setSearchValue(event.target.value)
								}}
								onClickSearch={() => {
									console.log('Searching for:', searchValue)
								}}
							/>
							<Button
								className="h-9 bg-secondary-1 text-white hover:bg-[#42a186]"
								onClick={() => {
									setIsNewScheduleModalOpen(true)
								}}
							>
								<PiCalendarPlusBold className="text-lg" />
								Create new
							</Button>
						</div>
					</div>
					<div className="mt-4 flex w-full flex-col gap-4">
						{loading &&
							Array.from({ length: 4 }, (_, index) => (
								<LoadingScheduleItem key={index} />
							))}
						{tabIndex === 0 &&
							mySchedules &&
							mySchedules.map((schedule) => (
								<MyScheduleItem
									className="w-full"
									key={schedule.id}
									schedule={schedule}
									statusColor={
										ScheduleStatus.find(
											(item) => item.status === schedule.status,
										)?.color
									}
								/>
							))}
						{tabIndex === 1 &&
							publicSchedules &&
							publicSchedules.map((schedule) => (
								<PublicScheduleItem
									className="w-full"
									key={schedule.id}
									schedule={schedule}
								/>
							))}
						<Pagination
							className="mt-4 w-full justify-center"
							numbOfPages={numbOfPages}
							currentPage={currentPage}
							setCurrentPage={(numb) => {
								setCurrentPage(numb)
							}}
						/>
					</div>
				</div>
			</div>
			{isNewScheduleModalOpen && (
				<SetupModal
					className="fixed left-0 top-0 z-10 h-screen w-screen"
					onCancel={() => setIsNewScheduleModalOpen(false)}
				/>
			)}
		</div>
	)
}

export default SchedulePage
