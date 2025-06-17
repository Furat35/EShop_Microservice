import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
  root: 'admin',
  plugins: [
    vue(),
    // vueDevTools(),
  ],
  resolve: {
    alias: {
      '@admin': fileURLToPath(new URL('.', import.meta.url)),
      '@shared': fileURLToPath(new URL('../src/shared', import.meta.url)),
    },
  },
  build: {
    outDir: './dist/admin',
    emptyOutDir: true,
  },
})
