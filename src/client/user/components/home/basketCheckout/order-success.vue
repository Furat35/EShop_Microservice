<template>
    <section class="py-5">
        <div class="container px-4 px-lg-5 mt-5 text-center" style="margin-bottom: 8%;">
            <div class="alert alert-success shadow-lg p-5 rounded">
                <h2 class="mb-4">🎉 Siparişiniz Başarıyla Alındı!</h2>
                <p class="lead">
                    Siparişiniz başarıyla tamamlandı. Sipariş detaylarınız e-posta adresinize gönderildi.
                </p>

                <div class="mt-4">
                    <router-link :to="{ name: 'catalog' }" class="btn btn-primary me-2">Ana Sayfaya Dön</router-link>
                    <router-link :to="{ name: 'orders' }" class="btn btn-outline-secondary">Siparişlerim</router-link>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
export default {
    name: "OrderSuccess"
};
</script>
