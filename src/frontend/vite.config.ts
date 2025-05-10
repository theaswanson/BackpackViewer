import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

// https://vite.dev/config/
export default defineConfig((configEnv) => ({
  base: configEnv.mode === "development" ? "/" : "/BackpackViewer/",
  plugins: [react()],
}));
