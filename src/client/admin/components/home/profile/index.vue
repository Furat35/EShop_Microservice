<template>
    <h1 class="h3 mb-2 text-gray-800">Update Profile</h1>
    <div class="card shadow mb-4">
        <div class="card-body">
            <div class="table-responsive">
                <div id="dataTable_wrapper" class="dataTables_wrapper dt-bootstrap4">
                    <div class="row justify-content-center mb-5">
                        <div class="col-sm-4">
                            <form @submit.prevent="profileUpdate">
                                <div class="form-group">
                                    <input type="text" class="form-control" id="id" hidden v-model="userUpdateModel.id">
                                </div>
                                <div class="form-group">
                                    <label for="fullname">Name Surname</label>
                                    <input type="text" class="form-control" id="fullname"
                                        v-model="userUpdateModel.fullname">
                                </div>
                                <div class="form-group">
                                    <label for="email">Email</label>
                                    <input type="email" class="form-control" id="email" v-model="userUpdateModel.email">
                                </div>
                                <div class="form-group">
                                    <label for="password">Password</label>
                                    <input type="text" class="form-control" id="password"
                                        v-model="userUpdateModel.password">
                                </div>
                                <button type="submit" class="btn btn-primary">Update</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Toast } from '@shared/helpers/sweetAlertHelpers';
import { getUserId } from '@shared/helpers/userExtensions';
import { UserListDto } from '@shared/models/Users/UserListDto';
import { UserUpdateDto } from '@shared/models/Users/UserUpdateDto';
import Swal from 'sweetalert2'

export default {
    data() {
        return {
            pageSize: 10,
            page: 0,
            user: new UserListDto,
            userUpdateModel: new UserUpdateDto
        }
    },
    async created() {
        await this.getProfileById(getUserId());
        Object.assign(this.userUpdateModel, this.user);
    },
    methods: {
        getProfileById(userId: number) {
            return this.$axios.get(`users/${userId}`)
                .then(res => Object.assign(this.user, res.data))
        },
        async profileUpdate() {
            await this.$axios.put(`users`, this.userUpdateModel)
                .then(res => {
                    Toast.fire({
                        icon: "success",
                        title: "Updated"
                    });
                })
                .catch(err =>
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: `Error occured during operation (${err.status} : ${err})!`,
                    }));
        }
    }
}
</script>