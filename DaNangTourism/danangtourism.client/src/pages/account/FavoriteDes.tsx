import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
import { useToast } from '../../hook/useToast'
import { SimpleDesProps } from '../../types/destination'
import axios from 'axios'
import {
	Button,
	CircleButton,
	DropdownSelect,
	Loader,
	Pagination,
	SearchBox,
	SortTypeButton,
} from '../../components'
import { PiEyeFill, PiPenFill, PiHeartBreakFill } from 'react-icons/pi'
import { NumberFormat } from '../../utils/Format'
import useConfirm from '../../hook/useConfirm'

const sortBy = [
	{
		value: 'name',
		label: 'Name',
	},
	{
		value: 'rating',
		label: 'Avg. Rating',
	},
]

const FavoriteDes: React.FC<{ className?: string }> = ({ className }) => {
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [currentPage, setCurrentPage] = useState(1)
	const limit = 12
	const [total, setTotal] = useState(0)
	const toast = useToast()
	const [destinations, setDestinations] = useState<SimpleDesProps[]>()
	const [loading, setLoading] = useState(true)

	const handleGetDestinations = async () => {
		setDestinations(undefined)
		setLoading(true)
		try {
			const response = await axios.get('/api/destination/list', {
				params: {
					page: currentPage,
					limit,
					sortBy: sortBy[sort.by].value,
					sortType: sort.type,
					isFavorite: true,
					...(searchValue !== '' ? { search: searchValue } : {}),
				},
			})
			const data = response.data.data
			setDestinations(data.items)
			setTotal(data.total)
		} catch (error: any) {
			if (error.response.status !== 404) {
				toast.error('Error', 'Failed to get destinations')
				console.error(error)
			}
		}
		setLoading(false)
	}

	useEffect(() => {
		handleGetDestinations()
	}, [searchValue, sort, currentPage])

	return (
		<div className={twMerge('w-full px-8 py-5', className)}>
			<div className="mb-3 flex w-full items-center justify-between">
				<div className="item-center relative flex gap-4">
					<SearchBox
						className="h-9 px-4"
						onChangeValue={(event) => setSearchValue(event.target.value)}
						onClickSearch={handleGetDestinations}
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
	destinations: SimpleDesProps[]
	onDeleted: () => void
}> = ({ destinations, onDeleted }) => {
	const toast = useToast()
	const confirm = useConfirm()
	const handleUnfavorite = async (id: number) => {
		const result = await confirm.showConfirmation(
			'Unfavorite',
			'Are you sure you want to remove this destination from favorite list?',
		)
		if (!result) return
		try {
			await axios.put('/api/destination/favorite', {
				destinationId: id,
				isFavorite: false,
			})
			toast.success('Success', 'Destination removed from favorite list')
			onDeleted()
		} catch (error) {
			toast.error('Error', 'Failed to remove destination from favorite list')
			console.log(error)
		}
	}
	return (
		<table className="w-full border-spacing-2">
			<thead className="border-b border-borderCol-1">
				<tr className=" h-8 text-center [&_*]:font-semibold">
					<th className="w-[100px] pl-2">Id</th>
					<th className="w-[180px]">Name</th>
					<th>Address</th>
					<th className="w-[88px]">Open time</th>
					<th className="w-[88px]">Close time</th>
					<th className="w-[72px]">Cost</th>
					<th className="w-[72px]">Rating</th>
					<th className="w-24 pr-2">Actions</th>
				</tr>
			</thead>
			<tbody className="[&>*:nth-child(odd)]:bg-gray-100 hover:[&_tr]:bg-[#64ccdc3f]">
				{destinations?.map((des) => (
					<tr key={des.id} className="h-10 text-center text-sm">
						<td className="pl-2">{des.id}</td>
						<td>
							<p className="line-clamp-1 text-left" title={des.name}>
								{des.name}
							</p>
						</td>
						<td>
							<p className="line-clamp-1 text-left" title={des.address}>
								{des.address}
							</p>
						</td>
						<td>{des.openTime}</td>
						<td>{des.closeTime}</td>
						<td>{'$' + NumberFormat(des.cost)}</td>
						<td>{des.rating.toFixed(2)}</td>
						<td className="flex h-10 items-center justify-center gap-3 pr-2">
							<CircleButton
								className="border-secondary-1 bg-[#76C8933f] text-secondary-1"
								title="View"
								onClick={() => window.open(`/destination/${des.id}`, '_blank')}
							>
								<PiEyeFill />
							</CircleButton>
							<CircleButton
								className=" border-tertiary-2 bg-[#ee685e3f] text-tertiary-2"
								title="Unfavorite"
								onClick={() => handleUnfavorite(des.id)}
							>
								<PiHeartBreakFill />
							</CircleButton>
						</td>
					</tr>
				))}
			</tbody>
		</table>
	)
}

export default FavoriteDes
