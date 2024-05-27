import { twMerge } from 'tailwind-merge'
import { MyScheduleItemProps } from '../../types/schedule'
import { timeAgo } from '../../utils/TimeFormatters'
import { Button } from '../../components'
import { useNavigate } from 'react-router-dom'

const MyScheduleItem: React.FC<{
	schedule: MyScheduleItemProps
	className?: string
	statusColor?: string
}> = ({ schedule, className, statusColor = 'bg-[#d4d4d4]' }) => {
	const navigate = useNavigate()
	return (
		<div
			className={twMerge(
				`flex flex-col gap-1 rounded-lg border border-borderCol-1 bg-white p-4 hover:border-primary-3 ${className}`,
			)}
		>
			<div className="flex items-center gap-3 py-1">
				<div
					className={`flex h-6 w-[88px] items-center justify-center gap-[6px] rounded-full text-[11px] text-white ${statusColor}`}
				>
					<span className="h-[5px] w-[5px] rounded-full bg-white"></span>
					{schedule.status}
				</div>
				<h3 className="line-clamp-1 text-xl font-semibold">{schedule.title}</h3>
			</div>
			<p className=" line-clamp-2 text-sm">{schedule.description}</p>
			<div className="flex w-full items-end gap-4">
				<div className="flex flex-1 flex-col gap-1">
					<div className="flex w-full items-center gap-2 overflow-hidden">
						<h5 className="text-sm font-semibold">Destinations: </h5>
						{schedule.destinations.map((destination, index) => {
							return (
								<span
									className="rounded-full border border-borderCol-1 px-2 py-0.5 text-xs transition-all hover:bg-[#0000000e]"
									key={index}
								>
									{destination}
								</span>
							)
						})}
					</div>
					<div className="flex w-full items-center gap-5 overflow-hidden text-sm">
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Start date: </h5>
							<p className="">{schedule.startDate}</p>
						</div>
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Total time: </h5>
							<p className="">{schedule.totalDays} days</p>
						</div>
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Total budget: </h5>
							<p className="">${schedule.totalBudget} </p>
						</div>
						<div className="inline-flex gap-2">
							<h5 className="font-semibold">Last updated: </h5>
							<p className="">{timeAgo(schedule.updatedAt)}</p>
						</div>
					</div>
				</div>
				<Button
					className="h-8 w-[112px] rounded-full bg-primary-3 text-white hover:bg-primary-2"
					onClick={() => navigate(`/schedule/${schedule.id}`)}
				>
					View detail
				</Button>
			</div>
		</div>
	)
}

export default MyScheduleItem
