import { ref } from 'vue'
import axios from 'axios'
import type { Song } from '@/types'
import { useRouter } from 'vue-router'

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
  const router = useRouter()

  async function fetch() {
    loading.value = true
    error.value = ''
    try {
      const token = localStorage.getItem('token') || ''
      const res = await axios.get('/api/songs/my', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      songs.value = res.data
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Błąd pobierania twoich utworów'
    } finally {
      loading.value = false
    }
  }

  async function upload(formData: FormData) {
    try {
      const token = localStorage.getItem('token') || '' 
      const res = await axios.post('/api/songs/upload', formData, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })
      await router.push('/songs/my')
      return res.data
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd tworzenia utworu'
      throw e
    }
  }

  async function delete_(id: number | string) {
    try {
      const token = localStorage.getItem('token') || ''
      await axios.delete(`/api/songs/${id}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      songs.value = songs.value.filter(s => s.id !== id)
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd usuwania utworu'
      throw e
    }
  }
  async function addToAlbum(songId: number, albumId: number) {
    try {
      const token = localStorage.getItem('token') || ''
      await axios.post(`/api/songs/${songId}/add-to-album`, { albumId }, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
    } catch (e: any) {
      error.value = e.response?.data?.error || 'Błąd dodawania utworu do albumu'
      throw e
    }     
  }
  return { songs, loading, error, fetch, upload, delete: delete_, addToAlbum }
}