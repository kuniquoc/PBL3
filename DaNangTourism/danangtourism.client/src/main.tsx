import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './styles/index.css'
import UserProvider from './context/UserContext'
import ToastProvider from './context/ToastContext.tsx'
import ConfirmProvider from './context/ConfirmContext.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
	<UserProvider>
		<ToastProvider>
			<ConfirmProvider>
				<App />
			</ConfirmProvider>
		</ToastProvider>
	</UserProvider>,
)
