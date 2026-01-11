<template>
  <aside class="side-nav">
    <nav>
      <RouterLink to="/playlists/my" class="item" exact-active-class="active">Moje Playlisty</RouterLink>
      <RouterLink to="/albums/my" class="item" exact-active-class="active">Moje Albumy</RouterLink>
      <RouterLink to="/songs/my" class="item" exact-active-class="active">Moje Utwory</RouterLink>
      <RouterLink to="/playlists" class="item" exact-active-class="active">Playlisty</RouterLink>
      <RouterLink to="/albums" class="item" exact-active-class="active">Albumy</RouterLink>
      <RouterLink to="/" class="item" exact-active-class="active">Utwory</RouterLink>
    </nav>
  </aside>
</template>

<script setup>
// no script needed
</script>

<style scoped>
.side-nav {
  width: 240px;
  flex: 0 0 240px;
  border-right: 1px solid #e0e0e0;
  padding: 1rem;
  background: #fafafa;
}
.item {
  display: block;
  padding: 0.5rem 0.25rem;
  color: #2c3e50;
  text-decoration: none;
  border-radius: 4px;
}
.item:hover { background: #eef3f8; }
.active { font-weight: 600; color: #1e88e5; }
</style>