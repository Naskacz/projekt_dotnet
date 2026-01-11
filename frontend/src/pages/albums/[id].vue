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
        <SongTable 
        v-if="album.songs.length" 
        :songs="album.songs"
        show-delete
      />
      </table>
      <p v-else>Brak utworów w tym albumie.</p>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { useAlbums } from '@/composables/api/useAlbums'
import SongTable from '@/components/SongTable.vue'
const route = useRoute()
const album = ref<any>(null)
const loading = ref(false)
const error = ref('')

const { fetchOne } = useAlbums()


onMounted(async () => {
  loading.value = true
  error.value = ''
  try {
    const id = route.params.id as string
    album.value = await fetchOne(id)
  } catch (e: any) {
    error.value = e.response?.data?.error || 'Błąd pobierania albumu'
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.album { max-width: 960px; margin: 2rem auto; padding: 0 1rem; }
</style>