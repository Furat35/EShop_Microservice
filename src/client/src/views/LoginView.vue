<template>
    <div class="main">
        <div class="login-container">
            <h2>Giriş Yap</h2>
            <form @submit.prevent="login">
                <input type="text" name="username" v-model="user.username" placeholder="Kullanıcı Adı" required>
                <input type="password" v-model="user.password" name="password" placeholder="Şifre" required>
                <button type="submit">Giriş Yap</button>
            </form>
            <div class="signup-link">
                <RouterLink :to="{ name: 'register' }">Kayıt Ol</RouterLink>
            </div>
        </div>
    </div>
</template>

<script lang='ts'>
import { LoginRequestDto } from '@/models/AuthModels/LoginRequestDto';

export default {
    data() {
        return {
            user: new LoginRequestDto()
        }
    },
    methods: {
        login() {
            this.$axios.post('auth/login', this.user)
                .then(res => {
                    localStorage.setItem('userInfo', JSON.stringify(res.data));
                    this.$router.push({ name: 'catalog' })
                })
                .catch(res => console.log(`${res.status} : ${res}`));
        }
    }

}
</script>

<style scoped>
.main {
    margin: 0;
    padding: 0;
    background: #f0f2f5;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    font-family: Arial, sans-serif;
}

.login-container {
    background: white;
    padding: 40px 30px;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    width: 100%;
    width: 400px;
    box-sizing: border-box;
}

.login-container h2 {
    text-align: center;
    margin-bottom: 24px;
    font-size: 28px;
    color: #333;
}

form {
    display: flex;
    flex-direction: column;
}

form input[type="text"],
form input[type="password"] {
    width: 100%;
    padding: 12px;
    margin-bottom: 16px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-sizing: border-box;
    font-size: 16px;
}

form button {
    width: 100%;
    padding: 12px;
    background-color: #4CAF50;
    border: none;
    border-radius: 8px;
    color: white;
    font-size: 18px;
    cursor: pointer;
    margin-top: 8px;
}

form button:hover {
    background-color: #45a049;
}

.signup-link {
    text-align: end;
    margin-top: 20px;
    font-size: 14px;
}

.signup-link a {
    color: #4CAF50;
    text-decoration: none;
}

.signup-link a:hover {
    text-decoration: underline;
}
</style>