export type DestinationType = {
	imgs: string[]
	general: GeneralType
	introduction: string
}

type GeneralType = {
	id: number
	name: string
	localName: string
	address: string
	rating: number
	cost: number
	openTime: string
	closeTime: string
	tags: string[]
}

export type ReviewsType = {
	summary: ReviewSummaryType
	list: ReviewType[]
}

export type ReviewSummaryType = {
	count: number
	average: number
	chart: RatingRatioType
}

type RatingRatioType = {
	[key: number]: number
}

export type ReviewType = {
	id: number
	name: string
	avatar: string
	rating: number
	time: string
	content: string
}
