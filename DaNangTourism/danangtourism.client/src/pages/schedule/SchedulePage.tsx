import { useEffect, useState } from 'react'
import {
	Button,
	DropdownSelect,
	Pagination,
	SearchBox,
	SortTypeButton,
} from '../../components'
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
import noItemImg from '../../assets/no-item.png'
import useUser from '../../hook/useUser'
import { useToast } from '../../hook/useToast'
const SortBys = [
	{
		label: 'Updated at',
		value: 'updatedAt',
	},
	{
		label: 'Title',
		value: 'title',
	},
	{
		label: 'Start date',
		value: 'startDate',
	},
]

const SchedulePage: React.FC = () => {
	document.title = 'Schedules | Danang Tourism'
	const [tabIndex, setTabIndex] = useState(0)
	const [mySchedules, setMySchedules] = useState<MyScheduleItemProps[]>()
	const [publicSchedules, setPublicSchedules] =
		useState<PublicScheduleItemProps[]>()
	const StatusArray = ScheduleStatus.map((item) => item.status)
	const [statusIndex, setStatusIndex] = useState(0)
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [searchValue, setSearchValue] = useState('')
	const [loading, setLoading] = useState(true)
	const [isNewScheduleModalOpen, setIsNewScheduleModalOpen] = useState(false)
	const [numbOfPages, setNumbOfPages] = useState(1)
	const [currentPage, setCurrentPage] = useState(1)
	const limit = 5

	const { user } = useUser()
	const toast = useToast()

	const getMySchedules = async () => {
		setLoading(true)
		setMySchedules(undefined)

		try {
			const response = await axios.get(`/api/schedule/mySchedule`, {
				params: {
					page: currentPage,
					limit: limit,
					status: statusIndex,
					sortBy: SortBys[sort.by].value,
					sortType: sort.type,
					...(searchValue && { search: searchValue }),
				},
			})
			const data = response.data.data
			setMySchedules(data.items)
			setNumbOfPages(Math.ceil(data.total / limit))
		} catch (error) {
			console.error(error)
		}
		setLoading(false)
	}

	const getPublicSchedules = async () => {
		setLoading(true)
		setPublicSchedules(undefined)
		try {
			const response = await axios.get(`/api/schedule/sharedSchedule`, {
				params: {
					page: currentPage,
					limit: limit,
					sortBy: SortBys[sort.by].value,
					sortType: sort.type,
					...(searchValue && { search: searchValue }),
				},
			})
			const data = response.data.data
			setPublicSchedules(data.items)
			setNumbOfPages(Math.ceil(data.total / limit))
		} catch (error) {
			console.error(error)
		}
		setLoading(false)
	}

	const handleSearch = () => {
		if (tabIndex === 0) getPublicSchedules()
		else getMySchedules()
	}

	useEffect(() => {
		handleSearch()
	}, [tabIndex, currentPage, statusIndex, sort])

	const handleOpenMySchedule = () => {
		if (!user || user.id === 0) {
			toast.info(
				'You are not logged in',
				'Please log in to view your schedules',
			)
			return
		}
		setTabIndex(1)
	}

	const handleOpenCreateModal = () => {
		if (!user || user.id === 0) {
			toast.info('Login required', 'Please log in to create a new schedule')
			return
		}
		setIsNewScheduleModalOpen(true)
	}

	useEffect(() => {
		if (searchValue === '') {
			handleSearch()
		}
	}, [searchValue])

	return (
		<div className="mx-auto flex min-h-screen justify-center gap-4 pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="flex w-full items-start justify-center gap-4">
				<div className="flex w-[200px] flex-col gap-3 pt-[52px]">
					<button
						className={`h-10 w-full rounded  border px-4 text-left text-sm font-semibold ${tabIndex == 0 ? 'border-borderCol-1 bg-white' : 'border-transparent text-txtCol-2 transition-all hover:bg-[#0000000e]'}`}
						onClick={() => setTabIndex(0)}
					>
						Explore everyone's
					</button>
					<button
						className={`h-10 w-full rounded border px-4 text-left text-sm font-semibold ${tabIndex == 1 ? 'border-borderCol-1 bg-white' : ' border-transparent hover:bg-[#0000000e]'} transition-all`}
						onClick={handleOpenMySchedule}
					>
						Your travel schedules
					</button>
				</div>
				<div className="flex-1">
					<div className="flex w-full items-center justify-between">
						<div className="flex gap-4">
							<DropdownSelect
								id="schedule-sort"
								className="h-9 w-[200px]"
								title="sort-blog"
								options={SortBys.map((item) => item.label)}
								value={sort.by}
								onChange={(event) => {
									setSort({
										by: Number(event.target.value),
										type: sort.type,
									})
								}}
							/>
							<SortTypeButton
								id="sort-type"
								className="mr-2 h-9 w-9"
								value={sort.type}
								onClick={() => {
									setSort({
										...sort,
										type: sort.type === 'asc' ? 'desc' : 'asc',
									})
								}}
							/>
							{tabIndex === 1 && (
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
							)}
						</div>
						<div className="flex gap-4">
							<SearchBox
								className="h-9 w-[200px]"
								onChangeValue={(event) => {
									setSearchValue(event.target.value)
								}}
								onClickSearch={handleSearch}
							/>
							<Button
								className="h-9 bg-secondary-1 text-white hover:bg-[#42a186]"
								onClick={handleOpenCreateModal}
							>
								<PiCalendarPlusBold className="text-lg" />
								Create new
							</Button>
						</div>
					</div>
					<div className="mt-4 flex w-full flex-col gap-4">
						{loading &&
							Array.from({ length: 3 }, (_, index) => (
								<LoadingScheduleItem key={index} />
							))}
						{tabIndex === 0 &&
							publicSchedules &&
							publicSchedules.map((schedule) => (
								<PublicScheduleItem
									className="w-full"
									key={schedule.id}
									schedule={schedule}
								/>
							))}
						{tabIndex === 1 &&
							mySchedules &&
							mySchedules.map((schedule) => (
								<MyScheduleItem
									className="w-full"
									key={schedule.id}
									schedule={schedule}
								/>
							))}

						{!loading &&
						((tabIndex === 0 && !publicSchedules) ||
							(tabIndex === 1 && !mySchedules)) ? (
							<div className="flex h-[480px] w-full flex-col items-center justify-center gap-5">
								<img
									className="h-[320px]"
									src={noItemImg}
									alt="No item found"
								/>
								<p className="text-3xl font-semibold tracking-wide text-txtCol-3">
									No schedule found
								</p>
							</div>
						) : (
							<Pagination
								className="mt-4 w-full justify-center"
								numbOfPages={numbOfPages}
								currentPage={currentPage}
								setCurrentPage={(numb) => {
									setCurrentPage(numb)
								}}
							/>
						)}
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
