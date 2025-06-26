<script setup lang="ts">
import { RouterView } from 'vue-router'
import type { AuthResponseDto } from '@shared/models/AuthModels/AuthResponseDto';
import emitter from './helpers/eventBus';
import { useUserStore } from './helpers/store';
</script>

<template>
  <RouterView />
  <div class="loading-backdrop" v-if="visible">
    <div class="spinner"></div>
  </div>
</template>

<script lang="ts">
export default {
  data() {
    return {
      visible: false
    }
  },
  beforeCreate() {
    const shouldGetRefreshToken = () => {
      if (!localStorage.getItem('userInfo'))
        return;
      console.log('test')
      const user = JSON.parse(localStorage.getItem('userInfo')!) as AuthResponseDto;
      const token = user?.accessToken;
      if (!token) return false;

      const payload = JSON.parse(atob(token.split('.')[1]));
      var expireDate = new Date(payload.exp * 1000)
      const exp = payload.exp / 1000 / 60;

      return Date.now() > expireDate.getTime() - 2 * 60 * 1000;
    }
    setInterval(() => {
      if (useUserStore().getIsAuthenticated && shouldGetRefreshToken())
        this.$axios.post('auth/refresh-token', { accessToken: useUserStore().getAccessToken, refreshToken: useUserStore().getRefreshToken })
          .then(res => {
            if (res)
              localStorage.setItem('userInfo', JSON.stringify(res.data));
          });
    }, 5 * 60 * 1_000)

    emitter.on('show-spinner', () => this.visible = true);
    emitter.on('hide-spinner', () => this.visible = false);
  },
  beforeUnmount() {
    emitter.off('show-spinner');
    emitter.off('hide-spinner');
  }
}

</script>

<style scoped>
.loading-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(255, 255, 255, 0.3);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.spinner {
  border: 8px solid #f3f3f3;
  border-top: 8px solid #007bff;
  border-radius: 50%;
  width: 60px;
  height: 60px;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}
</style>