<template>
    <h1 class="h3 mb-2 text-gray-800">Katalog</h1>
    <div class="card shadow mb-4">
        <div class="card-body">
            <div class="table-responsive">
                <div id="dataTable_wrapper" class="dataTables_wrapper dt-bootstrap4">
                    <div class="row">
                        <div class="col-sm-12 col-md-6">
                            <div class="dataTables_length" id="dataTable_length"><label>Göster <select
                                        @change="changePageSize" name="dataTable_length" aria-controls="dataTable"
                                        class="custom-select custom-select-sm form-control form-control-sm"
                                        v-model="pageSize">
                                        <option value="10">10</option>
                                        <option value="25">25</option>
                                        <option value="50">50</option>
                                        <option value="100">100</option>
                                    </select></label>
                                <button type="button" class="btn btn-primary ml-2" data-dismiss="modal"
                                    data-toggle="modal" data-target="#createCatalogModal">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                        class="bi bi-person-add" viewBox="0 0 16 16">
                                        <path
                                            d="M12.5 16a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7m.5-5v1h1a.5.5 0 0 1 0 1h-1v1a.5.5 0 0 1-1 0v-1h-1a.5.5 0 0 1 0-1h1v-1a.5.5 0 0 1 1 0m-2-6a3 3 0 1 1-6 0 3 3 0 0 1 6 0M8 7a2 2 0 1 0 0-4 2 2 0 0 0 0 4" />
                                        <path
                                            d="M8.256 14a4.5 4.5 0 0 1-.229-1.004H3c.001-.246.154-.986.832-1.664C4.484 10.68 5.711 10 8 10q.39 0 .74.025c.226-.341.496-.65.804-.918Q8.844 9.002 8 9c-5 0-6 3-6 4s1 1 1 1z" />
                                    </svg> Ekle
                                </button>
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-6 " style="text-align: end;">
                            <div id="dataTable_filter" class="dataTables_filter ">
                                <label class="mr-2">Ara
                                    <input type="search" class="form-control form-control-sm" placeholder=""
                                        aria-controls="dataTable" @keydown.enter="search($event)"></label>
                                <button class="btn btn-success p-1" @click="clearSearchInput">Temizle</button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <table class="table table-bordered dataTable" id="dataTable" width="100%" cellspacing="0"
                                role="grid" aria-describedby="dataTable_info" style="width: 100%;">
                                <thead>
                                    <tr role="row">
                                        <th class="col-1" tabindex="0" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="#: activate to sort column descending">#
                                        </th>
                                        <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="Name: activate to sort column descending">
                                            Ürün Adı</th>
                                        <th class="col-1" tabindex="2" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Description: activate to sort column ascending">
                                            Açıklama</th>
                                        <th class="col-1" tabindex="3" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Price: activate to sort column ascending">Fiyat</th>
                                        <th class="col-1" tabindex="4" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Available Stock: activate to sort column ascending">
                                            Stok</th>
                                        <th class="col-1" tabindex="5" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Catalog Type: activate to sort column ascending">
                                            Kategori</th>
                                        <th class="col-1" tabindex="6" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Catalog Brand: activate to sort column ascending">
                                            Marka</th>
                                        <th class="col-1" tabindex="7" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Salary: activate to sort column ascending">İşlemler
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(catalog, index) in catalogs.data"
                                        :class="index % 2 == 0 ? 'even' : 'odd'" :key="catalog.id">
                                        <td>{{ index + 1 }}</td>
                                        <td class="sorting_1">{{ catalog.name }}</td>
                                        <td>{{ catalog.description }}</td>
                                        <td>{{ catalog.price }}</td>
                                        <td>{{ catalog.availableStock }}</td>
                                        <td>{{ catalog.catalogType.type }}</td>
                                        <td>{{ catalog.catalogBrand.brand }}</td>
                                        <td>
                                            <button type="button" data-toggle="modal" data-target="#updateCatalogModal"
                                                class="btn btn-outline-success" style="margin-right: 10px;"
                                                @click="showupdateCatalogModal(catalog.id)">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13"
                                                    fill="currentColor" class="bi bi-building-dash" viewBox="0 0 13 13">
                                                    <path
                                                        d="M12.5 16a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7M11 12h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1 0-1" />
                                                    <path
                                                        d="M2 1a1 1 0 0 1 1-1h10a1 1 0 0 1 1 1v6.5a.5.5 0 0 1-1 0V1H3v14h3v-2.5a.5.5 0 0 1 .5-.5H8v4H3a1 1 0 0 1-1-1z" />
                                                    <path
                                                        d="M4.5 2a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm-6 3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm-6 3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5z" />
                                                </svg>
                                            </button>
                                            <button type="button" class="btn btn-outline-danger"
                                                @click="deleteCatalog(catalog.name, catalog.id)">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13"
                                                    fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 13 13">
                                                    <path
                                                        d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                                    <path fill-rule="evenodd"
                                                        d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z" />
                                                </svg>
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-5">
                            <div class="dataTables_info" id="dataTable_info" role="status" aria-live="polite">
                                Toplam Kayıt : {{ catalogs.count }}
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-7">
                            <div class="dataTables_paginate paging_simple_numbers" id="dataTable_paginate">
                                <ul class="pagination">
                                    <li :class="['paginate_button', 'page-item', 'previous', catalogs.hasPrevious ? '' : 'disabled']"
                                        id="dataTable_previous"><button aria-controls="dataTable"
                                            :data-dt-idx="catalogs.pageIndex" tabindex="0" class="page-link"
                                            @click="changePageIndex(catalogs.pageIndex - 1)">Önceki</button></li>
                                    <li class="paginate_button page-item" v-if="catalogs.hasPrevious">
                                        <button @click="changePageIndex(catalogs.pageIndex - 1)"
                                            aria-controls="dataTable" :data-dt-idx="catalogs.pageIndex" tabindex="0"
                                            class="page-link">{{
                                                catalogs.pageIndex }}</button>
                                    </li>
                                    <li class="paginate_button page-item active">
                                        <button @click="changePageIndex(catalogs.pageIndex)" aria-controls="dataTable"
                                            :data-dt-idx="catalogs.pageIndex + 1" tabindex="0" class="page-link">{{
                                                catalogs.pageIndex + 1 }}</button>
                                    </li>
                                    <li class="paginate_button page-item" v-if="catalogs.hasNext">
                                        <button @click="changePageIndex(catalogs.pageIndex + 1)"
                                            aria-controls="dataTable" :data-dt-idx="catalogs.pageIndex + 2" tabindex="0"
                                            class="page-link">{{
                                                catalogs.pageIndex + 2 }}</button>
                                    </li>
                                    <li :class="['paginate_button', 'page-item', 'next', catalogs.hasNext ? '' : 'disabled']"
                                        id="dataTable_next"><button href="#" aria-controls="dataTable"
                                            :data-dt-idx="catalogs.pageIndex + 2" tabindex="0" class="page-link"
                                            @click="changePageIndex(catalogs.pageIndex + 1)">Sonraki</button></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Update Modal -->
    <div class="modal fade" id="updateCatalogModal" tabindex="-1" aria-labelledby="updateCatalogModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="updateCatalogModal">Güncelle</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent>
                        <div class="form-group">
                            <label for="id" class="col-form-label" hidden>Id</label>
                            <input type="text" class="form-control" id="id" hidden v-model="catalogItemUpdate.id">
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-form-label">Ürün Adı</label>
                            <input type="text" class="form-control" id="name" v-model="catalogItemUpdate.name">
                        </div>
                        <div class="form-row">
                            <div class="form-group col">
                                <label for="price" class="col-form-label">Fiyat</label>
                                <input type="text" class="form-control" id="price" v-model="catalogItemUpdate.price">
                            </div>
                            <div class="form-group col">
                                <label for="available-stock" class="col-form-label">Stock</label>
                                <input type="text" class="form-control" id="available-stock"
                                    v-model="catalogItemUpdate.availableStock">
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="description" class="col-form-label">Açıklama</label>
                            <textarea class="form-control" id="description"
                                v-model="catalogItemUpdate.description"></textarea>
                        </div>
                        <div class="form-row">
                            <div class="form-group col">
                                <label for="category-type">Kategori</label>
                                <select class="form-control" id="category-type"
                                    v-model="catalogItemUpdate.catalogTypeId">
                                    <option v-for="catalogType in catalogTypes" :value="catalogType.id">{{
                                        catalogType.type }}</option>
                                </select>
                            </div>
                            <div class="form-group col">
                                <label for="category-brand">Marka</label>
                                <select class="form-control" id="category-brand"
                                    v-model="catalogItemUpdate.catalogBrandId">
                                    <option v-for="catalogBrand in catalogBrands" :value="catalogBrand.id">{{
                                        catalogBrand.brand }}</option>
                                </select>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" @click="updateCatalog">Kaydet</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Create Modal -->
    <div class="modal fade" id="createCatalogModal" tabindex="-1" aria-labelledby="createCatalogModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="createCatalogModal">Ekle</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent>
                        <div class="form-group">
                            <label for="id" class="col-form-label" hidden>Id</label>
                            <input type="text" class="form-control" id="id" hidden v-model="createCatalogItem.id">
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-form-label">Ürün Adı</label>
                            <input type="text" class="form-control" id="name" v-model="createCatalogItem.name">
                        </div>
                        <div class="form-row">
                            <div class="form-group col">
                                <label for="price" class="col-form-label">Fiyat</label>
                                <input type="text" class="form-control" id="price" v-model="createCatalogItem.price">
                            </div>
                            <div class="form-group col">
                                <label for="available-stock" class="col-form-label">Stock</label>
                                <input type="text" class="form-control" id="available-stock"
                                    v-model="createCatalogItem.availableStock">
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="description" class="col-form-label">Açıklama</label>
                            <textarea class="form-control" id="description"
                                v-model="createCatalogItem.description"></textarea>
                        </div>
                        <div class="form-row">
                            <div class="form-group col">
                                <label for="category-type">Kategori</label>
                                <select class="form-control" id="category-type"
                                    v-model="createCatalogItem.catalogTypeId">
                                    <option v-for="catalogType in catalogTypes" :value="catalogType.id">{{
                                        catalogType.type }}</option>
                                </select>
                            </div>
                            <div class="form-group col">
                                <label for="category-brand">Marka</label>
                                <select class="form-control" id="category-brand"
                                    v-model="createCatalogItem.catalogBrandId">
                                    <option v-for="catalogBrand in catalogBrands" :value="catalogBrand.id">{{
                                        catalogBrand.brand }}</option>
                                </select>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-success" @click="createCatalog">Kaydet</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Toast } from '@/helpers/sweetAlertHelpers';
