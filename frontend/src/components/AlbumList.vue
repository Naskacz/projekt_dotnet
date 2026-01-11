<template>
  <table class="albums-table">
    <thead>
      <tr>
        <th>Nazwa</th>
        <th>Artysta</th>
        <th>Rok</th>
        <th>Liczba utworów</th>
        <th v-if="showActions">Akcje</th>
      </tr>
    </thead>
    <tbody>
      <template v-for="album in albums" :key="album.id">
        <tr>
          <td>
            <RouterLink :to="`/albums/${album.id}`">{{ album.name }}</RouterLink>
          </td>
          <td>{{ album.artist }}</td>
          <td>{{ album.releaseYear }}</td>
          <td>{{ album.songs?.length || 0 }}</td>
          <td v-if="showActions" class="actions">
            <button @click="$emit('delete', album.id)" class="btn-delete">Usuń</button>
          </td>
        </tr>
      </template>
    </tbody>
  </table>
</template>

<script setup>
import SongTable from './SongTable.vue'

defineProps({
  albums: {
    type: Array,
    required: true
  },
  showActions: {
    type: Boolean,
    default: false
  }
})

defineEmits(['delete'])
</script>

<style scoped>
.albums-table { width: 100%; border-collapse: collapse; background: #fff; margin-top: 1rem; }
.albums-table th, .albums-table td { padding: 0.75rem; border-bottom: 1px solid #e0e0e0; text-align: left; }
.albums-table thead { background: #2c3e50; color: #fff; }
.actions { text-align: center; }
.btn-delete { background: #c00; color: #fff; border: none; padding: 0.4rem 0.8rem; border-radius: 4px; cursor: pointer; }
.btn-delete:hover { background: #900; }
</style>