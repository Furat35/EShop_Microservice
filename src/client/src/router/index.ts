import { userIsAuthenticated } from '@/helpers/userExtensions'
import HomeView from '@/views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import NotFoundView from '@/views/statusCodes/NotFoundView.vue'
import { createRouter, createWebHashHistory } from 'vue-router'
import CatalogComponent from '@/components/home/catalogs/index.vue'
import CatalogTypeComponent from '@/components/home/catalogTypes/index.vue'
import CatalogBrandComponent from '@/components/home/catalogBrands/index.vue'
import OrdersComponent from '@/components/home/orders/index.vue'
import ProfileComponent from '@/components/home/profile/index.vue'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      name: 'home',
      path: '/',
      component: HomeView,
      children: [
        {
          name: 'catalog',
          path: 'catalog',
          component: CatalogComponent,
        },
        {
          name: 'catalogType',
          path: 'catalogTypes',
          component: CatalogTypeComponent,
        },
        {
          name: 'catalogBrand',
          path: 'catalogBrand',
          component: CatalogBrandComponent,
        },
        {
          name: 'orders',
          path: 'orders',
          component: OrdersComponent,
        },
        {
          name: 'profile',
          path: 'profile',
          component: ProfileComponent,
        },
      ],
    },
    {
      name: 'login',
      path: '/login',
      component: LoginView,
    },
    {
      name: 'register',
      path: '/register',
      component: RegisterView,
    },
    {
      name: 'notFound',
      path: '/notFound',
      component: NotFoundView,
    },
  ],
})

const authNotRequired = ['login', 'register', 'notFound']
const pathsNotAllowedAfterLogin = ['register']

router.beforeEach((to, from, next) => {
  const isAuthenticated = userIsAuthenticated()
  if (!isAuthenticated) {
    if (authNotRequired.includes(to.name as string)) next()
    else next({ name: 'notFound' })
  } else {
    if (pathsNotAllowedAfterLogin.includes(to.name as string)) return
    next()
  }
})

export { router }
