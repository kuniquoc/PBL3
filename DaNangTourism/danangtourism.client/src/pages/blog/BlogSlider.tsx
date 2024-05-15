import { twMerge } from 'tailwind-merge'
import { BlogType } from '../../types/blog.types'
import { useEffect, useState } from 'react'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import { timeAgo } from '../../utils/TimeFormatters'
import { Button, SliderNav } from '../../components'
import { AnimatePresence, motion } from 'framer-motion'
import { BlogTypeColors } from '../../styles/Styles'

const BlogSlider: React.FC<{
	className?: string
}> = ({ className }) => {
	const [blogs, setBlogs] = useState<BlogType[]>()
	const [currentIndex, setCurrentIndex] = useState(0)
	const navigate = useNavigate()
	useEffect(() => {
		getBlogs()
	}, [])

	const getBlogs = async () => {
		setBlogs(undefined)
		try {
			const response = await axios.get('/api/blog/blogs-recommend.json')
			await new Promise((resolve) => setTimeout(resolve, 3000))
			setBlogs(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}

	useEffect(() => {
		if (blogs) {
			const interval = setInterval(() => {
				setCurrentIndex((prev) => (prev + 1) % blogs.length)
			}, 5000)
			return () => clearInterval(interval)
		}
	}, [currentIndex, blogs])

	if (!blogs)
		return <div className={twMerge(`skeleton rounded-lg ${className}`)}></div>
	return (
		<div
			className={twMerge(`relative overflow-hidden rounded-lg ${className}`)}
		>
			<AnimatePresence>
				<motion.img
					className="h-full w-full object-cover"
					key={blogs[currentIndex].id}
					src={blogs[currentIndex].image}
					alt={`blog-img-${blogs[currentIndex].id}`}
					initial={{ opacity: 0 }}
					animate={{ opacity: 1 }}
					exit={{
						zIndex: -1,
						position: 'absolute',
						opacity: 0,
					}}
					transition={{ duration: 1 }}
				/>
			</AnimatePresence>
			<div className="absolute left-0 top-0 flex h-full w-full select-none flex-col items-center justify-between bg-[#00000050] p-4">
				<div className="w-full">
					<div className="mb-1 w-full text-right text-xs text-white">
						Editor's Pick
					</div>
					<div className="w-full rounded bg-[#ffffffa4] p-3">
						<div className="flex items-center gap-2">
							<span
								className={`min-w-[58px] rounded px-2 py-1 text-center text-xs font-semibold uppercase text-white ${BlogTypeColors[blogs[currentIndex].type]}`}
							>
								{blogs[currentIndex]?.type}
							</span>
							<span className="text-xs text-txtCol-2">
								{timeAgo(blogs[currentIndex]?.created_at)}
							</span>
						</div>
						<h4 className="mt-2 text-xl font-bold ">
							{blogs[currentIndex]?.title}
						</h4>
					</div>
					<div className="mt-1 text-xs text-white">
						by {blogs[currentIndex]?.author}
					</div>
				</div>
				<div className="flex w-full items-end justify-between">
					<SliderNav
						count={blogs.length}
						current={currentIndex}
						onClick={(index) => setCurrentIndex(index)}
						activeNodeWidth={40}
					></SliderNav>
					<Button
						className="w-[120px] rounded-full border-2 border-white text-white hover:bg-[#ffffff30]"
						onClick={() => navigate(`/blog/${blogs[currentIndex]?.id}`)}
					>
						Read blog
					</Button>
				</div>
			</div>
		</div>
	)
}

export default BlogSlider
