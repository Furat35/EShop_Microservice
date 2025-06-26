import { createApp } from 'vue'
import App from '@user/App.vue'
import { router } from '@user/router'
import { axios } from '@user/helpers/axios'
import { createPinia } from 'pinia'
import piniaPersist from 'pinia-plugin-persistedstate'
import eventBus from './helpers/eventBus'

const app = createApp(App)
app.config.globalProperties.$axios = axios
app.config.globalProperties.$bus = eventBus

app.use(router)
const pinia = createPinia()
pinia.use(piniaPersist)
app.use(pinia)

app.mount('#app')
