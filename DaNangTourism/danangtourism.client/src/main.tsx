import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './styles/index.css'
import UserProvider from './context/UserContext'
import ToastProvider from './context/ToastContext.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
	<UserProvider>
		<ToastProvider>
			<App />
		</ToastProvider>
	</UserProvider>,
)
