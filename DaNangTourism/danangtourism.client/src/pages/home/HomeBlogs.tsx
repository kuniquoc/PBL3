import axios from 'axios'
import { useEffect, useState } from 'react'
import { PiCaretRightBold } from 'react-icons/pi'
import { twMerge } from 'tailwind-merge'
import { Button } from '../../components/Buttons'
import { useNavigate } from 'react-router-dom'
import { timeAgo } from '../../utils/TimeFormatters'
import { motion } from 'framer-motion'

type BlogType = {
	id: number
	title: string
	type: string
	image: string
	description: string
	author: string
	created_at: string
}

const HomeBlogs: React.FC<{ className?: string }> = ({ className }) => {
	const [blogs, setBlogs] = useState<BlogType[]>([])

	const getBlogs = async () => {
		try {
			const response = await axios.get('/api/blog/home-blogs.json')
			setBlogs(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}

	useEffect(() => {
		getBlogs()
	}, [])
	return (
		<div className={twMerge(`${className}`)}>
			<div className="flex h-6 w-full items-center justify-between">
				<h3 className="text-xl font-semibold leading-normal tracking-wide">
					Newest blogs
				</h3>
				<div className="flex cursor-pointer items-center justify-start gap-2 hover:text-primary-1 hover:underline">
					<h4 className="text-base font-normal leading-none">View more</h4>
					<PiCaretRightBold />
				</div>
			</div>
			<div className="mt-3 flex h-[400px] w-full items-center gap-5">
				<FirstBlog blog={blogs[0]} className="" />
				<div className="flex h-full flex-1 flex-col gap-5">
					<BlogCard blog={blogs[1]} className="flex-1" key={blogs[1]?.id} />
					<BlogCard blog={blogs[2]} className="flex-1" key={blogs[2]?.id} />
				</div>
				<div className="flex h-full flex-1 flex-col gap-5">
					<BlogCard blog={blogs[3]} className="flex-1" key={blogs[3]?.id} />
					<BlogCard blog={blogs[4]} className="flex-1" key={blogs[4]?.id} />
				</div>
			</div>
		</div>
	)
}

const FirstBlog: React.FC<{ blog: BlogType; className?: string }> = ({
	blog,
	className,
}) => {
	const navigate = useNavigate()
	const [isHover, setIsHover] = useState(false)
	return (
		<div
			className={twMerge(
				`relative h-[400px] w-[400px] overflow-hidden rounded-lg bg-gray-200 ${className}`,
			)}
		>
			<img
				className="h-full w-full object-cover"
				src={blog?.image}
				alt={blog?.title}
			/>
			<div
				className="absolute left-0 top-0 flex h-full w-full select-none flex-col items-center justify-between bg-[#00000050] p-4"
				onMouseEnter={() => setIsHover(true)}
				onMouseLeave={() => setIsHover(false)}
			>
				<div className="w-full">
					<motion.div
						className="w-full rounded p-3"
						layoutId={`header-container-${blog?.id}`}
						animate={
							isHover ? { backgroundColor: '#ffffffa4' } : { color: '#ffffff' }
						}
					>
						{isHover && (
							<motion.div
								className="flex items-center gap-2"
								layoutId={`header-${blog?.id}`}
								initial={{ opacity: 0, y: -10 }}
								animate={{ opacity: 1, y: 0 }}
								transition={{ duration: 0.3 }}
							>
								<span className="rounded bg-secondary-2 px-2 py-1 text-xs font-semibold uppercase text-white">
									{blog?.type}
								</span>
								<span className="text-xs text-txtCol-2">
									{timeAgo(blog?.created_at)}
								</span>
							</motion.div>
						)}
						<motion.h4
							className="mt-2 text-xl font-bold "
							layoutId={`blog-title-${blog?.id}`}
							animate={
								!isHover ? { textAlign: 'center' } : { textAlign: 'left' }
							}
							transition={{ duration: 0.3 }}
						>
							{blog?.title}
						</motion.h4>
					</motion.div>
					{isHover && (
						<motion.div
							className="mt-1 text-xs text-white"
							layoutId={`blog-author-${blog?.id}`}
							initial={{ opacity: 0, y: 10 }}
							animate={{ opacity: 1, y: 0 }}
							transition={{ duration: 0.3 }}
						>
							by {blog?.author}
						</motion.div>
					)}
				</div>
				<motion.div animate={isHover ? { opacity: 1 } : { opacity: 0.75 }}>
					<Button
						className="w-[120px] rounded-full border-2 border-white text-white hover:bg-[#ffffff30]"
						onClick={() => navigate(`/blog/${blog?.id}`)}
					>
						Read blog
					</Button>
				</motion.div>
			</div>
		</div>
	)
}

const BlogCard: React.FC<{ blog: BlogType; className?: string }> = ({
	blog,
	className,
}) => {
	const navigate = useNavigate()
	const [isHover, setIsHover] = useState(false)
	return (
		<div
			className={twMerge(
				`relative w-full overflow-hidden rounded-lg bg-gray-200 ${className}`,
			)}
		>
			<img src={blog?.image} alt={blog?.title} />
			<div
				className="item-center absolute left-0 top-0 flex h-full w-full select-none flex-col justify-between bg-[#00000050] p-4"
				onMouseEnter={() => setIsHover(true)}
				onMouseLeave={() => setIsHover(false)}
			>
				<motion.div
					className="my-auto text-center text-xl font-semibold text-white"
					initial={{ opacity: 0, y: 40 }}
					animate={{ opacity: 1, y: 0 }}
					transition={{ duration: 0.2 }}
					layoutId={`blog-title-${blog?.id}`}
				>
					{blog?.title}
				</motion.div>
				{isHover && (
					<motion.div
						className="flex w-full justify-center"
						initial={{ opacity: 0, y: 40 }}
						animate={{ opacity: 1, y: 0 }}
						transition={{ duration: 0.2 }}
						layoutId={`blog-button-${blog?.id}`}
					>
						<Button
							className="w-[120px] rounded-full border-2 border-white text-white hover:bg-[#ffffff30]"
							onClick={() => navigate(`/blog/${blog?.id}`)}
						>
							Read blog
						</Button>
					</motion.div>
				)}
			</div>
		</div>
	)
}

export default HomeBlogs
