import { Button } from '../../components'
import { useState } from 'react'
import TextEditor from '../../components/TextEditor'
import { PiXBold } from 'react-icons/pi'
import { uploadToCloudinary } from '../../utils/Cloundinary'

const DestinationEditor: React.FC = () => {
	document.title = 'New Destination | Da Nang Explore'

	const [desName, setDesName] = useState('')
	const [desAddress, setDesAddress] = useState('')
	const [details, setDetails] = useState({
		cost: 0,
		is247: false,
		openTime: '',
		closeTime: '',
	})
	const [desTags, setDesTags] = useState<string[]>()
	const [desImgs, setDesImgs] = useState<string[]>([
		'http://res.cloudinary.com/dxhuysuy5/image/upload/v1715501230/dne/tumblr_e5826c781ae3ed2e7e3d4f6ec02b89ae_b0551d49_1280_nxrnxg.jpg',
	])
	const [desContent, setDesContent] = useState('')

	const handleReset = () => {
		setDesName('')
		setDesAddress('')
		setDetails({
			cost: 0,
			is247: false,
			openTime: '',
			closeTime: '',
		})
		setDesTags([])
		setDesImgs([])
		setDesContent('')
	}

	const handlePostDestination = async () => {
		console.log({
			message: 'Post destination',
			name: desName,
			address: desAddress,
			tags: desTags,
			images: desImgs,
			details: details,
			content: desContent,
		})
		if (
			!desName ||
			!desAddress ||
			!desTags ||
			!details.cost ||
			!desImgs ||
			!desContent
		) {
			alert('Please fill in all fields')
			return
		}
		if (!details.is247 && (!details.openTime || !details.closeTime)) {
			alert('Tick open 24/7 or fill in open and close time')
			return
		}

		if (details.is247) {
			details.openTime = '00:00'
			details.closeTime = '23:59'
		}

		alert('Destination posted successfully, check console for details')
		window.location.reload()
	}
	const [imgFile, setImgFile] = useState<File>()
	const [uploading, setUploading] = useState(false)
	const handleUpload = async () => {
		if (!imgFile) return
		setUploading(true)
		const url = await uploadToCloudinary(imgFile)
		setDesImgs((prevImgs) => [...(prevImgs || []), url])
		setUploading(false)
	}
	// useEffect(() => {
	// 	console.log(blogContent.replace(/"/g, '\\"'))
	// }, [blogContent])

	return (
		<div className="mx-auto min-h-screen xl:max-w-screen-xl">
			<div className="w-full pb-5 pt-[64px]">
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
							value={desName}
							onChange={(e) => setDesName(e.target.value)}
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
									value={details.cost}
									onChange={(e) => {
										setDetails({
											...details,
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
								checked={details.is247}
								onChange={(e) => {
									setDetails({
										...details,
										is247: e.target.checked,
										openTime: '',
										closeTime: '',
									})
								}}
							/>
							<label className="w-[100px] font-semibold" htmlFor="des-cost">
								Open 24/7
							</label>
						</div>
						<div
							className={`ml-[134px] inline-flex items-center gap-4 ${details.is247 && 'hidden'}`}
						>
							<label className="font-semibold" htmlFor="des-open">
								Opening time
							</label>
							<input
								className="h-9 invalid:focus:border-tertiary-1"
								id="des-open"
								type="time"
								value={details.openTime}
								onChange={(e) =>
									setDetails({
										...details,
										openTime: e.target.value,
									})
								}
								required={!details.is247}
							/>
						</div>
						<div
							className={`ml-[134px] inline-flex items-center gap-4 ${details.is247 && 'hidden'}`}
						>
							<label className="font-semibold" htmlFor="des-close">
								Closing time
							</label>
							<input
								className="h-9 invalid:focus:border-tertiary-1"
								id="des-close"
								type="time"
								value={details.closeTime}
								onChange={(e) =>
									setDetails({
										...details,
										closeTime: e.target.value,
									})
								}
								min={!details.is247 ? details.openTime : ''}
								required={!details.is247}
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
							value={desAddress}
							onChange={(e) => setDesAddress(e.target.value)}
							required
						/>
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-tags">
							Tags
						</label>
						<input
							className={`h-9 w-[280px] border-borderCol-1 px-3 text-sm ${desTags?.length === 3 && 'bg-[#f9f9f9] focus:border-tertiary-1'}`}
							id="des-tags"
							type="text"
							placeholder={
								desTags?.length
									? desTags.length < 3
										? 'Enter tags (press Enter to add)'
										: 'Maximum 3 tags allowed'
									: 'Enter tags (press Enter to add)'
							}
							onKeyDown={(e) => {
								if (e.key === 'Enter') {
									const tagInput = e.target as HTMLInputElement
									const tag = tagInput.value.trim()
									if (tag) {
										setDesTags((prevTags) => [...(prevTags || []), tag])
										tagInput.value = ''
									}
								}
							}}
							readOnly={desTags?.length === 3}
						/>
						<div className="inline-flex h-9 flex-1 items-center gap-2 overflow-x-auto">
							{desTags?.map((tag, index) => (
								<div
									key={index}
									className="flex items-center gap-1 rounded-full border border-[#cccccc] py-1 pl-3 pr-1 text-xs font-semibold hover:bg-[#2898c813]"
								>
									<span className=" translate-y-[-1px]">{tag}</span>
									<button
										className="flex h-5 w-5 items-center justify-center text-xs hover:text-tertiary-1"
										onClick={() => {
											setDesTags((prevTags) =>
												prevTags?.filter((_, i) => i !== index),
											)
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
							{desImgs?.map((img, index) => (
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
											setDesImgs((prevImgs) =>
												prevImgs?.filter((_, i) => i !== index),
											)
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
						<div className="w-[100px] font-semibold">Content</div>
						<TextEditor
							className="h-[600px] w-[1082px]"
							value={desContent}
							onChange={(value) => {
								setDesContent(value)
							}}
							placeholder="Write your blog content here..."
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
							onClick={handlePostDestination}
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
