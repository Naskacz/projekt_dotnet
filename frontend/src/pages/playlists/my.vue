<template>
  <section class="playlists">
    <h2>Moje playlisty</h2>

    <form class="create" @submit.prevent="handleCreate">
      <input v-model="name" type="text" placeholder="Nazwa playlisty" required />
      <label class="checkbox">
        <input type="checkbox" v-model="isPublic" /> Publiczna
      </label>
      <button type="submit" :disabled="loading">Utwórz</button>
    </form>

    <p v-if="error" class="error">{{ error }}</p>

    <div v-if="playlists.length" class="list">
      <article v-for="p in playlists" :key="p.id" class="card">
        <header>
          <h3>{{ p.name }}</h3>
          <small>{{ p.isPublic ? 'Publiczna' : 'Prywatna' }}</small>
        </header>
        <p class="meta">{{ (p.songs?.length || 0) }} utworów</p>
        <div class="actions">
          <router-link :to="`/playlists/${p.id}`">Otwórz</router-link>
          <button @click="handleDelete(p.id)">Usuń</button>
        </div>
      </article>
    </div>
    <p v-else>Brak playlist.</p>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { usePlaylists } from '@/composables/api/usePlaylists'

const { playlists, loading, error, fetchMy, create, delete: deletePlaylist } = usePlaylists()

const name = ref('')
const isPublic = ref(false)

onMounted(fetchMy)

async function handleCreate() {
  if (!name.value.trim()) return
  try {
    await create(name.value.trim(), isPublic.value)
    name.value = ''
    isPublic.value = false
  } catch (e) {
    // error już ustawione
  }
}

async function handleDelete(id: number | string) {
  if (!confirm('Usunąć playlistę?')) return
  try {
    await deletePlaylist(id)
  } catch (e) {
    // error już ustawione
  }
}
</script>

<style scoped>
.playlists { max-width: 900px; margin: 2rem auto; padding: 0 1rem; }
.create { display: flex; gap: 0.5rem;  margin-bottom: 1rem; }
.create input[type="text"] { flex: 1; padding: 0.6rem 0.8rem; border: 1px solid #ddd; border-radius: 6px; }
.create button { padding: 0.6rem 1rem; border: none; background: #2c3e50; color: #fff; border-radius: 6px; cursor: pointer; }
.create .checkbox { display: flex; align-items: center; gap: 0.35rem; font-size: 0.9rem; color: #555; }
.list { display: grid; gap: 1rem; }
.card { border: 1px solid #e5e7eb; border-radius: 8px; padding: 1rem; background: #fff; box-shadow: 0 2px 6px rgba(0,0,0,0.04); }
.card header { display: flex; justify-content: space-between; align-items: baseline; }
.meta { color: #666; margin: 0.4rem 0; }
.actions { display: flex; gap: 0.5rem; }
.actions a, .actions button { padding: 0.45rem 0.8rem; border-radius: 6px; border: 1px solid #e5e7eb; background: #f9fafb; cursor: pointer; text-decoration: none; color: #111; }
.actions button { background: #fee2e2; border-color: #fecaca; color: #b91c1c; }
.error { color: #b91c1c; margin-bottom: 0.5rem; }
</style>