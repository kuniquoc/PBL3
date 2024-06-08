import { useState } from 'react'
import { twMerge } from 'tailwind-merge'
import { useNavigate } from 'react-router-dom'

import { IBlogLine } from '../../interfaces/blog'
import { BlogTypeColors } from '../../styles/Styles'
import { Button } from '../../components'
import { timeAgo } from '../../utils/TimeFormatters'
import { NumberFormat } from '../../utils/Format'

const BlogItem: React.FC<{
	className?: string
	blog: IBlogLine
}> = ({ className, blog }) => {
	const navigate = useNavigate()
	const [imgLoaded, setImgLoaded] = useState(false)
	return (
		<div
			className={twMerge(
				` flex h-[200px] gap-4 overflow-hidden rounded-lg border border-borderCol-1 bg-white p-4 transition-all hover:border-primary-3 ${className}`,
			)}
		>
			<div className="flex flex-1 flex-col items-start justify-between">
				<div>
					<div className="flex items-center gap-2">
						<span
							className={`min-w-[58px] rounded px-2 py-0.5 text-center text-[11px] font-semibold uppercase text-white ${BlogTypeColors[blog.type]}`}
						>
							{blog.type}
						</span>
						<div className="text-xs">{NumberFormat(blog.views)} views</div>
						<span className="h-1 w-1 rounded-full bg-txtCol-2"></span>
						<div className="text-xs">{timeAgo(blog.createdAt)}</div>
					</div>
					<h4
						className=" my-[5px] line-clamp-1 text-xl font-bold"
						title={blog.title}
					>
						{blog.title}
					</h4>
					<p className="line-clamp-3 text-sm">{blog.introduction}</p>
				</div>
				<div className="flex items-center gap-2">
					<Button
						className="h-8 w-[120px] rounded-full bg-primary-2 font-semibold text-white hover:bg-primary-1"
						onClick={() => navigate(`/blog/${blog.id}`)}
					>
						Read full blog
					</Button>
					<img
						className="ml-2 h-8 w-8 rounded-full"
						src={blog.author.avatar}
						alt={blog.author.name + ' avatar'}
					/>
					<h4 className=" text-sm font-semibold">{blog.author.name}</h4>
				</div>
			</div>
			<div className="aspect-square h-full overflow-hidden rounded">
				{!imgLoaded && <span className="skeleton h-full w-full"></span>}
				<img
					className="h-full w-full object-cover"
					src={blog.image}
					alt={`blog-img-${blog.id}`}
					onLoad={() => setImgLoaded(true)}
				/>
			</div>
		</div>
	)
}

export default BlogItem

const LoadingBlogItem: React.FC<{ className?: string }> = ({ className }) => {
	return (
		<div
			className={twMerge(
				`flex h-[200px] gap-4 overflow-hidden rounded-lg border border-borderCol-1 bg-white p-4 ${className}`,
			)}
		>
			<div className="flex flex-1 flex-col items-start justify-between">
				<div className="w-full">
					<div className="skeleton h-[20px] w-[200px] rounded-sm"></div>
					<div className="skeleton my-2 h-[26px] w-full rounded-sm"></div>
					<div className="skeleton my-[6px] h-4 rounded-sm"></div>
					<div className="skeleton my-[6px] h-4 rounded-sm"></div>
					<div className="skeleton my-[6px] h-4 rounded-sm"></div>
				</div>

				<div className="skeleton h-8 w-[120px] rounded-sm"></div>
			</div>
			<div className="skeleton aspect-square h-full rounded"></div>
		</div>
	)
}

export { LoadingBlogItem }
