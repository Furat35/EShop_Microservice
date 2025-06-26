<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container px-4 px-lg-5">
            <router-link class="nav-link fs-4 fw-bold" style="font-style: italic;" aria-current="page"
                :to="{ name: 'catalog' }">Ortac</router-link>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false"
                aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0 ms-lg-4">
                    <li class="nav-item">
                        <router-link class="nav-link active" aria-current="page"
                            :to="{ name: 'catalog' }">Home</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link active" aria-current="page" :to="{ name: 'orders' }"
                            v-if="userStore.getIsAuthenticated">Orders</router-link>
                    </li>
                </ul>
                <form class="d-flex me-2">
                    <router-link :to="{ name: 'basket' }" class="btn btn-outline-dark" type="submit"
                        v-if="userStore.getIsAuthenticated">
                        <i class="bi-cart-fill me-1"></i>
                        Basket
                        <span class="badge bg-dark text-white ms-1 rounded-pill">{{ basketItemCount }}</span>
                    </router-link>
                    <router-link :to="{ name: 'login' }" class="btn btn-outline-dark" type="submit" v-else>
                        Login
                    </router-link>
                </form>
                <form class="d-flex">
                    <a @click="logout()" class="btn btn-outline-dark" type="submit" v-if="userStore.getIsAuthenticated">
                        Logout
                    </a>
                </form>
            </div>
        </div>
    </nav>

    <router-view></router-view>

    <footer class="py-5 bg-dark">
        <div class="container">
            <p class="m-0 text-center text-white">Copyright &copy; 2025</p>
        </div>
    </footer>
</template>

<script lang="ts">
import emitter from '../helpers/eventBus';
import { useUserStore } from '@user/helpers/store';
import { Toast } from '@shared/helpers/sweetAlertHelpers';

export default {
    data() {
        return {
            basketItemCount: 0
        }
    },
    mounted() {
        emitter.on('basket-updated', async () => {
            this.basketItemCount = await this.getCart();
        });
    },
    beforeUnmount() {
        emitter.off('basket-updated');
    },
    methods: {
        async getCart() {
            if (!useUserStore().getIsAuthenticated) return;
            const response = await this.$axios.get('basket');
            return response.data.data.items.length;
        },
        logout() {
            this.userStore.logout()
            Toast.fire({
                icon: "success",
                title: `Loged out`
            });
            this.$router.push({ name: 'catalog' })
        }
    },
    computed: {
        userStore() {
            return useUserStore()
        }
    }
}
</script>