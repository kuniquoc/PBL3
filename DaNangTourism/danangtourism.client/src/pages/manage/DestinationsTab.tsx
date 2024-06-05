import { twMerge } from 'tailwind-merge'
import { DropdownSelect, Loader, Pagination, SearchBox } from '../../components'
import React, { useEffect, useState } from 'react'
import { Button, CircleButton, SortTypeButton } from '../../components/Buttons'
import { ManageDesProps } from '../../types/destination'
import { useToast } from '../../hook/useToast'
import { PiEyeFill, PiPenFill, PiTrashSimpleFill } from 'react-icons/pi'
import axios from 'axios'
import { toDisplayDateTime } from '../../utils/TimeFormatters'

const sortBy = [
	{
		value: 'created_at',
		label: 'Release date',
	},
	{
		value: 'name',
		label: 'Name',
	},
	{
		value: 'rating',
		label: 'Avg. Rating',
	},
	{
		value: 'review',
		label: 'Review count',
	},
	{
		value: 'favorite',
		label: 'Favorite count',
	},
]

const DestinationsTab: React.FC<{ className?: string }> = ({ className }) => {
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [currentPage, setCurrentPage] = useState(1)
	const limit = 12
	const [total, setTotal] = useState(0)
	const toast = useToast()
	const [destinations, setDestinations] = useState<ManageDesProps[]>()
	const [loading, setLoading] = useState(true)

	const handleGetDestinations = async () => {
		setDestinations(undefined)
		setLoading(true)
		try {
			const response = await axios.get(`api/destination/manageList`, {
				params: {
					page: currentPage,
					limit,
					sortBy: sortBy[sort.by].value,
					sortType: sort.type,
					...(searchValue && { search: searchValue }),
				},
			})
			const res = response.data.data
			setDestinations(res.items)
			setTotal(res.total)
		} catch (error: any) {
			if (error.response.status !== 404) {
				toast.error('Error', 'Failed to get destinations')
				console.log(error)
			}
		}
		setLoading(false)
	}

	useEffect(() => {
		handleGetDestinations()
	}, [currentPage, sort, searchValue])

	return (
		<div
			className={twMerge(
				'w-full rounded border border-borderCol-1 bg-white px-6 py-4',
				className,
			)}
		>
			<div className="mb-3 flex w-full items-center justify-between">
				<div className="item-center relative flex gap-4">
					<SearchBox
						className="h-9 px-4"
						onChangeValue={(event) => setSearchValue(event.target.value)}
						onClickSearch={handleGetDestinations}
					/>
					<Button
						className="h-9 bg-secondary-1 text-white hover:bg-[#42a186]"
						onClick={() => {
							window.open('/destination/new', '_blank')
						}}
					>
						<PiPenFill className="text-lg" />
						New destination
					</Button>
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
				{destinations ? (
					<DesTable
						destinations={destinations}
						onDeleted={handleGetDestinations}
					/>
				) : (
					<div className="flex h-[512.4px] w-full items-center justify-center bg-gray-50">
						{loading ? (
							<Loader className="h-16 w-16" />
						) : (
							<h3 className=" text-2xl font-semibold text-txtCol-3">
								No destination found
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

const DesTable: React.FC<{
	destinations: ManageDesProps[]
	onDeleted: () => void
}> = ({ destinations, onDeleted }) => {
	const toast = useToast()
	const handleDeleteDestination = async (id: number) => {
		try {
			await axios.delete(`api/destination/delete/${id}`)
			toast.success('Success', 'Destination deleted')
			onDeleted()
		} catch (error) {
			toast.error('Error', 'Failed to delete destination')
			console.log(error)
		}
	}
	return (
		<table className="w-full border-spacing-2">
			<thead className="border-b border-borderCol-1">
				<tr className=" h-8 text-center [&_*]:font-semibold">
					<th className=" w-[100px] pl-2">Id</th>
					<th className="">Name</th>
					<th className="w-20">Rating</th>
					<th className="w-20">Review</th>
					<th className="w-20">Favorite</th>
					<th className="w-36">Created At</th>
					<th className="w-32 pr-2">Actions</th>
				</tr>
			</thead>
			<tbody className="[&>*:nth-child(odd)]:bg-gray-100 hover:[&_tr]:bg-[#64ccdc3f]">
				{destinations?.map((des) => (
					<tr key={des.id} className="h-10 text-center text-sm">
						<td className="pl-2">{des.id}</td>
						<td className="text-left">{des.name}</td>
						<td>{des.rating.toFixed(2)}</td>
						<td>{des.review}</td>
						<td>{des.favorite}</td>
						<td>{toDisplayDateTime(des.created_at)}</td>
						<td className="flex h-10 items-center justify-center gap-3 pr-2">
							<CircleButton
								className="border-secondary-1 bg-[#76C8933f] text-secondary-1"
								onClick={() => window.open(`/destination/${des.id}`, '_blank')}
							>
								<PiEyeFill />
							</CircleButton>
							<CircleButton
								className="border-primary-2 bg-[#64B8DC3f] text-primary-2"
								onClick={() =>
									window.open(`/destination/edit/${des.id}`, '_blank')
								}
							>
								<PiPenFill />
							</CircleButton>
							<CircleButton
								className=" border-tertiary-2 bg-[#ee685e3f] text-tertiary-2"
								onClick={() => handleDeleteDestination(des.id)}
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

export default DestinationsTab
