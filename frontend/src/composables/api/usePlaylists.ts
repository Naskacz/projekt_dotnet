import { ref } from 'vue'
import axios from 'axios'
import type { Playlist } from '@/types'

export function usePlaylists() {
  const playlists = ref<Playlist[]>([])
  const loading = ref(false)
  const error = ref('')

  async function fetchAll() {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get('/api/playlists')
      playlists.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania playlist'
    } finally {
      loading.value = false
    }
  }

  async function fetchOne(id: number | string) {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get(`/api/playlists/${id}`)
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania playlisty'
      throw e
    } finally {
      loading.value = false
    }
  }

  return { playlists, loading, error, fetchAll, fetchOne }
}

export function useMyPlaylists() {
  const playlists = ref<Playlist[]>([])
  const loading = ref(false)
  const error = ref('')

  async function fetch() {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get('/api/playlists/my')
      playlists.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania twoich playlist'
    } finally {
      loading.value = false
    }
  }

  async function create(data: { name: string; description?: string }) {
    try {
      const res = await axios.post('/api/playlists/create', data)
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd tworzenia playlisty'
      throw e
    }
  }

  async function delete_(id: number | string) {
    try {
      await axios.delete(`/api/playlists/${id}`)
      playlists.value = playlists.value.filter(p => p.id !== id)
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd usuwania playlisty'
      throw e
    }
  }

  return { playlists, loading, error, fetch, create, delete: delete_ }
}