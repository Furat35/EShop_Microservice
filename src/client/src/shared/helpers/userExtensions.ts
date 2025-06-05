import { AuthResponseDto } from '@shared/models/AuthModels/AuthResponseDto'

export function userIsAuthenticated(): boolean {
  const user = localStorage.getItem('userInfo')
  return !!user
}

export function getUsername() {
  const user = localStorage.getItem('userInfo')
  if (user === null) throw new Error('Kullanıcı bulunamadı')
  return (JSON.parse(user) as AuthResponseDto).userName
}

export function getUserId() {
  const user = localStorage.getItem('userInfo')
  if (user === null) throw new Error('Kullanıcı bulunamadı')
  let accessToken = (JSON.parse(user) as AuthResponseDto).accessToken
  const payloadBase64 = accessToken.split('.')[1]
  const payloadJson = atob(payloadBase64.replace(/-/g, '+').replace(/_/g, '/'))
  const payload = JSON.parse(payloadJson)

  var x = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
  console.log(x)
  return x
}

export function getUserTokens() {
  const user = localStorage.getItem('userInfo')
  if (user === null) throw new Error('Kullanıcı bulunamadı')
  let accessToken = (JSON.parse(user) as AuthResponseDto).accessToken
  let refreshToken = (JSON.parse(user) as AuthResponseDto).refreshToken
  return { accessToken, refreshToken }
}
