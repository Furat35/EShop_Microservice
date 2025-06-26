import { useUserStore } from '@admin/helpers/store'
import axios from 'axios'

const instance = axios.create({
  baseURL: import.meta.env.VITE_GATEWAY_URL,
})

instance.interceptors.request.use((config) => {
  const userStore = useUserStore()
  const token = userStore.getAccessToken

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

export { instance as axios }
