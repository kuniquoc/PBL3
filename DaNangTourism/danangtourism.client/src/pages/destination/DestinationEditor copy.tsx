import { Button } from '../../components'
import { useState } from 'react'
import TextEditor from '../../components/TextEditor'
import { PiXBold } from 'react-icons/pi'

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
	const [desImgs, setDesImgs] = useState<string[]>()
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
		if (
			!desName ||
			!desAddress ||
			!desContent ||
			!desTags ||
			!desImgs ||
			!details.cost
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

		console.log({
			message: 'Post destination',
			name: desName,
			address: desAddress,
			tags: desTags,
			images: desImgs,
			cost: details.cost,
			openingTime: details.openTime,
			closingTime: details.closeTime,
			content: desContent,
		})
		alert('Destination posted successfully, check console for details')
		window.location.reload()
	}
	// useEffect(() => {
	// 	console.log(blogContent.replace(/"/g, '\\"'))
	// }, [blogContent])

	return (
		<div className="mx-auto min-h-screen xl:max-w-screen-xl">
			<form className="w-full pb-5 pt-[64px]">
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
									className="hide-arr dollar h-9 w-[72px] px-3 text-sm invalid:focus:border-tertiary-1"
									id="des-cost"
									type="number"
									placeholder="0"
									required
								/>
								<div className="absolute left-3 top-[6px] font-semibold">$</div>
							</div>
						</div>
						<div className="ml-[132px] inline-flex items-center gap-4">
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
							<label className="w-[100px] font-semibold" htmlFor="des-247">
								Open 24/7
							</label>
						</div>
						<div
							className={`ml-[132px] inline-flex items-center gap-4 ${details.is247 && 'hidden'}`}
						>
							<label className="font-semibold" htmlFor="des-open">
								Opening time
							</label>
							<input
								className="h-9 invalid:focus:border-tertiary-1"
								id="des-open"
								type="time"
								value={details.openTime}
								required={!details.is247}
								onChange={(e) =>
									setDetails({
										...details,
										openTime: e.target.value,
									})
								}
							/>
						</div>
						<div
							className={`ml-[132px] inline-flex items-center gap-4 ${details.is247 && 'hidden'}`}
						>
							<label className="font-semibold" htmlFor="des-close">
								Closing time
							</label>
							<input
								className={`h-9 invalid:focus:border-tertiary-1`}
								id="des-close"
								type="time"
								value={details.closeTime}
								min={details.openTime}
								required={!details.is247}
								onChange={(e) =>
									setDetails({
										...details,
										closeTime: e.target.value,
									})
								}
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
							required
						/>
					</div>
					<div className="flex w-full items-center gap-4">
						<label className="w-[100px] font-semibold" htmlFor="des-tags">
							Tags
						</label>
						<input
							className={`h-9 w-[260px] border-borderCol-1 px-3 text-sm ${desTags?.length === 3 && 'bg-[#f9f9f9] focus:border-tertiary-1'}`}
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
									if (desTags?.length === 3) return
									setDesTags((prevTags) => [
										...(prevTags || []),
										(e.target as HTMLInputElement).value,
									])
									;(e.target as HTMLInputElement).value = ''
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
			</form>
		</div>
	)
}

export default DestinationEditor
