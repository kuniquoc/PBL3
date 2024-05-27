import {
	PiClock,
	PiClockAfternoonFill,
	PiHeart,
	PiHeartFill,
	PiStarFill,
} from 'react-icons/pi'
import { useState } from 'react'
import { DesListItemProps } from '../../types/destination'

interface DesPreviewCardProps extends DesListItemProps {
	className?: string
	onVisit?: () => void
}

const DesPreviewCard: React.FC<DesPreviewCardProps> = ({
	className = '',
	name,
	address,
	image,
	rating,
	cost,
	openTime,
	closeTime,
	tags,
	favorite,
	onVisit,
}) => {
	const [favor, setFavorite] = useState(favorite)
	const [imgLoaded, setImgLoaded] = useState(false)

	const is247 = openTime === '00:00' && closeTime === '23:59'
	if (is247) {
		openTime = '24/7'
		closeTime = 'None'
	}
	return (
		<div
			className={`relative flex min-h-10 w-[300px] flex-col flex-nowrap items-start rounded-[8px] border border-borderCol-1 bg-[#fff] p-3 transition-shadow hover:shadow-lg ${className}`}
		>
			<img
				src={image}
				alt={name}
				className={`aspect-[16/9] w-full cursor-pointer rounded object-cover ${imgLoaded ? 'block' : 'hidden'}`}
				onLoad={() => setImgLoaded(true)}
				onClick={onVisit}
			/>
			{!imgLoaded && (
				<span className="skeleton w-full rounded pt-[56.25%]"></span>
			)}
			<div className="my-[10px] flex w-full gap-1 overflow-hidden">
				<div
					className="flex flex-1 cursor-pointer flex-col items-start overflow-hidden"
					onClick={onVisit}
				>
					<h3
						className="line-clamp-1 text-base font-semibold hover:text-primary-1 hover:underline"
						title={name}
					>
						{name}
					</h3>
					<p className=" line-clamp-1 text-xs" title={address}>
						{address}
					</p>
				</div>
				<button
					className="h-[30px] text-3xl"
					onClick={() => setFavorite(!favor)}
				>
					{favor ? (
						<PiHeartFill className=" text-tertiary-1" />
					) : (
						<PiHeart className=" hover:text-tertiary-2" />
					)}
				</button>
			</div>
			<div className="mb-1 flex w-full items-center gap-1">
				<div className="flex flex-1 items-center gap-1 text-sm font-semibold">
					<p className=" text-txtCol-2">Rating: </p>
					<p>{rating}</p>
					<PiStarFill className="text-[#FFC70D]" />
				</div>
				<div className="flex flex-1 items-center gap-1 text-sm font-semibold">
					<p className="text-txtCol-2">Avg. Cost: </p>
					<p className="text-xs">$</p>
					<p>{cost == 0 ? 'Free' : cost}</p>
				</div>
			</div>
			<div className="mb-3 flex w-full items-center gap-1 text-sm">
				<div className="flex flex-1 items-center gap-1">
					<PiClock />
					<p>Open: {openTime}</p>
				</div>
				<div className="flex flex-1 items-center gap-1">
					<PiClockAfternoonFill />
					<p>Close: {closeTime}</p>
				</div>
			</div>
			<div className="flex w-full items-center gap-2 overflow-hidden">
				{tags &&
					tags.map((tag, index) => (
						<div
							key={index}
							className="rounded-full border border-borderCol-1 px-2 py-0.5 text-xs"
						>
							{tag}
						</div>
					))}
			</div>
		</div>
	)
}

export default DesPreviewCard

const DPCLoading: React.FC = () => {
	return (
		<div
			className={`relative flex min-h-10 w-[300px] cursor-pointer flex-col flex-nowrap items-start rounded-[8px] border border-borderCol-1 bg-[#fff] p-3`}
		>
			<span className="skeleton w-full rounded pt-[56.25%]"></span>
			<div className="my-[10px] flex w-full overflow-hidden">
				<div className="flex flex-1 flex-col items-start overflow-hidden">
					<span className="skeleton my-0.5 h-[18px] w-[180px] rounded-sm"></span>
					<span className="skeleton my-0.5 h-[14px] w-[212px] rounded-sm"></span>
				</div>
			</div>
			<div className="mb-1 flex w-full items-center">
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[18.35px] w-[100px] rounded-sm"></span>
				</div>
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[18.35px] w-[100px] rounded-sm"></span>
				</div>
			</div>
			<div className="mb-1 flex w-full items-center text-sm">
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[18.35px] w-[100px] rounded-sm"></span>
				</div>
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[18.35px] w-[100px] rounded-sm"></span>
				</div>
			</div>
			<div className="flex w-full items-center gap-2 overflow-hidden">
				<span className="skeleton mt-2 h-5 w-[180px] rounded-sm"></span>
			</div>
		</div>
	)
}

export { DPCLoading }
