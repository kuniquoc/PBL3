import { motion } from 'framer-motion'
import { variantsDefault, variantsY } from '../styles/variants'
import { twMerge } from 'tailwind-merge'
import { Button } from './Buttons'
import { useEffect } from 'react'

interface ConfirmDialogProps {
	className?: string
	title: string
	message: string
	onOk: () => void
	onCancel: () => void
}

const ConfirmDialog: React.FC<ConfirmDialogProps> = (props) => {
	useEffect(() => {
		const handleEsc = (e: KeyboardEvent) => {
			if (e.key === 'Escape') {
				props.onCancel()
			} else if (e.key === 'Enter') {
				e.preventDefault()
				props.onOk()
			}
		}
		window.addEventListener('keydown', handleEsc)
		return () => window.removeEventListener('keydown', handleEsc)
	}, [])

	return (
		<motion.div
			className={twMerge(
				'fixed left-0 top-0 z-20 flex h-screen w-screen items-center justify-center bg-[#0000004b]',
				props.className,
			)}
			onMouseDown={(e) => {
				if (e.target === e.currentTarget) {
					props.onCancel()
				}
			}}
			variants={variantsDefault}
			initial="hidden"
			animate="visible"
			exit="hidden"
		>
			<motion.div
				className="flex w-[560px] items-center p-5"
				variants={variantsY}
				initial="top"
				animate="visible"
				exit="bottom"
				custom={50}
			>
				<div className="flex w-[520px] flex-col items-center overflow-hidden rounded-xl bg-white">
					<div className="w-full bg-gray-100 px-14 py-4 text-center text-xl font-semibold">
						{props.title}
					</div>
					<div className="flex min-h-[180px] w-full items-center justify-center border-y border-borderCol-1 px-14 py-10 text-center">
						{props.message}
					</div>
					<div className="flex w-full justify-between px-14 py-4">
						<Button
							className=" w-[120px] border-2 border-tertiary-1 text-tertiary-1 hover:bg-[#e75b5121]"
							onClick={props.onCancel}
						>
							Cancel
						</Button>
						<Button
							className=" w-[120px] bg-primary-2 text-white hover:bg-primary-1"
							onClick={props.onOk}
						>
							Confirm
						</Button>
					</div>
				</div>
			</motion.div>
		</motion.div>
	)
}

export default ConfirmDialog
