interface IBDesInfo {
	localName: string
	address: string
	cost: number
	openTime: string
	closeTime: string
	tags: string[]
}

export interface IDesInfo extends IBDesInfo {
	rating: number
}

interface IBDesItem {
	id: number
	name: string
	address: string
	rating: number
}

export interface IDesLine extends IBDesItem {
	cost: number
	openTime: string
	closeTime: string
}

export interface IDesHome extends IBDesItem {
	image: string
}

export interface IDesCard extends IDesHome, IDesLine {
	tags: string[]
	favorite: boolean
}

export interface IGeneralReview {
	rating: number
	totalReview: number
	detail: { [key: number]: number }
}

interface IDesDetailInfo extends IBDesInfo {
	name: string
	images: string[]
}

export interface IDesDetail {
	id: number
	introduction: string
	googleMapUrl: string
	favorite: boolean
	generalReview: IGeneralReview
	information: IDesDetailInfo
}

export interface IDesEditor extends IDesDetailInfo {
	id: number
	introduction: string
	googleMapUrl: string
}

export interface IReview {
	id: number
	author: string
	avatar: string
	comment: string
	rating: number
	createdAt: string
}

export interface IDesManage {
	id: number
	name: string
	rating: number
	review: number
	favorite: number
	created_at: string
}
