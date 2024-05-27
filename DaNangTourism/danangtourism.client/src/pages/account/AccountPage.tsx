import { useContext, useEffect, useState } from 'react'
import { UserContext } from '../../context/UserContext'
import PageNotFound from '../PageNotFound'
import { TabButton } from '../../components/Buttons'
import { PiHardDrivesBold, PiMapPinBold, PiUserBold } from 'react-icons/pi'
import MyAccount from './MyAccount'
import { useLocation, useNavigate } from 'react-router-dom'

const AccountPage: React.FC = () => {
	document.title = 'My Account | Da Nang Explore'
	const { user } = useContext(UserContext)

	const [tabIndex, setTabIndex] = useState(0)
	const location = useLocation()
	const navigate = useNavigate()

	useEffect(() => {
		const params = new URLSearchParams(location.search)
		const tab = params.get('tab')
		switch (tab) {
			case '0':
				setTabIndex(0)
				break
			case '1':
				setTabIndex(1)
				break
			case '2':
				setTabIndex(2)
				break
			default:
				setTabIndex(0)
				break
		}
	}, [location])

	const handleTabChange = (index: number) => {
		setTabIndex(index)
		navigate(`/account?tab=${index}`)
	}

	return (
		<div className="mx-auto flex min-h-screen justify-center gap-4 pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="flex w-full items-start justify-center gap-4">
				<div className="flex w-[240px] flex-col gap-3">
					<TabButton
						index={0}
						tabIndex={tabIndex}
						onClick={() => handleTabChange(0)}
					>
						<PiUserBold className="text-xl" />
						My Account
					</TabButton>
					<TabButton
						index={1}
						tabIndex={tabIndex}
						onClick={() => handleTabChange(1)}
					>
						<PiMapPinBold className="text-xl" />
						Favorite Destinations
					</TabButton>
					<TabButton
						index={2}
						tabIndex={tabIndex}
						onClick={() => handleTabChange(2)}
					>
						<PiHardDrivesBold className="text-xl" />
						My Blogs
					</TabButton>
				</div>
				<div className="flex-1">
					{tabIndex === 0 && (
						<MyAccount className="rounded border border-borderCol-1 bg-white" />
					)}
				</div>
			</div>
		</div>
	)
}

export default AccountPage
