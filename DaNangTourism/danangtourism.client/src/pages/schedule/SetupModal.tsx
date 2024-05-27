import axios from 'axios'
import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
import { Button, DropdownSelect, ToggleButton } from '../../components'
import { useNavigate } from 'react-router-dom'

const StatusArray = ['Planning', 'Ongoing', 'Completed', 'Cancelled']

const AddDestinationModal: React.FC<{
	scheduleId?: number
	className?: string
	onCancel: () => void
}> = ({ scheduleId = 0, className = '', onCancel }) => {
	const [scheduleGeneral, setScheduleGeneral] = useState({
		title: '',
		description: '',
		isPublic: false,
		status: 'Planing',
	})
	const getScheduleGeneral = async (id: number) => {
		try {
			const response = await axios.get(`/api/schedule/id-${id}.json`)
			setScheduleGeneral(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}
	const navigate = useNavigate()
	const handleSave = (id: number) => {
		if (id === 0) {
			handleCreate()
		} else {
			// Update schedule
			window.location.reload()
		}
	}

	const handleCreate = () => {
		if (scheduleGeneral.title === '' || scheduleGeneral.description === '') {
			alert('Please fill in all fields')
			return
		}
		console.log('Create new schedule:', scheduleGeneral)
		navigate(`/schedule/30000001`)
	}

	useEffect(() => {
		if (scheduleId !== 0) getScheduleGeneral(scheduleId)
	}, [scheduleId])

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
							onClick={() => {
								const isDelete = window.confirm(
									'Are you sure you want to delete this schedule?',
								)
								if (isDelete) {
									console.log('Delete schedule:', scheduleId)
									window.location.reload()
								}
							}}
						>
							Delete
						</Button>
					) : (
						<div></div>
					)}
					<div className="flex items-center gap-5">
						<Button
							className="h-8 w-[100px] border-[2px] border-tertiary-1 text-tertiary-1 hover:bg-[#e75b5125]"
							onClick={onCancel}
						>
							Cancel
						</Button>
						<Button
							className="h-8 w-[100px] bg-primary-2 text-white hover:bg-primary-1"
							onClick={() => handleSave(scheduleId)}
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
