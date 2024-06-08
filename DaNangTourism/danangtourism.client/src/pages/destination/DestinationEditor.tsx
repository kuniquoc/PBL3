import { useEffect, useState } from 'react'
import axios from 'axios'
import { PiXBold } from 'react-icons/pi'
import { useLocation, useNavigate, useParams } from 'react-router-dom'

import PageNotFound from '../PageNotFound'
import { Button, Loader, TextEditor } from '../../components'
import { IDesEditor } from '../../interfaces/destination'
import { uploadToCloudinary } from '../../utils/Cloudinary'
import { useToast, useConfirm } from '../../hook'

const initDes = {
	id: 0,
	name: '',
	localName: '',
	address: '',
	images: [],
	cost: 0,
	openTime: '',
	closeTime: '',
	tags: [],
	introduction: '',
	googleMapUrl: '',
}

const DestinationEditor: React.FC = () => {
	const toast = useToast()
	const location = useLocation()
	const { id } = useParams()
	const [editMode, setEditMode] = useState(false)
	const [loading, setLoading] = useState(true)
	const [invalid, setInvalid] = useState(false)
	const [des, setDes] = useState<IDesEditor>(initDes)
	const [is247, setIs247] = useState(false)
	const navigate = useNavigate()
	const confirm = useConfirm()

	useEffect(() => {
		const path = location.pathname.split('/')
		if (path.includes('edit')) {
			document.title = 'Edit Destination | Da Nang Explore'
			setEditMode(true)
			handleGetDes(Number(id))
		} else {
			document.title = 'New Destination | Da Nang Explore'
			setLoading(false)
		}
	}, [location])

	const handleGetDes = async (desId: number) => {
		setLoading(true)
		try {
			const response = await axios.get(`/api/destination/GetToUpdate/${desId}`)
			setDes(response.data.data)
		} catch (error) {
			setInvalid(true)
			toast.error('Invalid destination', 'Destination not found')
		}
		setLoading(false)
	}

	useEffect(() => {
		if (des.openTime === '00:00' && des.closeTime === '23:59') {
			setIs247(true)
		} else {
			setIs247(false)
		}
	}, [des.openTime, des.closeTime])

	const handleNegative = () => {
		if (editMode) {
			handleDelete()
		} else {
			setDes(initDes)
		}
	}

	const validate = () => {
		if (!des.name) {
			toast.error('Empty name', 'Please enter destination name')
			return false
		}
		if (!des.address) {
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
		if (des.tags.length === 0) {
			toast.error('Empty tags', 'Please enter destination tags')
			return false
		}
		if (des.images.length === 0) {
			toast.error('Empty images', 'Please upload destination images')
			return false
		}
		if (!is247 && (!des.openTime || !des.closeTime)) {
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
				const response = await axios.put(
					'/api/destination/update/' + Number(id),
					des,
				)
				if (response.status === 200) {
					toast.success('Update success', 'Destination updated successfully')
				}
			} catch (error) {
				toast.error('Update failed', 'Failed to update destination')
			}
		} else {
			try {
				const response = await axios.post('/api/destination/create', des)
				toast.success('Post success', 'Destination posted successfully')
				navigate('/destination/' + response.data.data.id)
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
				images: [...des.images, url],
			})
			toast.success('Upload success', 'Image uploaded successfully')
		} else {
			toast.error('Upload failed', 'Failed to upload image')
		}

		setUploading(false)
	}

	const handleDelete = async () => {
		const result = await confirm.showConfirmation(
			'Delete destination',
			'Are you sure you want to delete this destination? This action cannot be undone.',
		)
		if (!result) return
		try {
			const response = await axios.delete(
				'/api/destination/delete/' + Number(id),
			)
			if (response.status === 200) {
				toast.success('Delete success', 'Destination deleted successfully')
				navigate('/destination')
			}
		} catch (error) {
			toast.error('Delete failed', 'Failed to delete destination')
			console.error(error)
		}
	}

	if (loading) {
		return (
			<div className="mx-auto flex min-h-screen items-center justify-center xl:max-w-screen-xl">
				<Loader />
			</div>
		)
	}
	if (!loading && editMode && invalid) return <PageNotFound />
	return (
		<div className="mx-auto min-h-screen xl:max-w-screen-xl">
			<div className="w-full pb-5 pt-[72px]">
				<div className="flex w-full flex-col gap-5 rounded-lg border border-borderCol-1 bg-white p-10 pb-5 shadow-custom">
					<div className="w-full text-center text-xl font-bold tracking-wider">
						{editMode ? 'Edit Destination' : 'New Destination'}
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-name">
							Name
						</label>
						<input
							className="h-9 w-[440px] border-borderCol-1 px-3 text-sm invalid:focus:border-tertiary-1"
							id="des-name"
							type="text"
							placeholder="Enter destination name"
							value={des.name}
							onChange={(e) => {
								setDes({
									...des,
									name: e.target.value,
								})
							}}
							required
						/>
						<label className="ml-4 w-[106px] font-semibold" htmlFor="des-name">
							Local Name
						</label>
						<input
							className="h-9 flex-1 border-borderCol-1 px-3 text-sm invalid:focus:border-tertiary-1"
							id="des-local-name"
							type="text"
							placeholder="Enter destination local name"
							value={des.localName}
							onChange={(e) => {
								setDes({
									...des,
									localName: e.target.value,
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
									value={des.cost}
									onChange={(e) => {
										setDes({
											...des,
											cost: Number(e.target.value),
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
										openTime: e.target.checked ? '00:00' : '',
										closeTime: e.target.checked ? '23:59' : '',
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
								value={des.openTime}
								onChange={(e) =>
									setDes({
										...des,
										openTime: e.target.value,
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
								value={des.closeTime}
								onChange={(e) =>
									setDes({
										...des,
										closeTime: e.target.value,
									})
								}
								min={!is247 ? des.openTime : ''}
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
							value={des.address}
							onChange={(e) => {
								setDes({
									...des,
									address: e.target.value,
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
							value={des?.googleMapUrl}
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
							className={`h-9 w-[280px] border-borderCol-1 px-3 text-sm ${des.tags?.length === 3 && 'bg-[#f9f9f9] focus:border-tertiary-1'}`}
							id="des-tags"
							type="text"
							placeholder={
								des.tags?.length === 3 ? 'Maximum tags reached' : 'Enter tags'
							}
							onKeyDown={(e) => {
								if (e.key === 'Enter') {
									const tagInput = e.target as HTMLInputElement
									const tag = tagInput.value.trim()
									if (tag) {
										setDes({
											...des,
											tags: [...des.tags, tag],
										})
										tagInput.value = ''
									}
								}
							}}
							readOnly={des.tags?.length === 3}
						/>
						<div className="inline-flex h-9 flex-1 items-center gap-2 overflow-x-auto">
							{des.tags?.map((tag, index) => (
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
												tags: des.tags.filter((_, i) => i !== index),
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
							{des.images?.map((img, index) => (
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
												images: des.images.filter((_, i) => i !== index),
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
							onClick={handleNegative}
						>
							{editMode ? 'Delete' : 'Clear'}
						</Button>
						<Button
							onClick={handleSubmit}
							className="w-[120px] bg-primary-2 text-white hover:bg-primary-1"
						>
							{editMode ? 'Update' : 'Post'}
						</Button>
					</div>
				</div>
			</div>
		</div>
	)
}

export default DestinationEditor
