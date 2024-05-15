import { motion } from 'framer-motion'

const PageNotFound: React.FC = () => {
	document.title = 'Page Not Found'
	return (
		<div className="mx-auto flex h-screen items-center justify-center py-14 xl:max-w-screen-xl">
			<motion.div
				className="flex flex-col items-center justify-center gap-4"
				initial={{
					opacity: 0,
					y: -80,
				}}
				animate={{
					opacity: 1,
					y: 0,
				}}
				transition={{ duration: 1.5, type: 'spring', stiffness: 200 }}
			>
				<h1 className="text-8xl font-bold tracking-wider">404</h1>
				<h2 className="text-4xl font-semibold">Page Not Found</h2>
			</motion.div>
		</div>
	)
}

export default PageNotFound
