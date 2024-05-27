import { PiMagnifyingGlassBold } from 'react-icons/pi'
import { useState } from 'react'
import { twMerge } from 'tailwind-merge'

type SearchBoxProps = {
	className?: string
	id?: string
	onChangeValue: (event: React.ChangeEvent<HTMLInputElement>) => void
	onClickSearch: () => void
}

const SearchBox: React.FC<SearchBoxProps> = ({
	className,
	id,
	onChangeValue,
	onClickSearch,
}) => {
	const [searchFocus, setSearchFocus] = useState(false)
	return (
		<div
			className={twMerge(
				`flex items-center rounded-md border border-borderCol-1 bg-bgCol-2 px-2 py-[6px]  ${searchFocus ? 'border-primary-2' : ''}`,
				className,
			)}
			id={id}
		>
			<input
				type="text"
				placeholder="Search"
				className="h-full w-full border-none bg-transparent text-sm focus:border-none"
				onFocus={() => setSearchFocus(true)}
				onBlur={() => setSearchFocus(false)}
				onChange={(event) => {
					onChangeValue(event)
				}}
			/>
			<button
				className="rounded-full p-1.5 text-txtCol-3 hover:bg-slate-100 hover:text-primary-1"
				onClick={onClickSearch}
			>
				<PiMagnifyingGlassBold />
			</button>
		</div>
	)
}

export default SearchBox
