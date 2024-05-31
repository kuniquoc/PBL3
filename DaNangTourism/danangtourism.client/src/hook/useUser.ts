import { useContext } from 'react'
import { UserContext, defaultUser } from '../context/UserContext'
import { useToast } from './useToast'
import axios from 'axios'

const useUser = () => {
	const { user, setUser } = useContext(UserContext)
	const toast = useToast()

	const LoadUser = async () => {
		try {
			const res = await axios.get('/api/auth/authenticated')
			const resData = await res.data.data
			setUser({
				...resData,
				rememberMe: true,
			})
			toast.success('Welcome back!', `Hello, ${resData.name}!`)
		} catch (error) {
			setUser(defaultUser.user)
		}
	}

	return { LoadUser, user, setUser, defaultUser }
}

export default useUser
