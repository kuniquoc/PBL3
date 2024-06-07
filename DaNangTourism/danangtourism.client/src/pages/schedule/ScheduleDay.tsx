import { twMerge } from 'tailwind-merge'
import {
	ScheduleDayProps,
	ScheduleDestinationProps,
} from '../../types/schedule'
import { dateDecay, dayOfWeek } from '../../utils/TimeFormatters'
import { useState } from 'react'
import { AnimatePresence, motion } from 'framer-motion'
import { Button } from '../../components'
import {
	PiArrowSquareOutBold,
	PiFloppyDiskBackFill,
	PiPenFill,
	PiTrashSimpleFill,
	PiXBold,
} from 'react-icons/pi'
import { useToast } from '../../hook/useToast'
import axios from 'axios'

const ScheduleDay: React.FC<{
	scheduleDay: ScheduleDayProps
	className?: string
	onChanged: () => void
}> = ({ scheduleDay, className = '', onChanged }) => {
	const date = dateDecay(scheduleDay.date)
	const day = dayOfWeek(scheduleDay.date)
	const numbOfDes = scheduleDay.destinations.length
	const [isHovered, setIsHovered] = useState(false)
	return (
		<div
			className={twMerge(`flex items-start gap-4 ${className}`)}
			onMouseOver={() => setIsHovered(true)}
			onMouseLeave={() => setIsHovered(false)}
		>
			<div className="flex w-[88px] flex-col items-end">
				<span className=" py-2 font-lora text-5xl font-bold">{date.day}</span>
				<span
					className={`font-semibold ${day == 'Saturday' ? ' text-orange-600' : ''} ${day == 'Sunday' ? 'text-tertiary-1' : ''}`}
				>
					{day}
				</span>
				<span className="text-sm">
					{date.month} {date.year}
				</span>
			</div>
			<motion.div
				className="flex flex-1 flex-col items-center gap-6 rounded-lg border border-borderCol-1 bg-white p-5"
				animate={
					isHovered ? { boxShadow: '2px 2px 8px 0px rgba(0, 0, 0, 0.25)' } : {}
				}
			>
				{scheduleDay.destinations.map((destination, index) => (
					<div className={`w-full`} key={index}>
						<ScheduleDestination
							className="w-full"
							destination={destination}
							onChanged={onChanged}
							date={scheduleDay.date}
						/>
						{index < numbOfDes - 1 && (
							<span className="mt-6 block h-px w-full bg-txtCol-2"></span>
						)}
					</div>
				))}
			</motion.div>
		</div>
	)
}

