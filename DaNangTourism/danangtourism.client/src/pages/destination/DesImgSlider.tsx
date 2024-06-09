import { useEffect, useState } from 'react'
import { PiArrowCircleLeft, PiArrowCircleRight } from 'react-icons/pi'
import { motion, AnimatePresence } from 'framer-motion'

interface DesImgSliderProps {
	imgUrls: string[]
	className?: string
	height?: string
	name: string
}

const DesImgSlider: React.FC<DesImgSliderProps> = ({
	imgUrls,
	className,
	height = 'calc(100vh - 64px - 16px - 48px)',
	name,
}) => {
	const [currentImg, setCurrentImg] = useState(0)
	const [direction, setDirection] = useState<'left' | 'right'>('right')
	function handleNextImg() {
		setDirection('right')
		setCurrentImg((prev) => (prev + 1) % imgUrls.length)
	}
	function handlePrevImg() {
		setDirection('left')
		setCurrentImg((prev) => (prev + imgUrls.length - 1) % imgUrls.length)
	}

	useEffect(() => {
		const interval = setInterval(() => {
			handleNextImg()
		}, 5000)
		return () => clearInterval(interval)
	}, [currentImg])

	return (
		<div
			className={`relative overflow-hidden rounded ${className}`}
			style={{
				height: height,
			}}
		>
			<AnimatePresence>
				<motion.img
					key={currentImg}
					className="absolute h-full w-full rounded object-cover"
					src={imgUrls[currentImg]}
					alt={`image ${currentImg}`}
					initial={{ x: direction === 'left' ? '-100%' : '100%', opacity: 1 }}
					animate={{ x: 0, opacity: 1 }}
					exit={{ zIndex: -1, position: 'absolute', opacity: 0.5, scale: 0.9 }}
					transition={{ duration: 0.5 }}
				/>
			</AnimatePresence>

			<div className="absolute top-0 flex w-full items-center gap-4 rounded-[inherit] bg-gradient-to-b from-[#0000009c] to-transparent px-10 pb-10 pt-5 text-xl font-bold">
				<span className="h-[1px] flex-1 bg-white"></span>
				<h1 className="text-white">{name}</h1>
				<span className="h-[1px] flex-1 bg-white"></span>
			</div>
			<div className="absolute left-0 top-1/2 flex w-full justify-between px-3 text-4xl text-white opacity-50 hover:opacity-80">
				<button
					className="rounded-full p-2 hover:bg-[#00000042]"
					onClick={() => handlePrevImg()}
				>
					<PiArrowCircleLeft />
				</button>
				<button
					className="rounded-full p-2 hover:bg-[#00000042]"
					onClick={() => handleNextImg()}
				>
					<PiArrowCircleRight />
				</button>
			</div>
			<div className="absolute bottom-0 flex w-full items-center justify-center gap-4 rounded-[inherit] py-2 pt-5 text-base">
				<h1 className="rounded-full bg-[#0000008e] px-4 tracking-widest text-white">{`${currentImg + 1}/${imgUrls.length}`}</h1>
			</div>
		</div>
	)
}

export default DesImgSlider
