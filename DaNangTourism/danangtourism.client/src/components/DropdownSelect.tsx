import { twMerge } from 'tailwind-merge'

type DropdownSelectProps = {
	id: string
	className: string
	options: string[]
	value: number
	onChange: (event: React.ChangeEvent<HTMLSelectElement>) => void
	title?: string
}

const DropdownSelect: React.FC<DropdownSelectProps> = ({
	id,
	className,
	options,
	value,
	onChange,
	title = '',
}) => {
	return (
		<select
			className={twMerge(
				`rounded-md border-borderCol-1 bg-bgCol-2 px-3 py-[6px] text-sm ${className ?? ''} `,
			)}
			title={title}
			id={id}
			value={value}
			onChange={onChange}
		>
			{options.map((option, index) => (
				<option key={index} value={index}>
					{option}
				</option>
			))}
		</select>
	)
}

export default DropdownSelect
