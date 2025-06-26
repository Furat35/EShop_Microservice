<template>
    <!-- Header-->
    <header class="bg-dark py-5">
        <div class="container px-4 px-lg-5 my-5">
            <div class="text-center text-white">
                <h1 class="display-4 fw-bolder">New Sezon Products</h1>
                <p class="lead fw-normal text-white-50 mb-0"></p>
            </div>
        </div>
    </header>
    <section class="py-3">
        <div class="container px-4">

            <div class="row mb-2">
                <div class="col-3">
                    <div class="dataTables_length" id="dataTable_length"><label class="me-2">Show <select
                                @change="changePageSize" name="dataTable_length" aria-controls="dataTable"
                                class="custom-select custom-select-sm form-control form-control-sm" v-model="pageSize">
                                <option value="25">25</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                            </select></label>
                    </div>
                </div>
                <div class="col-9" style="text-align: end;">
                    <div>

                        <div id="dataTable_filter" class="dataTables_filter ">
                            <label for="category-type" class="me-4" style="text-align: start;">Category
                                <select class="form-select form-select-sm" id="category-type"
                                    v-model="catalogItemFilter.catalogTypeId" @change="catalogTypeChanged">
                                    <option value="">All</option>
                                    <option v-for="catalogType in catalogTypes" :value="catalogType.id">{{
                                        catalogType.type }}</option>
                                </select></label>
                            <label for="category-brand" class="me-4" style="text-align: start;">Brand
                                <select class="form-select form-select-sm" id="category-brand"
                                    v-model="catalogItemFilter.catalogBrandId" @change="catalogBrandChanged">
                                    <option value="">All</option>
                                    <option v-for="catalogBrand in catalogBrands" :value="catalogBrand.id">{{
                                        catalogBrand.brand }}</option>
                                </select></label>
                            <label class="mr-2 me-2" style="text-align: start">Search
                                <input type="search" class="form-control form-control-sm" placeholder=""
                                    aria-controls="dataTable" @keydown.enter="search($event)"></label>
                            <button class="btn btn-success p-1" @click="clearSearchInput">Clear</button>
                        </div>
                    </div>
                </div>
                <div>

                </div>
            </div>

            <div class="row gx-4 row-cols-md-4 row-cols-xl-4">
                <div class="col mb-5" v-for="catalog in catalogs.data" :key="catalog.id">
                    <div class="card h-100">
                        <img class="card-img-top" :src="getApiGatewayUrl + 'img' + catalog.pictureUri" alt="..." />
                        <div class="card-body p-2 p-xl-4">
                            <div class="text-center">
                                <h5 class="fw-bolder" :title="catalog.name">{{ catalog.name.substring(0, 15) }} {{
                                    catalog.name.length > 0 ?
                                        '..' : '' }}</h5>
                                <span v-if="catalog.discountAmount">
                                    <span style="text-decoration: line-through;">{{ catalog.price }} TL </span>
                                    <span class="fw-bold fs-6 ms-2 me-1"> {{ (catalog.price -
                                        catalog.discountAmount).toFixed(2) }}
                                        TL</span>
                                    <span class="text-danger"> (-%{{ Math.round((catalog.discountAmount /
                                        catalog.price)
                                        * 100) }}) </span>
                                </span>
                                <span v-else>
                                    {{ catalog.price }} TL
                                </span>

                            </div>
                        </div>
                        <div class="card-footer p-3 pt-0 border-top-0 bg-transparent">
                            <div class="text-center">
                                <router-link class="btn btn-outline-dark mt-auto me-3 p-1 " style="width: 44%;"
                                    aria-current="page"
                                    :to="{ name: 'catalogItem', params: { id: catalog.id } }">View</router-link>
                                <router-link class="btn btn-success mt-auto p-1 fs-6" aria-current="page"
                                    style="width: 44%;" :to="{ name: 'catalog' }" @click="addToBasket(catalog)"><i
                                        class="bi-cart-fill me-1"></i> Add</router-link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row justify-content-center">
                <PaginationComponent @page-changed="pageChanged" :pagination-model="catalogs" />
            </div>

        </div>
    </section>
</template>

<script lang="ts">
import { Toast } from '@shared/helpers/sweetAlertHelpers';
import type { CatalogBrandListDto } from '@shared/models/CatalogBrands/CatalogBrandListDto';
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
            pageSize: 25,
            page: 0,
            catalogItemUpdate: new CatalogItemUpdateDto(),
            catalogItemFilter: {
                catalogTypeId: '',
                catalogBrandId: '',
            },
            catalogTypes: [] as CatalogTypeListDto[],
            catalogBrands: [] as CatalogBrandListDto[]
        }
    },
    async created() {
        await this.getCatalogs();
        this.getCatalogTypes();
        this.getCatalogBrands();
        emitter.emit('basket-updated');
    },
    computed: {
        getApiGatewayUrl() {
            return import.meta.env.VITE_GATEWAY_URL;
        }
    },
    methods: {
        changePageSize() {
            this.getCatalogs();
        },
        pageChanged(pagination) {
            this.getCatalogs(pagination.page);
        },
        catalogTypeChanged() {
            this.$axios.get(`catalogs/type/brand?catalogTypeId=${this.catalogItemFilter.catalogTypeId}&catalogBrandId=${this.catalogItemFilter.catalogBrandId}&page=${this.page}&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogs, res.data.data);
                    this.page = this.catalogs.page;
                });
        },
        catalogBrandChanged() {
            this.$axios.get(`catalogs/type/brand?catalogTypeId=${this.catalogItemFilter.catalogTypeId}&catalogBrandId=${this.catalogItemFilter.catalogBrandId}&page=${this.page}&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogs, res.data.data);
                    this.page = this.catalogs.page;
                });
        },
        async getCatalogs(page) {
            emitter.emit('show-spinner');
            await this.$axios.get(`catalogs?page=${page ?? this.page}&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogs, res.data.data);
                    this.page = this.catalogs.page;
                })
                .finally(err => emitter.emit('hide-spinner'));
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
                    title: "Search can't be empty"
                });
                return;
            }

            this.$axios.get(`catalogs/name/${name.target.value}?page=${this.page}&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogs, res.data.data);
                    this.page = this.catalogs.page;
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
            basketItem.quantity = 1
            basketItem.pictureUrl = item.pictureUri
            var response = await this.$axios.post(`basket/additem`, basketItem);
            if (response.status === 200)
                Toast.fire({
                    icon: "success",
                    title: `${basketItem.itemName} is added to basket`
                });
            else
                Toast.fire({
                    icon: "error",
                    title: `Error occured during operation!`
                });
            emitter.emit('basket-updated');

        }
    }

}
</script>