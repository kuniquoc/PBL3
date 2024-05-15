import { AnimatePresence } from 'framer-motion'
import { ToastProps } from '../types/global.types'
import Toast from './Toast'

const ToastContainer: React.FC<{
	toasts: ToastProps[]
	children: React.ReactNode
}> = ({ toasts, children }) => {
	return (
		<>
			{children}
			<div className="fixed right-5 top-14 z-50 flex flex-col gap-3">
				<AnimatePresence>
					{toasts.map((toast) => (
						<Toast key={toast.id} {...toast} />
					))}
				</AnimatePresence>
			</div>
		</>
	)
}
export default ToastContainer