const ScheduleDestination: React.FC<{
	className?: string
	date: string
	destination: ScheduleDestinationProps
	onChanged: () => void
}> = ({ className = '', date, destination, onChanged }) => {
	const [editable, setEditable] = useState(false)
	const [des, setDes] = useState<ScheduleDestinationProps>(destination)
	const [isHovered, setIsHovered] = useState(false)
	const toast = useToast()
	const handleSave = async () => {
		setEditable(false)
		try {
			await axios.put(`/api/schedule/updateDestination/${des.id}`, {
				date: date,
				arrivalTime: des.arrivalTime,
				leaveTime: des.leaveTime,
				budget: des.budget,
				note: des.note,
			})
			toast.success('Success', 'Destination saved successfully')
			onChanged()
		} catch (error) {
			toast.error('Error', 'Failed to save destination')
			console.error(error)
		}
	}

	const handleCancel = () => {
		setDes(destination)
		setEditable(false)
	}

	const handleRemove = async () => {
		try {
			await axios.delete(`/api/schedule/removeDestination/${des.id}`)
			toast.success('Success', 'Destination removed successfully')
			onChanged()
		} catch (error) {
			toast.error('Error', 'Failed to remove destination')
			console.error(error)
		}
	}

	return (
		<div
			className={twMerge(
				`relative flex h-[144px] items-start gap-4 ${className}`,
			)}
			onMouseOver={() => setIsHovered(true)}
			onMouseLeave={() => setIsHovered(false)}
		>
			<div className="flex h-full w-[136px] flex-col items-center">
				<div className="relative w-full">
					<input
						type="time"
						value={des.arrivalTime}
						onChange={(event) =>
							setDes({ ...des, arrivalTime: event.target.value })
						}
						className={`w-full border-2 pb-[2px] pt-1 font-oxygenMono ${editable ? 'focus:border-2' : 'border-secondary-1 bg-[#f9f9f9]'}`}
					/>
					{!editable && (
						<div className="absolute left-0 top-0 h-full w-full"></div>
					)}
				</div>

				<span
					className={`w-[2px] flex-1 ${editable ? 'bg-[#eeeeee]' : 'bg-gradient-to-b from-secondary-1 to-tertiary-2'}`}
				></span>
				<div className="relative w-full">
					<input
						type="time"
						value={des.leaveTime}
						onChange={(event) =>
							setDes({ ...des, leaveTime: event.target.value })
						}
						className={`w-full border-2 pb-[2px] pt-1 font-oxygenMono focus:border-2  ${editable ? 'focus:border-2' : 'border-tertiary-2 bg-[#f9f9f9]'}`}
					/>
					{!editable && (
						<div className="absolute left-0 top-0 h-full w-full"></div>
					)}
				</div>
			</div>
			<div className="flex-1 text-sm">
				<div className="mb-2 flex items-center gap-4">
					<div className="w-[80px] font-semibold">Destination</div>
					<input
						type="text"
						value={des.name}
						className="relative flex-1 rounded bg-[#f9f9f9]  font-semibold focus:border-borderCol-1"
						readOnly
					/>
				</div>
				<a
					className="absolute right-1 top-0 p-1.5 hover:text-primary-1"
					href={`/destination/${des.destinationId}`}
					target="_blank"
					title="View destination"
				>
					<PiArrowSquareOutBold />
				</a>
				<div className="mb-2 flex items-center gap-4">
					<div className="w-[80px] font-semibold">Address</div>
					<input
						type="text"
						value={des.address}
						className="relative flex-1 rounded bg-[#f9f9f9]  focus:border-borderCol-1"
						readOnly
					/>
				</div>
				<div className="mb-2 flex items-center gap-4">
					<div className="w-[80px] font-semibold">Budget ($)</div>
					<input
						type="number"
						value={des.budget}
						onChange={(event) =>
							setDes({ ...des, budget: Number(event.target.value) })
						}
						className={`w-[72px] rounded ${editable ? 'focus:border focus:border-primary-2' : 'bg-[#f9f9f9] focus:border-borderCol-1'}`}
						readOnly={!editable}
					/>
				</div>
				<div className="flex items-center gap-4">
					<div className="w-[80px] font-semibold">Note</div>
					<input
						type="text"
						value={des.note}
						onChange={(event) => setDes({ ...des, note: event.target.value })}
						className={`flex-1 rounded italic text-primary-1 ${editable ? 'focus:border focus:border-primary-2' : 'bg-[#f9f9f9] focus:border-borderCol-1'}`}
						readOnly={!editable}
					/>
				</div>
			</div>
			<AnimatePresence>
				{(isHovered || editable) && (
					<motion.div
						className="absolute right-[-80px] top-0 h-full pl-9 pr-4"
						exit={{ opacity: 0, x: -8 }}
						initial={{ opacity: 0, x: -8 }}
						animate={{ opacity: 1, x: 0 }}
						transition={{ duration: 0.25 }}
					>
						{!editable ? (
							<div className="flex h-full flex-col items-center justify-center gap-8">
								<Button
									className="rounded-full bg-primary-2 p-2 text-white shadow-custom"
									onClick={() => setEditable(true)}
								>
									<PiPenFill className="text-lg" />
								</Button>
								<Button
									className="rounded-full bg-tertiary-1 p-2 text-white shadow-custom"
									onClick={handleRemove}
								>
									<PiTrashSimpleFill className="text-lg" />
								</Button>
							</div>
						) : (
							<div className="flex h-full flex-col items-center justify-center gap-8">
								<Button
									className="rounded-full bg-secondary-1 p-2 text-white shadow-custom"
									onClick={handleSave}
								>
									<PiFloppyDiskBackFill className="text-lg" />
								</Button>
								<Button
									className="rounded-full bg-tertiary-2 p-2 text-white shadow-custom"
									onClick={handleCancel}
								>
									<PiXBold className="text-lg" />
								</Button>
							</div>
						)}
					</motion.div>
				)}
			</AnimatePresence>
		</div>
	)
}

export default ScheduleDay
