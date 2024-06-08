export interface IAuthor {
	id: number
	name: string
	avatar: string
}

interface IBaseUser {
	id: number
	name: string
	email: string
	role: string
}

export interface IUserDetail extends IBaseUser {
	avatar: string
	dateOfBirth: string
}

export interface IUserManage extends IBaseUser {
	createdAt: string
}
