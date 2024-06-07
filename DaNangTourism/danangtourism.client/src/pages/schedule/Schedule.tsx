import { useParams } from 'react-router-dom'
import { ScheduleDetailProps } from '../../types/schedule'
import { useEffect, useState } from 'react'
import axios from 'axios'
import { Button, Loader } from '../../components'
import PageNotFound from '../PageNotFound'
import { timeAgo } from '../../utils/TimeFormatters'
import ScheduleOverview from './ScheduleOverview'
import ScheduleDay from './ScheduleDay'
import { PiGearFill, PiMapPinLineFill } from 'react-icons/pi'
import AddDestinationModal from './AddDestinationModal'
import SetupModal from './SetupModal'

const Schedule: React.FC = () => {
	const { id } = useParams()
	const [schedule, setSchedule] = useState<ScheduleDetailProps>()
	const [loading, setLoading] = useState(true)
	const [isAddModalOpen, setIsAddModalOpen] = useState(false)
	const [isSetupModalOpen, setIsSetupModalOpen] = useState(false)
	const getSchedule = async (id: string) => {
		try {
			setSchedule(undefined)
			const response = await axios.get(`/api/schedule/detail/${id}`)
			setSchedule(response.data.data)
		} catch (error) {
			console.error(error)
		}
		setLoading(false)
	}

	const handleModalClose = (changed: boolean) => {
		setIsSetupModalOpen(false)
		if (changed) getSchedule(id ?? '')
	}

	useEffect(() => {
		getSchedule(id ?? '')
	}, [id])

	useEffect(() => {
		document.title = (schedule?.title ?? 'Schedules') + ' | Danang Tourism'
	}, [schedule])

	if (loading)
		return (
			<div className="mx-auto mt-12 flex min-h-screen items-center justify-center xl:max-w-screen-xl">
				<Loader />
			</div>
		)
	else if (!schedule) return <PageNotFound />
	return (
		<div className="relative mx-auto min-h-screen xl:max-w-screen-xl">
			<div className="w-full pb-5 pt-[64px] text-txtCol-1">
				<div className="flex w-full flex-col items-start justify-start gap-2">
					<h2 className="my-2 text-3xl font-bold">{schedule.title}</h2>
					<div className="flex items-center gap-2 text-base">
						<h4 className="font-semibold">Creator:</h4>
						<p>by {schedule.creator}</p>
						<span className="mx-1 h-[6px] w-[6px] rounded-full bg-txtCol-2"></span>
						<h4 className="font-semibold">Last updated:</h4>
						<p>{timeAgo(schedule.updatedAt)}</p>
					</div>
					<p className="text-base">{schedule.description}</p>
				</div>
				<div className="item-start mt-5 flex w-full gap-16">
					<div className="flex flex-1 flex-col items-start gap-4">
						<Button
							className="ml-[100px] h-9 w-[172px] bg-secondary-2 font-semibold text-white hover:bg-secondary-1 "
							onClick={() => setIsAddModalOpen(true)}
						>
							<PiMapPinLineFill className="text-lg" />
							Add destination
						</Button>
						{schedule.days.map((day, index) => (
							<ScheduleDay
								className="mb-2 w-full"
								key={index}
								scheduleDay={day}
								onChanged={() => getSchedule(id ?? '')}
							/>
						))}
					</div>
					<div className="flex w-[300px] flex-col items-end gap-4">
						<Button
							className="h-9 w-[100px] bg-primary-2 font-semibold text-white hover:bg-primary-1"
							onClick={() => setIsSetupModalOpen(true)}
						>
							<PiGearFill className="text-lg" />
							Set up
						</Button>
						<ScheduleOverview
							className="w-full"
							numbOfDes={schedule.days.reduce(
								(total, day) => total + day.destinations.length,
								0,
							)}
							totalTime={schedule.totalDays}
							totalBudget={schedule.totalBudget}
						/>
					</div>
				</div>
			</div>
			{isAddModalOpen && (
				<AddDestinationModal
					className="fixed left-0 top-0 z-10 h-screen w-screen"
					onCancel={() => setIsAddModalOpen(false)}
					onSubmitted={() => {
						setIsAddModalOpen(false)
						getSchedule(id ?? '')
					}}
					scheduleId={schedule.id}
				/>
			)}
			{isSetupModalOpen && (
				<SetupModal
					scheduleId={schedule.id}
					general={{
						title: schedule.title,
						description: schedule.description,
						isPublic: schedule.isPublic,
						status: schedule.status,
					}}
					className="fixed left-0 top-0 z-10 h-screen w-screen"
					onCancel={handleModalClose}
				/>
			)}
		</div>
	)
}

export default Schedule
