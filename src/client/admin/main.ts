import { createApp } from 'vue'
import App from '@admin/App.vue'
import { router } from '@admin/router'
import { axios } from '@admin/helpers/axios'
import { createPinia } from 'pinia'
import piniaPersist from 'pinia-plugin-persistedstate'
import eventBus from '@admin/helpers/eventBus'

const app = createApp(App)
app.config.globalProperties.$axios = axios
app.config.globalProperties.$bus = eventBus

app.use(router)
const pinia = createPinia()
pinia.use(piniaPersist)
app.use(pinia)

app.mount('#app')
