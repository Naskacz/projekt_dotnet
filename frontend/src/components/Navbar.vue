<template>
    <nav class="navbar">
        <div class="navbar-brand">
        <router-link to="/" class="navbar-item">Aplikacja</router-link>
        </div>
        <div class="navbar-item navbar-search">
            <input type="text" placeholder="" />
            <button class="search-button" onclick="search()"> Szukaj </button>
        </div>
        <div class="navbar-item">
            <button @click="logout" class="button is-light">Wyloguj się</button>
        </div>
    </nav>
</template>
<script setup>
import { ref } from 'vue'
async function logout() {
  localStorage.removeItem('token')
  window.location.href = '/login'
}
async function search() {
  // Implement search functionality here
}
</script>

<style scoped>

.navbar {
  background-color: black;
  padding: 1rem 5rem;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  border-bottom: 2px solid white;
}
.navbar-item {
  font-weight: bold;
  font-size: 1.2rem;
  color: white;
}
.button {
  margin-left: 1rem;
  background-color: transparent;
  color: white;
  border: 1px solid white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}
.navbar-search input {
  padding: 0.5rem;
  border-radius: 4px;
  border: none;
  min-width: 20dvw;
}
.navbar-search .search-button {
  margin-left: 0.5rem;
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  background-color: white;
  color: black;
  font-weight: bold;
  cursor: pointer;
}
</style>