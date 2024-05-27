import { ToastProps } from '../types/global'

interface State {
	toasts: ToastProps[]
}

interface Action {
	type: string
	payload: ToastProps
}

export const toastReducer = (state: State, action: Action) => {
	switch (action.type) {
		case 'ADD_TOAST':
			return {
				...state,
				toasts: [...state.toasts, action.payload],
			}
		case 'REMOVE_TOAST': {
			const updatedToasts = state.toasts.filter(
				(toast: ToastProps) => toast.id !== action.payload.id,
			)
			return {
				...state,
				toasts: updatedToasts,
			}
		}
		default:
			throw new Error(`Unsupported action type ${action.type}`)
	}
}
