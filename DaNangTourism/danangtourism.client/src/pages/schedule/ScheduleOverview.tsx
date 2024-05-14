import { twMerge } from 'tailwind-merge'

const ScheduleOverview: React.FC<{
	className?: string
	numbOfDes: number
	totalTime: number
	totalBudget: number
}> = ({ className, numbOfDes, totalTime, totalBudget }) => {
	return (
		<div
			className={twMerge(
				`flex flex-col items-center rounded-lg border border-borderCol-1 bg-white px-4 py-5 ${className}`,
			)}
		>
			<h3 className="mb-2 text-lg font-semibold text-primary-1">Overview</h3>
			<div className="flex w-full items-center gap-4 border-b border-txtCol-1 py-1 text-sm">
				<p className="w-[172px] font-semibold">Number of destinations</p>
				<p className="flex-1">{numbOfDes}</p>
			</div>
			<div className="flex w-full items-center gap-4 border-b border-txtCol-1 py-1 text-sm">
				<p className="w-[172px] font-semibold">Total estimated time</p>
				<p className="flex-1">{totalTime} days</p>
			</div>
			<div className="flex w-full items-center gap-4 py-1 text-sm">
				<p className="w-[172px] font-semibold">Total estimated budget</p>
				<p className="flex-1">${totalBudget}</p>
			</div>
		</div>
	)
}

export default ScheduleOverview
