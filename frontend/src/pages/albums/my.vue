<template>
  <section class="albums">
    <h2>Moje Albumy</h2>
    <AlbumList 
    v-if="albums.length" 
    :albums="albums"
    show-actions
    @delete="handleDelete"
    />
    <p v-else>Brak albumów.</p>
    <p> <router-link class="router-link" to="/albums/create">Utwórz nowy album</router-link> </p>     
  </section>
</template>

<script setup>
import { onMounted } from 'vue'
import { useMyAlbums } from '@/composables/api/useAlbums'
import AlbumList from '@/components/AlbumList.vue'

const { albums, loading, error, fetch, delete: deleteAlbum } = useMyAlbums()

onMounted(fetch)

async function handleDelete(id) {
  if(!confirm('Czy na pewno chcesz usunąć ten album?')) return
  try{
    await deleteAlbum(id)
    await fetch()
  } catch (error) {
    console.error('Error deleting album:', error);
  }
}
</script>

<style scoped>
.albums { max-width: 1200px; margin: 2rem auto; padding: 0 1rem; }
.router-link {
  color: #667eea;
  font-weight: 600;
  text-decoration: none;
}
.router-link:hover {
  color: #4f46e5;
  text-decoration: underline;
}
</style>
