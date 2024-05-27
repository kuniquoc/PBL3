export interface AuthorProps {
	id: number
	name: string
	avatar: string
}

export interface UserDetailsProps {
	id: number
	name: string
	avatar: string
	email: string
	role: string
	dateOfBirth: string
}

export interface ManageUserProps {
	id: number
	name: string
	email: string
	role: string
	createdAt: string
}
