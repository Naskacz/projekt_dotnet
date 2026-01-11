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
      <input v-model="year" type="number" min="0" max="2100" />
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
import { useMySongs } from '@/composables/api/useSongs'

const file = ref<File | null>(null)
const title = ref('')
const artist = ref('')
const year = ref('')
const genre = ref('')
const message = ref('')
const isError = ref(false)

const { upload, loading, error } = useMySongs()

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

  message.value = ''
  isError.value = false

  const fd = new FormData()
  fd.append('file', file.value)
  fd.append('title', title.value)
  fd.append('artist', artist.value)
  fd.append('year', year.value)
  fd.append('genre', genre.value)

  try {
    await upload(fd)
    message.value = 'Przesłano'
    isError.value = false
  } catch (e: any) {
    message.value = error.value || e.message || 'Błąd'
    isError.value = true
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
input {
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