import './styles/App.css'
import 'react-quill/dist/quill.snow.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import {
	Navbar,
	PageNotFound,
	Home,
	DestinationPage,
	BlogPage,
	Blog,
	Destination,
	SchedulePage,
	Schedule,
	BlogEditor,
	DestinationEditor,
	LoginForm,
	RegisterForm,
	AccountPage,
	ManagePage,
} from './pages'
import { useContext, useEffect, useState } from 'react'
import { UserContext, defaultUser } from './context/UserContext'
import ScrollToTop from './utils/ScrollToTop'
import { AnimatePresence } from 'framer-motion'
import Cookies from 'js-cookie'
import axios from 'axios'
import { useToast } from './hook/useToast'

function App() {
	const [accountModal, setAccountModal] = useState(0)
	const { user, setUser } = useContext(UserContext)
	const toast = useToast()
	const auth = async () => {
		try {
			const res = await axios.get('/api/auth/authenticated')
			const resData = await res.data.data
			setUser({
				...resData,
				rememberMe: true,
			})
			toast.success('Welcome back!', `Hello, ${resData.name}!`)
		} catch (error) {
			setUser(defaultUser.user)
			toast.info('Welcome, guess!', 'Please login to access all our features.')
		}
	}

	useEffect(() => {
		auth()
	}, [])

	return (
		<>
			<BrowserRouter>
				<ScrollToTop />
				<Navbar
					onLogin={() => setAccountModal(1)}
					onSignUp={() => setAccountModal(2)}
				/>
				<Routes>
					<Route path="/" element={<Home />}></Route>
					<Route path="/destination" element={<DestinationPage />}></Route>
					<Route path="/destination/:id" element={<Destination />}></Route>
					<Route path="/blog" element={<BlogPage />}></Route>
					<Route path="/blog/:id" element={<Blog />}></Route>
					<Route path="/schedule" element={<SchedulePage />}></Route>
					<Route path="/schedule/:id" element={<Schedule />}></Route>
					// protected route
					{user.id !== 0 && (
						<>
							<Route path="/account" element={<AccountPage />}></Route>
							<Route path="/blog/new" element={<BlogEditor />}></Route>
						</>
					)}
					// private route
					{user.role === 'admin' && (
						<>
							<Route path="/manage" element={<ManagePage />}></Route>
							<Route
								path="/destination/new"
								element={<DestinationEditor />}
							></Route>
							<Route
								path="/destination/edit/:id"
								element={<DestinationEditor />}
							></Route>
						</>
					)}
					<Route path="*" element={<PageNotFound />}></Route>
				</Routes>
			</BrowserRouter>
			<AnimatePresence>
				{accountModal === 1 && (
					<LoginForm
						onClose={() => setAccountModal(0)}
						onSwitch={() => setAccountModal(2)}
					/>
				)}
				{accountModal === 2 && (
					<RegisterForm
						onClose={() => setAccountModal(0)}
						onSwitch={() => setAccountModal(1)}
					/>
				)}
			</AnimatePresence>
		</>
	)
}

export default App
