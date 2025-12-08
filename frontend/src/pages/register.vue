<template>
  <div class="register-container">
    <div class="register-card">
      <h2>Rejestracja</h2>
      
      <div v-if="error" class="error-message">{{ error }}</div>
      <div v-if="success" class="success-message">{{ success }}</div>
      
      <form @submit.prevent="handleRegister">
        <div class="form-group">
          <label for="email">Email</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
          />
        </div>
        
        <div class="form-group">
          <label for="password">Hasło</label>
          <input
            type="password"
            id="password"
            v-model="password"
            required
          />
        </div>
        
        <button type="submit" :disabled="loading" class="btn-register">
          {{ loading ? 'Rejestracja...' : 'Zarejestruj się' }}
        </button>
      </form>
      
      <p class="login-link">
        Masz już konto? <router-link to="/login">Zaloguj się</router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const router = useRouter();
const email = ref('');
const password = ref('');
const loading = ref(false);
const error = ref('');
const success = ref('');

async function handleRegister() {
  loading.value = true;
  error.value = '';
  success.value = '';
  
  try {
    await axios.post('/api/auth/register', {
      email: email.value,
      password: password.value
    });
    
    success.value = 'Rejestracja udana! Przekierowanie...';
    
    setTimeout(() => {
      router.push('/login');
    }, 1500);
    
  } catch (err) {
    error.value = err.response?.data?.message || 'Błąd rejestracji';
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
}

.register-card {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
}

h2 {
  text-align: center;
  margin-bottom: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
}

input {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.btn-register {
  width: 100%;
  padding: 0.75rem;
  background: #2c3e50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-register:disabled {
  background: #ccc;
}

.error-message {
  background: #fee;
  color: #c00;
  padding: 0.5rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.success-message {
  background: #efe;
  color: #0a0;
  padding: 0.5rem;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.login-link {
  text-align: center;
  margin-top: 1rem;
}
</style>