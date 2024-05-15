import { motion } from 'framer-motion'
import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
type ButtonProps = {
	children: React.ReactNode
	className?: string
	onClick: () => void
	disabled?: boolean
}
const buttonVariants = {
	hover: {
		scale: 1.03,
	},
	tap: {
		scale: 0.93,
		opacity: 0.85,
	},
}

const Button: React.FC<ButtonProps> = ({
	children,
	className,
	onClick,
	disabled,
}) => {
	return (
		<motion.button
			className={twMerge(
				`flex items-center justify-center gap-2 rounded-md px-3 py-1.5 text-sm font-medium transition-all ${className ?? ''} ${disabled ? 'cursor-not-allowed' : 'cursor-pointer'}`,
			)}
			whileTap={disabled ? {} : 'tap'}
			variants={buttonVariants}
			transition={{ duration: 0.1 }}
			onClick={onClick}
			disabled={disabled}
		>
			{children}
		</motion.button>
	)
}

type ToggleButtonProps = {
	className?: string
	onClick: () => void
	text: string
	toggledText: string
	icon?: React.ReactNode
	initToggled: boolean
	btnColor: string
}

const ToggleButton: React.FC<ToggleButtonProps> = ({
	className,
	onClick,
	text,
	toggledText,
	initToggled,
	icon,
	btnColor,
}) => {
	const [toggled, setToggled] = useState(initToggled)

	useEffect(() => {
		setToggled(initToggled)
	}, [initToggled])

	return (
		<motion.button
			className={`flex items-center justify-center gap-2 rounded-md border-2 px-3 py-1.5 text-sm font-medium text-white transition-all ${className}`}
			style={{ borderColor: btnColor }}
			whileHover="hover"
			whileTap="tap"
			variants={buttonVariants}
			title={toggled.toString()}
			animate={
				toggled
					? {
							color: btnColor,
						}
					: { backgroundColor: btnColor }
			}
			transition={{ duration: 0.1 }}
			onClick={() => {
				onClick()
				setToggled(!toggled)
			}}
		>
			{icon}
			{toggled ? toggledText : text}
		</motion.button>
	)
}

export { Button, ToggleButton }
