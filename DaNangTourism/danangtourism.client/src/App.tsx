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
import { useEffect, useState } from 'react'
import ScrollToTop from './utils/ScrollToTop'
import { AnimatePresence } from 'framer-motion'
import { Loader } from './components'
import useUser from './hook/useUser'
import ForgotPasswordForm from './pages/account/ForgotPasswordForm'
function App() {
	const [accountModal, setAccountModal] = useState(0)
	const [loading, setLoading] = useState(true)
	const { LoadUser, user } = useUser()
	const auth = async () => {
		setLoading(true)
		await LoadUser()
		await new Promise((resolve) => setTimeout(resolve, 100))
		setLoading(false)
	}

	useEffect(() => {
		auth()
	}, [])

	return (
		<>
			<BrowserRouter>
				<ScrollToTop />
				{loading && (
					<div className="fixed left-0 top-0 z-20 flex h-screen w-screen items-center justify-center bg-bgCol-1">
						<Loader />
					</div>
				)}
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
							<Route path="/blog/edit/:id" element={<BlogEditor />}></Route>
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
						onForgot={() => setAccountModal(3)}
					/>
				)}
				{accountModal === 2 && (
					<RegisterForm
						onClose={() => setAccountModal(0)}
						onSwitch={() => setAccountModal(1)}
					/>
				)}
				{accountModal === 3 && (
					<ForgotPasswordForm
						onClose={() => setAccountModal(0)}
						onSwitch={() => setAccountModal(1)}
					/>
				)}
			</AnimatePresence>
		</>
	)
}

export default App
