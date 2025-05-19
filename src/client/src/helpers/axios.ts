import type { AuthResponseDto } from '@/models/AuthModels/AuthResponseDto'
import axios from 'axios'

let localStorageToken = localStorage.getItem('userInfo')
let accessToken =
  localStorageToken != null ? (JSON.parse(localStorageToken) as AuthResponseDto).accessToken : ''

export default axios.create({
  baseURL: 'http://localhost:5000/',
  headers: { Authorization: accessToken != '' ? `Bearer ${accessToken}` : '' },
})
