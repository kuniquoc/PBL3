import { PiStarFill } from 'react-icons/pi'
import { twMerge } from 'tailwind-merge'
const Stars: React.FC<{ rating: number; className?: string }> = ({
	rating,
	className,
}) => {
	return (
		<div className={twMerge(`item-center flex gap-1`, className)}>
			{[1, 2, 3, 4, 5].map((star) => {
				return (
					<PiStarFill
						key={star}
						className={
							Math.round(rating) >= star ? 'text-[#FFC70D]' : 'text-gray-200'
						}
					/>
				)
			})}
		</div>
	)
}

export default Stars
