import { ref } from 'vue'
import axios from 'axios'
import type { Album, Song } from '@/types'

export function useAlbums() {
  const albums = ref<Album[]>([])
  const loading = ref(false)
  const error = ref('')

  async function fetchAll() {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get('/api/albums')
      albums.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania albumów'
    } finally {
      loading.value = false
    }
  }

  async function fetchOne(id: number | string) {
    loading.value = true
    error.value = ''
    try {
      const res = await axios.get(`/api/albums/${id}`)
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania albumu'
      throw e
    } finally {
      loading.value = false
    }
  }

  return { albums, loading, error, fetchAll, fetchOne }
}

export function useMyAlbums() {
  const albums = ref<Album[]>([])
  const loading = ref(false)
  const error = ref('')

  async function fetch() {
    loading.value = true
    error.value = ''
    try {
      const token = localStorage.getItem('token') || ''
      const res = await axios.get('/api/albums/my', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      albums.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania twoich albumów'
    } finally {
      loading.value = false
    }
  }

  async function create(formData: FormData) {
    try {
      const token = localStorage.getItem('token') || ''
      const res = await axios.post('/api/albums/create', formData, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd tworzenia albumu'
      throw e
    }
  }

  async function delete_(id: number | string) {
    try {
      const token = localStorage.getItem('token') || ''
      await axios.delete(`/api/albums/${id}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      albums.value = albums.value.filter(a => a.id !== id)
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd usuwania albumu'
      throw e
    }
  }

  return { albums, loading, error, fetch, create, delete: delete_ }
}