import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'

import { ThemeProvider } from './context/ThemeContext'

const normalizeLegacySpaRoute = () => {
  const { pathname, search, hash, origin } = window.location

  // HashRouter expects app routes after "#/".
  // When users open "/admin/login" directly on the deployed static site,
  // React only sees "/" and the route falls through.
  if (!hash && pathname !== '/' && pathname !== '/index.html') {
    window.location.replace(`${origin}/#${pathname}${search}`)
  }
}

normalizeLegacySpaRoute()

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ThemeProvider>
      <App />
    </ThemeProvider>
  </React.StrictMode>,
)

