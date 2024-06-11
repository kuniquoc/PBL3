import { motion } from 'framer-motion'
import { variantsDefault, variantsY } from '../../styles/variants'
import { useState } from 'react'
import { Button } from '../../components'
import { PiCaretLeftBold } from 'react-icons/pi'
import { useToast } from '../../hook'
import axios from 'axios'

const ForgotPasswordForm: React.FC<{
	onClose: () => void
	onSwitch: () => void
}> = ({ onClose, onSwitch }) => {
	const [email, setEmail] = useState('')
	const [code, setCode] = useState('')
	const [warning, setWarning] = useState('')
	const [sendTimeout, setSendTimeout] = useState(0)
	const [haveSent, setHaveSent] = useState(false)
	const toast = useToast()

	const validateEmail = () => {
		if (!email) {
			setWarning('Email is required')
			return false
		} else if (!email.match(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g)) {
			setWarning('Invalid email')
			return false
		}
		setWarning('')
		return true
	}

	const handleSendCode = async () => {
		if (sendTimeout > 0 || !validateEmail()) return
		try {
			const res = await axios.post('/api/auth/sendCode', {
				email,
				isRegister: false,
			})
			if (res.status === 200) {
				toast.success(
					'Code sent',
					'Please check your email for the verification code',
				)
				setHaveSent(true)
				setSendTimeout(30)
				const interval = setInterval(() => {
					setSendTimeout((prev) => prev - 1)
				}, 1000)
				setTimeout(() => {
					clearInterval(interval)
				}, 30000)
			}
		} catch (error: any) {
			toast.error('Failed to send code', error.response.data.message)
		}
	}

	const handleReset = async () => {
		if (!code || code.length !== 6) {
			toast.error('Invalid code', 'Code must be 6 characters long')
			return
		}
		if (!validateEmail()) return
		try {
			const res = await axios.post('/api/auth/resetPassword', { email, code })
			if (res.status === 200) {
				toast.success('Success', 'Password reset successfully')
			}
			onSwitch()
		} catch (error: any) {
			toast.error('Failed', error.response.data.message)
		}
	}

	return (
		<motion.div
			className={`fixed left-0 top-0 z-10  flex h-screen w-screen items-center justify-center bg-[#0000004b]`}
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
				<motion.div
					className="flex w-[540px] flex-col items-center gap-4 rounded-xl bg-white px-12 py-8"
					transition={{
						layout: { duration: 0.2 },
					}}
					layout="preserve-aspect"
				>
					<div className="flex w-full items-center justify-between">
						<Button
							className="px-0 hover:text-primary-1 hover:underline"
							onClick={onSwitch}
						>
							<PiCaretLeftBold />
							Back
						</Button>
						<h1 className=" text-xl font-semibold tracking-wider">
							Reset Password
						</h1>
					</div>

					<div className="flex w-full flex-col gap-1">
						<label className="font-semibold" htmlFor="reset-email">
							Email
						</label>
						<div className="flex w-full items-center gap-4">
							<input
								className="h-10 w-full border-2 bg-[#fdfdfd] px-4 text-sm focus:border-2"
								type="email"
								id="reset-email"
								placeholder="Enter your email address"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								onKeyDown={(e) => {
									if (e.key === 'Enter') {
									}
								}}
							/>
							<Button
								className="h-10 w-[180px] bg-secondary-1 text-white"
								onClick={handleSendCode}
								disabled={sendTimeout > 0}
							>
								{sendTimeout > 0 ? `Resend in ${sendTimeout}s` : 'Send Code'}
							</Button>
						</div>
						<p className="text-sm text-tertiary-1">{warning}</p>
					</div>
					{haveSent && (
						<motion.div
							className="flex w-full flex-col gap-1"
							initial={{ opacity: 0, y: -10 }}
							animate={{ opacity: 1, y: 0 }}
							transition={{ duration: 0.2 }}
						>
							<label className="font-semibold" htmlFor="reset-code">
								Verification Code
							</label>
							<div className="flex w-full items-center justify-between gap-4">
								<input
									type="text"
									className="h-10 w-[150px] border-2 border-borderCol-1 pl-4 pr-2 pt-[6px] font-oxygenMono text-xl tracking-[8px] focus:border-2 "
									placeholder="000000"
									maxLength={6}
									value={code}
									onChange={(e) => {
										if (e.target.value.match(/^\d{0,6}$/)) {
											setCode(e.target.value)
										}
									}}
								/>
								<Button
									className="h-10 w-[150px] bg-primary-2 text-white hover:bg-primary-1"
									onClick={handleReset}
								>
									Reset Password
								</Button>
							</div>
						</motion.div>
					)}
				</motion.div>
			</motion.div>
		</motion.div>
	)
}
export default ForgotPasswordForm
