<script setup lang="ts">
import { RouterView } from 'vue-router'
import type { AuthResponseDto } from '@shared/models/AuthModels/AuthResponseDto';
import { useUserStore } from './helpers/store';
</script>

<template>
  <RouterView />
</template>

<script lang="ts">
export default {
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
        this.$axios.post('auth/refresh-token', useUserStore().getAccessToken)
          .then(res => {
            if (res)
              localStorage.setItem('userInfo', JSON.stringify(res.data));
          });
    }, 5 * 60 * 1_000)


  }
}

</script>