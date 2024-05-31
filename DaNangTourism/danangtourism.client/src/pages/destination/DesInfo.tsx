import { PiStarFill } from 'react-icons/pi'
const DesInfo: React.FC<{
	localName: string
	address: string
	rating: number
	cost: number
	openTime: string
	closeTime: string
	tags: string[]
}> = ({ localName, address, rating, cost, openTime, closeTime, tags }) => {
	return (
		<div className="w-full rounded-lg border border-borderCol-1 bg-white p-3">
			<div className="mb-4 inline-flex h-[30px] w-full items-center justify-center gap-2.5 rounded bg-borderCol-2 py-1 font-bold tracking-wide">
				Information
			</div>
			<ul className="flex w-full flex-col items-center justify-start gap-3">
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Local name</p>
					<p className="flex-1">{localName}</p>
				</li>
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Address</p>
					<p className="flex-1">{address}</p>
				</li>
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Avg. rating</p>
					<div className="flex flex-1 items-center gap-1">
						{rating.toFixed(1)} <PiStarFill className="text-[#FFC70D]" />
					</div>
				</li>
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Avg. cost</p>
					<p className="flex-1">{'$ ' + cost}</p>
				</li>
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Open time</p>
					<p className="flex-1">{openTime}</p>
				</li>
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Close time</p>
					<p className="flex-1">{closeTime}</p>
				</li>
				<li className="flex w-full items-start gap-[10px] text-sm">
					<p className="w-[100px] font-semibold">Tags</p>
					<p className="flex-1">
						{tags.map((tag: string, index: number) => (
							<span key={index} className="mr-1 italic">
								{tag}
								{index !== tags.length - 1 && ', '}
							</span>
						))}
					</p>
				</li>
			</ul>
		</div>
	)
}

export default DesInfo
