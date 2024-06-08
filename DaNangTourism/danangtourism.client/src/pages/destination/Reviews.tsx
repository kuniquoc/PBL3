import { ChangeEvent, useEffect, useState } from 'react'
import axios from 'axios'
import { twMerge } from 'tailwind-merge'
import { PiDotsThreeVerticalBold, PiStarFill } from 'react-icons/pi'
import { IGeneralReview, IReview } from '../../interfaces/destination'
import { Button, DropdownSelect, Pagination, Stars } from '../../components'
import { NumberFormat } from '../../utils/Format'
import { timeAgo } from '../../utils/TimeFormatters'
import { useToast, useUser } from '../../hook'

const SortOptions = [
	{
		value: 0,
		params: {
			sortBy: 'rating',
			sortType: 'desc',
		},
		label: 'Highest rating',
	},
	{
		value: 1,
		params: {
			sortBy: 'rating',
			sortType: 'asc',
		},
		label: 'Lowest rating',
	},
	{
		value: 2,
		params: {
			sortBy: 'created_at',
			sortType: 'desc',
		},
		label: 'Newest',
	},
	{
		value: 3,
		params: {
			sortBy: 'created_at',
			sortType: 'asc',
		},
		label: 'Oldest',
	},
]

const Reviews: React.FC<{
	destinationId: number
	className?: string
	general: IGeneralReview
	onChanged: () => void
}> = ({ destinationId, className, general, onChanged }) => {
	const [reviews, setReviews] = useState<IReview[]>()
	const [sortOption, setSortOption] = useState(0)
	const [currentPage, setCurrentPage] = useState(1)
	const [numbOfPages, setNumbOfPages] = useState(1)

	const handleGetReviews = async () => {
		try {
			const response = await axios.get('/api/review/list/' + destinationId, {
				params: {
					page: currentPage,
					limit: 3,
					...SortOptions[sortOption].params,
				},
			})
			const data = response.data.data
			setReviews(data.items)
			setNumbOfPages(Math.ceil(data.total / data.limit))
		} catch (error) {
			console.error(error)
		}
	}

	useEffect(() => {
		handleGetReviews()
	}, [destinationId, sortOption, currentPage])
	return (
		<div className={`flex gap-5 pt-5 ${className}`}>
			<div className="flex-1">
				<div className="relative flex min-h-[185.6px] flex-col items-center gap-4 rounded-lg border border-borderCol-1 bg-white p-4">
					<div className="relative mb-1 w-full items-center justify-center">
						<h2 className="text-center text-lg font-semibold">Reviews</h2>
						<DropdownSelect
							id={''}
							className="absolute right-0 top-0 w-[140px] border-2 focus:border-2"
							options={SortOptions.map((option) => option.label)}
							value={sortOption}
							onChange={(e: ChangeEvent<HTMLSelectElement>) => {
								setSortOption(Number(e.target.value))
							}}
						/>
					</div>
					{reviews ? (
						reviews.map((review) => (
							<Review
								key={review.id}
								review={review}
								onDeleted={() => {
									handleGetReviews()
									onChanged()
								}}
							/>
						))
					) : (
						<p className="mt-2 w-full rounded-lg bg-bgCol-3 py-6 text-center text-xl text-txtCol-3">
							This destination have no review yet
						</p>
					)}
					{reviews && (
						<Pagination
							className="mt-2"
							numbOfPages={numbOfPages}
							currentPage={currentPage}
							setCurrentPage={(numb) => {
								setCurrentPage(numb)
							}}
						/>
					)}
				</div>
			</div>
			<div className="item-center flex w-[380px] flex-col gap-5">
				<GeneralReview general={general} />
				<ReviewForm
					desId={destinationId}
					onPosted={() => {
						handleGetReviews()
						onChanged()
					}}
				/>
			</div>
		</div>
	)
}

