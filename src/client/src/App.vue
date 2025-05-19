<script setup lang="ts">
import { RouterView } from 'vue-router'
import { getUserTokens, userIsAuthenticated } from './helpers/userExtensions';
import type { AuthResponseDto } from './models/AuthModels/AuthResponseDto';
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
      if (userIsAuthenticated() && shouldGetRefreshToken())
        this.$axios.post('auth/refresh-token', getUserTokens())
          .then(res => {
            if (res)
              localStorage.setItem('userInfo', JSON.stringify(res.data));
          });
    }, 10_000)


  }
}

</script>