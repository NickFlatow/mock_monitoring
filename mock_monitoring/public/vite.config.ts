import { defineConfig } from 'vite';


export default defineConfig({
    root: '.',
    base: './',

    server: {
        // host: '172.16.0.5',
        proxy: {
            // dev index 
            './': {
                ws:true,
                changeOrigin: true,
                target:'/index.html',
            }
        },
        fs: {
            // Allow serving files from one level up to the project root
            allow: ['..'],
        },
  
    },
    build: {
        
        sourcemap: true,
        // add rollup specific options here
        rollupOptions: {
            // https://rollupjs.org/configuration-options/
            input: [
                "./index.html",
            ],
            output: {
                // assetFileNames: assetInfo => {
     
                //     if (assetInfo.name?.endsWith('.mp3')) {
                //         //This will place the tpl.html files in the correct directory
                //         // return getTemplateFilePathFromAsset(assetInfo.name.split('.')[0]) + '/' + assetInfo.name;
                //         return 'assets/[name]-[hash][extname]';
                //     }
                //     // Default naming scheme for other assets
                //     return 'assets/[name]-[hash][extname]';
                // },
                // {
                //     dir: './dist',
                //     entryFileNames: '[name].js',
                //     format: 'esm',
                //     sourcemap: true,
                // },
            },
        },
    },
    css: {
        // devSourcemap: true,
        preprocessorOptions: {
            less: {
                sourceMap: {}
            },
        }
    },
})
