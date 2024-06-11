import { IAuthor } from './user'

export const BlogTypes = ['all', 'places', 'tips']

export interface IBlogBase {
	id: number
	title: string
	type: string
	image: string
	createdAt: string
}

export interface IBlogCard extends IBlogBase {
	author: string
}

export interface IBlogDetail extends IBlogBase {
	description: string
	author: IAuthor
	views: number
	content: string
}

export interface IHomeBlog extends IBlogBase {
	author: string
}

export interface IBlogLine extends IBlogBase {
	views: number
	introduction: string
	author: IAuthor
}

export interface IBlogManage extends IBlogBase {
	author: string
	status: string
	views: number
}

export interface IMyBlog extends IBlogBase {
	views: number
	status: string
}

export interface IBlogEditor {
	title: string
	typeIndex: number
	introduction: string
	image: string
	content: string
}
