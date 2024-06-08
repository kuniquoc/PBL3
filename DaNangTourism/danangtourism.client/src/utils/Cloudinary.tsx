const uploadToCloudinary = async (file: File): Promise<string> => {
	const formData = new FormData()
	formData.append('file', file)
	formData.append('upload_preset', 'danang_explore')
	const res = await fetch(
		`https://api.cloudinary.com/v1_1/${'dxhuysuy5'}/upload`,
		{ method: 'POST', body: formData },
	)
	const data = await res.json()
	const url = data.url

	return url
}

export { uploadToCloudinary }
