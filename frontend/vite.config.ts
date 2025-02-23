import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import basicSsl from "@vitejs/plugin-basic-ssl";

export default defineConfig({
    plugins: [react(), basicSsl()],
    server: {
        host: "localhost",
        port: 3000,
        https: true,
        watch: {
            usePolling: true,
        },
    },
});
