<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container px-4 px-lg-5">
            <router-link class="nav-link fs-4" style="font-style: italic;" aria-current="page"
                :to="{ name: 'catalog' }">Ortac</router-link>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false"
                aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0 ms-lg-4">
                    <li class="nav-item">
                        <router-link class="nav-link active" aria-current="page" :to="{ name: 'catalog' }">Ana
                            Sayfa</router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link active" aria-current="page"
                            :to="{ name: 'orders' }">Siparişlerim</router-link>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" id="navbarDropdown" href="#" role="button"
                            data-bs-toggle="dropdown" aria-expanded="false">Shop</a>
                        <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <li><a class="dropdown-item" href="#!">All Products</a></li>
                            <li>
                                <hr class="dropdown-divider" />
                            </li>
                            <li><a class="dropdown-item" href="#!">Popüler</a></li>
                            <li><a class="dropdown-item" href="#!">Yeni</a></li>
                        </ul>
                    </li>
                </ul>
                <form class="d-flex">
                    <router-link :to="{ name: 'basket' }" class="btn btn-outline-dark" type="submit">
                        <i class="bi-cart-fill me-1"></i>
                        Sepetim
                        <span class="badge bg-dark text-white ms-1 rounded-pill">{{ basketItemCount }}</span>
                    </router-link>
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
            const response = await this.$axios.get('basket');
            return response.data.data.items.length;
        },
    }
}
</script>