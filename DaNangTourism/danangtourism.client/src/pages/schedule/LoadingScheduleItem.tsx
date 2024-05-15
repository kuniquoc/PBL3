import { twMerge } from 'tailwind-merge'

const LoadingScheduleItem: React.FC<{ className?: string }> = ({
	className = '',
}) => {
	return (
		<div
			className={twMerge(
				`flex flex-col gap-1 rounded-lg border border-borderCol-1 bg-white p-4 ${className}`,
			)}
		>
			<div className="my-1 flex items-center gap-3">
				<div className="skeleton h-[26px] w-[88px]"></div>
				<div className="skeleton h-[26px] w-[620px]"></div>
			</div>
			<div className="skeleton h-[18px] w-full rounded"></div>
			<div className="skeleton h-[18px] w-full rounded"></div>
			<div className="flex w-full items-end gap-4">
				<div className="flex flex-1 flex-col gap-1">
					<div className="skeleton mt-0.5 h-5 w-[680px] rounded"></div>
					<div className="skeleton mt-0.5 h-5 w-[620px] rounded"></div>
				</div>
				<div className="skeleton mt-1 h-7 w-[112px] rounded"></div>
			</div>
		</div>
	)
}

export default LoadingScheduleItem
