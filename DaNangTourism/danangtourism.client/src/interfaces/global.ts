export interface Toast {
	type: 'success' | 'warning' | 'error' | 'info'
	title: string
	message: string
}

export interface IToast extends Toast {
	id: number
}
