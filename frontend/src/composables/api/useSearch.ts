import axios from 'axios'
import { ref } from 'vue'

export function useSearch() {
  const results = ref<any[]>([])
  const loading = ref(false)
  const error = ref('')

  async function search(query: string) {
    if (!query.trim()) {
      results.value = []
      return
    }

    loading.value = true
    error.value = ''

    try {
      const res = await axios.get('/api/search', { params: { q: query } })
      results.value = res.data.results
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd wyszukiwania'
      results.value = []
    } finally {
      loading.value = false
    }
  }

  return { results, loading, error, search }
}