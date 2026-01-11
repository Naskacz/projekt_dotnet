<template>
    <section class="songs-container">
        <h1>Song List</h1>
        <div v-if="songs.length === 0" class="no-songs">Brak utworów</div>
        <table v-else class="songs-table">
            <thead>
                <tr>
                    <th>Tytuł</th>
                    <th>Artysta</th>
                    <th>Rok</th>
                    <th>Gatunek</th>
                    <th>Dodane przez</th>
                    <th>Plik</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="song in songs" :key="song.id">
                    <td>{{ song.title }}</td>
                    <td>{{ song.artist }}</td>
                    <td>{{ song.year }}</td>
                    <td>{{ song.genre }}</td>
                    <td>{{ song.createdBy }}</td>
                    <td>
                        <audio :src="song.fileUrl" controls></audio>
                    </td>
                </tr>
            </tbody>
        </table>
    </section>
    <p> <router-link to="/songs/create">Prześlij nowy utwór</router-link> </p>
</template>

<script setup>
    import axios from 'axios';
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';
    const songs = ref([]);
    async function fetchSongs() {
        try {
            const response = await axios.get('/api/songs');
            songs.value = response.data;
        } catch (error) {
            console.error('Error fetching songs:', error);
        }
    }
    onMounted(() => {
        fetchSongs();
    });

</script>

<style scoped>
.songs-container {
    max-width: 1200px;
    margin: 2rem auto;
    padding: 0 1.5rem;
}

h1 {
    text-align: center;
    color: #333;
    margin-bottom: 2rem;
    font-size: 2.5rem;
}

.no-songs {
    text-align: center;
    color: #999;
    padding: 2rem;
    font-size: 1.1rem;
}

.songs-table {
    width: 100%;
    border-collapse: collapse;
    background: white;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    border-radius: 8px;
    overflow: hidden;
}

.songs-table thead {
    background: #2c3e50;
    color: white;
}

.songs-table th {
    padding: 1rem;
    text-align: left;
    font-weight: 600;
    font-size: 0.95rem;
}

.songs-table td {
    padding: 1rem;
    border-bottom: 1px solid #e0e0e0;
}

.songs-table tbody tr {
    transition: background-color 0.3s ease;
}

.songs-table tbody tr:hover {
    background-color: #f8f9fa;
}

.songs-table tbody tr:last-child td {
    border-bottom: none;
}

</style>