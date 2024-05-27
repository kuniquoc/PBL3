const SliderNav: React.FC<{
	count: number
	current: number
	activeNodeWidth?: number
	onClick: (index: number) => void
}> = ({ count, current, onClick, activeNodeWidth = 60 }) => {
	return (
		<div className="flex items-center justify-center gap-2 rounded-[inherit] text-base">
			{Array.from({ length: count }).map((_, index) => (
				<button
					key={index}
					className={` h-2 rounded-full transition-all duration-1000 hover:bg-primary-3 ${
						current === index ? 'bg-white' : ' w-2 bg-borderCol-1'
					}`}
					style={{
						width:
							activeNodeWidth && current === index
								? `${activeNodeWidth}px`
								: '8px',
					}}
					onClick={() => onClick(index)}
				></button>
			))}
		</div>
	)
}

export default SliderNav
