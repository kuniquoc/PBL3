import { motion } from 'framer-motion'
import { twMerge } from 'tailwind-merge'
import { defaultUser } from '../../context/UserContext'
import { useConfirm, useToast, useUser } from '../../hook'
import {
	PiHardDrivesFill,
	PiMapPinFill,
	PiSignOutBold,
	PiUserFill,
} from 'react-icons/pi'
import { Link, useNavigate } from 'react-router-dom'
import axios from 'axios'

const AccountMenu: React.FC<{
	className?: string
	onClose: () => void
}> = ({ className = '', onClose }) => {
	const { user, setUser } = useUser()
	const toast = useToast()
	const navigate = useNavigate()
	const confirm = useConfirm()

	const handleSignOut = async () => {
		const result = await confirm.showConfirmation(
			'Sign Out',
			'Are you sure you want to sign out?',
		)
		if (!result) return
		try {
			await axios.get('/api/auth/logout')
			setUser(defaultUser.user)
			onClose()
			toast.info('Goodbye!', 'You have been signed out')
			navigate('/')
		} catch (error) {
			console.error(error)
		}
	}

	return (
		<motion.div
			className={twMerge(
				'shadow-modal w-[280px] select-none rounded-lg bg-white px-3 pb-2 pt-4',
				className,
			)}
			initial={{ opacity: 0, y: '100%' }}
			animate={{ opacity: 1, y: '105%' }}
			exit={{ opacity: 0, y: '100%' }}
		>
			<p className="mb-1 line-clamp-1 select-text px-2 text-right text-sm text-primary-1">
				{user.email}
			</p>
			<Link
				className="flex cursor-pointer items-center rounded py-2 pl-2 pr-1 transition-all hover:bg-gray-100 active:bg-gray-200"
				to="/account"
				onClick={onClose}
			>
				<div className="flex h-8 w-8 items-center justify-center rounded-full bg-gray-300 text-xl">
					<PiUserFill />
				</div>
				<p className="ml-4 flex-1 text-sm font-semibold">My Account</p>
			</Link>
			<Link
				className="flex cursor-pointer items-center rounded py-2 pl-2 pr-1 transition-all hover:bg-gray-100 active:bg-gray-200"
				to="/account?tab=1"
			>
				<div className="flex h-8 w-8 items-center justify-center rounded-full bg-gray-300 text-xl">
					<PiMapPinFill />
				</div>
				<p className="ml-4 flex-1 text-sm font-semibold">
					Favorite Destinations
				</p>
			</Link>
			<Link
				className="flex cursor-pointer items-center rounded py-2 pl-2 pr-1 transition-all hover:bg-gray-100 active:bg-gray-200"
				to="/account?tab=2"
			>
				<div className="flex h-8 w-8 items-center justify-center rounded-full bg-gray-300 text-xl">
					<PiHardDrivesFill />
				</div>
				<p className="ml-4 flex-1 text-sm font-semibold">My Blogs</p>
			</Link>
			<div className="my-1 h-[1px] w-full bg-borderCol-1"></div>
			<button
				className="flex w-full items-center rounded py-2 pl-2 pr-1 transition-all hover:bg-gray-100 active:bg-gray-200"
				onClick={handleSignOut}
			>
				<div className="flex h-8 w-8 items-center justify-center rounded-full bg-gray-300 text-xl">
					<PiSignOutBold />
				</div>
				<p className="ml-4 flex-1 text-left text-sm font-semibold">Sign Out</p>
			</button>
		</motion.div>
	)
}

export default AccountMenu
