<template>
    <div class="col-sm-12 col-md-7">
        <div class=" dataTables_paginate paging_simple_numbers" id="dataTable_paginate">
            <ul class="pagination justify-content-center">
                <li :class="['paginate_button', 'page-item', 'previous', paginationModel.hasPrevious ? '' : 'disabled']"
                    id="dataTable_previous"><button aria-controls="dataTable" :data-dt-idx="paginationModel.page"
                        tabindex="0" class="page-link" @click="changePage(paginationModel.page - 1)">Previous</button>
                </li>
                <li class="paginate_button page-item" v-if="paginationModel.hasPrevious">
                    <button @click="changePage(paginationModel.page - 1)" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.page" tabindex="0" class="page-link">{{
                            paginationModel.page }}</button>
                </li>
                <li class="paginate_button page-item active">
                    <button @click="changePage(paginationModel.page)" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.page + 1" tabindex="0" class="page-link">{{
                            paginationModel.page + 1 }}</button>
                </li>
                <li class="paginate_button page-item" v-if="paginationModel.hasNext">
                    <button @click="changePage(paginationModel.page + 1)" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.page + 2" tabindex="0" class="page-link">{{
                            paginationModel.page + 2 }}</button>
                </li>
                <li :class="['paginate_button', 'page-item', 'next', paginationModel.hasNext ? '' : 'disabled']"
                    id="dataTable_next"><button href="#" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.page + 2" tabindex="0" class="page-link"
                        @click="changePage(paginationModel.page + 1)">Next</button>
                </li>
            </ul>
        </div>
    </div>
</template>


<script lang="ts">
import { PaginationModel } from '@shared/models/PaginationModel'

export default {
    props: {
        paginationModel: PaginationModel<any>,
    },
    data() {
        return {
            pageSize: 10,
            page: 0
        }
    },
    mounted() {
        this.page = this.paginationModel.page;
    },
    methods: {
        changePage(page: number) {
            this.page = page;
            this.$emit('page-changed', { page: this.page });
        }
    }
}
</script>
