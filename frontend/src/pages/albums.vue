<template>
  <section class="albums">
    <h2>Albumy</h2>

    <div v-if="loading">Ładowanie...</div>
    <div v-else-if="error" class="error">{{ error }}</div>

    <table v-else-if="albums.length" class="albums-table">
      <thead>
        <tr>
          <th>Nazwa</th>
          <th>Artysta</th>
          <th>Rok</th>
          <th>Liczba utworów</th>
        </tr>
      </thead>
      <tbody>
        <template v-for="album in albums" :key="album.id">
          <tr>
            <td>{{ album.name }}</td>
            <td>{{ album.artist }}</td>
            <td>{{ album.releaseYear }}</td>
            <td>{{ album.songs?.length || 0 }}</td>
          </tr>
          <tr>
            <td colspan="4">
              <table class="songs-table" v-if="album.songs?.length">
                <thead>
                  <tr>
                    <th>Tytuł</th>
                    <th>Artysta</th>
                    <th>Rok</th>
                    <th>Gatunek</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="song in album.songs" :key="song.id">
                    <td>{{ song.title }}</td>
                    <td>{{ song.artist }}</td>
                    <td>{{ song.year }}</td>
                    <td>{{ song.genre }}</td>
                  </tr>
                </tbody>
              </table>
              <div v-else class="no-songs">Brak utworów</div>
            </td>
          </tr>
        </template>
      </tbody>
    </table>

    <p v-else>Brak albumów.</p>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const albums = ref([])
const loading = ref(false)
const error = ref('')

async function fetchAlbums() {
  loading.value = true
  error.value = ''
  try {
    // Backend ma [HttpPost("albums")] → /api/albums/albums
    const res = await axios.post('/api/albums/albums')
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
.albums-table, .songs-table { width: 100%; border-collapse: collapse; background: #fff; margin-top: 1rem; }
.albums-table th, .albums-table td, .songs-table th, .songs-table td {
  padding: 0.75rem; border-bottom: 1px solid #e0e0e0; text-align: left;
}
.albums-table thead, .songs-table thead { background: #2c3e50; color: #fff; }
.no-songs { color: #666; padding: 0.5rem 0; }
.error { color: #c00; background: #fee; padding: 0.75rem; border-radius: 4px; }
</style>