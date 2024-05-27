const buttonVariants = {
	hover: {
		scale: 1.03,
	},
	tap: {
		scale: 0.97,
		opacity: 0.85,
	},
}

const BlogTypeColors: { [key: string]: string } = {
	all: 'bg-primary-3',
	places: 'bg-secondary-2',
	tips: 'bg-tertiary-3',
}

const buttonClass = `rounded-md py-1.5 px-3 gap-2 text-sm font-medium transition-all flex items-center justify-center`

export { buttonVariants, buttonClass, BlogTypeColors }
