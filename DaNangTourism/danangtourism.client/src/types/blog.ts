import { AuthorProps } from './user'

export interface BlogCardProps {
	id: number
	title: string
	type: string
	image: string
	author: string
	createdAt: string
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
	createdAt: string
	views: number
	content: string
}

export interface HomeBlogProps {
	id: number
	type: string
	title: string
	image: string
	author: string
	createdAt: string
}

export interface BlogLineProps {
	id: number
	title: string
	type: string
	image: string
	createdAt: string
	views: number
	introduction: string
	author: AuthorProps
}

export interface ManageBlogProps {
	id: number
	title: string
	type: string
	author: string
	createdAt: string
	status: string
	views: number
}
