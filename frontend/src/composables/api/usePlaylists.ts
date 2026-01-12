import { ref } from 'vue'
import axios from 'axios'
import type { Playlist } from '@/types'

export function usePlaylists() {
  const playlists = ref<Playlist[]>([])
  const loading = ref(false)
  const error = ref('')

  function authHeader() {
    const token = localStorage.getItem('token') || ''
    return token ? { Authorization: `Bearer ${token}` } : {}
  }

  async function fetchMy() {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get('/api/playlists/my', {
        headers: authHeader()
      })
      playlists.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania playlist'
    } finally {
      loading.value = false
    }
  }

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
      const res = await axios.get(`/api/playlists/${id}`, {
        headers: authHeader()
      })
      return res.data as Playlist
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania playlisty'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function create(name: string, isPublic = false) {
    error.value = ''
    try {
      const res = await axios.post('/api/playlists', { name, isPublic }, {
        headers: authHeader()
      })
      playlists.value.unshift(res.data)
      return res.data as Playlist
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd tworzenia playlisty'
      throw e
    }
  }

  async function delete_(id: number | string) {
    error.value = ''
    try {
      await axios.delete(`/api/playlists/${id}`, {
        headers: authHeader()
      })
      playlists.value = playlists.value.filter(p => p.id !== id)
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd usuwania playlisty'
      throw e
    }
  }

  async function addSong(playlistId: number | string, songId: number | string) {
    error.value = ''
    try {
      await axios.post(`/api/playlists/${playlistId}/songs`, { songId }, {
        headers: authHeader()
      })
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd dodawania utworu do playlisty'
      throw e
    }
  }

  async function removeSong(playlistId: number | string, songId: number | string) {
    error.value = ''
    try {
      await axios.delete(`/api/playlists/${playlistId}/songs/${songId}`, {
        headers: authHeader()
      })
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd usuwania utworu z playlisty'
      throw e
    }
  }

  return { playlists, loading, error, fetchAll, fetchMy, fetchOne, create, delete: delete_, addSong, removeSong }
}