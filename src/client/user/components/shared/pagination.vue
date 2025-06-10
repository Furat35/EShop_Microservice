<template>
    <div class="col-sm-12 col-md-7">
        <div class=" dataTables_paginate paging_simple_numbers" id="dataTable_paginate">
            <ul class="pagination justify-content-center">
                <li :class="['paginate_button', 'page-item', 'previous', paginationModel.hasPrevious ? '' : 'disabled']"
                    id="dataTable_previous"><button aria-controls="dataTable" :data-dt-idx="paginationModel.pageIndex"
                        tabindex="0" class="page-link"
                        @click="changePageIndex(paginationModel.pageIndex - 1)">Önceki</button>
                </li>
                <li class="paginate_button page-item" v-if="paginationModel.hasPrevious">
                    <button @click="changePageIndex(paginationModel.pageIndex - 1)" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.pageIndex" tabindex="0" class="page-link">{{
                            paginationModel.pageIndex }}</button>
                </li>
                <li class="paginate_button page-item active">
                    <button @click="changePageIndex(paginationModel.pageIndex)" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.pageIndex + 1" tabindex="0" class="page-link">{{
                            paginationModel.pageIndex + 1 }}</button>
                </li>
                <li class="paginate_button page-item" v-if="paginationModel.hasNext">
                    <button @click="changePageIndex(paginationModel.pageIndex + 1)" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.pageIndex + 2" tabindex="0" class="page-link">{{
                            paginationModel.pageIndex + 2 }}</button>
                </li>
                <li :class="['paginate_button', 'page-item', 'next', paginationModel.hasNext ? '' : 'disabled']"
                    id="dataTable_next"><button href="#" aria-controls="dataTable"
                        :data-dt-idx="paginationModel.pageIndex + 2" tabindex="0" class="page-link"
                        @click="changePageIndex(paginationModel.pageIndex + 1)">Sonraki</button>
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
            pageIndex: 0
        }
    },
    mounted() {
        this.pageIndex = this.paginationModel.pageIndex;
    },
    methods: {
        changePageIndex(pageIndex: number) {
            this.pageIndex = pageIndex;
            this.$emit('page-changed', { pageIndex: this.pageIndex });
        }
    }
}
</script>
