<template>
  <section class="albums">
    <h2>Albumy</h2>
    <AlbumList v-if="albums.length" :albums="albums"/>
    <p v-else>Brak albumów.</p>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import AlbumList from '@/components/AlbumList.vue'
import { useAlbums } from '@/composables/api/useAlbums'


const { albums, loading, error, fetchAll } = useAlbums()

onMounted(fetchAll)
</script>

<style scoped>
.albums { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>
