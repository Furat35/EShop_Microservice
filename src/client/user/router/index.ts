import HomeView from '@user/views/HomeView.vue'
import { createRouter, createWebHashHistory } from 'vue-router'
import CatalogComponent from '@user/components/home/catalog/index.vue'
import CatalogItemComponent from '@user/components/home/catalogItem/index.vue'
import BasketComponent from '@user/components/home/basket/index.vue'
import BasketCheckoutComponent from '@user/components/home/basketCheckout/index.vue'
import OrderSuccessComponent from '@user/components/home/basketCheckout/order-success.vue'
import OrderComponent from '@user/components/home/orders/index.vue'
import LoginView from '@user/views/LoginView.vue'
import RegisterView from '@user/views/RegisterView.vue'
import NotFoundView from '@user/views/statusCodes/NotFoundView.vue'
import { useUserStore } from '@user/helpers/store'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      name: 'home',
      path: '/',
      component: HomeView,
      redirect: { name: 'catalog' },
      children: [
        {
          name: 'catalog',
          path: 'catalog',
          component: CatalogComponent,
        },
        {
          name: 'catalogItem',
          path: 'catalog/:id',
          component: CatalogItemComponent,
        },
        {
          name: 'basket',
          path: 'basket',
          component: BasketComponent,
        },
        {
          name: 'basketCheckout',
          path: 'basketCheckout',
          component: BasketCheckoutComponent,
        },
        {
          path: '/order-success',
          name: 'OrderSuccess',
          component: OrderSuccessComponent,
        },
        {
          path: '/orders',
          name: 'orders',
          component: OrderComponent,
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

const authNotRequired = ['login', 'register', 'notFound', 'home', 'catalog', 'catalogItem']
const pathsNotAllowedAfterLogin = ['register']

router.beforeEach((to, from, next) => {
  const isAuthenticated = useUserStore().getIsAuthenticated
  if (!isAuthenticated) {
    if (authNotRequired.includes(to.name as string)) next()
    else next({ name: 'notFound' })
  } else {
    if (pathsNotAllowedAfterLogin.includes(to.name as string)) return
    next()
  }
})

export { router }
