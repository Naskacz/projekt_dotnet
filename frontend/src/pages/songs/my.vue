<template>
  <section class="songs">
    <h2>Moje Utwory</h2>
    <SongTable 
      v-if="songs.length" 
      :songs="songs"
      selectable
      show-delete
      show-playlist-action
      show-album-action
      @add-to-album="openAlbumModal"
      @add-to-playlist="openPlaylistModal"
      @delete="handleDelete"
    />
    <p v-else>Brak utworów.</p>

    <AddToAlbumModal
      :show="showAlbumModal"
      :albums="albums"
      :loading="addingToAlbum"
      :error="albumModalError"
      @close="closeAlbumModal"
      @add="handleAddToAlbum"
    />

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
import { useMySongs } from '@/composables/api/useSongs'
import { useMyAlbums } from '@/composables/api/useAlbums'
import { usePlaylists } from '@/composables/api/usePlaylists'
import SongTable from '@/components/SongTable.vue'
import AddToAlbumModal from '@/components/AddToAlbumModal.vue'
import AddToPlaylistModal from '@/components/AddToPlaylistModal.vue'

const { songs, loading, error, fetch, delete: deleteSong, addToAlbum } = useMySongs()
const { albums, fetch: fetchAlbums } = useMyAlbums()
const { playlists, fetchAll: fetchPlaylists, addSong: addSongToPlaylist } = usePlaylists()

// Album modal
const showAlbumModal = ref(false)
const selectedSongIds = ref<number[]>([])
const addingToAlbum = ref(false)
const albumModalError = ref('')

// Playlist modal
const showPlaylistModal = ref(false)
const addingToPlaylist = ref(false)
const playlistModalError = ref('')

onMounted(async () => {
  await fetch()
  await fetchAlbums()
  await fetchPlaylists()
})

// Album handlers
function openAlbumModal(songIds: (string | number)[]) {
  selectedSongIds.value = songIds.map(id => typeof id === 'string' ? parseInt(id, 10) : id)
  showAlbumModal.value = true
  albumModalError.value = ''
}

function closeAlbumModal() {
  showAlbumModal.value = false
  selectedSongIds.value = []
  albumModalError.value = ''
}

async function handleAddToAlbum(albumId: number) {
  addingToAlbum.value = true
  albumModalError.value = ''
  
  try {
    for (const songId of selectedSongIds.value) {
      await addToAlbum(songId, albumId)
    }
    closeAlbumModal()
  } catch (e: any) {
    albumModalError.value = e.message || 'Błąd dodawania do albumu'
  } finally {
    addingToAlbum.value = false
  }
}

// Playlist handlers
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

// Delete handler
async function handleDelete(id: number | string) {
  if (confirm('Czy na pewno chcesz usunąć ten utwór?')) {
    try {
      await deleteSong(id)
    } catch (e) {
      alert('Błąd usuwania utworu')
    }
  }
}
</script>

<style scoped>
.songs { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>