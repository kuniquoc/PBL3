import { Button, DropdownSelect } from '../../components'
import { useEffect, useState } from 'react'
import TextEditor from '../../components/TextEditor'
import { useToast } from '../../hook/useToast'
import { PiUploadBold, PiXBold } from 'react-icons/pi'
import { uploadToCloudinary } from '../../utils/Cloundinary'

const EmptyBlog = {
	title: '',
	typeIndex: 0,
	introduction: '',
	image: '',
	content: '',
}
const BlogTypeOptions = ['All', 'Places', 'Tips']

const BlogEditor: React.FC = () => {
	document.title = 'New Blog | Danang Tourism'
	const [blog, setBlog] = useState(EmptyBlog)
	const [imgFile, setImgFile] = useState<File>()
	const toast = useToast()
	const handlePostBlog = async () => {
		if (!blog.title || !blog.introduction || !blog.content) {
			toast.error('Error', 'Please fill all fields to post blog')
			return
		}
		if (!imgFile) {
			toast.error('Error', 'Please upload an image to post blog')
			return
		}
		toast.info('Uploading', 'Please wait while we upload your blog...')
		blog.image = await uploadToCloudinary(imgFile)
		console.log(blog)
		toast.success('Success', 'Blog posted successfully')
	}

	const handleReset = () => {
		setBlog(EmptyBlog)
	}

	useEffect(() => {
		console.log(blog.content.replace(/"/g, '\\"'))
	}, [blog.content])

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
							onClick={handleReset}
						>
							Reset
						</Button>
						<Button
							onClick={handlePostBlog}
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

export default BlogEditor
