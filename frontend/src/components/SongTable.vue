<template>
  <table class="songs-table">
    <thead>
      <tr>
        <th>Tytuł</th>
        <th>Artysta</th>
        <th>Rok</th>
        <th>Gatunek</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="song in songs" :key="song.id">
        <td>{{ song.title }}</td>
        <td>{{ song.artist }}</td>
        <td>{{ song.year }}</td>
        <td>{{ song.genre }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup>
defineProps({
  songs: {
    type: Array,
    required: true
  }
})
</script>

<style scoped>
.songs-table { width: 100%; border-collapse: collapse; background: #f9f9f9; margin: 0.5rem 0; }
.songs-table th, .songs-table td { padding: 0.5rem; border-bottom: 1px solid #ddd; text-align: left; font-size: 0.9rem; }
.songs-table thead { background: #34495e; color: #fff; }
</style>