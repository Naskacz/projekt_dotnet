<template>
	<section class="playlists">
		<h2>Playlisty</h2>
		<p v-if="error" class="error">{{ error }}</p>

		<div v-if="playlists.length" class="list">
			<article v-for="p in playlists" :key="p.id" class="card">
				<header>
					<h3>{{ p.name }}</h3>
					<small>{{ p.isPublic ? 'Publiczna' : 'Prywatna' }}</small>
				</header>
				<p class="meta">{{ (p.songs?.length || 0) }} utworów</p>
				<router-link :to="`/playlists/${p.id}`" class="link">Otwórz</router-link>
			</article>
		</div>
		<p v-else>Brak playlist.</p>
	</section>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { usePlaylists } from '@/composables/api/usePlaylists'

const { playlists, error, fetchAll } = usePlaylists()

onMounted(fetchAll)
</script>

<style scoped>
.playlists { max-width: 900px; margin: 2rem auto; padding: 0 1rem; }
.list { display: grid; gap: 1rem; }
.card { border: 1px solid #e5e7eb; border-radius: 8px; padding: 1rem; background: #fff; box-shadow: 0 2px 6px rgba(0,0,0,0.04); }
.card header { display: flex; justify-content: space-between; align-items: baseline; }
.meta { color: #666; margin: 0.4rem 0; }
.link { display: inline-block; margin-top: 0.5rem; color: #2c3e50; text-decoration: none; font-weight: 600; }
.link:hover { text-decoration: underline; }
.error { color: #b91c1c; margin-bottom: 0.5rem; }
</style>
