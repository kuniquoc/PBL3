/* eslint-disable @typescript-eslint/no-unused-vars */
import { createContext, useReducer } from 'react'
import { toastReducer } from '../reducers/ToastReducer'
import { Toast } from '../types/global.types'
import ToastContainer from '../components/ToastContainer'

export const ToastContext = createContext({
	success: (_title: string, _message: string) => {},
	warning: (_title: string, _message: string) => {},
	error: (_title: string, _message: string) => {},
	info: (_title: string, _message: string) => {},
	remove: (_id: number) => {},
})

const defaultState = {
	toasts: [],
}

const ToastProvider = ({ children }: { children: React.ReactNode }) => {
	const [state, dispatch] = useReducer(toastReducer, defaultState)

	const addToast = (toast: Toast) => {
		const id = Math.floor(Math.random() * 10000000)
		dispatch({ type: 'ADD_TOAST', payload: { ...toast, id } })
	}

	const remove = (id: number) => {
		dispatch({
			type: 'REMOVE_TOAST',
			payload: { type: 'success', title: '', message: '', id },
		})
	}

	const success = (title: string, message: string) => {
		addToast({ type: 'success', title, message })
	}

	const warning = (title: string, message: string) => {
		addToast({ type: 'warning', title, message })
	}

	const error = (title: string, message: string) => {
		addToast({ type: 'error', title, message })
	}

	const info = (title: string, message: string) => {
		addToast({ type: 'info', title, message })
	}

	const value = {
		success,
		warning,
		error,
		info,
		remove,
	}

	return (
		<ToastContext.Provider value={value}>
			<ToastContainer toasts={state.toasts}>{children}</ToastContainer>
		</ToastContext.Provider>
	)
}

export default ToastProvider
