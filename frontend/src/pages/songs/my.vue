<template>
  <section class="songs">
    <h2>Moje Utwory</h2>
    <SongTable 
      v-if="songs.length" 
      :songs="songs"
      selectable
      show-delete
      @add-to-album="openModal"
      @delete="handleDelete"
    />
    <p v-else>Brak utworów.</p>

    <AddToAlbumModal
      :show="showModal"
      :albums="albums"
      :loading="addingToAlbum"
      :error="modalError"
      @close="closeModal"
      @add="handleAddToAlbum"
    />
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useMySongs } from '@/composables/api/useSongs'
import { useMyAlbums } from '@/composables/api/useAlbums'
import SongTable from '@/components/SongTable.vue'
import AddToAlbumModal from '@/components/AddToAlbumModal.vue'

const { songs, loading, error, fetch, delete: deleteSong, addToAlbum } = useMySongs()
const { albums, fetch: fetchAlbums } = useMyAlbums()

const showModal = ref(false)
const selectedSongIds = ref<number[]>([])
const addingToAlbum = ref(false)
const modalError = ref('')

onMounted(async () => {
  await fetch()
  await fetchAlbums()
})

function openModal(songIds: (string | number)[]) {
  selectedSongIds.value = songIds.map(id => typeof id === 'string' ? parseInt(id, 10) : id)
  showModal.value = true
  modalError.value = ''
}

function closeModal() {
  showModal.value = false
  selectedSongIds.value = []
  modalError.value = ''
}

async function handleAddToAlbum(albumId: number) {
  addingToAlbum.value = true
  modalError.value = ''
  
  try {
    for (const songId of selectedSongIds.value) {
      await addToAlbum(songId, albumId)
    }
    closeModal()
  } catch (e: any) {
    modalError.value = e.message || 'Błąd dodawania do albumu'
  } finally {
    addingToAlbum.value = false
  }
}

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