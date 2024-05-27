/* eslint-disable react-refresh/only-export-components */
/* eslint-disable @typescript-eslint/no-unused-vars */
import {
	createContext,
	useState,
	Dispatch,
	SetStateAction,
	ReactNode,
} from 'react'

export interface User {
	id: number
	name: string
	avatar: string
	email: string
	role: string
	rememberMe: boolean
}

export interface UserContextInterface {
	user: User
	setUser: Dispatch<SetStateAction<User>>
}

export const defaultUser = {
	user: {
		id: 0,
		name: '',
		avatar: '',
		email: '',
		role: '',
		rememberMe: false,
	},
	setUser: (_user: User) => {},
} as UserContextInterface

export const UserContext = createContext(defaultUser)

export default function UserProvider({ children }: { children: ReactNode }) {
	const [user, setUser] = useState<User>(defaultUser.user)

	return (
		<UserContext.Provider value={{ user, setUser }}>
			{children}
		</UserContext.Provider>
	)
}
