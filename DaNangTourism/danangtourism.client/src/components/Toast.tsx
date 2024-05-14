import { motion } from 'framer-motion'
import {
	PiCheckCircleFill,
	PiInfoFill,
	PiWarningCircleFill,
	PiXBold,
	PiXCircleFill,
} from 'react-icons/pi'
import { ToastProps } from '../types/global.types'
import { useToast } from '../hook/useToast'
import { useEffect } from 'react'

const ToastTypes = {
	success: {
		icon: <PiCheckCircleFill />,
		color: '#2a9d8f',
	},
	warning: {
		icon: <PiWarningCircleFill />,
		color: '#e9c46a',
	},
	error: {
		icon: <PiXCircleFill />,
		color: '#E75A51',
	},
	info: {
		icon: <PiInfoFill />,
		color: '#2898C8',
	},
}

const Toast: React.FC<ToastProps> = ({ type, title, message, id }) => {
	const { icon, color } = ToastTypes[type]
	const toast = useToast()
	useEffect(() => {
		const timer = setTimeout(() => {
			toast.remove(id)
		}, 5000)
		return () => clearTimeout(timer)
	}, [id])
	return (
		<motion.div
			id={'toast-' + id}
			layoutId={'toast-' + id}
			initial={{ opacity: 0, x: '100%' }}
			animate={{ opacity: 1, x: 0 }}
			exit={{ opacity: 0, y: -50 }}
			className={`w-[380px] rounded-lg border-l-[6px] bg-gray-100 py-2 pl-3 pr-1 shadow-lg`}
			style={{ borderLeftColor: color }}
		>
			<div className="flex items-center gap-3">
				<div className="text-2xl" style={{ color: color }}>
					{icon}
				</div>
				<div className="flex-1">
					<h4 className="line-clamp-1 text-base font-semibold">{title}</h4>
					<p className="line-clamp-1 text-sm">{message}</p>
				</div>
				<button
					className="rounded-full p-2 hover:bg-gray-200 hover:text-tertiary-1"
					onClick={() => toast.remove(id)}
				>
					<PiXBold />
				</button>
			</div>
		</motion.div>
	)
}

export default Toast
