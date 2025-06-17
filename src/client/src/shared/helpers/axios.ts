import type { AuthResponseDto } from '@shared/models/AuthModels/AuthResponseDto'
import axios from 'axios'

let localStorageToken = localStorage.getItem('userInfo')
let accessToken =
  localStorageToken != null ? (JSON.parse(localStorageToken) as AuthResponseDto).accessToken : ''

export default axios.create({
  baseURL: import.meta.env.VITE_GATEWAY_URL,
  headers: { Authorization: accessToken != '' ? `Bearer ${accessToken}` : '' },
})
