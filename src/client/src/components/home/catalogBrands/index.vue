<template>
    <h1 class="h3 mb-2 text-gray-800">Markalar</h1>
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
                                <button brand="button" class="btn btn-primary ml-2" data-dismiss="modal"
                                    data-toggle="modal" data-target="#catalogBrandCreateModal">
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
                        <div class="col-sm-12 col-md-6" style="text-align: end;">
                            <div id="dataTable_filter" class="dataTables_filter"><label class="mr-2">Ara<input
                                        type="search" class="form-control form-control-sm" placeholder=""
                                        @keydown.enter="search($event)" aria-controls="dataTable"></label>
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
                                            Marka</th>
                                        <th class="col-1" tabindex="3" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Salary: activate to sort column ascending">İşlemler
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(catalogBrand, index) in catalogBrands.data"
                                        :class="index % 2 == 0 ? 'even' : 'odd'" :key="catalogBrand.id">
                                        <td>{{ index + 1 }}</td>
                                        <td class="sorting_1">{{ catalogBrand.brand }}</td>
                                        <td>
                                            <button brand="button" data-toggle="modal"
                                                data-target="#catalogBrandUpdateModal" class="btn btn-outline-success"
                                                style="margin-right: 10px;"
                                                @click="showCatalogBrandUpdateModal(catalogBrand.id)">
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
                                            <button brand="button" class="btn btn-outline-danger"
                                                @click="catalogBrandDelete(catalogBrand.brand, catalogBrand.id)">
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
                                Toplam Kayıt : {{ catalogBrands.count }}
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-7">
                            <div class="dataTables_paginate paging_simple_numbers" id="dataTable_paginate">
                                <ul class="pagination">
                                    <li :class="['paginate_button', 'page-item', 'previous', catalogBrands.hasPrevious ? '' : 'disabled']"
                                        id="dataTable_previous"><button aria-controls="dataTable"
                                            :data-dt-idx="catalogBrands.pageIndex" tabindex="0" class="page-link"
                                            @click="changePageIndex(catalogBrands.pageIndex - 1)">Önceki</button></li>
                                    <li class="paginate_button page-item" v-if="catalogBrands.hasPrevious">
                                        <button @click="changePageIndex(catalogBrands.pageIndex - 1)"
                                            aria-controls="dataTable" :data-dt-idx="catalogBrands.pageIndex"
                                            tabindex="0" class="page-link">{{
                                                catalogBrands.pageIndex }}</button>
                                    </li>
                                    <li class="paginate_button page-item active">
                                        <button @click="changePageIndex(catalogBrands.pageIndex)"
                                            aria-controls="dataTable" :data-dt-idx="catalogBrands.pageIndex + 1"
                                            tabindex="0" class="page-link">{{
                                                catalogBrands.pageIndex + 1 }}</button>
                                    </li>
                                    <li class="paginate_button page-item" v-if="catalogBrands.hasNext">
                                        <button @click="changePageIndex(catalogBrands.pageIndex + 1)"
                                            aria-controls="dataTable" :data-dt-idx="catalogBrands.pageIndex + 2"
                                            tabindex="0" class="page-link">{{
                                                catalogBrands.pageIndex + 2 }}</button>
                                    </li>
                                    <li :class="['paginate_button', 'page-item', 'next', catalogBrands.hasNext ? '' : 'disabled']"
                                        id="dataTable_next"><button href="#" aria-controls="dataTable"
                                            :data-dt-idx="catalogBrands.pageIndex + 2" tabindex="0" class="page-link"
                                            @click="changePageIndex(catalogBrands.pageIndex + 1)">Sonraki</button></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Update Modal -->
    <div class="modal fade" id="catalogBrandUpdateModal" tabindex="-1" aria-labelledby="catalogBrandUpdateModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="catalogBrandUpdateModal">Güncelle</h5>
                    <button brand="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent>
                        <div class="form-group">
                            <label for="id" class="col-form-label" hidden>Id</label>
                            <input type="text" class="form-control" id="id" hidden v-model="catalogBrandUpdateDto.id">
                        </div>
                        <div class="form-group">
                            <label for="category-brand" class="col-form-label">Marka</label>
                            <input type="text" class="form-control" id="category-brand"
                                v-model="catalogBrandUpdateDto.brand">
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button brand="button" class="btn btn-success" @click="catalogBrandUpdate">Kaydet</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Create Modal -->
    <div class="modal fade" id="catalogBrandCreateModal" tabindex="-1" aria-labelledby="catalogBrandCreateModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="catalogBrandCreateModal">Ekle</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent>
                        <div class="form-group">
                            <label for="category-brand" class="col-form-label">Marka</label>
                            <input type="text" class="form-control" id="category-brand"
                                v-model="catalogBrandCreateDto.brand">
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-success" @click="catalogBrandCreate">Kaydet</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Toast } from '@/helpers/sweetAlertHelpers';
import { CatalogBrandCreateDto } from '@/models/CatalogBrands/CatalogBrandCreateDto';
import { CatalogBrandListDto } from '@/models/CatalogBrands/CatalogBrandListDto';
import { CatalogBrandUpdateDto } from '@/models/CatalogBrands/CatalogBrandUpdateDto';
import { PaginationModel } from '@/models/PaginationModel';
import Swal from 'sweetalert2'

