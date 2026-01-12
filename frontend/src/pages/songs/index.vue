<template>
  <section class="songs">
    <h2>Utwory</h2>
    <SongTable 
      v-if="songs.length" 
      :songs="songs"
      selectable
      show-playlist-action
      @add-to-playlist="openPlaylistModal"
    />
    <p v-else>Brak utworów.</p>

    <AddToPlaylistModal
      :show="showPlaylistModal"
      :playlists="playlists"
      :loading="addingToPlaylist"
      :error="playlistModalError"
      @close="closePlaylistModal"
      @add="handleAddToPlaylist"
    />
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useSongs } from '@/composables/api/useSongs'
import { usePlaylists } from '@/composables/api/usePlaylists'
import SongTable from '@/components/SongTable.vue'
import AddToPlaylistModal from '@/components/AddToPlaylistModal.vue'

const { songs, loading, error, fetchAll } = useSongs()
const { playlists, fetchMy: fetchPlaylists, addSong: addSongToPlaylist } = usePlaylists()

const showPlaylistModal = ref(false)
const selectedSongIds = ref<number[]>([])
const addingToPlaylist = ref(false)
const playlistModalError = ref('')

onMounted(async () => {
  await fetchAll()
  await fetchPlaylists()
})

function openPlaylistModal(songIds: (string | number)[]) {
  selectedSongIds.value = songIds.map(id => typeof id === 'string' ? parseInt(id, 10) : id)
  showPlaylistModal.value = true
  playlistModalError.value = ''
}

function closePlaylistModal() {
  showPlaylistModal.value = false
  selectedSongIds.value = []
  playlistModalError.value = ''
}

async function handleAddToPlaylist(playlistId: number) {
  addingToPlaylist.value = true
  playlistModalError.value = ''
  
  try {
    for (const songId of selectedSongIds.value) {
      await addSongToPlaylist(playlistId, songId)
    }
    closePlaylistModal()
  } catch (e: any) {
    playlistModalError.value = e.message || 'Błąd dodawania do playlisty'
  } finally {
    addingToPlaylist.value = false
  }
}
</script>

<style scoped>
.songs { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>
