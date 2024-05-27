export interface Toast {
	type: 'success' | 'warning' | 'error' | 'info'
	title: string
	message: string
}

export interface ToastProps extends Toast {
	id: number
}
