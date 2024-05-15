import {
	PiClock,
	PiClockAfternoonFill,
	PiHeart,
	PiHeartFill,
	PiStarFill,
} from 'react-icons/pi'
import { useState } from 'react'
type DesPreviewCardProps = {
	className?: string
	name: string
	location: string
	rating: number
	cost: number
	openTime: string
	closeTime: string
	thumbnail: string
	tags: string[]
	pin?: string
	favorite?: boolean
	onVisit?: () => void
}

const DesPreviewCard: React.FC<DesPreviewCardProps> = ({
	className = '',
	name,
	location,
	rating,
	cost,
	openTime,
	closeTime,
	thumbnail,
	tags,
	pin,
	favorite,
	onVisit,
}) => {
	const [favor, setFavorite] = useState(favorite)
	const [imgLoaded, setImgLoaded] = useState(false)
	return (
		<div
			className={`relative flex min-h-10 w-[240px] flex-col flex-nowrap items-start rounded-[8px] border border-borderCol-1 bg-[#fff] p-2 ${className}`}
		>
			<img
				src={thumbnail}
				alt={name}
				className={`aspect-[16/9] w-full cursor-pointer rounded object-cover ${imgLoaded ? 'block' : 'hidden'}`}
				onLoad={() => setImgLoaded(true)}
				onClick={onVisit}
			/>
			{!imgLoaded && (
				<span className="skeleton w-full rounded pt-[56.25%]"></span>
			)}
			<div className="my-2 flex w-full gap-1 overflow-hidden">
				<div
					className="flex flex-1 cursor-pointer flex-col items-start overflow-hidden"
					onClick={onVisit}
				>
					<h3
						className="line-clamp-1 text-sm font-semibold hover:text-primary-1 hover:underline"
						title={name}
					>
						{name}
					</h3>
					<p className=" line-clamp-1 text-[11px]" title={location}>
						{location}
					</p>
				</div>
				<button
					className="h-[30px] text-2xl"
					onClick={() => setFavorite(!favor)}
				>
					{favor ? (
						<PiHeartFill className=" text-tertiary-1" />
					) : (
						<PiHeart className=" hover:text-tertiary-2" />
					)}
				</button>
			</div>
			<div className="mb-1 flex w-full items-center">
				<div className="flex flex-1 items-center gap-1 text-xs font-semibold">
					<p className=" text-txtCol-2">Rating: </p>
					<p>{rating}</p>
					<PiStarFill className="text-[#FFC70D]" />
				</div>
				<div className="flex flex-1 items-center gap-1 text-xs font-semibold">
					<p className="text-txtCol-2">Avg. Cost: </p>
					<p className="text-xs">$</p>
					<p>{cost}</p>
				</div>
			</div>
			<div className="mb-2 flex w-full items-center text-xs">
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
							className="rounded-full border border-borderCol-1 px-2 py-0.5 text-[10px]"
						>
							{tag}
						</div>
					))}
			</div>
			{pin && (
				<div className="absolute right-4 top-4 flex items-center gap-1 rounded-full bg-[#1d1c28af] px-3 py-0.5 text-[10px] text-white">
					<div
						className={`h-1.5 w-1.5 rounded-full ${pin == 'Hot' ? 'bg-tertiary-1' : ''} ${pin == 'New' ? 'bg-secondary-1' : ''} ${pin == 'Recommend' ? 'bg-primary-1' : ''}`}
					></div>
					{pin}
				</div>
			)}
		</div>
	)
}

export default DesPreviewCard

const DPCLoading: React.FC = () => {
	return (
		<div
			className={`relative flex min-h-10 w-[240px] cursor-pointer flex-col flex-nowrap items-start rounded-[8px] border border-borderCol-1 bg-[#fff] p-3`}
		>
			<span className="skeleton w-full rounded pt-[56.25%]"></span>
			<div className="my-2 flex w-full overflow-hidden">
				<div className="flex flex-1 flex-col items-start overflow-hidden">
					<span className="skeleton my-0.5 h-4 w-[140px] rounded-sm"></span>
					<span className="skeleton my-0.5 h-[12.5px] w-[180px] rounded-sm"></span>
				</div>
			</div>
			<div className="mb-1 flex w-full items-center">
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[14px] w-[80px] rounded-sm"></span>
				</div>
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[14px] w-[92px] rounded-sm"></span>
				</div>
			</div>
			<div className="mb-1 flex w-full items-center text-sm">
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[14px] w-[100x] rounded-sm"></span>
				</div>
				<div className="flex-1">
					<span className="skeleton my-[1px] h-[14px] w-[100px] rounded-sm"></span>
				</div>
			</div>
			<div className="flex w-full items-center gap-2 overflow-hidden">
				<span className="skeleton mt-1 h-[18px] w-[180px] rounded-sm"></span>
			</div>
		</div>
	)
}

export { DPCLoading }
