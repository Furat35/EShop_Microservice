import { ComponentCustomProperties } from 'vue'
import { AxiosInstance } from 'axios'
import { Router } from 'vue-router'

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $axios: AxiosInstance // Add this line to declare $axios
    $router: Router
  }
}
