<template>
  <section class="upload">
    <h2>Prześlij utwór</h2>

    <label>
      Title
      <input v-model="title" type="text" />
    </label>
    <label>
      Artist
      <input v-model="artist" type="text" />
    </label>
    <label>
      Year
      <input v-model="year" type="text" />
    </label>
    <label>
      Genre
      <input v-model="genre" type="text" />
    </label>
    <label>
      Plik
      <input @change="onFileChange" type="file" accept="audio/*" />
    </label>

    <button :disabled="!file || loading" @click="submit">Wyślij</button>

    <div v-if="message" :class="{ error: isError }">{{ message }}</div>
  </section>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'

const file = ref<File | null>(null)
const title = ref('')
const artist = ref('')
const year = ref('')
const genre = ref('')
const loading = ref(false)
const message = ref('')
const isError = ref(false)

function onFileChange(e: Event) {
  const input = e.target as HTMLInputElement
  file.value = input.files && input.files[0] ? input.files[0] : null
  message.value = ''
  isError.value = false
}

async function submit() {
  if (!file.value) {
    message.value = 'Wybierz plik'
    isError.value = true
    return
  }

  loading.value = true
  message.value = ''
  isError.value = false

  const fd = new FormData()
  fd.append('file', file.value)
  fd.append('title', title.value)
  fd.append('artist', artist.value)
  fd.append('year', year.value)
  fd.append('genre', genre.value)

  try {
    const res = await axios.post('/api/songs/upload', fd)
    message.value = (res.data && res.data.message) ? res.data.message : `OK (${res.status})`
  } catch (err: any) {
    if (err.response) {
      message.value = `Błąd ${err.response.status}: ${err.response.data || err.response.statusText}`
    } else {
      message.value = `Błąd: ${err.message || 'sieć'}`
    }
    isError.value = true
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.upload {
  max-width: 480px;
  margin: 1rem auto;
}
label {
  display: block;
  margin-bottom: 0.5rem;
}
input[type="text"] {
  width: 100%;
  padding: 0.4rem;
  margin-top: 0.25rem;
}
button {
  margin-top: 0.5rem;
}
.error {
  color: #c00;
  margin-top: 0.5rem;
}
</style>