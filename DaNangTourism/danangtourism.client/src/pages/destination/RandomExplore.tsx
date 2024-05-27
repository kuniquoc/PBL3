import axios from 'axios'
import { useEffect, useState } from 'react'
import { Stars } from '../../components'
import { SortDesProps } from '../../types/destination'

const RandomExplore: React.FC = () => {
	const [randomDess, setRandomDess] = useState<SortDesProps[]>([])

	const getRandomDestination = async () => {
		try {
			const response = await axios.get('/api/destination/random.json')
			setRandomDess(response.data.data)
		} catch (error) {
			console.error(error)
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
			{randomDess.map((des) => (
				<RandomCard key={des.id} {...des} />
			))}
		</div>
	)
}

const RandomCard: React.FC<SortDesProps> = ({
	id,
	name,
	address,
	tags,
	rating,
	image,
}) => {
	return (
		<div
			className=" flex cursor-pointer items-center gap-3 rounded-lg border border-borderCol-1 bg-white p-3 transition-colors hover:bg-[#52cbff0e]"
			onClick={() => console.log('Go to des', id)}
		>
			<img className="h-20 w-20 rounded" src={image} />
			<div className="flex shrink grow basis-0 flex-col items-center justify-between self-stretch">
				<div className="font-['Open Sans'] self-stretch text-sm font-semibold text-slate-950">
					{name}
				</div>
				<div className="font-['Open Sans'] self-stretch text-xs font-normal text-slate-950">
					{address}
				</div>
				<div className="font-['Open Sans'] self-stretch text-xs font-normal text-primary-2">
					{tags.join(', ')}
				</div>
				<Stars rating={rating} className="w-full justify-start" />
			</div>
		</div>
	)
}

export default RandomExplore
