<template>
  <section class="albums">
    <h2>Moje Albumy</h2>
    <AlbumList v-if="albums.length" :albums="albums"/>
    <p v-else>Brak albumów.</p>
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useMyAlbums } from '@/composables/api/useAlbums'
import AlbumList from '@/components/AlbumList.vue'

const { albums, loading, error, fetch, delete: deleteAlbum } = useMyAlbums()

onMounted(fetch)

</script>

<style scoped>
.albums { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
</style>
