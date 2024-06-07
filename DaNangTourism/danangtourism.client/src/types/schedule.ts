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

export interface ScheduleItemProps {
	id: number
	title: string
	description: string
	destinations: string[]
	startDate: string
	totalDays: number
	totalBudget: number
}

export interface MyScheduleItemProps extends ScheduleItemProps {
	status: string
	updatedAt: string
}

export interface PublicScheduleItemProps extends ScheduleItemProps {
	creator: string
}

export interface ScheduleDestinationProps {
	id: number
	destinationId: number
	name: string
	address: string
	arrivalTime: string
	leaveTime: string
	budget: number
	note: string
}

export interface ScheduleDayProps {
	date: string
	destinations: ScheduleDestinationProps[]
}

export interface ScheduleDetailProps extends ScheduleItemProps {
	status: string
	updatedAt: string
	creator: string
	isPublic: boolean
	numbOfDes: number
	days: ScheduleDayProps[]
}

export interface ScheduleGeneralProps {
	title: string
	description: string
	isPublic: boolean
	status: string
}
