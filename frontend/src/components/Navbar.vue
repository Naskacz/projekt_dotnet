<template>
    <nav class="navbar">
        <div class="navbar-brand">
        <router-link to="/" class="navbar-item">Aplikacja</router-link>
        </div>
        <div class="navbar-item navbar-search">
            <input 
              v-model="searchQuery"
              @input="handleSearch"
              type="text" 
              placeholder="Szukaj utworów, playlist..." 
            />
            <div v-if="showResults && results.length" class="search-results">
              <div v-for="r in results" :key="`${r.type}-${r.id}`" class="result-item">
                <a
                  class="result-link"
                  @click.prevent="handleSelect(r.type === 'song' ? `/songs/${r.id}` : r.type === 'album' ? `/albums/${r.id}` : `/playlists/${r.id}`)"
                >
                  {{ r.type === 'song' ? '(Utwór)' : r.type === 'album' ? '(Album)' : '(Playlista)' }}
                  {{ r.title || r.Title || r.name || r.Name }}
                  <span v-if="r.artist || r.Artist">- {{ r.artist || r.Artist }}</span>
                </a>
              </div>
            </div>
        </div>
        <div class="navbar-item">
            <button v-if="token" @click="logout" class="button is-light">Wyloguj się</button>
            <router-link v-else to="/login" class="button is-light">Zaloguj się</router-link>
        </div>
    </nav>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { useSearch } from '@/composables/api/useSearch'
import { useRouter } from 'vue-router'

const router = useRouter()
const token = ref(localStorage.getItem('token') || '')
const searchQuery = ref('')
const showResults = ref(false)
const { results, search } = useSearch()

function updateToken() {
  token.value = localStorage.getItem('token') || ''
}

async function logout() {
  localStorage.removeItem('token')
  window.dispatchEvent(new Event('token-changed'))
  window.location.href = '/login'
}

function handleSearch() {
  if (searchQuery.value.length > 2) {
    search(searchQuery.value)
    showResults.value = true
  } else {
    results.value = []
    showResults.value = false
  }
}

function closeResults() {
  showResults.value = false
}

function handleSelect(to: string) {
  closeResults()
  router.push(to)
}

function onClickOutside(e: MouseEvent) {
  const target = e.target as HTMLElement
  if (!target.closest('.navbar-search')) {
    closeResults()
  }
}

onMounted(() => {
  window.addEventListener('token-changed', updateToken)
  document.addEventListener('click', onClickOutside)
})

onBeforeUnmount(() => {
  window.removeEventListener('token-changed', updateToken)
  document.removeEventListener('click', onClickOutside)
})
</script>

<style scoped>
.navbar {
  background-color: black;
  padding: 1rem 5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  justify-content: space-between;
  border-bottom: 2px solid white;
}

.navbar-item {
  font-weight: bold;
  font-size: 1.2rem;
  color: white;
}
.button {
  margin-left: 1rem;
  background-color: transparent;
  color: white;
  border: 1px solid white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
}
.navbar-search {
  position: relative;
  flex: 1 1 520px;
  display: flex;
  justify-content: center;
  margin: 0 2rem;
}
.navbar-search input {
  width: 100%;
  max-width: 520px;
  padding: 0.5rem;
  border-radius: 4px;
  border: none;
}
.search-results {
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translateX(-50%);
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  max-width: 520px;
  width: 100%;
  max-height: 300px;
  overflow-y: auto;
  z-index: 100;
  margin-top: 0.5rem;
}
.result-item {
  padding: 0.75rem;
  border-bottom: 1px solid #f0f0f0;
}
.result-item:last-child {
  border-bottom: none;
}
.result-link {
  color: #333;
  text-decoration: none;
  display: block;
}
.result-link:hover {
  color: #667eea;
  font-weight: 500;
}
</style>