import HomeView from '@admin/views/HomeView.vue'
import LoginView from '@admin/views/LoginView.vue'
import RegisterView from '@admin/views/RegisterView.vue'
import NotFoundView from '@admin/views/statusCodes/NotFoundView.vue'
import { createRouter, createWebHashHistory } from 'vue-router'
import CatalogComponent from '@admin/components/home/catalogs/index.vue'
import CatalogTypeComponent from '@admin/components/home/catalogTypes/index.vue'
import CatalogBrandComponent from '@admin/components/home/catalogBrands/index.vue'
import OrdersComponent from '@admin/components/home/orders/index.vue'
import ProfileComponent from '@admin/components/home/profile/index.vue'
import { useUserStore } from '@admin/helpers/store'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      name: 'home',
      path: '/admin/',
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
      path: '/admin/login',
      component: LoginView,
    },
    {
      name: 'register',
      path: '/admin/register',
      component: RegisterView,
    },
    {
      name: 'notFound',
      path: '/notFound',
      component: NotFoundView,
    },
  ],
})

const authNotRequired = ['login', 'register', 'notFound', 'home']
const pathsNotAllowedAfterLogin = ['register']

router.beforeEach((to, from, next) => {
  console.log()
  console.log(to.fullPath)
  console.log(to.fullPath)
  const isAuthenticated = useUserStore().getIsAuthenticated
  if (!isAuthenticated) {
    if (authNotRequired.includes(to.name as string)) next()
    else next({ name: 'notFound' })
  } else {
    if (pathsNotAllowedAfterLogin.includes(to.name as string)) return
    if (to.fullPath === '/' || to.fullPath === '/admin') next({ name: 'catalog' })
    next()
  }
})

export { router }
