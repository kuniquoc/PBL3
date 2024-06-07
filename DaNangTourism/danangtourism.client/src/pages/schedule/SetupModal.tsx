import axios from 'axios'
import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
import { Button, DropdownSelect, ToggleButton } from '../../components'
import { useNavigate } from 'react-router-dom'
import { useToast } from '../../hook/useToast'
import { ScheduleGeneralProps, ScheduleStatus } from '../../types/schedule'

const AddDestinationModal: React.FC<{
	scheduleId?: number
	general?: ScheduleGeneralProps
	className?: string
	onCancel: (changed: boolean) => void
}> = ({ scheduleId = 0, className = '', onCancel, general }) => {
	const [scheduleGeneral, setScheduleGeneral] = useState<ScheduleGeneralProps>({
		title: '',
		description: '',
		isPublic: false,
		status: 'Planning',
	})
	const StatusArray = ScheduleStatus.filter(
		(item) => item.status !== 'All',
	).map((item) => item.status)

	const toast = useToast()

	const navigate = useNavigate()
	const handleSubmit = () => {
		if (scheduleId === 0) {
			createSchedule()
		} else {
			updateSchedule()
		}
	}

	const validate = () => {
		if (scheduleGeneral.title === '' || scheduleGeneral.description === '') {
			toast.error('Empty field', 'Please fill in all required fields')
			return false
		}
		if (scheduleGeneral.description.length > 255) {
			toast.error(
				'Description is too long',
				'Please enter a shorter description',
			)
			return false
		}
		return true
	}
	const createSchedule = async () => {
		if (!validate()) return
		try {
			const response = await axios.post(`/api/schedule/create`, {
				title: scheduleGeneral.title,
				description: scheduleGeneral.description,
				isPublic: scheduleGeneral.isPublic,
			})
			toast.success(
				'Create schedule success',
				'You have created a new schedule',
			)
			navigate(`/schedule/${response.data.data.id}`)
		} catch (error) {
			console.error(error)
			toast.error('Create schedule failed', 'Please try again later')
		}
	}

	const updateSchedule = async () => {
		if (!validate()) return
		try {
			await axios.put(`/api/schedule/update/${scheduleId}`, {
				title: scheduleGeneral.title,
				description: scheduleGeneral.description,
				isPublic: scheduleGeneral.isPublic,
				status: StatusArray.indexOf(scheduleGeneral.status) + 1,
			})
			toast.success('Update schedule success', 'You have updated the schedule')
			onCancel(true)
		} catch (error) {
			console.error(error)
			toast.error('Update schedule failed', 'Please try again later')
		}
	}

	const handleDelete = async () => {
		try {
			await axios.delete(`/api/schedule/delete/${scheduleId}`)
			toast.success('Delete schedule success', 'You have deleted the schedule')
			navigate(`/schedule`)
		} catch (error) {
			console.error(error)
			toast.error('Delete schedule failed', 'Please try again later')
		}
	}

	useEffect(() => {
		if (general)
			setScheduleGeneral({
				title: general.title,
				description: general.description,
				isPublic: general.isPublic,
				status:
					general.status.charAt(0).toUpperCase() + general.status.slice(1),
			})
	}, [general])

	return (
		<div
			className={twMerge(
				`flex items-center justify-center bg-[#0000004b] ${className}`,
			)}
		>
			<div className="flex w-[600px] flex-col items-center gap-4 rounded-xl bg-white p-5">
				<h4 className="text-lg font-semibold">Set up schedule</h4>
				<div className="flex w-full flex-1 items-center gap-4">
					<label className="w-[92px] font-semibold">Title</label>
					<input
						className="flex-1 rounded-lg border border-borderCol-1 p-2 "
						type="text"
						value={scheduleGeneral.title}
						placeholder="Enter schedule title here"
						onChange={(e) =>
							setScheduleGeneral({ ...scheduleGeneral, title: e.target.value })
						}
						maxLength={100}
					/>
				</div>
				<div className="flex w-full flex-1 items-start gap-4">
					<label className="w-[92px] font-semibold">Description</label>
					<textarea
						className="h-[180px] flex-1 resize-none rounded-lg border border-borderCol-1 p-2"
						value={scheduleGeneral.description}
						placeholder="Enter schedule description here"
						onChange={(e) =>
							setScheduleGeneral({
								...scheduleGeneral,
								description: e.target.value,
							})
						}
						maxLength={255}
					/>
				</div>
				<div className="flex w-full items-center gap-4">
					<label className="w-[92px] font-semibold">Visibility</label>
					<ToggleButton
						onClick={() =>
							setScheduleGeneral({
								...scheduleGeneral,
								isPublic: !scheduleGeneral.isPublic,
							})
						}
						text="Change to public"
						toggledText="Change to private"
						initToggled={scheduleGeneral.isPublic}
						btnColor="#52B69A"
					></ToggleButton>
					{scheduleId !== 0 && (
						<div className="inline-flex flex-1 items-center gap-4 pl-10">
							<label className="w-[72px] font-semibold">Status</label>
							<DropdownSelect
								id="schedule-status"
								className="h-9 flex-1"
								title="status-blog"
								options={StatusArray}
								value={StatusArray.indexOf(scheduleGeneral.status)}
								onChange={(event) => {
									setScheduleGeneral({
										...scheduleGeneral,
										status: StatusArray[Number(event.target.value)],
									})
								}}
							/>
						</div>
					)}
				</div>
				<div className="mt-2 flex w-full items-center justify-between">
					{scheduleId !== 0 ? (
						<Button
							className="h-8 w-[100px] bg-tertiary-1 text-white"
							onClick={handleDelete}
						>
							Delete
						</Button>
					) : (
						<div></div>
					)}
					<div className="flex items-center gap-5">
						<Button
							className="h-8 w-[100px] border-[2px] border-tertiary-1 text-tertiary-1 hover:bg-[#e75b5125]"
							onClick={() => onCancel(false)}
						>
							Cancel
						</Button>
						<Button
							className="h-8 w-[100px] bg-primary-2 text-white hover:bg-primary-1"
							onClick={handleSubmit}
						>
							{scheduleId === 0 ? 'Create' : 'Save'}
						</Button>
					</div>
				</div>
			</div>
		</div>
	)
}

export default AddDestinationModal
