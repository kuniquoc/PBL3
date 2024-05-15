export type BlogType = {
	id: number
	title: string
	type: string
	image: string
	description: string
	author: string
	created_at: string
}

export type BlogDetailType = {
	id: number
	title: string
	type: string
	image: string
	description: string
	author: {
		id: number
		name: string
		avatar: string
	}
	created_at: string
	content: string
}
