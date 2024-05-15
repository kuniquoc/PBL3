/* eslint-disable no-useless-escape */
import { twMerge } from 'tailwind-merge'
import { Button } from '../../components'
import { useEffect, useState } from 'react'
// import { UserContext } from '../../context/UserContext'

const LoginForm: React.FC<{ className?: string; onClose: () => void }> = ({
	className = '',
	onClose,
}) => {
	const [formData, setFormData] = useState({
		email: '',
		password: '',
		remember: false,
	})
	const [warnings, setWarnings] = useState({
		email: '',
		password: '',
	})
	const [firstMount, setFirstMount] = useState(true)

	const validateData = () => {
		let isValid = true
		if (!formData.email) {
			setWarnings((prev) => ({ ...prev, email: 'Email is required' }))
			isValid = false
		} else if (!formData.email.match(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g)) {
			setWarnings((prev) => ({ ...prev, email: 'Invalid email' }))
			isValid = false
		} else {
			setWarnings((prev) => ({ ...prev, email: '' }))
		}
		if (!formData.password) {
			setWarnings((prev) => ({ ...prev, password: 'Password is required' }))
			isValid = false
		} else if (!formData.password.match(/^[A-Za-z0-9]{6,20}$/)) {
			setWarnings((prev) => ({
				...prev,
				password: 'Password must be 6-20 characters of letters and numbers',
			}))
			isValid = false
		} else {
			setWarnings((prev) => ({ ...prev, password: '' }))
		}

		return isValid
	}
	// const { setUser } = useContext(UserContext)
	const handleLogin = async () => {
		setFirstMount(false)
		const UserData = {
			id: 40000001,
			username: 'Admin Account',
			email: 'adminaccount@email.con',
			avatar:
				'https://ps.w.org/user-avatar-reloaded/assets/icon-256x256.png?rev=2540745',
			roleId: 1,
		}
		if (validateData()) {
			// setUser(UserData)
			sessionStorage.setItem('user', JSON.stringify(UserData))
			window.location.reload()
		}
	}

	useEffect(() => {
		if (!firstMount) validateData()
	}, [formData])

	return (
		<div
			className={twMerge(
				`flex items-center justify-center bg-[#0000004b] ${className} fixed left-0 top-0 z-10 h-screen w-screen`,
			)}
			onMouseDown={(e) => {
				if (e.target === e.currentTarget) {
					onClose()
				}
			}}
		>
			<div className="flex w-[560px] items-center p-5">
				<div className="flex w-[520px] flex-col items-center gap-4 rounded-xl bg-white px-14 py-10">
					<h1 className="inline-block bg-gradient-to-br from-primary-1 to-secondary-2 bg-clip-text text-3xl font-bold text-transparent">
						Welcome back !
					</h1>
					<p>Login to explore all destinations & features</p>
					<div className="flex w-full flex-col gap-1">
						<label className="font-semibold" htmlFor="login-email">
							Email
						</label>
						<input
							className="h-10 w-full border-2 bg-[#fdfdfd] px-4 text-sm focus:border-2"
							type="email"
							id="login-email"
							placeholder="Enter your email address"
							value={formData.email}
							onChange={(e) =>
								setFormData({ ...formData, email: e.target.value })
							}
						/>
						<p className="text-sm text-tertiary-1">{warnings.email}</p>
					</div>
					<div className="flex w-full flex-col gap-1">
						<label className="font-semibold" htmlFor="login-password">
							Password
						</label>
						<input
							className="h-10 w-full border-2 bg-[#fdfdfd] px-4 text-sm focus:border-2"
							type="password"
							id="login-password"
							placeholder="Enter your password"
							value={formData.password}
							onChange={(e) =>
								setFormData({ ...formData, password: e.target.value })
							}
						/>
						<p className="text-sm text-tertiary-1">{warnings.password}</p>
					</div>
					<div className="item-center flex w-full justify-between text-sm">
						<div className="inline-flex items-center gap-2">
							<input
								className="large"
								type="checkbox"
								id="login-remember"
								checked={formData.remember}
								onChange={(e) =>
									setFormData({ ...formData, remember: e.target.checked })
								}
							/>
							<label htmlFor="login-remember">Remember me</label>
						</div>
						<button className="text-primary-1 hover:underline">
							Forgot your password
						</button>
					</div>
					<div className="mt-2 flex w-full items-center">
						<Button
							className="h-10 w-full rounded bg-primary-2 text-base text-white hover:bg-primary-1"
							onClick={handleLogin}
						>
							Login
						</Button>
					</div>
					<div className="mt-2 flex w-full items-center justify-center gap-2 text-sm">
						<p className="text-txtCol-2">Don't have account?</p>
						<button className="text-primary-1 hover:underline">
							Sign up here
						</button>
					</div>
				</div>
			</div>
		</div>
	)
}

export default LoginForm
