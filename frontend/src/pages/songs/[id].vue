<template>
  <section class="song">
    <h2>{{ song?.title || 'Utwór' }}</h2>
    <p class="meta">
      {{ song?.artist }} • {{ song?.year || 'brak roku' }} • {{ song?.genre || 'brak gatunku' }}
    </p>

    <audio v-if="song?.fileUrl" :src="song.fileUrl" controls class="player" />

    <p v-if="error" class="error">{{ error }}</p>
    <p v-else-if="loading">Ładowanie...</p>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useSongs } from '@/composables/api/useSongs'
import type { Song } from '@/types'

const route = useRoute()
const { fetchOne } = useSongs()

const song = ref<Song | null>(null)
const loading = ref(false)
const error = ref('')

onMounted(async () => {
  loading.value = true
  error.value = ''
  try {
    const id = route.params.id as string
    song.value = await fetchOne(id)
  } catch (e: any) {
    error.value = e.response?.data?.error || 'Błąd pobierania utworu'
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.song { max-width: 800px; margin: 2rem auto; padding: 0 1rem; }
.meta { color: #666; margin-bottom: 1rem; }
.player { width: 100%; max-width: 480px; margin-top: 0.5rem; }
.error { color: #b91c1c; margin-top: 1rem; }
</style>