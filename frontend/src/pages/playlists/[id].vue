<template>
  <section class="playlist">
    <h2>{{ playlist?.name || 'Playlista' }}</h2>
    <p class="meta">{{ playlist?.isPublic ? 'Publiczna' : 'Prywatna' }} • {{ playlist?.songs?.length || 0 }} utworów</p>
    <SongTable
      v-if="playlist?.songs?.length"
      :songs="playlist.songs"
      show-delete
      @delete="handleRemove"
    />
    <p v-else>Brak utworów w playliście.</p>

    <p v-if="error" class="error">{{ error }}</p>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import SongTable from '@/components/SongTable.vue'
import { usePlaylists } from '@/composables/api/usePlaylists'
import { useMySongs } from '@/composables/api/useSongs'
import type { Playlist, Song } from '@/types'

const route = useRoute()
const { fetchOne, addSong, removeSong } = usePlaylists()
const { songs: mySongs, fetch: fetchMySongs } = useMySongs()

const playlist = ref<Playlist | null>(null)
const error = ref('')
const selectedSongId = ref<number | null>(null)

async function load() {
  try {
    const id = route.params.id as string
    const data = await fetchOne(id)
    playlist.value = data
  } catch (e: any) {
    error.value = e.response?.data?.message || 'Błąd pobierania playlisty'
  }
}

async function handleAdd() {
  if (!playlist.value || !selectedSongId.value) return
  try {
    await addSong(Number(playlist.value.id), selectedSongId.value)
    await load()
  } catch (e: any) {
    error.value = e.response?.data?.message || 'Błąd dodawania utworu'
  }
}

async function handleRemove(songId: number | string) {
  if (!playlist.value) return
  try {
    await removeSong(Number(playlist.value.id), Number(songId))
    await load()
  } catch (e: any) {
    error.value = e.response?.data?.message || 'Błąd usuwania utworu'
  }
}

onMounted(async () => {
  await Promise.all([load(), fetchMySongs()])
})
</script>

<style scoped>
.playlist { max-width: 1000px; margin: 2rem auto; padding: 0 1rem; }
.meta { color: #666; margin-bottom: 1rem; }
.adder { display: flex; gap: 0.5rem; align-items: center; margin-bottom: 1rem; }
select { padding: 0.6rem; border: 1px solid #ddd; border-radius: 6px; min-width: 240px; }
button { padding: 0.6rem 1rem; border: none; background: #2c3e50; color: #fff; border-radius: 6px; cursor: pointer; }
.error { color: #b91c1c; margin-top: 0.5rem; }
</style>