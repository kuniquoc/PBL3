export type ScheduleGeneralType = {
	id: number
	title: string
	description: string
	start_date: string
	total_time: string
	total_budget: number
}

export type MyScheduleItemType = {
	general: ScheduleGeneralType
	status: string
	last_updated: string
	destinations: string[]
}

export type PublicScheduleItemType = {
	general: ScheduleGeneralType
	destinations: string[]
	author: string
}

export type ScheduleDestinationType = {
	id: number
	name: string
	address: string
	date: string
	arrival_time: string
	departure_time: string
	budget: number
	note: string
}

export type ScheduleDayType = {
	date: string
	destinations: ScheduleDestinationType[]
}

export type ScheduleType = {
	id: number
	title: string
	description: string
	isPublic: boolean
	status: string
	author: {
		id: number
		name: string
		avatar: string
	}
	updated_at: string
	details: ScheduleDayType[]
}
