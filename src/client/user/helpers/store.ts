import { LoginResponseDto } from '@shared/models/AuthModels/LoginResponseDto'
import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
  state: () => ({
    userInfo: new LoginResponseDto(),
  }),
  getters: {
    getUsername: (state) => state.userInfo.userName,
    getRefreshToken: (state) => state.userInfo.refreshToken,
    getAccessToken: (state) => state.userInfo.accessToken,
    getIsAuthenticated: (state) => !!(state.userInfo.accessToken && state.userInfo.refreshToken),
  },
  actions: {
    setUserInfo(loginResponseModel: LoginResponseDto) {
      Object.assign(this.userInfo, loginResponseModel)
    },
    logout() {
      this.$reset()
    },
  },
  persist: true,
})
