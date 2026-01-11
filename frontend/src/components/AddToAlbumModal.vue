<template>
  <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal">
      <h3>Dodaj do albumu</h3>
      
      <label>
        Wybierz album:
        <select v-model="selectedAlbumId">
          <option :value="null">-- Wybierz --</option>
          <option v-for="album in albums" :key="album.id" :value="album.id">
            {{ album.name }} ({{ album.artist }})
          </option>
        </select>
      </label>

      <div class="actions">
        <button @click="$emit('close')">Anuluj</button>
        <button 
          :disabled="!selectedAlbumId || loading" 
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
import type { Album } from '@/types'

interface Props {
  show: boolean
  albums: Album[]
  loading?: boolean
  error?: string
}

const props = defineProps<Props>()
const emit = defineEmits<{
  close: []
  add: [albumId: number]
}>()

const selectedAlbumId = ref<number | null>(null)

watch(() => props.show, (val) => {
  if (!val) selectedAlbumId.value = null
})

function handleAdd() {
  if (selectedAlbumId.value) {
    emit('add', selectedAlbumId.value)
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
}
.actions {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-end;
  margin-top: 1rem;
}
.error {
  color: #c00;
  margin-top: 1rem;
}
</style>