import type { CatalogBrandListDto } from '@/models/CatalogBrands/CatalogBrandListDto';
import { CatalogItemCreateDto } from '@/models/CatalogItems/CatalogItemCreateDto';
import { CatalogItemListDto } from '@/models/CatalogItems/CatalogItemListDto';
import { CatalogItemUpdateDto } from '@/models/CatalogItems/CatalogItemUpdateDto';
import { CatalogTypeListDto } from '@/models/CatalogTypes/CatalogTypeListDto';
import { PaginationModel } from '@/models/PaginationModel';
import Swal from 'sweetalert2'

export default {
    data() {
        return {
            catalogs: new PaginationModel<CatalogItemListDto>(),
            pageSize: 10,
            pageIndex: 0,
            catalogItemUpdate: new CatalogItemUpdateDto(),
            createCatalogItem: new CatalogItemCreateDto(),
            catalogTypes: [] as CatalogTypeListDto[],
            catalogBrands: [] as CatalogBrandListDto[]
        }
    },
    async created() {
        this.getCatalogs();
        await this.getCatalogTypes();
        await this.getCatalogBrands();
    },
    methods: {
        async getCatalogs() {
            await this.$axios.get(`catalogs?pageIndex=${this.pageIndex}&&pageSize=${this.pageSize}`)
                .then(res => {
                    var x = res.data.data;
                    Object.assign(this.catalogs, res.data.data);
                    this.pageIndex = this.catalogs.pageIndex;
                })
                .catch(err => console.log(`${err.status} : ${err}`));
        },
        changePageIndex(pageIndex: number) {
            this.pageIndex = pageIndex;
            this.getCatalogs();
        },
        changePageSize() {
            this.getCatalogs();
        },
        getCatalogById(catalogId: number) {
            return this.$axios.get(`catalogs/${catalogId}`)
                .then(res => res.data.data)
                .catch(err => console.log(`${err.status} : ${err}`));
        },
        getCatalogTypes() {
            return this.$axios.get(`catalogs/types`)
                .then(res => Object.assign(this.catalogTypes, res.data.data.data))
                .catch(res => console.log(`${res.status} : ${res}`));
        },
        getCatalogBrands() {
            return this.$axios.get(`catalogs/brands`)
                .then(res => Object.assign(this.catalogBrands, res.data.data.data))
                .catch(err => console.log(`${err.status} : ${err}`));
        },
        async showupdateCatalogModal(catalogId: number) {
            Object.assign(this.catalogItemUpdate, await this.getCatalogById(catalogId));
        },
        updateCatalog() {
            this.$axios.put(`catalogs`, this.catalogItemUpdate)
                .then(res => {
                    this.getCatalogs();
                    Toast.fire({
                        icon: "success",
                        title: "Güncelleme işlemi başarılı"
                    });
                })
                .catch(err =>
                    Swal.fire({
                        icon: "error",
                        title: "Hata!",
                        text: `İşlem sırasında hata oluştu (${err.status} : ${err})!`,
                        footer: '<a href="#">Why do I have this issue?</a>'
                    }));
        },
        createCatalog() {
            this.$axios.post(`catalogs`, this.createCatalogItem)
                .then(res => {
                    this.getCatalogs();
                    Toast.fire({
                        icon: "success",
                        title: "Ürün başarıyla eklendi"
                    });
                    Object.assign(this.createCatalogItem, new CatalogItemCreateDto());
                })
                .catch(err =>
                    Swal.fire({
                        icon: "error",
                        title: "Hata!",
                        text: `İşlem sırasında hata oluştu (${err.status} : ${err})!`,
                        footer: '<a href="#">Why do I have this issue?</a>'
                    }));
        },
        deleteCatalog(catalogName: string, catalogId: number) {
            Swal.fire({
                title: `${catalogName} Silmek istediğine emin misin?`,
                showCancelButton: true,
                confirmButtonText: "Sil",
                cancelButtonText: "İptal"
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$axios.delete(`catalogs/${catalogId}`,)
                        .then(res => {

                            this.getCatalogs();
                            Toast.fire({
                                icon: "success",
                                title: "Silme işlemi başarılı"
                            });
                        })
                        .catch(err =>
                            Swal.fire({
                                icon: "error",
                                title: "Hata!",
                                text: `İşlem sırasında hata oluştu (${err.status} : ${err})!`,
                                footer: '<a href="#">Why do I have this issue?</a>'
                            }));
                }
            });
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
                    console.log(res);
                    Object.assign(this.catalogs, res.data);
                    this.pageIndex = this.catalogs.pageIndex;
                })
                .catch(err => console.log(`${err.status} : ${err}`));
        },
        clearSearchInput(event: any) {
            this.getCatalogs();
            event.target.value = '';
        }
    }
}
</script>