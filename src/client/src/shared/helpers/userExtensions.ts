import { useUserStore } from '@admin/helpers/store'

export function getUsername() {
  if (!useUserStore().getIsAuthenticated) throw new Error('Kullanıcı bulunamadı')
  return useUserStore().getUsername
}

export function getUserId() {
  if (!useUserStore().getIsAuthenticated) throw new Error('Kullanıcı bulunamadı')
  let accessToken = useUserStore().getAccessToken
  const payloadBase64 = accessToken.split('.')[1]
  const payloadJson = atob(payloadBase64.replace(/-/g, '+').replace(/_/g, '/'))
  const payload = JSON.parse(payloadJson)

  var x = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
  return x
}
