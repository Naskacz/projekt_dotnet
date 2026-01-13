import { createRouter, createWebHistory } from 'vue-router'
import { routes } from 'vue-router/auto-routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  
  if (to.path !== '/login' && !token) {
    next('/login');
  } 
  else if (to.path === '/' && token) {
    next('/songs');
  } 
  else {
    next();
  }
});

export default router
