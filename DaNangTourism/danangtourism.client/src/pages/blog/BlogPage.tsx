import { ChangeEvent, useEffect, useState } from 'react'
import { SearchBox, Button, Pagination, DropdownSelect } from '../../components'
import { PiPenFill } from 'react-icons/pi'
import axios from 'axios'
import { BlogLineProps } from '../../types/blog'
import BlogItem, { LoadingBlogItem } from './BlogItem'
import BlogSlider from './BlogSlider'
import { useNavigate } from 'react-router-dom'
const BlogsSortOptions = ['Sort by', 'Newest', 'Oldest', 'Popular']

const BlogPage: React.FC = () => {
	document.title = 'Blogs | Da Nang Explore'
	const navigate = useNavigate()
	const [sort, setSort] = useState(0)
	const [searchValue, setSearchValue] = useState('')
	const [blogs, setBlogs] = useState<BlogLineProps[]>()
	const [currentPage, setCurrentPage] = useState(1)
	const numbOfPages = 10
	const itemsPerPage = 5

	const getBlogs = async (page: number) => {
		try {
			setBlogs(undefined)
			const response = await axios.get(`/api/blog/blogs-${page}.json`)
			await new Promise((resolve) => setTimeout(resolve, 3000))
			setBlogs(response.data.data)
		} catch (error) {
			console.error(error)
		}
	}

	useEffect(() => {
		getBlogs(currentPage)
	}, [currentPage])

	return (
		<div className="mx-auto min-h-screen justify-center pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className=" flex w-full items-center justify-between">
				<DropdownSelect
					id="blogs-sort-options"
					className="h-9 w-[220px]"
					options={BlogsSortOptions}
					value={sort}
					onChange={(event: ChangeEvent<HTMLSelectElement>) => {
						setSort(Number(event.target.value))
					}}
				/>
				<div className="inline-flex items-center">
					<SearchBox
						className="h-9 w-[220px]"
						onChangeValue={(event) => {
							setSearchValue(event.target.value)
						}}
						onClickSearch={() => {
							console.log('Searching for:', searchValue)
						}}
					/>
					<Button
						className="ml-4 h-9 bg-secondary-1 text-white hover:bg-[#42a186]"
						onClick={() => {
							navigate('/blog/new')
						}}
					>
						<PiPenFill className="text-lg" />
						Write a blog
					</Button>
				</div>
			</div>
			<div className="mt-5 flex w-full items-start gap-4">
				<div className="flex flex-1 flex-col items-center justify-start gap-4">
					{blogs
						? blogs?.map((blog) => (
								<BlogItem key={blog.id} blog={blog} className="w-full" />
							))
						: Array.from({ length: itemsPerPage }, (_, index) => (
								<LoadingBlogItem key={index} className="w-full" />
							))}

					<Pagination
						className="mt-2 w-full justify-center"
						numbOfPages={numbOfPages}
						currentPage={currentPage}
						setCurrentPage={(numb) => {
							setCurrentPage(numb)
							console.log(numb)
						}}
					/>
				</div>
				<BlogSlider className="aspect-square w-[416px]" />
			</div>
		</div>
	)
}

export default BlogPage
