<template>
    <div class="container mt-6">
        <h1>404</h1>
        <p>Oops! Page Not Found</p>
        <p>
            <RouterLink :to="{ name: 'notFound' }">Home</RouterLink>
        </p>
    </div>
</template>

<script lang="ts">
export default {
    name: 'NotFound'
};
</script>

<style scoped>
body {
    font-family: Arial, sans-serif;
    background-color: #f8f9fa;
    text-align: center;
    padding: 50px;
}

h1 {
    font-size: 100px;
    color: #343a40;
}

p {
    font-size: 24px;
    color: #6c757d;
}

a {
    font-size: 18px;
    color: #007bff;
    text-decoration: none;
}

a:hover {
    text-decoration: underline;
}

.container {
    max-width: 600px;
    margin: 0 auto;
}
</style>