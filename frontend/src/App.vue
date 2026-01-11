<script setup lang="ts">
import { RouterView } from 'vue-router'
import Navbar from './components/Navbar.vue'
import SideNav from './components/SideNav.vue'
</script>

<template>
  <div class="layout">
    <header>
      <Navbar />
    </header>
    <div class="content">
      <SideNav />
      <main class="main">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<style scoped>
body {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  margin-left: 0px;
}

.layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.content {
  display: flex;
  flex: 1;
  min-height: 0;
}

.main {
  flex: 1;
  padding: 1rem;
  overflow: auto;
}
</style>
