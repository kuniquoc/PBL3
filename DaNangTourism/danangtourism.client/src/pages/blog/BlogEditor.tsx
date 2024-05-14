import { Button, DropdownSelect } from '../../components'
import { useEffect, useState } from 'react'
import TextEditor from '../../components/TextEditor'

const BlogTypeOptions = ['All', 'Places', 'Tips']
const BlogEditor: React.FC = () => {
	document.title = 'New Blog | Da Nang Explore'
	const [blogTypeIndex, setBlogTypeIndex] = useState(0)
	const [blogContent, setBlogContent] = useState('')
	const [blogTitle, setBlogTitle] = useState('')
	const [blogIntroduction, setBlogIntroduction] = useState('')

	const handlePostBlog = async () => {
		if (!blogTitle || !blogIntroduction || !blogContent) {
			alert('Please fill in all fields')
			return
		}
		console.log({
			message: 'Post blog',
			title: blogTitle,
			type: BlogTypeOptions[blogTypeIndex],
			introduction: blogIntroduction,
			content: blogContent,
		})
		alert('Blog posted successfully, check console for details')
		window.location.reload()
	}

	const handleReset = () => {
		setBlogTitle('')
		setBlogTypeIndex(0)
		setBlogIntroduction('')
		setBlogContent('')
	}

	useEffect(() => {
		console.log(blogContent.replace(/"/g, '\\"'))
	}, [blogContent])

	return (
		<div className="mx-auto min-h-screen xl:max-w-screen-xl">
			<div className="w-full pb-5 pt-[64px]">
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
							value={blogTitle}
							onChange={(event) => {
								setBlogTitle(event.target.value)
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
							value={blogTypeIndex}
							onChange={(event) => {
								setBlogTypeIndex(Number(event.target.value))
							}}
						/>
					</div>
					<div className="flex w-full items-start gap-4">
						<div className="w-[100px] border-borderCol-1 font-semibold">
							Introduction
						</div>
						<textarea
							className="h-[78px] flex-1 resize-none px-3 py-2 text-sm"
							id="blog-title"
							placeholder="Write a introduction for your blog"
							value={blogIntroduction}
							onChange={(event) => {
								setBlogIntroduction(event.target.value)
							}}
						/>
					</div>
					<div className="flex w-full items-start gap-4">
						<div className="w-[100px] font-semibold">Content</div>
						<TextEditor
							className="h-[600px] w-[1082px]"
							value={blogContent}
							onChange={(value) => {
								setBlogContent(value)
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
