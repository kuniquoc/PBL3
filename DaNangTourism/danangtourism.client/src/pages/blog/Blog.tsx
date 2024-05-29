import { useEffect, useState } from 'react'
import { BlogDetailType } from '../../types/blog'
import { useParams } from 'react-router-dom'
import axios from 'axios'
import { Button, Loader } from '../../components'
import PageNotFound from '../PageNotFound'
import { BlogTypeColors } from '../../styles/Styles'
import { timeAgo } from '../../utils/TimeFormatters'
import { PiShareFatFill } from 'react-icons/pi'

const Blog: React.FC = () => {
	const [blog, setBlog] = useState<BlogDetailType | undefined>(undefined)
	const [loading, setLoading] = useState(true)
	const { id } = useParams()

	const getBlog = async (id: string) => {
		try {
			setBlog(undefined)
			const response = await axios.get(`/api/blog/id-${id}.json`)
			// simulate delay
			await new Promise((resolve) => setTimeout(resolve, 2500))
			setBlog(response.data.data)
			console.log(response.data.data)
		} catch (error) {
			console.error(error)
		}
		setLoading(false)
	}

	useEffect(() => {
		getBlog(id ?? '')
	}, [id])

	useEffect(() => {
		document.title = (blog?.title ?? 'Blogs') + ' | Danang Tourism'
	}, [blog])

	if (loading)
		return (
			<div className="mx-auto mt-12 flex min-h-screen items-center justify-center xl:max-w-screen-xl">
				<Loader />
			</div>
		)
	else if (!blog) return <PageNotFound />
	return (
		<div className="mx-auto min-h-screen xl:max-w-screen-xl">
			<div className="w-full pb-5 pt-[72px] text-txtCol-1 ">
				<div className="mx-auto flex w-[800px] flex-col items-center">
					<span
						className={`min-w-[72px] rounded px-2 py-1 text-center text-sm font-semibold uppercase text-white ${BlogTypeColors[blog.type]}`}
					>
						{blog.type}
					</span>
					<h1 className="mt-2 text-center text-4xl font-bold">{blog.title}</h1>
					<div className="mt-3 flex items-center gap-3">
						<div className="text-sm">{timeAgo(blog.createdAt)}</div>
						<span className="h-1 w-1 rounded-full bg-txtCol-2"></span>
						<img
							className=" h- w-6 rounded-full object-cover"
							src={blog.author.avatar}
							alt={blog.author.name}
						></img>
						<div className="text-sm">by {blog.author.name}</div>
					</div>
				</div>
				<div className="relative mt-10 w-full rounded-lg bg-white px-20 py-10 shadow-[0_4px_20px_0px_rgba(0,0,0,0.25)]">
					<div
						className="ql-editor flex w-full flex-col gap-1"
						dangerouslySetInnerHTML={{ __html: blog.content }}
					></div>
					<div className="absolute right-10 top-0 translate-y-[-50%]">
						<Button
							className="bg-primary-2 text-white shadow-[0_4px_20px_0px_rgba(0,0,0,0.25)] hover:bg-primary-1"
							onClick={() => {
								console.log('Share')
							}}
						>
							<PiShareFatFill />
							Share
						</Button>
					</div>
				</div>
			</div>
		</div>
	)
}

export default Blog
