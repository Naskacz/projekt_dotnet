# frontend/Dockerfile
FROM node:20

# ustaw katalog roboczy
WORKDIR /app

# kopiuj package.json i package-lock.json / pnpm-lock.yaml
COPY package*.json ./

# zainstaluj zależności
RUN npm install

# kopiuj resztę frontendu
COPY . .

# expose port Vite
EXPOSE 5173

# komenda startowa (dev server)
CMD ["npm", "run", "dev", "--", "--host"]
