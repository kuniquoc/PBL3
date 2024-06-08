import { IToast } from '../interfaces/global'

interface State {
	toasts: IToast[]
}

interface Action {
	type: string
	payload: IToast
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
				(toast: IToast) => toast.id !== action.payload.id,
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
