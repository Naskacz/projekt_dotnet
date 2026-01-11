<template>
  <section class="songs">
    <h2>Utwory</h2>
    <SongTable v-if="songs.length" :songs="songs"/>
    <p v-else>Brak utworów.</p>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useSongs } from '@/composables/api/useSongs'
import SongTable from '@/components/SongTable.vue'

const { songs, loading, error, fetchAll, fetchOne } = useSongs()

onMounted(fetchAll)
</script>

<style scoped>
.songs { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>
