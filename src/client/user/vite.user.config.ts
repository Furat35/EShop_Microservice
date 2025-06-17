import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
  root: 'user',
  plugins: [
    vue(),
    // vueDevTools(),
  ],
  resolve: {
    alias: {
      '@user': fileURLToPath(new URL('.', import.meta.url)),
      '@shared': fileURLToPath(new URL('../src/shared', import.meta.url)),
    },
  },
  build: {
    outDir: './dist/user',
    emptyOutDir: true,
  },
})
