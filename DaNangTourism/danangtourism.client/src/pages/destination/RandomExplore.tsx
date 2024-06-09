import { useEffect, useState } from 'react'
import axios from 'axios'
import { Stars } from '../../components'
import { IDesHome } from '../../interfaces/destination'
import { useToast } from '../../hook'

const RandomExplore: React.FC = () => {
	const [randomDess, setRandomDess] = useState<IDesHome[]>([])
	const toast = useToast()
	const getRandomDestination = async () => {
		try {
			const response = await axios.get('/api/destination/random', {
				params: { limit: 3 },
			})
			setRandomDess(response.data.data)
		} catch (error) {
			console.error(error)
			toast.error(
				'Error',
				'Failed to fetch random destinations. Please try again later',
			)
		}
	}
	useEffect(() => {
		getRandomDestination()
	}, [])

	return (
		<div className="flex w-full flex-col gap-2">
			<div className="mb-0.5 flex h-8 w-full items-center justify-center rounded border border-borderCol-1 bg-white font-bold tracking-wide">
				Random Explore
			</div>
			{randomDess?.map((des) => <RandomCard key={des.id} {...des} />)}
		</div>
	)
}

const RandomCard: React.FC<IDesHome> = ({
	id,
	name,
	address,
	rating,
	image,
}) => {
	return (
		<div
			className=" flex cursor-pointer items-center gap-3 rounded-lg border border-borderCol-1 bg-white p-2 transition-colors hover:bg-[#52cbff0e]"
			onClick={() => console.log('Go to des', id)}
		>
			<img className="h-20 w-20 rounded object-cover" src={image} />
			<div className="flex h-full w-full flex-col justify-between py-0.5">
				<div className="text-left font-semibold text-slate-950">{name}</div>
				<div className="mb-2 line-clamp-1 text-[13px] font-normal text-slate-950">
					{address}
				</div>
				<Stars rating={rating} className="w-full justify-start text-xl" />
			</div>
		</div>
	)
}

export default RandomExplore
