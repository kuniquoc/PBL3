import axios from 'axios'
import { twMerge } from 'tailwind-merge'
import { IPublicSchedule } from '../../interfaces/schedule'
import { Button } from '../../components'
import { useNavigate } from 'react-router-dom'
import { useToast, useUser } from '../../hook'

const PublicScheduleItem: React.FC<{
	schedule: IPublicSchedule
	className?: string
}> = ({ schedule, className }) => {
	const navigate = useNavigate()
	const toast = useToast()
	const { user } = useUser()
	const handleCloneSchedule = async () => {
		if (!user || user.id === 0) {
			toast.info('Login required', 'Please login to clone this schedule')
			return
		}
		try {
			const response = await axios.post(`/api/schedule/clone/${schedule.id}`)
			navigate(`/schedule/${response.data.data.id}`)
		} catch (error) {
			console.error(error)
			toast
		}
	}
	return (
		<div
			className={twMerge(
				`flex flex-col gap-1 rounded-lg border border-borderCol-1 bg-white p-4 hover:border-primary-3 ${className}`,
			)}
		>
			<div className="flex items-center gap-3 py-1">
				<div
					className={`flex h-6 w-[80px] items-center justify-center gap-[6px] rounded-full bg-secondary-2 text-[11px] text-white`}
				>
					<span className="h-[5px] w-[5px] rounded-full bg-white"></span>
					Sharing
				</div>
				<h3 className="line-clamp-1 text-xl font-semibold">{schedule.title}</h3>
			</div>
			<p className=" line-clamp-2 text-sm">{schedule.description}</p>
			<div className="flex w-full items-end gap-4">
				<div className="flex flex-1  flex-col gap-1">
					<div className="flex w-full items-center gap-2 overflow-hidden">
						<h5 className="text-sm font-semibold">Destinations: </h5>
						{schedule.destinations.length > 0 ? (
							schedule.destinations.map((destination, index) => {
								return (
									<span
										className="rounded-full border border-borderCol-1 px-2 py-0.5 text-xs transition-all hover:bg-[#0000000e]"
										key={index}
									>
										{destination}
									</span>
								)
							})
						) : (
							<p className="text-sm">No destination</p>
						)}
					</div>
					<div className="flex w-full items-center gap-5 overflow-hidden text-sm">
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Total time: </h5>
							<p className="">{schedule.totalDays} days</p>
						</div>
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Total budget: </h5>
							<p className="">${schedule.totalBudget}</p>
						</div>
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Shared by: </h5>
							<p className="">{schedule.creator}</p>
						</div>
					</div>
				</div>
				<Button
					className="h-8 w-[132px] rounded-full bg-primary-3 text-white hover:bg-primary-2"
					onClick={handleCloneSchedule}
				>
					Clone schedule
				</Button>
			</div>
		</div>
	)
}

export default PublicScheduleItem
