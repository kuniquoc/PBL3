import { useContext } from 'react'
import { UserContext, defaultUser } from '../context/UserContext'
import axios from 'axios'

const useUser = () => {
	const { user, setUser } = useContext(UserContext)

	const LoadUser = async () => {
		try {
			const res = await axios.get('/api/auth/authenticated')
			const resData = await res.data.data
			setUser({
				...resData,
				rememberMe: true,
			})
		} catch (error) {
			setUser(defaultUser.user)
		}
	}

	return { LoadUser, user, setUser, defaultUser }
}

export default useUser
