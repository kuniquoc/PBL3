import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
import { useToast } from '../../hook/useToast'
import { MyBlogProps } from '../../types/blog'
import axios from 'axios'
import {
	SearchBox,
	DropdownSelect,
	SortTypeButton,
	Loader,
	Pagination,
	CircleButton,
} from '../../components'
import { PiEyeFill, PiTrashSimpleFill, PiPenFill } from 'react-icons/pi'
import { NumberFormat } from '../../utils/Format'
import { toDisplayDateTime } from '../../utils/TimeFormatters'
import useConfirm from '../../hook/useConfirm'

const sortBy = [
	{
		value: 'created_at',
		label: 'Release date',
	},
	{
		value: 'title',
		label: 'Title',
	},
	{
		value: 'views',
		label: 'Views count',
	},
]

const blogStatus = [
	{ value: 'all', label: 'All' },
	{
		value: 'pending',
		label: 'Pending',
	},
	{
		value: 'published',

		label: 'Published',
	},
	{
		value: 'rejected',

		label: 'Rejected',
	},
]

const MyBlog: React.FC<{ className?: string }> = ({ className }) => {
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [status, setStatus] = useState(0)
	const [currentPage, setCurrentPage] = useState(1)
	const limit = 12
	const [total, setTotal] = useState(0)
	const toast = useToast()
	const [blogs, setBlogs] = useState<MyBlogProps[]>()
	const [loading, setLoading] = useState(true)

	const handleGetBlogs = async () => {
		setBlogs(undefined)
		setLoading(true)
		try {
			const response = await axios.get(`api/blog/myBlogs`, {
				params: {
					page: currentPage,
					limit: limit,
					sortBy: sortBy[sort.by].value,
					sortType: sort.type,
					...(status && { status: blogStatus[status].value }),
					...(searchValue && { search: searchValue }),
				},
			})
			const res = response.data.data
			setBlogs(res.items)
			setTotal(res.total)
		} catch (error) {
			toast.error('Error', 'Failed to get blogs')
			console.log(error)
		}
		setLoading(false)
	}

	useEffect(() => {
		handleGetBlogs()
	}, [currentPage, sort, status, searchValue])

	return (
		<div className={twMerge('w-full px-8 py-5', className)}>
			<div className="mb-3 flex w-full items-center justify-between">
				<div className="item-center relative flex gap-4">
					<SearchBox
						className="h-9 w-[220px]"
						onChangeValue={(event) => setSearchValue(event.target.value)}
						onClickSearch={handleGetBlogs}
					/>
					<DropdownSelect
						id="sort-by"
						className="h-9 w-[120px]"
						options={blogStatus.map((item) => item.label)}
						value={status}
						onChange={(event) => {
							setStatus(Number(event.target.value))
						}}
					/>
				</div>

				<div className="item-center relative flex gap-4">
					<DropdownSelect
						id="sort-by"
						className="h-9 w-[168px]"
						options={sortBy.map((item) => item.label)}
						value={sort.by}
						onChange={(event) => {
							setSort({
								...sort,
								by: Number(event.target.value),
							})
						}}
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
			</div>
			<div className="mb-3 flex w-full flex-col items-center border border-borderCol-1">
				{blogs && blogs.length > 0 ? (
					<BlogsTable blogs={blogs} onUpdated={handleGetBlogs} />
				) : (
					<div className="flex h-[512.4px] w-full items-center justify-center bg-gray-50">
						{loading ? (
							<Loader className="h-16 w-16" />
						) : (
							<h3 className=" text-2xl font-semibold text-txtCol-3">
								No blog found
							</h3>
						)}
					</div>
				)}
			</div>
			<div className="flex items-center justify-between">
				<div className="flex h-8 items-center rounded border border-borderCol-1 px-3 text-txtCol-2">
					{(currentPage - 1) * limit + 1}
					{' - '}
					{currentPage * limit > total ? total : currentPage * limit}
					{' of '}
					{total}
				</div>
				<Pagination
					numbOfPages={Math.ceil(total / limit)}
					currentPage={currentPage}
					setCurrentPage={setCurrentPage}
				/>
			</div>
		</div>
	)
}

const BlogsTable: React.FC<{
	blogs: MyBlogProps[]
	onUpdated: () => void
}> = ({ blogs, onUpdated }) => {
	const toast = useToast()
	const confirm = useConfirm()
	const handleDelete = async (id: number) => {
		const result = await confirm.showConfirmation(
			'Delete blog',
			'Are you sure you want to delete this blog?',
		)
		if (!result) return
		try {
			await axios.delete(`api/blog/delete/${id}`)
			toast.success('Success', 'Blog deleted')
			onUpdated()
		} catch (error) {
			toast.error('Error', 'Failed to delete blog')
			console.log(error)
		}
	}
	return (
		<table className="w-full border-spacing-2">
			<thead className="border-b border-borderCol-1">
				<tr className=" h-8 text-center [&_*]:font-semibold">
					<th className="w-[100px] pl-2">Id</th>
					<th>Title</th>
					<th className="w-16">Type</th>
					<th className="w-20">View</th>
					<th className="w-36">Created At</th>
					<th className="w-24">Status</th>
					<th className=" w-24 pr-2">Actions</th>
				</tr>
			</thead>
			<tbody className="[&>*:nth-child(odd)]:bg-gray-100 hover:[&_tr]:bg-[#64ccdc3f]">
				{blogs?.map((blog) => (
					<tr key={blog.id} className="h-10 text-center text-sm">
						<td className="pl-2">{blog.id}</td>
						<td className="line-clamp-1 text-left" title={blog.title}>
							{blog.title}
						</td>
						<td className=" capitalize">{blog.type}</td>
						<td>{NumberFormat(blog.views)}</td>
						<td>{toDisplayDateTime(blog.createdAt)}</td>
						<td
							className="capitalize"
							style={{
								color:
									blog.status === 'pending'
										? '#E7B941'
										: blog.status === 'published'
											? '#64B8DC'
											: '#E75A51',
							}}
						>
							{blog.status}
						</td>
						<td className="flex h-10 items-center justify-center gap-3 pr-2">
							<CircleButton
								className="border-secondary-1 bg-[#76C8933f] text-secondary-1"
								title="View blog"
								onClick={() => window.open(`/blog/${blog.id}`, '_blank')}
							>
								<PiEyeFill />
							</CircleButton>
							<CircleButton
								className="border-primary-2 bg-[#64B8DC3f] text-primary-2"
								onClick={() => window.open(`/blog/edit/${blog.id}`, '_blank')}
							>
								<PiPenFill />
							</CircleButton>
							<CircleButton
								className=" border-tertiary-2 bg-[#ee685e3f] text-tertiary-2"
								title="Delete blog"
								onClick={() => handleDelete(blog.id)}
							>
								<PiTrashSimpleFill />
							</CircleButton>
						</td>
					</tr>
				))}
			</tbody>
		</table>
	)
}

export default MyBlog
