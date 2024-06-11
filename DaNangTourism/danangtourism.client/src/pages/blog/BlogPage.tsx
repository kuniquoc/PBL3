import { useEffect, useState } from 'react'
import { PiPenFill } from 'react-icons/pi'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import {
	SearchBox,
	Button,
	Pagination,
	DropdownSelect,
	SortTypeButton,
} from '../../components'
import BlogSlider from './BlogSlider'
import BlogItem, { LoadingBlogItem } from './BlogItem'
import { IBlogLine } from '../../interfaces/blog'
import { useToast, useUser } from '../../hook'
import noItemImg from '../../assets/no-item.png'

const sortBy = [
	{ value: 'created_at', label: 'Created Time' },
	{ value: 'title', label: 'Title' },
	{ value: 'views', label: 'Views' },
]

const BlogPage: React.FC = () => {
	document.title = 'Blogs | Danang Tourism'
	const navigate = useNavigate()
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [searchValue, setSearchValue] = useState('')
	const [blogs, setBlogs] = useState<IBlogLine[]>()
	const [loading, setLoading] = useState(true)
	const [currentPage, setCurrentPage] = useState(1)
	const [numbOfPages, setNumbOfPages] = useState(1)
	const itemsPerPage = 5

	const { user } = useUser()
	const toast = useToast()
	const getBlogs = async () => {
		setLoading(true)
		try {
			setBlogs(undefined)
			const response = await axios.get(`/api/blog/list`, {
				params: {
					page: currentPage,
					limit: itemsPerPage,
					sortBy: sortBy[sort.by].value,
					sortType: sort.type,
					...(searchValue && { search: searchValue }),
				},
			})
			const data = response.data.data
			setBlogs(data.items)
			setNumbOfPages(Math.ceil(data.total / itemsPerPage))
		} catch (error) {
			console.error(error)
		}
		setLoading(false)
	}

	const handleOpenCreateBlogModal = () => {
		if (!user || user.id === 0) {
			toast.info('Login required', 'Please login to write a blog')
			return
		}
		navigate('/blog/new')
	}

	useEffect(() => {
		getBlogs()
	}, [currentPage, sort])

	return (
		<div className="mx-auto min-h-screen justify-center pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className=" flex w-full items-center justify-between">
				<div className="flex gap-4">
					<DropdownSelect
						id="blogs-sort-options"
						className="h-9 w-[220px]"
						options={sortBy.map((item) => item.label)}
						value={sort.by}
						onChange={(e) => setSort({ ...sort, by: Number(e.target.value) })}
					/>
					<SortTypeButton
						id="sort-type"
						className="h-9 w-9"
						value={sort.type}
						onClick={() => {
							setSort({
								...sort,
								type: sort.type === 'asc' ? 'desc' : 'asc',
							})
						}}
					/>
				</div>
				<div className="inline-flex items-center">
					<SearchBox
						className="h-9 w-[270px]"
						onChangeValue={(event) => {
							setSearchValue(event.target.value)
						}}
						onClickSearch={getBlogs}
					/>
					<Button
						className="ml-4 h-9 bg-secondary-1 text-white hover:bg-[#42a186]"
						onClick={handleOpenCreateBlogModal}
					>
						<PiPenFill className="text-lg" />
						Write a blog
					</Button>
				</div>
			</div>
			<div className="mt-5 flex w-full items-start gap-4">
				<div className="flex flex-1 flex-col items-center justify-start gap-4">
					{blogs ? (
						blogs?.map((blog) => (
							<BlogItem key={blog.id} blog={blog} className="w-full" />
						))
					) : loading ? (
						Array.from({ length: itemsPerPage }, (_, index) => (
							<LoadingBlogItem key={index} className="w-full" />
						))
					) : (
						<div className="flex h-[400px] w-full flex-col items-center justify-center gap-5 pr-16">
							<img className="h-[280px]" src={noItemImg} alt="No item found" />
							<p className="text-2xl font-semibold tracking-wide text-txtCol-3">
								No blog found
							</p>
						</div>
					)}

					{blogs && (
						<Pagination
							className="mt-2 w-full justify-center"
							numbOfPages={numbOfPages}
							currentPage={currentPage}
							setCurrentPage={(numb) => {
								setCurrentPage(numb)
							}}
						/>
					)}
				</div>
				<BlogSlider className="aspect-square w-[416px]" />
			</div>
		</div>
	)
}

export default BlogPage
