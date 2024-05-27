/* eslint-disable no-useless-escape */
import { twMerge } from 'tailwind-merge'
import { Button } from '../../components'
import { useEffect, useState } from 'react'
import { motion } from 'framer-motion'
import { useToast } from '../../hook/useToast'
import ReCAPTCHA from 'react-google-recaptcha'
import { variantsDefault, variantsY } from '../../styles/variants'
// import { UserContext } from '../../context/UserContext'
// 6LcV0uYpAAAAABNE0DW6qJ8fPNjoydVhG_HYKo7u
const RegisterForm: React.FC<{
	className?: string
	onClose: () => void
	onSwitch: () => void
}> = ({ className = '', onClose, onSwitch }) => {
	const [formData, setFormData] = useState({
		email: '',
		password: '',
		confirm: '',
	})
	const [warnings, setWarnings] = useState({
		email: '',
		password: '',
		confirm: '',
	})
	const [capVal, setCapVal] = useState<string | null>(null)
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

		if (!formData.confirm) {
			setWarnings((prev) => ({
				...prev,
				confirm: 'Please confirm your password',
			}))
			isValid = false
		} else if (formData.confirm !== formData.password) {
			setWarnings((prev) => ({ ...prev, confirm: 'Password does not match' }))
			isValid = false
		} else {
			setWarnings((prev) => ({ ...prev, confirm: '' }))
		}
		return isValid
	}
	// const { setUser } = useContext(UserContext)
	const toast = useToast()
	const handleSignUp = async () => {
		setFirstMount(false)
		if (!capVal) {
			toast.error('Captcha validation failed', 'Please complete the captcha')
			return
		}
		if (validateData()) {
			toast.success('Sign up success', 'You have successfully signed up')
			onClose()
		} else {
			toast.error('Sign up failed', 'Please check your input')
		}
	}

	useEffect(() => {
		if (!firstMount) validateData()
	}, [formData])

	return (
		<motion.div
			className={twMerge(
				`flex items-center justify-center bg-[#0000004b] ${className} fixed left-0 top-0 z-10 h-screen w-screen`,
			)}
			onMouseDown={(e) => {
				if (e.target === e.currentTarget) {
					onClose()
				}
			}}
			variants={variantsDefault}
			initial="hidden"
			animate="visible"
			exit="hidden"
		>
			<motion.div
				className="flex w-[560px] items-center p-5"
				variants={variantsY}
				initial="top"
				animate="visible"
				exit="bottom"
				custom={100}
			>
				<div className="flex w-[520px] flex-col items-center gap-2 rounded-xl bg-white px-14 py-10">
					<h1 className="inline-block bg-gradient-to-br from-primary-1 to-secondary-2 bg-clip-text text-3xl font-bold text-transparent">
						Register
					</h1>
					<div className="flex w-full flex-col gap-1">
						<label className="font-semibold" htmlFor="signup-email">
							Email
						</label>
						<input
							className="h-10 w-full border-2 bg-[#fdfdfd] px-4 text-sm focus:border-2"
							type="email"
							id="signup-email"
							placeholder="Enter your email address"
							value={formData.email}
							onChange={(e) =>
								setFormData({ ...formData, email: e.target.value })
							}
						/>
						<p className="text-sm text-tertiary-1">{warnings.email}</p>
					</div>
					<div className="flex w-full flex-col gap-1">
						<label className="font-semibold" htmlFor="signup-password">
							Password
						</label>
						<input
							className="h-10 w-full border-2 bg-[#fdfdfd] px-4 text-sm focus:border-2"
							type="password"
							id="signup-password"
							placeholder="Enter your password"
							value={formData.password}
							onChange={(e) =>
								setFormData({ ...formData, password: e.target.value })
							}
						/>
						<p className="text-sm text-tertiary-1">{warnings.password}</p>
					</div>
					<div className="flex w-full flex-col gap-1">
						<label className="font-semibold" htmlFor="signup-password">
							Confirm Password
						</label>
						<input
							className="h-10 w-full border-2 bg-[#fdfdfd] px-4 text-sm focus:border-2"
							type="password"
							id="signup-confirm"
							placeholder="Re-enter your password"
							value={formData.confirm}
							onChange={(e) =>
								setFormData({ ...formData, confirm: e.target.value })
							}
						/>
						<p className="text-sm text-tertiary-1">{warnings.confirm}</p>
					</div>
					<ReCAPTCHA
						className="mt-2"
						sitekey="6LcV0uYpAAAAABNE0DW6qJ8fPNjoydVhG_HYKo7u"
						onChange={(val) => setCapVal(val)}
					/>
					<div className="mt-2 flex w-full items-center">
						<Button
							className="h-10 w-full rounded bg-primary-2 text-base text-white hover:bg-primary-1"
							onClick={handleSignUp}
						>
							Sign Up
						</Button>
					</div>
					<div className="mt-2 flex w-full items-center justify-center gap-2 text-sm">
						<p className="text-txtCol-2">Already have account?</p>
						<button
							className="text-primary-1 hover:underline"
							onClick={onSwitch}
						>
							Login here
						</button>
					</div>
				</div>
			</motion.div>
		</motion.div>
	)
}

export default RegisterForm
