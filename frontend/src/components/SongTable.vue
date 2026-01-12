<template>
  <div>
    <div v-if="selectable" class="actions">
      <button 
        v-if="showAlbumAction"
        :disabled="selectedSongs.length === 0" 
        @click="$emit('add-to-album', selectedSongs)"
      >
        Dodaj/Przenieś do albumu ({{ selectedSongs.length }})
      </button>
      <button 
        v-if="showPlaylistAction"
        class="secondary"
        :disabled="selectedSongs.length === 0" 
        @click="$emit('add-to-playlist', selectedSongs)"
      >
        Dodaj do playlisty ({{ selectedSongs.length }})
      </button>
    </div>
    
    <table class="song-table">
      <thead>
        <tr>
          <th v-if="selectable">
            <input 
              type="checkbox" 
              @change="toggleAll" 
              :checked="allSelected"
            />
          </th>
          <th>Tytuł</th>
          <th>Artysta</th>
          <th>Rok</th>
          <th>Gatunek</th>
          <th>Odsłuch</th>
          <th v-if="showDelete">Akcje</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="song in songs" :key="song.id">
          <td v-if="selectable">
            <input 
              type="checkbox" 
              :value="song.id" 
              v-model="selectedSongs"
            />
          </td>
          <td>{{ song.title }}</td>
          <td>{{ song.artist }}</td>
          <td>{{ song.year }}</td>
          <td>{{ song.genre }}</td>
          <td>
            <audio v-if="song.fileUrl" :src="song.fileUrl" controls style="width:160px" />
          </td>
          <td v-if="showDelete">
            <button @click="$emit('delete', song.id)">Usuń</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import type { Song } from '@/types'

interface Props {
  songs: Song[]
  selectable?: boolean
  showDelete?: boolean
  showAlbumAction?: boolean
  showPlaylistAction?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  selectable: false,
  showDelete: false,
  showAlbumAction: false,
  showPlaylistAction: false
})

defineEmits<{
  delete: [id: number | string]
  'add-to-album': [songIds: (string | number)[]]
  'add-to-playlist': [songIds: (string | number)[]]
}>()

const selectedSongs = ref<(string | number)[]>([])

const allSelected = computed(() => 
  props.songs.length > 0 && selectedSongs.value.length === props.songs.length
)

function toggleAll(e: Event) {
  const checked = (e.target as HTMLInputElement).checked
  selectedSongs.value = checked ? props.songs.map(s => s.id) : []
}
</script>

<style scoped>
.song-table { 
  width: 100%;
  border-collapse: collapse; 
  background: #fff;
  margin-top: 1rem; 
}
.song-table th, .song-table td { 
  padding: 0.75rem; 
  border-bottom: 1px solid #e0e0e0; 
  text-align: left; 
}
.song-table thead { 
  background: #2c3e50; 
  color: #fff; 
}
.song-table button { 
  background: #c00; 
  color: #fff; 
  border: none; 
  padding: 0.4rem 0.8rem; 
  border-radius: 4px; 
  cursor: pointer; 
}
.actions { 
  margin-bottom: 0.5rem;
}
.actions button { 
  background: #3dc57b; 
  color: #fff; 
  border: none; 
  padding: 0.4rem 0.8rem; 
  border-radius: 4px; 
  cursor: pointer; 
}
.actions button.secondary {
  background: #3b82f6;
}
.actions button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.btn-delete { 
  background: #c00; 
  color: #fff; 
  border: none; 
  padding: 0.4rem 0.8rem; 
  border-radius: 4px; 
  cursor: pointer; 
}
.btn-delete:hover { 
  background: #900; 
}
</style>