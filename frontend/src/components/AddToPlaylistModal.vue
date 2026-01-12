<template>
  <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal">
      <h3>Dodaj do playlisty</h3>
      
      <label>
        Wybierz playlistę:
        <select v-model="selectedPlaylistId">
          <option :value="null">-- Wybierz --</option>
          <option v-for="playlist in playlists" :key="playlist.id" :value="playlist.id">
            {{ playlist.name }} {{ playlist.isPublic ? '(publiczna)' : '(prywatna)' }}
          </option>
        </select>
      </label>

      <div class="actions">
        <button @click="$emit('close')">Anuluj</button>
        <button 
          :disabled="!selectedPlaylistId || loading" 
          @click="handleAdd"
        >
          Dodaj
        </button>
      </div>

      <div v-if="error" class="error">{{ error }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import type { Playlist } from '@/types'

interface Props {
  show: boolean
  playlists: Playlist[]
  loading?: boolean
  error?: string
}

const props = defineProps<Props>()
const emit = defineEmits<{
  close: []
  add: [playlistId: number]
}>()

const selectedPlaylistId = ref<number | null>(null)

watch(() => props.show, (val) => {
  if (!val) selectedPlaylistId.value = null
})

function handleAdd() {
  if (selectedPlaylistId.value) {
    emit('add', selectedPlaylistId.value)
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
.modal {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  min-width: 400px;
  max-width: 90%;
}
.modal h3 {
  margin-top: 0;
}
label {
  display: block;
  margin-bottom: 1rem;
}
select {
  width: 100%;
  padding: 0.5rem;
  margin-top: 0.25rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}
.actions {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-end;
  margin-top: 1rem;
}
button {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
button:first-child {
  background: #ccc;
}
button:last-child {
  background: #9333ea;
  color: white;
}
button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.error {
  color: #c00;
  margin-top: 1rem;
}
</style>