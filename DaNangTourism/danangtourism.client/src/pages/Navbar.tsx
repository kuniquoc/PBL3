import { useLocation, Link } from 'react-router-dom'
import {
	motion,
	useScroll,
	useMotionValueEvent,
	AnimatePresence,
} from 'framer-motion'
import {
	PiCaretDownBold,
	PiHouseBold,
	PiCompassBold,
	PiArticleBold,
	PiCalendarBlankBold,
	PiTableBold,
} from 'react-icons/pi'
import { useContext, useState } from 'react'
import { UserContext } from '../context/UserContext'
import { Button } from '../components'
import AccountMenu from './account/AccountMenu'

const NavItems = [
	{
		name: 'Home',
		icon: PiHouseBold,
		path: '/',
	},
	{
		name: 'Destination',
		icon: PiCompassBold,
		path: '/destination',
	},
	{
		name: 'Blog',
		icon: PiArticleBold,
		path: '/blog',
	},
	{
		name: 'Schedule',
		icon: PiCalendarBlankBold,
		path: '/schedule',
	},
	{
		name: 'Manage',
		icon: PiTableBold,
		path: '/manage',
	},
]

interface NavbarProps {
	onSignUp: () => void
	onLogin: () => void
}
const Navbar: React.FC<NavbarProps> = ({ onSignUp, onLogin }) => {
	const location = useLocation()
	const firstPath = '/' + location.pathname.split('/')[1]
	const [hidden, setHidden] = useState(false)
	const [showMenu, setShowMenu] = useState(false)
	const { scrollY } = useScroll()
	const windowHeight = window.innerHeight
	const scrolledOnePage = scrollY.get() >= windowHeight
	const isHome = scrolledOnePage ? false : firstPath === '/'

	useMotionValueEvent(scrollY, 'change', (latest) => {
		const previous = scrollY.getPrevious() as number
		if (latest > previous && latest > 100) setHidden(true)
		else setHidden(false)
	})

	const { user } = useContext(UserContext)

	return (
		<motion.nav
			variants={{
				visible: { y: 0 },
				hidden: { y: '-100%' },
			}}
			animate={hidden ? 'hidden' : 'visible'}
			transition={{ duration: 0.35, ease: 'easeInOut' }}
			className={`fixed left-0 top-0 z-10 flex h-12 w-full justify-center ${isHome ? 'bg-transparent' : 'bg-bgCol-2 shadow-md'}`}
		>
			<div className="my-auto flex h-10 w-full items-center justify-between xl:max-w-screen-xl">
				<div className="h-full w-[200px]">
					<Link
						className={`cursor-pointer select-none text-xl font-bold leading-10 ${isHome ? 'text-primary-3' : 'text-primary-1'} `}
						to="/"
					>
						Da Nang Explore
					</Link>
				</div>
				<div className="h-full">
					<ul className="flex h-full gap-1">
						{NavItems.map(
							(item, index) =>
								(item.name !== 'Manage' ||
									(item.name === 'Manage' && user.role === 'admin')) && (
									<li key={index}>
										<Link
											to={item.path}
											className={`relative flex min-w-[120px] items-center justify-center gap-2 px-3 text-base ${
												firstPath === item.path
													? isHome
														? 'text-primary-3'
														: 'text-primary-1'
													: isHome
														? 'text-txtCol-4'
														: 'text-txtCol-3'
											} rounded-lg transition-all ${isHome ? 'hover:bg-[#00000040]' : 'hover:bg-[#00000015]'}`}
										>
											<item.icon />
											<p
												className={`${
													firstPath === item.path
														? isHome
															? 'text-white'
															: 'text-txtCol-1'
														: isHome
															? 'text-txtCol-4'
															: 'text-txtCol-3'
												} text-sm font-semibold leading-10`}
											>
												{item.name}
											</p>
											{firstPath === item.path && (
												<motion.span
													className="absolute bottom-[-4px] h-[2px] w-full bg-primary-2"
													layoutId="underline"
													transition={{ duration: 0.2 }}
												></motion.span>
											)}
										</Link>
									</li>
								),
						)}
					</ul>
				</div>
				{user.id !== 0 ? (
					<div className="relative flex w-[200px] items-center justify-end gap-4">
						<p
							className={`text-sm font-semibold ${isHome ? 'text-txtCol-4' : ''}`}
						>
							{user.name}
						</p>
						<button
							className="relative h-8 w-8 rounded-full"
							onClick={() => setShowMenu(!showMenu)}
						>
							<img
								className="h-full w-full rounded-full object-cover"
								src={user.avatar}
								alt="User Avatar"
							/>
							<span
								className={`absolute bottom-[-3px] right-[-3px] rounded-full ${isHome ? '' : 'bg-bgCol-2'}`}
							>
								<div className=" mx-[2px] my-[2px] rounded-[inherit] bg-[#dddddd] px-[1px] py-[1px] text-xs">
									<PiCaretDownBold />
								</div>
							</span>
						</button>
						<AnimatePresence>
							{showMenu && (
								<AccountMenu
									className="absolute bottom-[-10px] right-0"
									onClose={() => {
										setShowMenu(false)
									}}
								/>
							)}
						</AnimatePresence>
					</div>
				) : (
					<div className="flex items-center justify-end gap-4">
						<Button
							className={`text-sm font-semibold ${isHome ? 'text-txtCol-4 hover:text-primary-3' : 'text-txtCol-1 hover:text-primary-1'}`}
							onClick={onSignUp}
						>
							Sign up
						</Button>
						<Button
							className={`w-[92px] bg-primary-2 text-sm font-semibold text-white hover:bg-primary-1 ${isHome ? 'opacity-80' : ''}`}
							onClick={onLogin}
						>
							Login
						</Button>
					</div>
				)}
			</div>
		</motion.nav>
	)
}

export default Navbar
