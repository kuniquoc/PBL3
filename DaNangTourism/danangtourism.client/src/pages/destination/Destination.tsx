import { useParams } from 'react-router-dom'
import PageNotFound from '../PageNotFound'
import DesImgSlider from './DesImgSlider'
import { DestinationDetailProps } from '../../types/destination'
import DesInfo from './DesInfo'
import RandomExplore from './RandomExplore'
import { useContext, useEffect, useState } from 'react'
import axios from 'axios'
import { PiCalendarPlusBold, PiHeartFill, PiShareFatFill } from 'react-icons/pi'
import { ToggleButton, Button } from '../../components/Buttons'
import Reviews from './Reviews'
import Loader from '../../components/Loader'
import { useToast } from '../../hook/useToast'
import { UserContext } from '../../context/UserContext'

const Destination: React.FC = () => {
	const [destination, setDestination] = useState<
		DestinationDetailProps | undefined
	>(undefined)
	const [loading, setLoading] = useState(true)
	const { id } = useParams()
	const toast = useToast()

	const getDestination = async (id: string) => {
		try {
			const response = await axios.get('/api/destination/detail/' + id)

			setDestination(response.data.data)
		} catch (error: any) {
			console.error(error)
			toast.error('Error', error.response.data.message)
		}
		setLoading(false)
	}

	useEffect(() => {
		const parsedId = parseInt(id ?? '', 10)
		if (isNaN(parsedId)) {
			return
		} else {
			getDestination(parsedId.toString())
		}
	}, [id])

	useEffect(() => {
		document.title =
			(destination?.information.name ?? 'Destination') + ' | Danang Tourism'
	}, [destination])

	if (loading)
		return (
			<div className="mx-auto mt-12 flex min-h-screen items-center justify-center xl:max-w-screen-xl">
				<Loader />
			</div>
		)
	else if (!destination) return <PageNotFound />
	else
		return (
			<div className="mx-auto min-h-screen xl:max-w-screen-xl">
				<div className="w-full pb-4 pt-[64px] text-txtCol-1 ">
					<DesImgSlider
						className="w-full"
						imgUrls={destination.information.images}
						name={destination.information.name}
					/>
				</div>
				<ButtonsBar
					destinationId={id ? Number(id) : 0}
					initFavorite={destination.favorite}
					mapUrl={destination.googleMapUrl}
				/>
				<div className=" mt-3 flex w-full gap-5 pt-5">
					<div className=" relative flex-1 rounded-lg border border-borderCol-1 bg-white p-5 pt-8">
						<div className="absolute left-1/2 top-0 w-[240px] translate-x-[-50%] translate-y-[-50%] rounded border border-borderCol-1 bg-white py-1 text-center text-base font-bold tracking-widest">
							Introduction
						</div>
						<div
							className="ql-editor flex flex-col gap-[10px]"
							dangerouslySetInnerHTML={{ __html: destination.introduction }}
						></div>
					</div>
					<div className="flex w-[380px] flex-col items-center gap-4">
						<DesInfo
							localName={destination.information.localName}
							address={destination.information.address}
							rating={destination.generalReview.rating}
							cost={destination.information.cost}
							openTime={destination.information.openTime}
							closeTime={destination.information.closeTime}
							tags={destination.information.tags}
						/>
						<RandomExplore />
					</div>
				</div>
				<Reviews
					destinationId={id ? Number(id) : 0}
					className="mb-5 w-full"
					general={destination?.generalReview}
					onChanged={() => getDestination(id ?? '')}
				/>
			</div>
		)
}

const ButtonsBar: React.FC<{
	destinationId: number
	mapUrl?: string
	initFavorite: boolean
}> = ({ destinationId, mapUrl, initFavorite }) => {
	const [favorite, setFavorite] = useState(initFavorite)
	const { user } = useContext(UserContext)
	const toast = useToast()
	const handleChangeFavorite = async () => {
		if (!user || user.id === 0) {
			toast.error('Error', 'Please login to favorite this destination')
			return
		}
		setFavorite(!favorite)
		try {
			await axios.put('/api/destination/favorite', {
				destinationId,
				favorite,
			})
		} catch (error: any) {
			console.error(error)
			toast.error('Error', error.response.data.message)
		}
	}

	return (
		<div className="flex w-full items-center justify-between ">
			<div className="flex gap-3">
				<ToggleButton
					className="w-[120px] border-2 font-semibold"
					onClick={handleChangeFavorite}
					text="Favorite"
					toggledText="Favorited"
					icon={<PiHeartFill className="text-xl" />}
					initToggled={initFavorite}
					btnColor="#ee685e"
				/>

				<Button
					className="w-[120px] border-2 border-secondary-1 font-semibold text-secondary-1 hover:bg-[#daf8f0]"
					onClick={() => {
						window.open(mapUrl, '_blank')
					}}
				>
					Google Map
				</Button>
			</div>
			<div className="flex gap-3">
				<Button
					className="border-2 border-txtCol-1 font-semibold text-txtCol-1 hover:bg-[#e8e8ff]"
					onClick={() => {
						console.log('Add to schedule ', destinationId)
					}}
				>
					<PiCalendarPlusBold className="text-xl" />
					Add to schedule
				</Button>
				<Button
					className="bg-primary-2 font-semibold text-white hover:bg-primary-1"
					onClick={() => {
						console.log('Share ', destinationId)
					}}
				>
					<PiShareFatFill className="text-xl" />
					Share
				</Button>
			</div>
		</div>
	)
}

export default Destination
