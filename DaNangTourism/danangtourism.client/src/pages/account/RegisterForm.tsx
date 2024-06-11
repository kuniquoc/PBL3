import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
import { motion } from 'framer-motion'
import axios from 'axios'
import ReCAPTCHA from 'react-google-recaptcha'

import { Button } from '../../components'
import { useToast } from '../../hook'
import { variantsDefault, variantsY } from '../../styles/variants'

// 6LcV0uYpAAAAABNE0DW6qJ8fPNjoydVhG_HYKo7u

const RegisterForm: React.FC<{
	className?: string
	onClose: () => void
	onSwitch: () => void
}> = ({ className = '', onClose, onSwitch }) => {
	const [formData, setFormData] = useState({
		email: '',
		code: '',
		password: '',
		confirm: '',
	})
	const [warnings, setWarnings] = useState({
		email: '',
		password: '',
	})
	const [capVal, setCapVal] = useState<string | null>(null)
	const [firstMount, setFirstMount] = useState(true)
	const [sendTimeout, setSendTimeout] = useState(0)
	const toast = useToast()

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
		} else if (!formData.confirm) {
			setWarnings((prev) => ({
				...prev,
				password: 'Please confirm your password',
			}))
			isValid = false
		} else if (formData.confirm !== formData.password) {
			setWarnings((prev) => ({ ...prev, password: 'Password does not match' }))
			isValid = false
		} else {
			setWarnings((prev) => ({ ...prev, password: '' }))
		}

		return isValid
	}

	const handleSignUp = async () => {
		if (!formData.code || formData.code.length !== 6) {
			toast.error('Confirmation code invalid', 'Code must be 6 characters long')
			return
		}
		setFirstMount(false)
		if (!capVal) {
			toast.error('Captcha validation failed', 'Please complete the captcha')
			return
		}
		if (validateData()) {
			try {
				const response = await axios.post('/api/auth/register', {
					email: formData.email,
					password: formData.password,
					code: formData.code,
				})
				if (response.status === 201) {
					toast.success('Sign up success', 'Please check your email to verify')
					onSwitch()
				}
			} catch (error: any) {
				toast.error('Sign up failed', error.response.data.message)
			}
		} else {
			toast.error('Sign up failed', 'Please check your input')
		}
	}

	const countDown = () => {
		setSendTimeout(30)
		const interval = setInterval(() => {
			setSendTimeout((prev) => prev - 1)
		}, 1000)
		setTimeout(() => {
			clearInterval(interval)
			setSendTimeout(0)
		}, 30000)
	}

	const handleSendCode = async () => {
		if (!formData.email) {
			toast.error('Email is required', 'Please enter your email address')
			return
		}
		try {
			const response = await axios.post('/api/auth/sendCode', {
				email: formData.email,
				isRegister: true,
			})
			if (response.status === 200) {
				toast.success('Confirmation code sent', 'Please check your email')
			}
			countDown()
		} catch (error: any) {
			toast.error('Failed to send code', error.response.data.message)
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
				<div className="flex w-[540px] flex-col items-center gap-4 rounded-xl bg-white px-12 py-8">
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
							onKeyDown={(e) => {
								if (e.key === 'Enter') {
									handleSignUp()
								}
							}}
						/>
						<p className="text-sm text-tertiary-1">{warnings.email}</p>
					</div>
					<div className="flex w-full flex-wrap items-center gap-1">
						<div className="mr-3 flex w-full flex-1 flex-col gap-1">
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
								onKeyDown={(e) => {
									if (e.key === 'Enter') {
										handleSignUp()
									}
								}}
							/>
						</div>
						<div className="flex w-full flex-1 flex-col gap-1">
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
								onKeyDown={(e) => {
									if (e.key === 'Enter') {
										handleSignUp()
									}
								}}
							/>
						</div>
						<p className="w-full text-sm text-tertiary-1">
							{warnings.password}
						</p>
					</div>

					<div className="flex w-full items-center gap-4">
						<div className="flex w-full flex-col items-center gap-2">
							<input
								type="text"
								className="h-9 w-[124px] border-2 border-borderCol-1 pl-4 pr-2 pt-[6px] font-oxygenMono tracking-[6px]"
								placeholder="000000"
								maxLength={6}
								value={formData.code}
								onChange={(e) => {
									if (e.target.value.match(/^\d{0,6}$/)) {
										setFormData({ ...formData, code: e.target.value })
									}
								}}
							/>
							<Button
								className="h-8 w-[120px] bg-secondary-1 text-white"
								onClick={handleSendCode}
								disabled={sendTimeout > 0}
							>
								{sendTimeout > 0 ? `Resend in ${sendTimeout}s` : 'Send Code'}
							</Button>
						</div>
						<ReCAPTCHA
							sitekey="6LcV0uYpAAAAABNE0DW6qJ8fPNjoydVhG_HYKo7u"
							onChange={(val) => setCapVal(val)}
						/>
					</div>
					<div className="mt-4 flex w-full items-center">
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