export default {
    data() {
        return {
            pageSize: 10,
            pageIndex: 0,
            catalogBrandUpdateDto: new CatalogBrandUpdateDto(),
            catalogBrandCreateDto: new CatalogBrandCreateDto(),
            catalogBrands: new PaginationModel<CatalogBrandListDto>()
        }
    },
    async created() {
        await this.getCatalogBrands();
    },
    methods: {
        changePageIndex(pageIndex: number) {
            this.pageIndex = pageIndex;
            this.getCatalogBrands();
        },
        changePageSize() {
            this.getCatalogBrands();
        },
        getCatalogBrandById(catalogBrandId: number) {
            return this.$axios.get(`catalogs/brands/${catalogBrandId}`)
                .then(res => res.data)
                .catch(err => console.log(`${err.status} : ${err}`));
        },
        getCatalogBrands() {
            return this.$axios.get(`catalogs/brands`)
                .then(res => {
                    Object.assign(this.catalogBrands, res.data);
                    this.pageIndex = this.catalogBrands.pageIndex;
                })
                .catch(res => console.log(`${res.status} : ${res}`));
        },
        async showCatalogBrandUpdateModal(catalogBrandId: number) {
            Object.assign(this.catalogBrandUpdateDto, await this.getCatalogBrandById(catalogBrandId))
        },
        catalogBrandUpdate() {
            this.$axios.put(`catalogs/brands`, this.catalogBrandUpdateDto)
                .then(res => {
                    this.getCatalogBrands();
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
        catalogBrandCreate() {
            this.$axios.post(`catalogs/brands`, this.catalogBrandCreateDto)
                .then(res => {
                    this.getCatalogBrands();
                    Toast.fire({
                        icon: "success",
                        title: "Ürün başarıyla eklendi"
                    });
                })
                .catch(err =>
                    Swal.fire({
                        icon: "error",
                        title: "Hata!",
                        text: `İşlem sırasında hata oluştu (${err.status} : ${err})!`,
                    }));
        },
        catalogBrandDelete(catalogBrandName: string, catalogBrandId: number) {
            Swal.fire({
                title: `${catalogBrandName} Silmek istediğine emin misin?`,
                showCancelButton: true,
                confirmButtonText: "Sil",
                cancelButtonText: "İptal"
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$axios.delete(`catalogs/brands/${catalogBrandId}`,)
                        .then(res => {
                            this.getCatalogBrands();
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

            this.$axios.get(`catalogs/brands/${name.target.value}?pageIndex=${this.pageIndex}&&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.catalogBrands, res.data);
                    this.pageIndex = this.catalogBrands.pageIndex;
                })
                .catch(err => console.log(`${err.status} : ${err}`));
        },
        clearSearchInput(event: any) {
            this.getCatalogBrands();
            event.target.value = '';
        }
    }
}
</script>