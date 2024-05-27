import { ChangeEvent, useEffect, useState } from 'react'
import { GeneralReviewProps, ReviewProps } from '../../types/destination'
import axios from 'axios'
import { PiDotsThreeVerticalBold, PiStarFill } from 'react-icons/pi'
import { Button, DropdownSelect, Pagination, Stars } from '../../components'
import { twMerge } from 'tailwind-merge'
import { NumberFormat } from '../../utils/Format'
import { timeAgo } from '../../utils/TimeFormatters'

const SortOptions = ['Newest', 'Oldest', 'Highest rating', 'Lowest rating']

const Reviews: React.FC<{
	destinationId: number
	className?: string
	general: GeneralReviewProps
}> = ({ destinationId, className, general }) => {
	const [reviews, setReviews] = useState<ReviewProps[]>()
	const [sortOption, setSortOption] = useState(0)
	const fetchReviews = async (destinationId: number) => {
		const response = await axios.get(
			`/api/destination/reviews-${destinationId}.json`,
		)
		const data = response.data.data
		setReviews(data.items)
		setNumbOfPages(Math.ceil(data.total / data.limit))
		setCurrentPage(data.page)
	}
	const [currentPage, setCurrentPage] = useState(1)
	const [numbOfPages, setNumbOfPages] = useState(1)

	useEffect(() => {
		fetchReviews(destinationId)
	}, [destinationId])
	if (!reviews) return null
	return (
		<div className={`flex gap-5 pt-5 ${className}`}>
			<div className=" relative flex flex-1 flex-col items-center gap-4 rounded-lg border border-borderCol-1 bg-white p-4">
				<div className="relative mb-1 w-full items-center justify-center">
					<h2 className="text-center text-lg font-semibold">Reviews</h2>
					<DropdownSelect
						id={''}
						className="absolute right-0 top-0 w-[120px] border-2 focus:border-2"
						options={SortOptions}
						value={sortOption}
						onChange={(e: ChangeEvent<HTMLSelectElement>) => {
							setSortOption(Number(e.target.value))
						}}
					/>
				</div>
				{reviews.map((review) => (
					<Review key={review.id} {...review} />
				))}
				<Pagination
					className="mt-2"
					numbOfPages={numbOfPages}
					currentPage={currentPage}
					setCurrentPage={(numb) => {
						setCurrentPage(numb)
					}}
				/>
			</div>
			<div className="item-center flex w-[380px] flex-col gap-5">
				<GeneralReview general={general} />
				<ReviewForm destinationid={destinationId} />
			</div>
		</div>
	)
}

const Review: React.FC<ReviewProps> = ({
	author,
	avatar,
	rating,
	createdAt,
	comment,
}) => {
	return (
		<div className="w-full rounded-xl border bg-gray-50 p-3 shadow">
			<div className="flex items-center justify-between">
				<div className="flex items-center gap-3">
					<img
						className="h-5 w-5 rounded-full object-cover"
						src={avatar}
						alt={author + ' avatar'}
					/>
					<h3 className=" text-sm font-semibold">{author}</h3>
					<Stars rating={rating} className="" />
				</div>
				<button className="flex h-5 w-5 items-center justify-center rounded-full">
					<PiDotsThreeVerticalBold />
				</button>
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
	general: GeneralReviewProps
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

const ReviewForm: React.FC<{ className?: string; destinationid: number }> = ({
	className,
	destinationid,
}) => {
	const [review, setReview] = useState('')
	const [rating, setRating] = useState(0)

	const submitReview = async (destinationid: number) => {
		if (!review || review.length < 10) {
			alert('Please write a review')
			return
		}
		if (!rating) {
			alert('Please rate from 1-5 stars')
			return
		}
		console.log({ review, rating, destinationid })
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
					onClick={() => submitReview(destinationid)}
				>
					Post
				</Button>
			</div>
		</div>
	)
}

export default Reviews
