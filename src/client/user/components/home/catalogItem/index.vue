<template>
    <section class="py-5">
        <div class="container px-4 px-lg-5 mt-5">
            <div class="row justify-content-center">
                <div class="col-5 mb-5">
                    <div class="card h-100">
                        <img class="card-img-top" :src="'http://localhost:5000/img' + item.pictureUri" alt="..." />
                        <div class="card-body p-4">
                            <div class="text-center">
                                <h5 class="fw-bolder">{{ item.name }}</h5>
                                {{ item.price }} TL
                            </div>
                        </div>
                        <div class="card-footer p-3 pt-0 border-top-0 bg-transparent">
                            <div class="text-center">
                                <router-link class="btn btn-success mt-auto p-1 fs-6" aria-current="page"
                                    style="width: 44%;" :to="{ name: 'catalog' }">Sepete Ekle</router-link>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-7 mb-5">
                    <div class="h-100">
                        <div class="h-100 card-body p-0 bg-white shadow-lg rounded-2xl overflow-hidden p-4 mt-6">
                            <div class="p-4">
                                <h2 class="text-xl font-semibold mb-2">{{ item.name }}</h2> <br>
                                <p class="text-green-600 font-bold mb-2">Fiyat : {{ item.price.toFixed(2) }} TL</p>
                                <p class="text-sm text-gray-500">Stok : {{ item.availableStock }}</p>
                                <hr>
                                <p class="text-gray-600 mb-2">Açıklama <br> {{ item.description }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>


</template>

<script lang="ts">
import { CatalogItemListDto } from '@shared/models/CatalogItems/CatalogItemListDto';

export default {
    data() {
        return {
            item: new CatalogItemListDto() as CatalogItemListDto
        }
    },
    async created() {
        const id = this.$router.currentRoute.value.params.id;
        try {
            const response = await this.$axios.get(`catalogs/${id}`)
            Object.assign(this.item, response.data.data)
        } catch (err) {
            console.error(err)
        }
    }
}

</script>