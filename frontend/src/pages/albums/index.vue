<template>
  <section class="albums">
    <h2>Albumy</h2>
    <AlbumList v-if="albums.length" :albums="albums"/>
    <p v-else>Brak albumów.</p>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import AlbumList from '@/components/AlbumList.vue'

const albums = ref([])
const loading = ref(false)
const error = ref('')

async function fetchAlbums() {
  loading.value = true
  error.value = ''
  try {
    const res = await axios.get('/api/albums') 
    albums.value = res.data
  } catch (e) {
    error.value = e.response?.data?.message || e.message || 'Błąd pobierania albumów'
  } finally {
    loading.value = false
  }
}

onMounted(fetchAlbums)
</script>

<style scoped>
.albums { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>
