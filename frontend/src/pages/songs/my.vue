<template>
  <section class="songs">
    <h2>Moje Utwory</h2>
    <SongTable v-if="songs.length" :songs="songs"/>
    <p v-else>Brak utworów.</p>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useMySongs } from '@/composables/api/useSongs'
import SongTable from '@/components/SongTable.vue'

const { songs, loading, error, fetch, create, delete: deleteSong } = useMySongs()

onMounted(fetch)
</script>

<style scoped>
.songs { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>