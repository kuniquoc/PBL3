export interface IDesInfo {
	id: number
	name: string
	image: string
	address: string
	openTime: string
	closeTime: string
	tags: string[]
}

export interface IGeneralReview {
	rating: number
	totalReview: number
	detail: {
		[key: number]: number
	}
}

export interface IDesEditor extends IDesInfo {
	introduction: string
	googleMapUrl: string
}

export interface IDesDetail extends IDesEditor {
	favorite: boolean
	information: IDesInfo
	generalReview: IGeneralReview
}

export interface IReview {
	id: number
	author: string
	avatar: string
	rating: number
	comment: string
	createdAt: string
}
