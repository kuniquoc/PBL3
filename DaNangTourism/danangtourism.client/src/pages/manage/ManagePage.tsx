import { useEffect, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { PiHardDrivesBold, PiMapPinBold, PiUsersBold } from 'react-icons/pi'
import DestinationsTab from './DestinationsTab'
import { TabButton } from '../../components'
import BlogsTab from './BlogsTab'
import UsersTab from './UsersTab'

const ManagePage: React.FC = () => {
	document.title = 'Manage | Danang Tourism'
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
		navigate(`/manage?tab=${index}`)
	}

	return (
		<div className="mx-auto flex min-h-screen justify-center gap-4 pb-6 pt-[72px] text-txtCol-1 xl:max-w-screen-xl">
			<div className="flex w-full items-start justify-center gap-4">
				<div className="flex w-[200px] flex-col gap-3">
					<TabButton
						index={0}
						tabIndex={tabIndex}
						onClick={() => handleTabChange(0)}
					>
						<PiMapPinBold className="text-xl" />
						Destinations
					</TabButton>
					<TabButton
						index={1}
						tabIndex={tabIndex}
						onClick={() => handleTabChange(1)}
					>
						<PiHardDrivesBold className="text-xl" />
						Blogs
					</TabButton>
					<TabButton
						index={2}
						tabIndex={tabIndex}
						onClick={() => handleTabChange(2)}
					>
						<PiUsersBold className="text-xl" />
						Users
					</TabButton>
				</div>
				<div className="flex-1">
					{tabIndex === 0 && <DestinationsTab />}
					{tabIndex === 1 && <BlogsTab />}
					{tabIndex === 2 && <UsersTab />}
				</div>
			</div>
		</div>
	)
}

export default ManagePage
