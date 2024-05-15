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
} from './pages'
import { useContext, useEffect, useState } from 'react'
import { UserContext } from './context/UserContext'
import ScrollToTop from './utils/ScrollToTop'

function App() {
	const [accountModal, setAccountModal] = useState(0)
	const { setUser } = useContext(UserContext)

	useEffect(() => {
		const localUser = sessionStorage.getItem('user')
		if (localUser) {
			const user = JSON.parse(localUser)
			setUser(user)
		}
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
					<Route
						path="/destination/new"
						element={<DestinationEditor />}
					></Route>
					<Route path="/destination/:id" element={<Destination />}></Route>
					<Route path="/blog" element={<BlogPage />}></Route>
					<Route path="/blog/new" element={<BlogEditor />}></Route>
					<Route path="/blog/:id" element={<Blog />}></Route>
					<Route path="/schedule" element={<SchedulePage />}></Route>
					<Route path="/schedule/:id" element={<Schedule />}></Route>
					<Route path="*" element={<PageNotFound />}></Route>
				</Routes>
			</BrowserRouter>
			{accountModal === 1 && <LoginForm onClose={() => setAccountModal(0)} />}
		</>
	)
}

export default App
