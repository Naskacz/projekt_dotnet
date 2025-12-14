<template>

    <nav class="navbar">
        <div class="navbar-brand">
        <router-link to="/" class="navbar-item">Muzyczna Aplikacja</router-link>
        </div>
        <div class="navbar-end">
        <div class="navbar-item">
            <button @click="logout" class="button is-light">Wyloguj się</button>
        </div>
        </div>
    </nav>

</template>
<script setup>
import { ref } from 'vue'
async function logout() {
  localStorage.removeItem('token')
  window.location.href = '/login'
}

</script>