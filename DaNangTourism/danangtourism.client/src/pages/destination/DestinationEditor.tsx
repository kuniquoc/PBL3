import { Button, TextEditor } from '../../components'
import { useEffect, useState } from 'react'
import { PiXBold } from 'react-icons/pi'
import { uploadToCloudinary } from '../../utils/Cloundinary'
import { useToast } from '../../hook/useToast'
import { useLocation, useParams } from 'react-router-dom'
import { DestinationEditorProps } from '../../types/destination'
import axios from 'axios'

const initDes = {
	id: 0,
	information: {
		name: '',
		localName: '',
		address: '',
		images: [],
		cost: 0,
		openTime: '',
		closeTime: '',
		tags: [],
	},
	introduction: '',
	googleMapUrl: '',
}

const DestinationEditor: React.FC = () => {
	document.title = 'New Destination | Da Nang Explore'
	const toast = useToast()
	const location = useLocation()
	const { id } = useParams()
	const [editMode, setEditMode] = useState(false)

	useEffect(() => {
		const path = location.pathname.split('/')
		if (path.includes('edit')) {
			setEditMode(true)
			handleGetDes(Number(id))
		}
	}, [location])

	const handleGetDes = async (desId: number) => {
		try {
			const response = await axios.get(`/api/destination/edit-${desId}.json`)
			setDes(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}

	const [des, setDes] = useState<DestinationEditorProps>(initDes)
	const [is247, setIs247] = useState(false)

	useEffect(() => {
		if (
			des.information.openTime === '00:00' &&
			des.information.closeTime === '23:59'
		) {
			setIs247(true)
		} else {
			setIs247(false)
		}
	}, [des.information.openTime, des.information.closeTime])

	const handleReset = () => {
		setDes(initDes)
	}

	const validate = () => {
		if (!des.information.name) {
			toast.error('Empty name', 'Please enter destination name')
			return false
		}
		if (!des.information.address) {
			toast.error('Empty address', 'Please enter destination address')
			return false
		}
		if (!des.googleMapUrl) {
			toast.error('Empty google map', 'Please enter google map URL')
			return false
		}
		if (!des.introduction) {
			toast.error('Empty content', 'Please enter destination content')
			return false
		}
		if (des.information.tags.length === 0) {
			toast.error('Empty tags', 'Please enter destination tags')
			return false
		}
		if (des.information.images.length === 0) {
			toast.error('Empty images', 'Please upload destination images')
			return false
		}
		if (!is247 && (!des.information.openTime || !des.information.closeTime)) {
			toast.error(
				'Empty time',
				'Please enter destination opening and closing time',
			)
			return false
		}
		return true
	}

	const handleSubmit = async () => {
		if (!validate()) return

		if (editMode) {
			try {
				toast.success('Update success', 'Destination updated successfully')
				console.log('Update destination', des)
			} catch (error) {
				toast.error('Update failed', 'Failed to update destination')
				console.error(error)
			}
		} else {
			try {
				toast.success('Post success', 'Destination posted successfully')
				console.log('Post destination', des)
			} catch (error) {
				toast.error('Post failed', 'Failed to post destination')
				console.error(error)
			}
		}
	}

	const [imgFile, setImgFile] = useState<File>()
	const [uploading, setUploading] = useState(false)
	const handleUpload = async () => {
		if (!imgFile) {
			toast.error('Empty image', 'Please select an image to upload')
			return
		}
		setUploading(true)
		const url = await uploadToCloudinary(imgFile)
		if (url) {
			setDes({
				...des,
				information: {
					...des.information,
					images: [...des.information.images, url],
				},
			})
			toast.success('Upload success', 'Image uploaded successfully')
		} else {
			toast.error('Upload failed', 'Failed to upload image')
		}

		setUploading(false)
	}

	return (
		<div className="mx-auto min-h-screen xl:max-w-screen-xl">
			<div className="w-full pb-5 pt-[72px]">
				<div className="flex w-full flex-col gap-5 rounded-lg border border-borderCol-1 bg-white p-10 pb-5 shadow-custom">
					<div className="w-full text-center text-xl font-bold tracking-wider">
						Create new destination
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-name">
							Name
						</label>
						<input
							className="h-9 flex-1 border-borderCol-1 px-3 text-sm invalid:focus:border-tertiary-1"
							id="des-name"
							type="text"
							placeholder="Enter destination name"
							value={des?.information.name || ''}
							onChange={(e) => {
								setDes({
									...des,
									information: {
										...des?.information,
										name: e.target.value,
									},
								})
							}}
							required
						/>
					</div>
					<div className="flex w-full items-center">
						<div className="inline-flex items-center gap-4">
							<label className="w-[100px] font-semibold" htmlFor="des-cost">
								Average cost
							</label>
							<div className="relative">
								<input
									className="hide-arr dollar h-9 w-[72px] border-borderCol-1 px-3 text-sm invalid:focus:border-tertiary-1"
									id="des-cost"
									type="number"
									placeholder="0"
									value={des?.information.cost || 0}
									onChange={(e) => {
										setDes({
											...des,
											information: {
												...des?.information,
												cost: Number(e.target.value),
											},
										})
									}}
									required
								/>
								<div className="absolute left-3 top-[6px] font-semibold">$</div>
							</div>
						</div>
						<div className="ml-[134px] inline-flex items-center gap-4">
							<input
								className="large h-6 w-6"
								id="des-247"
								type="checkbox"
								checked={is247}
								onChange={(e) => {
									setDes({
										...des,
										information: {
											...des?.information,
											openTime: e.target.checked ? '00:00' : '',
											closeTime: e.target.checked ? '23:59' : '',
										},
									})
								}}
							/>
							<label className="w-[100px] font-semibold" htmlFor="des-cost">
								Open 24/7
							</label>
						</div>
						<div
							className={`ml-[134px] inline-flex items-center gap-4 ${is247 && 'hidden'}`}
						>
							<label className="font-semibold" htmlFor="des-open">
								Opening time
							</label>
							<input
								className="h-9 invalid:focus:border-tertiary-1"
								id="des-open"
								type="time"
								value={des?.information.openTime || ''}
								onChange={(e) =>
									setDes({
										...des,
										information: {
											...des?.information,
											openTime: e.target.value,
										},
									})
								}
								required={!is247}
							/>
						</div>
						<div
							className={`ml-[134px] inline-flex items-center gap-4 ${is247 && 'hidden'}`}
						>
							<label className="font-semibold" htmlFor="des-close">
								Closing time
							</label>
							<input
								className="h-9 invalid:focus:border-tertiary-1"
								id="des-close"
								type="time"
								value={des?.information.closeTime || ''}
								onChange={(e) =>
									setDes({
										...des,
										information: {
											...des?.information,
											closeTime: e.target.value,
										},
									})
								}
								min={!is247 ? des.information.openTime : ''}
								required={!is247}
							/>
						</div>
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-address">
							Address
						</label>
						<input
							className="h-9 flex-1 border-borderCol-1 px-3 text-sm invalid:focus:border-tertiary-1"
							id="des-address"
							type="text"
							placeholder="Enter destination address"
							value={des?.information.address || ''}
							onChange={(e) => {
								setDes({
									...des,
									information: {
										...des?.information,
										address: e.target.value,
									},
								})
							}}
							required
						/>
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-map">
							Google Map
						</label>
						<input
							className="h-9 flex-1 border-borderCol-1 px-3 text-sm invalid:focus:border-tertiary-1"
							id="des-map"
							type="text"
							placeholder="Enter google map URL"
							value={des?.googleMapUrl || ''}
							onChange={(e) => {
								setDes({
									...des,
									googleMapUrl: e.target.value,
								})
							}}
							required
						/>
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-tags">
							Tags
						</label>
						<input
							className={`h-9 w-[280px] border-borderCol-1 px-3 text-sm ${des.information.tags?.length === 3 && 'bg-[#f9f9f9] focus:border-tertiary-1'}`}
							id="des-tags"
							type="text"
							placeholder={
								des.information.tags?.length === 3
									? 'Maximum tags reached'
									: 'Enter tags'
							}
							onKeyDown={(e) => {
								if (e.key === 'Enter') {
									const tagInput = e.target as HTMLInputElement
									const tag = tagInput.value.trim()
									if (tag) {
										setDes({
											...des,
											information: {
												...des?.information,
												tags: [...des.information.tags, tag],
											},
										})
										tagInput.value = ''
									}
								}
							}}
							readOnly={des.information.tags?.length === 3}
						/>
						<div className="inline-flex h-9 flex-1 items-center gap-2 overflow-x-auto">
							{des.information.tags?.map((tag, index) => (
								<div
									key={index}
									className="flex items-center gap-1 rounded-full border border-[#cccccc] py-1 pl-3 pr-1 text-xs font-semibold hover:bg-[#2898c813]"
								>
									<span className=" translate-y-[-1px]">{tag}</span>
									<button
										className="flex h-5 w-5 items-center justify-center text-xs hover:text-tertiary-1"
										onClick={() => {
											setDes({
												...des,
												information: {
													...des?.information,
													tags: des?.information.tags.filter(
														(_, i) => i !== index,
													),
												},
											})
										}}
									>
										<PiXBold />
									</button>
								</div>
							))}
						</div>
					</div>
					<div className="flex h-[120px] w-full items-start gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-imgs">
							Images
						</label>
						<div className="flex h-full w-[280px] flex-col overflow-y-auto rounded-lg border border-borderCol-1 p-2 text-sm">
							{des.information.images?.map((img, index) => (
								<div className="flex h-8 w-full items-center justify-between gap-2 rounded px-2 py-1.5 hover:bg-[#EDEDED]">
									<a
										className="line-clamp-1 w-full hover:text-primary-1 hover:underline"
										href={img}
										target="_blank"
									>
										{img.split('/').pop()}
									</a>
									<button
										className="flex h-5 w-5 items-center justify-center text-xs hover:text-tertiary-1"
										onClick={() => {
											setDes({
												...des,
												information: {
													...des?.information,
													images: des?.information.images.filter(
														(_, i) => i !== index,
													),
												},
											})
										}}
									>
										<PiXBold />
									</button>
								</div>
							))}
						</div>
						<div className="relative flex h-full w-[320px] flex-col items-end justify-between rounded-lg border border-borderCol-1 p-3 text-sm">
							<input
								className="flex w-full px-4 py-3"
								type="file"
								accept="image/*"
								onChange={(e) => {
									setImgFile(e.target.files?.[0])
								}}
							/>
							<Button
								className="h-8 w-[100px] bg-primary-2 text-white hover:bg-primary-1"
								onClick={handleUpload}
								disabled={uploading}
							>
								{uploading ? 'Uploading...' : 'Upload'}
							</Button>
						</div>
					</div>
					<div className="flex w-full items-start gap-4">
						<div className="w-[100px] font-semibold">Introduction</div>
						<TextEditor
							className="h-[600px] w-[1082px]"
							value={des.introduction || ''}
							onChange={(value) => {
								setDes({
									...des,
									introduction: value,
								})
							}}
							placeholder="Write destination introduction here..."
						/>
					</div>
					<div className="flex w-full items-center justify-between pl-[116px]">
						<Button
							className="w-[120px] border-[2px] border-tertiary-2 font-semibold text-tertiary-2 hover:bg-[#ff201017]"
							onClick={handleReset}
						>
							Reset
						</Button>
						<Button
							onClick={handleSubmit}
							className="w-[120px] bg-primary-2 text-white hover:bg-primary-1"
						>
							Post
						</Button>
					</div>
				</div>
			</div>
		</div>
	)
}

export default DestinationEditor
