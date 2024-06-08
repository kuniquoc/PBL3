import { useContext } from 'react'
import { ModalContextType, ConfirmContext } from '../context/ConfirmContext'

const useConfirm = (): ModalContextType => useContext(ConfirmContext)

export default useConfirm
