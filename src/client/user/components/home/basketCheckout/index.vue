<template>
    <section class="py-5">
        <div class="container px-4 px-lg-5 mt-5">
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <form @submit.prevent="submitBasketCheckout" class="p-4 border rounded shadow-sm bg-light">
                        <h4 class="mb-4 text-center">Adsress & Paymet Info</h4>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="country" class="form-label">Country</label>
                                <input type="text" id="country" class="form-control"
                                    v-model="basketCheckoutModel.country" required />
                            </div>
                            <div class="col-md-6">
                                <label for="state" class="form-label">City</label>
                                <input type="text" id="state" class="form-control"
                                    v-model="basketCheckoutModel.state" />
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-md-4">
                                <label for="city" class="form-label">State</label>
                                <input type="text" id="city" class="form-control" v-model="basketCheckoutModel.city"
                                    required />
                            </div>
                            <div class="col-md-4">
                                <label for="street" class="form-label">Street</label>
                                <input type="text" id="street" class="form-control" v-model="basketCheckoutModel.street"
                                    required />
                            </div>
                            <div class="col-md-4">
                                <label for="zip" class="form-label">Zip Code</label>
                                <input type="text" id="zip" class="form-control" v-model="basketCheckoutModel.zipCode"
                                    required />
                            </div>
                        </div>



                        <h5 class="mb-3 mt-4">Card Info</h5>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="cardNumber" class="form-label">Card Number</label>
                                <input type="text" id="cardNumber" class="form-control"
                                    v-model="basketCheckoutModel.cardNumber" required />
                            </div>
                            <div class="col-md-6">
                                <label for="cardHolder" class="form-label">Name Surname</label>
                                <input type="text" id="cardHolder" class="form-control"
                                    v-model="basketCheckoutModel.cardHolderName" required />
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="expiration" class="form-label">Expiry Date</label>
                                <input type="date" id="expiration" class="form-control"
                                    v-model="basketCheckoutModel.cardExpiration" required />
                            </div>
                            <div class="col-md-6">
                                <label for="cvv" class="form-label">CVV</label>
                                <input type="text" id="cvv" class="form-control"
                                    v-model="basketCheckoutModel.cardSecurityNumber" required />
                            </div>
                        </div>

                        <!-- <div class="mb-3">
                            <label for="cardTypeId" class="form-label">Card Type (ID)</label>
                            <input type="number" id="cardTypeId" class="form-control"
                                v-model="basketCheckoutModel.cardTypeId" required />
                        </div> -->

                        <div class="mb-4">
                            <label for="description" class="form-label">Description</label>
                            <textarea id="description" class="form-control" rows="3"
                                v-model="basketCheckoutModel.description"></textarea>
                        </div>

                        <div class="d-grid">
                            <button type="submit" class="btn btn-primary btn-lg">Complete Order</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </section>
</template>


<script lang="ts">
import { BasketCheckoutDto } from '@shared/models/BasketCheckouts/BasketCheckoutDto';
import { Toast } from '@shared/helpers/sweetAlertHelpers';
import emitter from '../../../helpers/eventBus';

export default {
    data() {
        return {
            basketCheckoutModel: new BasketCheckoutDto() as BasketCheckoutDto
        }
    },
    methods: {
        async submitBasketCheckout() {
            try {
                emitter.emit('show-spinner');
                const response = await this.$axios.post(`basket/checkout`, this.basketCheckoutModel)
                if (response.status === 204) {
                    emitter.emit('hide-spinner');
                    Toast.fire({
                        icon: "success",
                        title: "Order placed successfully!"
                    });
                    emitter.emit('basket-updated')
                    this.$router.push({ name: 'OrderSuccess' });
                }
                else {
                    emitter.emit('hide-spinner');
                    Toast.fire({
                        icon: "error",
                        title: `Error occured during operation!`
                    });
                }

            } catch (err) {
                console.error(err)
            }
        },
    }

}

</script>