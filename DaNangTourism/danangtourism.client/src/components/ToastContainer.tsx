import { AnimatePresence } from 'framer-motion'
import { IToast } from '../interfaces/global'
import Toast from './Toast'

const ToastContainer: React.FC<{
	toasts: IToast[]
	children: React.ReactNode
}> = ({ toasts, children }) => {
	return (
		<>
			{children}
			<div className="fixed bottom-5 left-5 z-50 flex flex-col gap-3">
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
