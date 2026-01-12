<template>
  <div class="login-container">
    <div class="login-card">
      <h2>Logowanie</h2>
      
      <div v-if="error" class="error-message">{{ error }}</div>
      <div v-if="success" class="success-message">{{ success }}</div>
      
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">Email</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
            placeholder="Wprowadź email"
          />
        </div>
        
        <div class="form-group">
          <label for="password">Hasło</label>
          <input
            type="password"
            id="password"
            v-model="password"
            required
            placeholder="Wprowadź hasło"
          />
        </div>
        
        <button type="submit" :disabled="loading" class="btn-login">
          {{ loading ? 'Logowanie...' : 'Zaloguj się' }}
        </button>
      </form>
      
      <p class="register-link">
        Nie masz konta? <router-link to="/register">Zarejestruj się</router-link>
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

async function handleLogin() {
  loading.value = true;
  error.value = '';
  success.value = '';
  
  try {
    const response = await axios.post('/api/auth/login', {
      email: email.value,
      password: password.value
    });
    
    console.log('=== LOGIN RESPONSE ===', response.data);
    console.log('=== TOKEN ===', response.data.token);
    
    // Wyczyść stary token
    localStorage.clear();
    
    // Zapisz nowy token
    localStorage.setItem('token', response.data.token);
    localStorage.setItem('userEmail', response.data.email);
    window.dispatchEvent(new Event('token-changed'))
    
    console.log('=== SAVED TOKEN ===', localStorage.getItem('token'));
    
    success.value = 'Zalogowano pomyślnie!';
    
    axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
    
    setTimeout(() => {
      router.push('/');
    }, 1000);
    
  } catch (err) {
    error.value = err.response?.data?.message || 'Błąd logowania';
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 2rem;
}

.login-card {
  background: white;
  padding: 2.5rem;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
}

h2 {
  text-align: center;
  color: #2c3e50;
  margin-bottom: 2rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  color: #555;
  font-weight: 500;
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.3s;
}

input:focus {
  outline: none;
  border-color: #2c3e50;
}

.btn-login {
  width: 100%;
  padding: 0.875rem;
  background: #2c3e50;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s;
}

.btn-login:hover:not(:disabled) {
  background: #1a252f;
}

.btn-login:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.error-message {
  background: #fee;
  color: #c00;
  padding: 0.75rem;
  border-radius: 6px;
  margin-bottom: 1rem;
}

.success-message {
  background: #efe;
  color: #0a0;
  padding: 0.75rem;
  border-radius: 6px;
  margin-bottom: 1rem;
}

.register-link {
  text-align: center;
  margin-top: 1.5rem;
  color: #666;
}

.register-link a {
  color: #2c3e50;
  text-decoration: none;
  font-weight: 600;
}

.register-link a:hover {
  text-decoration: underline;
}
</style>