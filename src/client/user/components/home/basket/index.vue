<template>
    <div class="cart-container">
        <h2>Sepetim</h2>

        <div v-if="basket.items.length === 0" style="margin-bottom: 200px; margin-top: 100px;text-align: center;">
            <p>Sepetiniz boş</p>
            <router-link :to="{ name: 'catalog' }" class="btn btn-success" style="font-weight: bold;">Ana
                Sayfaya Dön</router-link>
        </div>

        <div v-else class="cart-list">
            <div class="cart-item" v-for="item in basket.items" :key="item.itemId">
                <img class="item-image" :src="'http://localhost:5000/img' + item.pictureUrl" alt="Ürün görseli" />

                <div class="item-details">
                    <h4>{{ item.itemName }}</h4>

                    <p class="price-info">
                        <span v-if="!item.discountAmount">
                            {{ item.unitPrice.toFixed(2) }} TL
                        </span>
                        <span v-else>
                            <span class="old-price">{{ item.unitPrice.toFixed(2) }} TL</span>
                            <span class="discounted-price">
                                {{ (item.unitPrice - item.discountAmount).toFixed(2) }} TL
                                <small class="discount-rate">
                                    (%{{ Math.round((item.discountAmount / item.unitPrice) * 100) }})
                                </small>
                            </span>
                        </span>
                    </p>

                    <p>
                        <span class="me-4">Adet: {{ item.quantity }}</span>
                        <button class="btn" @click="increaseItemQuantity(item.itemId)"><i
                                class="bi bi-plus-lg"></i></button>
                        <button class="btn" @click="decreaseItemQuantity(item.itemId)"><i
                                class="bi bi-dash-lg"></i></button>
                    </p>

                    <div class="item-footer">
                        <strong>Toplam: {{ (item.quantity * (item.unitPrice - item.discountAmount)).toFixed(2) }}
                            TL</strong>
                        <button @click="removeItem(item.itemId)" class="btn btn-danger ms-3">Sil</button>
                    </div>
                </div>
            </div>
            <hr>
            <div class="cart-total">
                <h4>Sepet Toplamı: {{ totalPrice.toFixed(2) }} TL</h4>
            </div>
            <div class="text-center">
                <router-link :to="{ name: 'basketCheckout' }" class="btn btn-success"
                    style="font-weight: bold;width: 15%;">Onayla</router-link>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { BasketListDto } from '@shared/models/Baskets/BasketListDto';
import emitter from '../../../helpers/eventBus';

export default {
    data() {
        return {
            basket: new BasketListDto(),
        };
    },
    async created() {
        await this.getCart();
        emitter.emit('basket-updated')
    },
    computed: {
        totalPrice() {
            return this.basket.items.reduce((total, item) => {
                const price = item.unitPrice - item.discountAmount;
                return total + price * item.quantity;
            }, 0);
        }
    },
    methods: {
        async getCart() {
            emitter.emit('show-spinner');
            const response = await this.$axios.get('basket');
            Object.assign(this.basket, response.data.data);
            emitter.emit('hide-spinner');
        },
        async removeItem(itemId) {
            this.basket.items = this.basket.items.filter(item => item.itemId !== itemId);
            await this.$axios.post(`basket/update`, this.basket);
            emitter.emit('basket-updated')
        },
        async increaseItemQuantity(itemId: number) {
            const item = this.basket.items.find(item => item.itemId === itemId);
            item.quantity += 1;
            await this.$axios.post(`basket/update`, this.basket);
        },
        async decreaseItemQuantity(itemId: number) {
            const item = this.basket.items.find(item => item.itemId === itemId);
            if (item.quantity === 1) {
                await this.removeItem(item.itemId);
                return;
            }
            item.quantity -= 1;
            await this.$axios.post(`basket/update`, this.basket);
            emitter.emit('basket-updated')

        }
    }
};
</script>

<style scoped>
.cart-container {
    padding: 20px;
    max-width: 900px;
    margin: 0 auto;
    margin-bottom: 5%;
}

.cart-list {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
}

.cart-item {
    display: flex;
    background-color: #f9f9f9;
    border-radius: 12px;
    padding: 15px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.item-image {
    width: 120px;
    height: 120px;
    object-fit: contain;
    border-radius: 8px;
    margin-right: 20px;
}

.item-details {
    flex-grow: 1;
}

.item-details h4 {
    margin-bottom: 8px;
    font-size: 18px;
}

.price-info {
    margin-bottom: 6px;
}

.old-price {
    text-decoration: line-through;
    color: #888;
    margin-right: 8px;
}

.discounted-price {
    color: #e60023;
    font-weight: bold;
}

.discount-rate {
    font-size: 12px;
    color: #666;
    margin-left: 4px;
}

.item-footer {
    margin-top: 10px;
    display: flex;
    align-items: center;
}

.cart-total {
    text-align: right;
    font-size: 20px;
    font-weight: bold;
}
</style>
