<template>
  <section class="album">
    <div v-if="loading">Ładowanie...</div>
    <div v-else-if="error" class="error">{{ error }}</div>
    <div v-else-if="album">
      <h2>{{ album.name }}</h2>
      <p class="meta">
        <span>Artysta: {{ album.artist }}</span>
        <span>Rok: {{ album.releaseYear }}</span>
        <span>Dodane przez: {{ album.createdBy }}</span>
      </p>
      <div v-if="album.coverUrl" class="cover">
        <img :src="album.coverUrl" alt="Okładka" />
      </div>

      <h3>Utwory ({{ album.songs?.length || 0 }})</h3>
      <table v-if="album.songs?.length" class="songs">
        <thead>
          <tr>
            <th>Tytuł</th>
            <th>Artysta</th>
            <th>Rok</th>
            <th>Gatunek</th>
            <th>Odsłuch</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="song in album.songs" :key="song.id">
            <td>{{ song.title }}</td>
            <td>{{ song.artist }}</td>
            <td>{{ song.year }}</td>
            <td>{{ song.genre }}</td>
            <td>
              <audio v-if="song.fileUrl" :src="song.fileUrl" controls style="width:160px" />
            </td>
          </tr>
        </tbody>
      </table>
      <p v-else>Brak utworów w tym albumie.</p>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'

const route = useRoute()
const album = ref<any>(null)
const loading = ref(false)
const error = ref('')

async function fetchAlbum() {
  loading.value = true
  error.value = ''
  try {
    const id = route.params.id  
    const res = await axios.get(`/api/albums/${id}`)
    album.value = res.data
  } catch (e: any) {
    error.value = e.response?.data?.error || 'Błąd pobierania albumu'
  } finally {
    loading.value = false
  }
}

onMounted(fetchAlbum)
</script>

<style scoped>
.album { max-width: 960px; margin: 2rem auto; padding: 0 1rem; }
</style>