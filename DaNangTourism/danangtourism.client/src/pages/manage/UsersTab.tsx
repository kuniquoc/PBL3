import { twMerge } from 'tailwind-merge'
import { DropdownSelect, Loader, Pagination, SearchBox } from '../../components'
import React, { useEffect, useState } from 'react'
import { CircleButton, SortTypeButton } from '../../components/Buttons'
import { useToast } from '../../hook/useToast'
import {
	PiShieldCheckFill,
	PiUserFill,
	PiTrashSimpleFill,
} from 'react-icons/pi'
import axios from 'axios'
import { toDisplayDateTime } from '../../utils/TimeFormatters'
import { ManageUserProps } from '../../types/user'
import useConfirm from '../../hook/useConfirm'

const sortBy = [
	{
		value: 'created_at',
		label: 'Create time',
	},
	{
		value: 'full_name',
		label: 'Name',
	},
]

const roles = [
	{
		value: 'all',
		label: 'All',
	},
	{
		value: 'admin',
		label: 'Admin',
	},
	{
		value: 'user',
		label: 'User',
	},
]

const UsersTab: React.FC<{ className?: string }> = ({ className }) => {
	const [searchValue, setSearchValue] = useState('')
	const [sort, setSort] = useState({
		by: 0,
		type: 'desc',
	})
	const [viewRole, setViewRole] = useState(0)
	const [currentPage, setCurrentPage] = useState(1)
	const limit = 12
	const [total, setTotal] = useState(0)
	const toast = useToast()
	const [users, setUsers] = useState<ManageUserProps[]>()

	const handleSearch = () => {
		handleGetUsers()
	}

	const handleGetUsers = async () => {
		setUsers(undefined)
		try {
			const response = await axios.get('/api/account/get/list', {
				params: {
					search: searchValue,
					page: currentPage,
					limit: limit,
					role: roles[viewRole].value,
					sortBy: sortBy[sort.by].value,
					sortType: sort.type,
				},
			})
			const res = response.data.data
			setUsers(res.items)
			setTotal(res.total)
		} catch (error) {
			toast.error('Error', 'Failed to get destinations')
			console.log(error)
		}
	}

	useEffect(() => {
		handleGetUsers()
	}, [currentPage, sort, viewRole, searchValue])

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
						className="h-9 w-[220px]"
						onChangeValue={(event) => setSearchValue(event.target.value)}
						onClickSearch={handleSearch}
					/>
					<DropdownSelect
						id="view-role"
						className="h-9 w-[120px]"
						options={roles.map((item) => item.label)}
						value={viewRole}
						onChange={(event) => {
							setViewRole(Number(event.target.value))
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
				{users ? (
					<UserTable users={users} onReload={handleGetUsers} />
				) : (
					<div className="flex h-[512.4px] w-full items-center justify-center bg-gray-50">
						<Loader className="h-16 w-16" />
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

const UserTable: React.FC<{
	users: ManageUserProps[]
	onReload: () => void
}> = ({ users, onReload }) => {
	const toast = useToast()
	const confirm = useConfirm()

	const handleSwitchRole = async (id: number) => {
		const result = await confirm.showConfirmation(
			'Switch role',
			'Are you sure you want to switch this user role?',
		)
		if (!result) return
		try {
			const response = await axios.put(`/api/account/update/role/${id}`)
			if (response.status === 200) {
				toast.success('Success', response.data.message)
				onReload()
			} else {
				toast.error('Error', 'Failed to update role')
			}
		} catch (error) {
			toast.error('Error', 'Failed to update role')
			console.log(error)
		}
	}

	const handleDeleteUser = async (id: number) => {
		const result = await confirm.showConfirmation(
			'Delete user',
			'Are you sure you want to delete this user? This action cannot be undone.',
		)
		if (!result) return
		try {
			const response = await axios.delete(`/api/account/delete/${id}`)
			if (response.status === 200) {
				toast.success('Success', 'User deleted')
				onReload()
			} else {
				toast.error('Error', 'Failed to delete user')
			}
		} catch (error) {
			toast.error('Error', 'Failed to delete user')
			console.log(error)
		}
	}

	return (
		<table className="w-full border-spacing-2">
			<thead className="border-b border-borderCol-1">
				<tr className=" h-8 text-center [&_*]:font-semibold">
					<th className="w-[120px] pl-2">Id</th>
					<th className="">Name</th>
					<th className="w-44">Email</th>
					<th className="w-24">Role</th>
					<th className="w-44">Created At</th>
					<th className="w-32 pr-2">Actions</th>
				</tr>
			</thead>
			<tbody className="[&>*:nth-child(odd)]:bg-gray-100 hover:[&_tr]:bg-[#64ccdc3f]">
				{users?.map((usr) => (
					<tr key={usr.id} className="h-10 text-center text-sm">
						<td className="pl-2">{usr.id}</td>
						<td className="text-left">{usr.name}</td>
						<td className="line-clamp-1 text-left">{usr.email}</td>
						<td className="capitalize">{usr.role}</td>
						<td>{toDisplayDateTime(usr.createdAt)}</td>
						<td className="flex h-10 items-center justify-center gap-3 pr-2">
							{usr.role === 'admin' ? (
								<CircleButton
									className="border-secondary-1 bg-[#76C8933f] text-secondary-1"
									title="Switch to user"
									onClick={() => handleSwitchRole(usr.id)}
								>
									<PiUserFill />
								</CircleButton>
							) : (
								<CircleButton
									className="border-primary-2 bg-[#64B8DC3f] text-primary-2"
									title="Set as admin"
									onClick={() => handleSwitchRole(usr.id)}
								>
									<PiShieldCheckFill />
								</CircleButton>
							)}
							{usr.role !== 'admin' && (
								<CircleButton
									className=" border-tertiary-2 bg-[#ee685e3f] text-tertiary-2"
									onClick={() => handleDeleteUser(usr.id)}
								>
									<PiTrashSimpleFill />
								</CircleButton>
							)}
						</td>
					</tr>
				))}
			</tbody>
		</table>
	)
}

export default UsersTab
