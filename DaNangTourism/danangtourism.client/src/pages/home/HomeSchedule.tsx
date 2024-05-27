import { motion } from 'framer-motion'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { twMerge } from 'tailwind-merge'

const HomeSchedule: React.FC<{ className?: string }> = ({ className }) => {
	const [isHovered, setIsHovered] = useState(false)
	const navigate = useNavigate()
	return (
		<div
			className={twMerge(`relative overflow-hidden rounded-lg ${className}`)}
		>
			<img
				className="h-[200px] w-full object-cover"
				src="https://duan-sungroup.com/wp-content/uploads/2022/12/Mercure-Danang-French-Village-Bana-Hills-noi-bat-giua-xu-so-hoa-noi-trong-may.png"
				alt="home-schedule-banner"
			/>
			<motion.div
				className="absolute left-0 top-0 h-full w-full cursor-pointer bg-[#00000050]"
				onMouseEnter={() => setIsHovered(true)}
				onMouseLeave={() => setIsHovered(false)}
				animate={isHovered ? { padding: 20 } : { padding: 12 }}
				onClick={() => navigate('/schedule')}
			>
				<motion.div
					className="flex h-full w-full items-center justify-center rounded-md border-[2px]"
					animate={
						isHovered
							? { borderColor: '#ffffff', backgroundColor: '#00000025' }
							: { borderColor: '#ffffff00' }
					}
				>
					<motion.div
						className="select-none text-center font-bold tracking-wider text-white"
						animate={isHovered ? { fontSize: '34px' } : { fontSize: '36px' }}
					>
						Start create your travel schedule
					</motion.div>
				</motion.div>
			</motion.div>
		</div>
	)
}

export default HomeSchedule
