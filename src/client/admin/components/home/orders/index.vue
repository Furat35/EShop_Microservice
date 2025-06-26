<template>
    <h1 class="h3 mb-2 text-gray-800">Orders</h1>
    <div class="card shadow mb-4">
        <div class="card-body">
            <div class="table-responsive">
                <div id="dataTable_wrapper" class="dataTables_wrapper dt-bootstrap4">
                    <div class="row mb-1">
                        <div class="col-sm-12 col-md-6">
                            <div class="dataTables_length" id="dataTable_length"><label>Show <select
                                        @change="changePageSize" name="dataTable_length" aria-controls="dataTable"
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
                            <table class="table table-bordered dataTable" id="dataTable" width="100%" cellspacing="0"
                                role="grid" aria-describedby="dataTable_info" style="width: 100%;">
                                <thead>
                                    <tr role="row">
                                        <th class="col-1" tabindex="0" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="#: activate to sort column descending">#
                                        </th>
                                        <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="Name: activate to sort column descending">
                                            Date</th>
                                        <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="Name: activate to sort column descending">
                                            Description</th>
                                        <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="Name: activate to sort column descending">
                                            Address</th>
                                        <th class="col-2" tabindex="1" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-sort="ascending" aria-label="Name: activate to sort column descending">
                                            Total Amount</th>
                                        <th class="col-1" tabindex="3" aria-controls="dataTable" rowspan="1" colspan="1"
                                            aria-label="Salary: activate to sort column ascending">Order Detail
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(order, index) in orders.data" :class="index % 2 == 0 ? 'even' : 'odd'"
                                        :key="order.id">
                                        <td>{{ (index + 1) + page * pageSize }}</td>
                                        <td class="sorting_1">{{ formatDate(order.createDate)
                                        }}</td>
                                        <td class="sorting_1">{{ order.description }}</td>
                                        <td class="sorting_1">{{ order.city }} / {{ order.country }}</td>
                                        <td class="sorting_1">{{ order.total }}</td>
                                        <td>
                                            <button type="button" data-toggle="modal" data-target="#orderItemsListModal"
                                                class="btn btn-outline-success" style="margin-right: 10px;"
                                                @click="getOrderItems(order.id)">
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
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-5">
                            <div class="dataTables_info" id="dataTable_info" role="status" aria-live="polite">
                                Total Records : {{ orders.count }}
                            </div>
                        </div>
                        <div class="row justify-content-center">
                            <PaginationComponent @page-changed="pageChanged" :pagination-model="orders" />
                        </div>
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
                    <h5 class="modal-title">Order Details</h5>
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
                                - Quantity : {{ orderItem.units }}<br>
                                - Price : {{ orderItem.unitPrice }}

                            </div><br>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-success">Close</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { OrderItemListDto } from '@shared/models/OrderItems/OrderItemListDto';
import { OrderListDto } from '@shared/models/Orders/OrderListDto';
import { PaginationModel } from '@shared/models/PaginationModel';
import { formatDate } from '@shared/helpers/dateHelper'
import PaginationComponent from '@admin/components/shared/pagination.vue'

export default {
    components: {
        PaginationComponent
    },
    data() {
        return {
            pageSize: 10,
            page: 0,
            orderItems: [] as OrderItemListDto[],
            orders: new PaginationModel<OrderListDto>()
        }
    },
    async created() {
        await this.getOrders();
    },
    methods: {
        formatDate,
        changePage(page: number) {
            this.page = page;
            this.getOrders();
        },
        changePageSize() {
            this.getOrders();
        },
        pageChanged(pagination) {
            this.page = pagination.page;
            this.getOrders();
        },
        getOrderById(orderId: string) {
            return this.$axios.get(`orders/${orderId}`)
                .then(res => res.data);
        },
        getOrders() {
            return this.$axios.get(`orders?page=${this.page}&pageSize=${this.pageSize}`)
                .then(res => {
                    Object.assign(this.orders, res.data);
                    this.page = this.orders.page;
                });
        },
        getOrderItems(orderId: string) {
            this.orderItems = this.orders.data.find(_ => _.id === orderId)!.orderItems;
        }
    }
}
</script>