const Review: React.FC<{
	review: IReview
	onDeleted: () => void
}> = ({
	review: { id, author, avatar, rating, comment, createdAt },
	onDeleted,
}) => {
	const toast = useToast()
	const [isShowDelete, setIsShowDelete] = useState(false)
	const handleDeleteReview = async () => {
		if (isShowDelete) {
			try {
				const response = await axios.delete('/api/review/delete/' + id)
				onDeleted()
				toast.success('Success', response.data.message)
			} catch (error: any) {
				console.error(error)
				toast.error('Error', error.response.data.message)
			}
			setIsShowDelete(false)
		}
	}

	return (
		<div className="w-full rounded-xl border bg-gray-50 p-3 shadow">
			<div className="relative flex items-center justify-between">
				<div className="flex items-center gap-3">
					<img
						className="h-5 w-5 rounded-full object-cover"
						src={avatar}
						alt={author + ' avatar'}
					/>
					<h3 className=" text-sm font-semibold">{author}</h3>
					<Stars rating={rating} className="" />
				</div>
				<button
					className="flex h-5 w-5 items-center justify-center rounded-full"
					onClick={() => setIsShowDelete(!isShowDelete)}
				>
					<PiDotsThreeVerticalBold />
				</button>
				{isShowDelete && (
					<div
						className="absolute right-0 top-0 -translate-x-6 cursor-pointer select-none rounded border border-tertiary-1 bg-white px-4 py-0.5 text-xs text-tertiary-1 shadow-lg transition-all hover:bg-[#e75b5110] active:scale-95"
						onClick={handleDeleteReview}
					>
						Delete
					</div>
				)}
			</div>
			<p className="mb-1 mt-2 text-sm leading-5">{comment}</p>
			<div className="flex w-full items-center justify-end">
				<p className=" text-xs text-txtCol-2">{timeAgo(createdAt)}</p>
			</div>
		</div>
	)
}

const GeneralReview: React.FC<{
	className?: string
	general: IGeneralReview
}> = ({ className, general }) => {
	return (
		<div
			className={twMerge(
				`flex flex-col items-center gap-3 rounded-lg border border-borderCol-1 bg-white p-3 ${className ?? ''}`,
			)}
		>
			<h2 className="text-center text-lg font-semibold">Review summary</h2>
			<div className="flex w-full items-center gap-2">
				<div className="flex-1">
					{[5, 4, 3, 2, 1].map((star) => {
						return (
							<div className="flex items-center gap-2" key={'star-' + star}>
								<span className="w-3">{star}</span>
								<div className="relative h-2 flex-1 rounded-full bg-[#F1F3F4]">
									<span
										className="absolute left-0 top-0 h-full rounded-full bg-[#FFC70D]"
										style={{ width: `${general.detail[star] * 100}%` }}
									></span>
								</div>
							</div>
						)
					})}
				</div>
				<div className="flex w-[120px] flex-col items-center justify-center">
					<span className=" mb-2 text-5xl font-semibold">
						{general.rating.toFixed(1)}
					</span>
					<Stars rating={general.rating} />
					<p className="text-sm">
						{NumberFormat(general.totalReview) + ' reviews'}
					</p>
				</div>
			</div>
		</div>
	)
}

const ReviewForm: React.FC<{
	className?: string
	desId: number
	onPosted: () => void
}> = ({ className, desId, onPosted }) => {
	const [review, setReview] = useState('')
	const [rating, setRating] = useState(0)
	const toast = useToast()
	const { user } = useUser()

	const submitReview = async (desId: number) => {
		if (!user || user.id === 0) {
			toast.error('Login required', 'Please log in to post a review')
			return
		}

		if (!review || review.length < 10) {
			toast.error('Review too short', 'Please write a longer review')
			return
		}
		if (!rating) {
			toast.error('Rating required', 'Please rate the destination')
			return
		}

		try {
			const response = await axios.post('/api/review/create', {
				destinationId: desId,
				rating,
				comment: review,
			})
			toast.success('Success', response.data.message)
			onPosted()
		} catch (error: any) {
			console.error(error)
			toast.error('Error', error.response.data.message)
		}
		setReview('')
		setRating(0)
	}

	return (
		<div
			className={twMerge(
				`flex flex-col items-center gap-3 rounded-lg border border-borderCol-1 bg-white p-3 ${className ?? ''}`,
			)}
		>
			<h2 className="text-center text-lg font-semibold">Write a review</h2>
			<textarea
				className="h-32 w-full resize-none rounded-md border border-borderCol-1 bg-gray-50 px-3 py-2 text-sm focus:border-primary-2"
				placeholder="Write your review here..."
				value={review}
				onChange={(e) => setReview(e.target.value)}
			></textarea>
			<div className="flex w-full items-center justify-between">
				<div className="item-center flex gap-1">
					<span className="mr-1 text-sm font-semibold text-txtCol-2">
						Rate:
					</span>
					{[1, 2, 3, 4, 5].map((star) => {
						return (
							<button
								key={star}
								className={`${rating >= star ? 'text-[#FFC70D]' : 'text-gray-200'} text-xl`}
								onClick={() => setRating(star)}
							>
								<PiStarFill />
							</button>
						)
					})}
				</div>
				<Button
					className="h-8 w-[100px] rounded-full bg-primary-2 text-sm font-semibold text-white hover:bg-primary-1"
					onClick={() => submitReview(desId)}
				>
					Post
				</Button>
			</div>
		</div>
	)
}

export default Reviews
