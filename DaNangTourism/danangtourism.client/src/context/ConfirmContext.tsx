import React, { useRef, useState } from 'react'
import ConfirmDialog from '../components/ConfirmDialog'
import { AnimatePresence } from 'framer-motion'

type UseModalShowReturnType = {
	show: boolean
	setShow: (value: boolean) => void
	onHide: () => void
}

export const useModalShow = (): UseModalShowReturnType => {
	const [show, setShow] = useState(false)

	const handleOnHide = () => {
		setShow(false)
	}

	return {
		show,
		setShow,
		onHide: handleOnHide,
	}
}

export type ModalContextType = {
	showConfirmation: (title: string, message: string) => Promise<boolean>
}

type ConfirmProviderProps = {
	children: React.ReactNode
}

export const ConfirmContext = React.createContext<ModalContextType>(
	{} as ModalContextType,
)

const ConfirmProvider: React.FC<ConfirmProviderProps> = (props) => {
	const { setShow, show, onHide } = useModalShow()
	const [content, setContent] = useState<{
		title: string
		message: string
	} | null>()
	const resolver = useRef<Function>()

	const handleShow = (title: string, message: string): Promise<boolean> => {
		setContent({
			title,
			message,
		})
		setShow(true)
		return new Promise(function (resolve) {
			resolver.current = resolve
		})
	}

	const modalContext: ModalContextType = {
		showConfirmation: handleShow,
	}

	const handleOk = () => {
		resolver.current && resolver.current(true)
		onHide()
	}

	const handleCancel = () => {
		resolver.current && resolver.current(false)
		onHide()
	}

	return (
		<ConfirmContext.Provider value={modalContext}>
			{props.children}
			<AnimatePresence>
				{show && content && (
					<ConfirmDialog
						title={content.title}
						message={content.message}
						onOk={handleOk}
						onCancel={handleCancel}
					/>
				)}
			</AnimatePresence>
		</ConfirmContext.Provider>
	)
}

export default ConfirmProvider
