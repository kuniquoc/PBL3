import { Button, DropdownSelect, Loader } from '../../components'
import { useEffect, useState } from 'react'
import TextEditor from '../../components/TextEditor'
import { useToast } from '../../hook/useToast'
import { PiUploadBold, PiXBold } from 'react-icons/pi'
import { uploadToCloudinary } from '../../utils/Cloundinary'
import { useLocation, useNavigate, useParams } from 'react-router-dom'
import PageNotFound from '../PageNotFound'
import axios from 'axios'
import useConfirm from '../../hook/useConfirm'
const EmptyBlog = {
	title: '',
	typeIndex: 0,
	introduction: '',
	image: '',
	content: '',
}
const BlogTypeOptions = ['All', 'Places', 'Tips']

const BlogEditor: React.FC = () => {
	const location = useLocation()
	const { id } = useParams()
	const [editMode, setEditMode] = useState(false)
	const [blog, setBlog] = useState(EmptyBlog)
	const [loading, setLoading] = useState(true)
	const [currentImg, setCurrentImg] = useState<File>()
	const [imgFile, setImgFile] = useState<File>()
	const [invalid, setInvalid] = useState(false)
	const toast = useToast()
	const confirm = useConfirm()
	const navigate = useNavigate()

	useEffect(() => {
		const path = location.pathname.split('/')
		if (path.includes('edit')) {
			document.title = 'Edit Blog | Da Nang Explore'
			setEditMode(true)
			getBlog()
		} else {
			document.title = 'New Blog | Da Nang Explore'
			setLoading(false)
		}
	}, [location])

	const getBlog = async () => {
		setLoading(true)
		try {
			const response = await axios.get(`/api/blog/GetToUpdate/${id}`)
			setBlog(response.data.data)
			await getImageBlob(response.data.data.image)
			setLoading(false)
		} catch (error) {
			console.log(error)
			setInvalid(true)
			setLoading(false)
		}
	}
	const getImageBlob = async (url: string) => {
		const response = await fetch(url)
		const blob = await response.blob()
		const file = new File([blob], 'current-thumbnail.png', {
			type: 'image/png',
		})
		setImgFile(file)
		setCurrentImg(file)
	}

	const validate = () => {
		if (!blog.title) {
			toast.error('Title is required', 'Please enter a title for your blog')
			return false
		}
		if (!blog.introduction) {
			toast.error(
				'Introduction is required',
				'Please enter a introduction for your blog',
			)
			return false
		}
		if (!blog.content) {
			toast.error('Content is required', 'Please enter a content for your blog')
			return false
		}
		if (!imgFile && !editMode) {
			toast.error(
				'Thumbnail is required',
				'Please choose a thumbnail for your blog',
			)
			return false
		}
		return true
	}

	const handleUploadImage = async () => {
		if (!imgFile) {
			toast.error('Empty image', 'Please select an image to upload')
			return
		}
		toast.info('Uploading image', 'Please wait while we upload your image')
		const url = await uploadToCloudinary(imgFile)
		if (url) {
			setBlog({ ...blog, image: url })
			toast.success(
				'Upload image success',
				'Your image has been uploaded successfully',
			)
		} else {
			toast.error(
				'Upload image failed',
				'There was an error while uploading your image',
			)
		}
	}

	const createBlog = async () => {
		try {
			const response = await axios.post('/api/blog/create', blog)
			toast.success(
				'Request posting blog success',
				'Your blog posting request has been sent successfully, please wait for admin to approve.',
			)
			navigate(`/blog/${response.data.data.id}`)
		} catch (error) {
			console.log(error)
			toast.error(
				'Request posting blog failed',
				'There was an error while sending your blog posting request',
			)
		}
	}

	const updateBlog = async () => {
		try {
			await axios.put(`/api/blog/update/${id}`, blog)
			toast.success(
				'Update blog success',
				'Your blog has been updated successfully',
			)
			navigate(`/blog/${id}`)
		} catch (error) {
			console.log(error)
			toast.error(
				'Update blog failed',
				'There was an error while updating your blog',
			)
		}
	}

	const deleteBlog = async () => {
		const result = await confirm.showConfirmation(
			'Delete blog',
			'Are you sure you want to delete this blog? This action cannot be undone.',
		)
		if (!result) return
		try {
			await axios.delete(`/api/blog/delete/${id}`)
			toast.success(
				'Delete blog success',
				'Your blog has been deleted successfully',
			)
		} catch (error) {
			console.log(error)
			toast.error(
				'Delete blog failed',
				'There was an error while deleting your blog',
			)
		}
	}

	const handleSubmit = async () => {
		if (!validate()) return
		if (currentImg !== imgFile) await handleUploadImage()
		if (!editMode) {
			await createBlog()
		} else {
			await updateBlog()
		}
	}

	const handleNegative = () => {
		if (editMode) {
			deleteBlog()
		} else {
			setBlog(EmptyBlog)
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
						Write new blog
					</div>
					<div className="flex w-full items-center gap-4">
						<div className="w-[100px] font-semibold">Title</div>
						<input
							className="h-9 flex-1 border-borderCol-1 px-3 text-sm"
							id="blog-title"
							type="text"
							placeholder="Enter your blog title"
							value={blog.title}
							onChange={(event) => {
								setBlog({ ...blog, title: event.target.value })
							}}
						/>
					</div>
					<div className="flex w-full items-center gap-4">
						<div className="w-[100px] font-semibold" id="blog-type">
							Type
						</div>
						<DropdownSelect
							id="blog-type"
							className="h-9 w-[140px] border-borderCol-1 text-sm"
							options={BlogTypeOptions}
							value={blog.typeIndex}
							onChange={(event) => {
								setBlog({ ...blog, typeIndex: Number(event.target.value) })
							}}
						/>
						<div className="ml-10 w-[100px] font-semibold" id="blog-type">
							Thumbnail
						</div>
						<label className="flex h-9 w-[152px] cursor-pointer items-center justify-center gap-2 rounded border border-borderCol-1 text-sm transition-all hover:border-primary-1">
							<input
								className="hidden"
								type="file"
								accept="image/*"
								onChange={(e) => {
									setImgFile(e.target.files?.[0])
								}}
							/>
							<PiUploadBold className="text-base" />
							Choose Image
						</label>
						{imgFile && (
							<div className="flex flex-1 items-center justify-start gap-2">
								<img
									src={URL.createObjectURL(imgFile)}
									alt="thumbnail"
									className="ml-2 h-9 w-9 rounded border border-borderCol-1 object-cover"
								/>
								<p className="line-clamp-1 text-sm text-txtCol-2">
									{imgFile.name}
								</p>
								<button
									className="h-9 w-9 text-base hover:text-tertiary-1"
									onClick={() => {
										setImgFile(undefined)
									}}
								>
									<PiXBold />
								</button>
							</div>
						)}
					</div>
					<div className="flex w-full items-start gap-4">
						<div className="w-[100px] border-borderCol-1 font-semibold">
							Introduction
						</div>
						<textarea
							className="h-[78px] flex-1 resize-none px-3 py-2 text-sm"
							id="blog-title"
							placeholder="Write a introduction for your blog"
							value={blog.introduction}
							onChange={(event) => {
								setBlog({ ...blog, introduction: event.target.value })
							}}
						/>
					</div>
					<div className="flex w-full items-start gap-4">
						<div className="w-[100px] font-semibold">Content</div>
						<TextEditor
							className="h-[600px] w-[1082px]"
							value={blog.content}
							onChange={(value) => {
								setBlog({ ...blog, content: value })
							}}
							placeholder="Write your blog content here..."
						/>
					</div>
					<div className="flex w-full items-center justify-between pl-[116px]">
						<Button
							className="w-[120px] border-[2px] border-tertiary-2 font-semibold text-tertiary-2 hover:bg-[#ff201017]"
							onClick={handleNegative}
						>
							{editMode ? 'Delete' : 'Reset'}
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

export default BlogEditor
