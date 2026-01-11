<template>
    <section class="create">
        <h2>Utwórz album</h2>

        <label>
        Name
        <input v-model="name" type="text" />
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

        <button @click="submit">Wyślij</button>

        <div v-if="message">{{ message }}</div>
    </section>


</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios';

const name = ref('');
const artist = ref('');
const year = ref('');
const genre = ref('');
const message = ref('');

async function submit(){
  console.log('Submitting album:', name.value, artist.value, year.value, genre.value);
    try {
        message.value = '';

        const formData = new FormData();
        formData.append('name', name.value);
        formData.append('artist', artist.value);
        formData.append('releaseyear', year.value);
        formData.append('genre', genre.value);
        const token = localStorage.getItem('token')
        const response = await axios.post('/api/albums/create', formData, {
            headers: { Authorization: `Bearer ${token}`}
        });

        message.value = 'Album created successfully!';
    } catch (error) {
        message.value = 'Error creating album.';
    }
}

</script>

<style>
.create {
  max-width: 480px;
  margin: 1rem auto;
}
.create label {
  display: block;
  margin-bottom: 0.5rem;
}
.create input {
  width: 100%;
  padding: 0.4rem;
  margin-top: 0.25rem;
}
.create button {
  margin-top: 0.5rem;
}
.error {
  color: #c00;
  margin-top: 0.5rem;
}
</style>