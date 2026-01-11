import { ref } from 'vue'
import axios from 'axios'
import type { Song } from '@/types'

export function useSongs() {
  const songs = ref<Song[]>([])
  const loading = ref(false)
  const error = ref('')

  async function fetchAll() {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get('/api/songs')
      songs.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania utworów'
    } finally {
      loading.value = false
    }
  }

  async function fetchOne(id: number | string) {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get(`/api/songs/${id}`)
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania utworu'
      throw e
    } finally {
      loading.value = false
    }
  }

  return { songs, loading, error, fetchAll, fetchOne }
}

export function useMySongs() {
  const songs = ref<Song[]>([])
  const loading = ref(false)
  const error = ref('')

  async function fetch() {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get('/api/songs/my')
      songs.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania twoich utworów'
    } finally {
      loading.value = false
    }
  }

  async function create(formData: FormData) {
    try {
      const res = await axios.post('/api/songs/create', formData)
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd tworzenia utworu'
      throw e
    }
  }

  async function delete_(id: number | string) {
    try {
      await axios.delete(`/api/songs/${id}`)
      songs.value = songs.value.filter(s => s.id !== id)
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd usuwania utworu'
      throw e
    }
  }

  return { songs, loading, error, fetch, create, delete: delete_ }
}