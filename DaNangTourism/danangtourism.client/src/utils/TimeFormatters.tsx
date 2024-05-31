const timeAgo = (dateString: string): string => {
	const date = new Date(dateString)
	const now = new Date()
	const seconds = Math.floor((now.getTime() - date.getTime()) / 1000)
	let interval = Math.floor(seconds / 31536000)
	if (interval > 1) {
		return `${interval} years ago`
	}
	interval = Math.floor(seconds / 2592000)
	if (interval > 1) {
		return `${interval} months ago`
	}
	interval = Math.floor(seconds / 604800)
	if (interval > 1) {
		return `${interval} weeks ago`
	}
	interval = Math.floor(seconds / 86400)
	if (interval > 1) {
		return `${interval} days ago`
	}
	interval = Math.floor(seconds / 3600)
	if (interval > 1) {
		return `${interval} hours ago`
	}
	interval = Math.floor(seconds / 60)
	if (interval > 1) {
		return `${interval} minutes ago`
	}
	if (interval <= 1) {
		return 'Just now'
	}
	return `${Math.floor(seconds)} seconds ago`
}

const dayOfWeek = (date: string): string => {
	const days = [
		'Sunday',
		'Monday',
		'Tuesday',
		'Wednesday',
		'Thursday',
		'Friday',
		'Saturday',
	]
	const day = new Date(date).getDay()
	return days[day]
}

const monthOfYear = (month: number) => {
	const months = [
		'January',
		'February',
		'March',
		'April',
		'May',
		'June',
		'July',
		'August',
		'September',
		'October',
		'November',
		'December',
	]
	return months[month]
}

const dateDecay = (date: string) => {
	const dateObj = new Date(date)
	return {
		day: dateObj.getDate().toString().padStart(2, '0'),
		month: monthOfYear(dateObj.getMonth()),
		monthNumber: (dateObj.getMonth() + 1).toString().padStart(2, '0'),
		year: dateObj.getFullYear(),
	}
}

const toDisplayDate = (date: string) => {
	const dateDecayed = dateDecay(date)
	return `${dateDecayed.day}-${dateDecayed.monthNumber}-${dateDecayed.year}`
}

const datetimeDecay = (date: string) => {
	const dateObj = new Date(new Date(date).getTime() - 7 * 60 * 60 * 1000)
	return {
		day: dateObj.getDate().toString().padStart(2, '0'),
		month: monthOfYear(dateObj.getMonth()),
		monthNumber: (dateObj.getMonth() + 1).toString().padStart(2, '0'),
		year: dateObj.getFullYear(),
		hour: dateObj.getHours().toString().padStart(2, '0'),
		minute: dateObj.getMinutes().toString().padStart(2, '0'),
	}
}

const toDisplayDateTime = (date: string) => {
	const dateDecayed = datetimeDecay(date)
	return `${dateDecayed.day}-${dateDecayed.monthNumber}-${dateDecayed.year} ${dateDecayed.hour}:${dateDecayed.minute}`
}
export {
	timeAgo,
	dayOfWeek,
	dateDecay,
	toDisplayDate,
	datetimeDecay,
	toDisplayDateTime,
}
