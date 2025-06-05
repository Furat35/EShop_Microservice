import { createApp } from 'vue'
import App from '@user/App.vue'
import { router } from '@user/router'
import axios from '@shared/helpers/axios'

const app = createApp(App)
app.use(router)
app.config.globalProperties.$axios = axios

app.mount('#app')
