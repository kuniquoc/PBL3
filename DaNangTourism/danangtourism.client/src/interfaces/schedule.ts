export const ScheduleStatus = [
	{
		status: 'All',
		color: undefined,
	},
	{
		status: 'Planning',
		color: 'bg-[#F1C142]',
	},
	{
		status: 'Ongoing',
		color: 'bg-primary-2',
	},
	{
		status: 'Completed',
		color: 'bg-[#8187DC]',
	},
	{
		status: 'Canceled',
		color: 'bg-tertiary-1',
	},
]

export interface IScheduleItem {
	id: number
	title: string
	description: string
	destinations: string[]
	startDate: string
	totalDays: number
	totalBudget: number
}

export interface IMySchedule extends IScheduleItem {
	status: string
	updatedAt: string
}

export interface IPublicSchedule extends IScheduleItem {
	creator: string
}

export interface IScheduleDes {
	id: number
	destinationId: number
	name: string
	address: string
	arrivalTime: string
	leaveTime: string
	budget: number
	note: string
}

export interface IScheduleDay {
	date: string
	destinations: IScheduleDes[]
}

export interface IScheduleDetail extends IScheduleItem {
	status: string
	updatedAt: string
	creator: string
	isPublic: boolean
	numbOfDes: number
	days: IScheduleDay[]
}

export interface IScheduleGeneral {
	title: string
	description: string
	isPublic: boolean
	status: string
}
