export interface DesListItemProps {
	id: number
	name: string
	address: string
	image: string
	rating: number
	cost: number
	openTime: string
	closeTime: string
	tags: string[]
	favorite: boolean
}

export interface DesInfoProps {
	name: string
	localName: string
	address: string
	images: string[]
	cost: number
	openTime: string
	closeTime: string
	tags: string[]
}

export interface GeneralReviewProps {
	rating: number
	totalReview: number
	detail: {
		[key: number]: number
	}
}

export interface DestinationEditorProps extends DesInfoProps {
	id: number
	introduction: string
	googleMapUrl: string
}

export interface DestinationDetailProps extends DestinationEditorProps {
	id: number
	favorite: boolean
	information: DesInfoProps
	introduction: string
	googleMapUrl: string
	generalReview: GeneralReviewProps
}

export interface ReviewProps {
	id: number
	author: string
	avatar: string
	rating: number
	comment: string
	createdAt: string
}

export interface SortDesProps {
	id: number
	name: string
	address: string
	rating: number
	image: string
}

export interface ManageDesProps {
	id: number
	name: string
	rating: number
	review: number
	favorite: number
	created_at: string
}
