export const variantsDefault = {
	visible: { opacity: 1 },
	hidden: { opacity: 0 },
}

export const variantsY = {
	visible: { opacity: 1, y: 0 },
	top: (space: number) => ({
		opacity: 0,
		y: -space,
	}),
	bottom: (space: number) => ({
		opacity: 0,
		y: space,
	}),
}

export const variantsX = {
	visible: { opacity: 1, x: 0 },
	left: (space: number) => ({
		opacity: 0,
		x: -space,
	}),
	right: (space: number) => ({
		opacity: 0,
		x: space,
	}),
}
