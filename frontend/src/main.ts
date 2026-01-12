// notify listeners (Navbar) when token changes on load/refresh
function broadcastTokenChange() {
  window.dispatchEvent(new Event('token-changed'))
}
import './assets/main.css'

import axios from 'axios'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

function getTokenExp(token: string): number | null {
  try {
    const payload = token.split('.')[1]
    if (!payload) return null
    const json = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')))
    return json.exp ?? null
  } catch {
    return null
  }
}

async function ensureFreshToken() {
  const token = localStorage.getItem('token')
  if (!token) return

  const exp = getTokenExp(token)
  if (!exp) {
    localStorage.removeItem('token')
    return
  }

  const now = Date.now() / 1000
  const bufferSeconds = 300

  if (exp - now <= bufferSeconds) {
    try {
      const res = await axios.post('/api/auth/refresh', null, {
        headers: { Authorization: `Bearer ${token}` }
      })
      const newToken = res.data.token
      localStorage.setItem('token', newToken)
      axios.defaults.headers.common['Authorization'] = `Bearer ${newToken}`
      broadcastTokenChange()
      return
    } catch (err) {
      localStorage.removeItem('token')
      broadcastTokenChange()
      return
    }
  }

  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
  broadcastTokenChange()
}

await ensureFreshToken()
const app = createApp(App)

app.use(router)

app.mount('#app')
