import { PiCaretLeftLight, PiCaretRightLight } from 'react-icons/pi'
import { useEffect, useState } from 'react'
import { twMerge } from 'tailwind-merge'
type PaginationProps = {
	className?: string
	numbOfPages: number
	currentPage: number
	setCurrentPage: React.Dispatch<React.SetStateAction<number>>
}

const Pagination: React.FC<PaginationProps> = ({
	className = '',
	numbOfPages,
	currentPage,
	setCurrentPage,
}) => {
	const pages = Array.from({ length: numbOfPages }, (_, i) => i + 1)
	const [displayPages, setDisplayPages] = useState<number[]>([])
	useEffect(() => {
		if (numbOfPages <= 7) setDisplayPages(pages)
		else {
			if (currentPage <= 4) setDisplayPages([1, 2, 3, 4, 5, -1, numbOfPages])
			else if (currentPage == 5) {
				if (numbOfPages == 8) setDisplayPages([1, -1, 4, 5, 6, 7, 8])
				else setDisplayPages([1, -1, 4, 5, 6, -1, numbOfPages])
			} else if (currentPage >= numbOfPages - 3)
				setDisplayPages([
					1,
					-1,
					numbOfPages - 4,
					numbOfPages - 3,
					numbOfPages - 2,
					numbOfPages - 1,
					numbOfPages,
				])
			else
				setDisplayPages([
					1,
					-1,
					currentPage - 1,
					currentPage,
					currentPage + 1,
					-1,
					numbOfPages,
				])
		}
	}, [currentPage, numbOfPages])

	return (
		<div className={twMerge(`flex items-center gap-2 ${className ?? ''}`)}>
			<button
				className="pagination-node flex gap-2 border border-borderCol-1 pl-2 pr-3 hover:border-primary-2"
				onClick={() => currentPage > 1 && setCurrentPage(currentPage - 1)}
			>
				<PiCaretLeftLight />
				Prev
			</button>
			{displayPages &&
				displayPages.map((page) => (
					<button
						key={page}
						className={`pagination-node flex gap-2 pl-3 pr-3 hover:border-primary-2 
            ${currentPage === page ? 'bg-primary-1 text-bgCol-1' : ''}
            ${page === -1 ? 'cursor-default bg-transparent' : 'cursor-pointer border border-borderCol-1'}
            `}
						onClick={() => {
							if (page !== -1) setCurrentPage(page)
						}}
					>
						{page === -1 ? '...' : page}
					</button>
				))}
			<button
				className="pagination-node flex gap-2 border border-borderCol-1 pl-3 pr-2 hover:border-primary-2"
				onClick={() =>
					currentPage < numbOfPages && setCurrentPage(currentPage + 1)
				}
			>
				Next
				<PiCaretRightLight />
			</button>
		</div>
	)
}

export default Pagination
