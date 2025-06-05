<template>
    <!-- Header-->
    <header class="bg-dark py-5">
        <div class="container px-4 px-lg-5 my-5">
            <div class="text-center text-white">
                <h1 class="display-4 fw-bolder">Yeni Sezon Ürünleri</h1>
                <p class="lead fw-normal text-white-50 mb-0"></p>
            </div>
        </div>
    </header>
    <section class="py-5">
        <div class="container px-4 px-lg-5 mt-5">
            <div class="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify-content-center">
                <div class="col mb-5" v-for="catalog in catalogs.data" :key="catalog.id">
                    <div class="card h-100">
                        <img class="card-img-top" :src="'http://localhost:5000/img' + catalog.pictureUri" alt="..." />
                        <div class="card-body p-4">
                            <div class="text-center">
                                <h5 class="fw-bolder">{{ catalog.name }}</h5>
                                <span v-if="catalog.discountAmount">
                                    <span style="text-decoration: line-through;">{{ catalog.price }}</span>
                                    {{ catalog.discountAmount }} (%{{ Math.round((catalog.discountAmount /
                                        catalog.price) * 100) }}) TL</span>
                                <span>{{ catalog.price }} TL</span>

                            </div>
                        </div>
                        <div class="card-footer p-3 pt-0 border-top-0 bg-transparent">
                            <div class="text-center">
                                <router-link class="btn btn-outline-dark mt-auto me-3 p-1 " style="width: 44%;"
                                    aria-current="page"
                                    :to="{ name: 'catalogItem', params: { id: catalog.id } }">İncele</router-link>
                                <router-link class="btn btn-success mt-auto p-1 fs-6" aria-current="page"
                                    style="width: 44%;" :to="{ name: 'catalog' }" @click="addToBasket(catalog)">Sepete
                                    Ekle</router-link>
                            </div>
                        </div>
                    </div>
                </div>
                <PaginationComponent @page-changed="pageChanged" :pagination-model="catalogs" />
            </div>
            <div class="row" style="text-align: center;">
            </div>

        </div>
    </section>
</template>

<script lang="ts">
import { Toast } from '@shared/helpers/sweetAlertHelpers';
import type { CatalogBrandListDto } from '@shared/models/CatalogBrands/CatalogBrandListDto';
import { CatalogItemCreateDto } from '@shared/models/CatalogItems/CatalogItemCreateDto';
import { CatalogItemListDto } from '@shared/models/CatalogItems/CatalogItemListDto';
import { CatalogItemUpdateDto } from '@shared/models/CatalogItems/CatalogItemUpdateDto';
import { CatalogTypeListDto } from '@shared/models/CatalogTypes/CatalogTypeListDto';
import { PaginationModel } from '@shared/models/PaginationModel';
import { BasketItemListDto } from '@shared/models/BasketItems/BasketItemListDto';
import emitter from '../../../helpers/eventBus';
import PaginationComponent from '@user/components/shared/pagination.vue'

export default {
    components: {
        PaginationComponent
    },
    data() {
        return {
            catalogs: new PaginationModel<CatalogItemListDto>(),
            pageSize: 12,
            pageIndex: 0,
            catalogItemUpdate: new CatalogItemUpdateDto(),
            createCatalogItem: new CatalogItemCreateDto(),
            catalogTypes: [] as CatalogTypeListDto[],
            catalogBrands: [] as CatalogBrandListDto[]
        }
    },
    async created() {
        this.getCatalogs();
        this.getCatalogTypes();
        this.getCatalogBrands();
        emitter.emit('basket-updated');
    },
    methods: {
        pageChanged(pagination) {
            this.pageIndex = pagination.pageIndex;
            this.getCatalogs();
        },
        async getCatalogs() {
            emitter.emit('show-spinner');
            await this.$axios.get(`catalogs?pageIndex=${this.pageIndex}&&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogs, res.data.data);
                    this.pageIndex = this.catalogs.pageIndex;
                    emitter.emit('hide-spinner');
                });
        },
        changePageIndex(pageIndex: number) {
            this.pageIndex = pageIndex;
            this.getCatalogs();
        },
        getCatalogById(catalogId: number) {
            return this.$axios.get(`catalogs/${catalogId}`)
                .then(res => res.data.data);
        },
        getCatalogTypes() {
            return this.$axios.get(`catalogs/types`)
                .then(res => Object.assign(this.catalogTypes, res.data.data.data));
        },
        getCatalogBrands() {
            return this.$axios.get(`catalogs/brands`)
                .then(res => Object.assign(this.catalogBrands, res.data.data.data));
        },
        search(name: any) {
            if (!name.target.value) {
                Toast.fire({
                    icon: "error",
                    title: "Ara alanı boş olamaz"
                });
                return;
            }

            this.$axios.get(`catalogs/name/${name.target.value}?pageIndex=${this.pageIndex}&&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogs, res.data);
                    this.pageIndex = this.catalogs.pageIndex;
                });
        },
        clearSearchInput(event: any) {
            this.getCatalogs();
            event.target.value = '';
        },
        async addToBasket(item) {
            const basketItem = new BasketItemListDto();
            basketItem.itemId = item.id
            basketItem.itemName = item.name
            basketItem.unitPrice = item.price
            basketItem.discountAmount = 0
            basketItem.quantity = 1
            basketItem.pictureUrl = item.pictureUri
            var response = await this.$axios.post(`basket/additem`, basketItem);
            if (response.status === 200)
                Toast.fire({
                    icon: "success",
                    title: `${basketItem.itemName} sepete eklendi`
                });
            else
                Toast.fire({
                    icon: "error",
                    title: `${basketItem.itemName} sepete eklenirken sorun oluştur. Tekrar deneyiniz!`
                });
            emitter.emit('basket-updated');

        }
    }

}
</script>