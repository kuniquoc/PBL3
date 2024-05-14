import { useCallback, useRef } from 'react'
import ReactQuill from 'react-quill'
import { uploadToCloudinary } from '../utils/Cloundinary'
import { twMerge } from 'tailwind-merge'

const TextEditor: React.FC<{
	className?: string
	placeholder?: string
	value: string
	onChange: (value: string) => void
}> = ({ className = '', placeholder = 'Enter text ...', value, onChange }) => {
	const reactQuillRef = useRef<ReactQuill>(null)

	const imageHandler = useCallback(() => {
		const input = document.createElement('input')
		input.setAttribute('type', 'file')
		input.setAttribute('accept', 'image/*')
		input.click()
		input.onchange = async () => {
			if (input !== null && input.files !== null) {
				const file = input.files[0]
				const url = await uploadToCloudinary(file)
				const quill = reactQuillRef.current
				if (quill) {
					const range = quill.getEditorSelection()
					range && quill.getEditor().insertEmbed(range.index, 'image', url)
				}
			}
		}
	}, [])

	return (
		<div className={twMerge(`${className}`)}>
			<ReactQuill
				className="content w-full rounded-lg border-borderCol-1"
				style={{ height: 'calc(100% - 44px)' }}
				ref={reactQuillRef}
				theme="snow"
				placeholder={placeholder}
				modules={{
					toolbar: {
						container: [
							[{ header: '1' }, { header: '2' }],
							['bold', 'italic', 'underline', 'blockquote'],
							[
								{ list: 'ordered' },
								{ list: 'bullet' },
								{ indent: '-1' },
								{ indent: '+1' },
							],
							['link', 'image'],
							['clean'],
						],
						handlers: {
							image: imageHandler,
						},
					},
					clipboard: {
						matchVisual: false,
					},
				}}
				formats={[
					'header',
					'bold',
					'italic',
					'underline',
					'blockquote',
					'list',
					'bullet',
					'indent',
					'link',
					'image',
				]}
				value={value}
				onChange={onChange}
			/>
		</div>
	)
}

export default TextEditor
