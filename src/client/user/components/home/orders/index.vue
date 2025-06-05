<template>
    <section class="py-4">
        <div class="container">
            <div class="shadow-sm mb-4 p-2">
                <div class="card-body">
                    <div class="table-responsive">
                        <div id="dataTable_wrapper" class="dataTables_wrapper dt-bootstrap4">
                            <div class="row mb-1">
                                <div class="col-sm-12 col-md-6">
                                    <div class="dataTables_length" id="dataTable_length"><label>Göster <select
                                                @change="changePageSize" name="dataTable_length"
                                                aria-controls="dataTable"
                                                class="custom-select custom-select-sm form-control form-control-sm"
                                                v-model="pageSize">
                                                <option value="10">10</option>
                                                <option value="25">25</option>
                                                <option value="50">50</option>
                                                <option value="100">100</option>
                                            </select></label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <table class="table table-bordered dataTable" id="dataTable" width="100%"
                                        cellspacing="0" role="grid" aria-describedby="dataTable_info"
                                        style="width: 100%;">
                                        <thead>
                                            <tr role="row">
                                                <th class="col-1" tabindex="0" aria-controls="dataTable" rowspan="1"
                                                    colspan="1" aria-sort="ascending"
                                                    aria-label="#: activate to sort column descending">#
                                                </th>
                                                <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1"
                                                    colspan="1" aria-sort="ascending"
                                                    aria-label="Name: activate to sort column descending">
                                                    Tarih</th>
                                                <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1"
                                                    colspan="1" aria-sort="ascending"
                                                    aria-label="Name: activate to sort column descending">
                                                    Açıklama</th>
                                                <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1"
                                                    colspan="1" aria-sort="ascending"
                                                    aria-label="Name: activate to sort column descending">
                                                    Adres</th>
                                                <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1"
                                                    colspan="1" aria-sort="ascending"
                                                    aria-label="Name: activate to sort column descending">
                                                    Toplam Tutar</th>
                                                <th class="col-1" tabindex="3" aria-controls="dataTable" rowspan="1"
                                                    colspan="1" aria-label="Salary: activate to sort column ascending">
                                                    Sipariş
                                                    Detayları
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="(order, index) in orders.data"
                                                :class="index % 2 == 0 ? 'even' : 'odd'" :key="order.id">
                                                <td>{{ pageSize * pageIndex + index + 1 }}</td>
                                                <td class="sorting_1">{{ formatDate(order.createDate)
                                                    }}</td>
                                                <td class="sorting_1">{{ order.description }}</td>
                                                <td class="sorting_1">{{ order.city }} / {{ order.country }}</td>
                                                <td class="sorting_1">{{ order.total }} TL</td>
                                                <td>
                                                    <button type="button" data-toggle="modal"
                                                        data-target="#orderItemsListModal"
                                                        class="btn btn-outline-success" style="margin-right: 10px;"
                                                        @click="getOrderItems(order.id)">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13"
                                                            fill="currentColor" class="bi bi-building-dash"
                                                            viewBox="0 0 13 13">
                                                            <path
                                                                d="M12.5 16a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7M11 12h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1 0-1" />
                                                            <path
                                                                d="M2 1a1 1 0 0 1 1-1h10a1 1 0 0 1 1 1v6.5a.5.5 0 0 1-1 0V1H3v14h3v-2.5a.5.5 0 0 1 .5-.5H8v4H3a1 1 0 0 1-1-1z" />
                                                            <path
                                                                d="M4.5 2a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm-6 3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm-6 3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5zm3 0a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5v-1a.5.5 0 0 0-.5-.5z" />
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
                                        Toplam Kayıt : {{ orders.count }}
                                    </div>
                                </div>

                                <PaginationComponent @page-changed="pageChanged" :pagination-model="orders" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Order Details -->
            <div class="modal fade" id="orderItemsListModal" tabindex="-1" aria-labelledby="orderItemsListModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Sipariş İçeriği</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span>&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <form @submit.prevent>
                                <div class="form-group" v-for="(orderItem, index) in orderItems">
                                    <div>
                                        <b style="text-decoration: underline;">
                                            {{ index + 1 }}. {{ orderItem.itemName }}</b><br>
                                        - Adet : {{ orderItem.units }}<br>
                                        - Fiyat : {{ orderItem.unitPrice }}

                                    </div><br>
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-success">Kapat</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script lang="ts">
import { OrderItemListDto } from '@shared/models/OrderItems/OrderItemListDto';
import { OrderListDto } from '@shared/models/Orders/OrderListDto';
import { PaginationModel } from '@shared/models/PaginationModel';
import { formatDate } from '@shared/helpers/dateHelper'
import PaginationComponent from '@user/components/shared/pagination.vue'
import emitter from '@user/helpers/eventBus';

export default {
    components: {
        PaginationComponent
    },
    data() {
        return {
            pageSize: 10,
            pageIndex: 0,
            orderItems: [] as OrderItemListDto[],
            orders: new PaginationModel<OrderListDto>()
        }
    },
    async mounted() {
        this.getOrders();
    },
    methods: {
        formatDate,
        pageChanged(pagination) {
            this.pageIndex = pagination.pageIndex;
            this.getOrders();
        },
        changePageSize() {
            this.getOrders();
        },
        getOrderById(orderId: string) {
            return this.$axios.get(`orders/${orderId}`)
                .then(res => res.data)
        },
        async getOrders() {
            emitter.emit('show-spinner');
            return await this.$axios.get(`orders/byuser?pageIndex=${this.pageIndex}&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.orders, res.data);
                    this.pageIndex = this.orders.pageIndex;
                    emitter.emit('hide-spinner');
                });
        },
        getOrderItems(orderId: string) {
            this.orderItems = this.orders.data.find(_ => _.id === orderId)!.orderItems;
        },
    }
}
</script>