import React from 'react';
import ReactDOM from 'react-dom/client';

import App from './App'; 

// Importing the Bootstrap CSS
// import 'bootstrap/dist/css/bootstrap.min.css';

try {
    const rootElem = document.getElementById('root');
    if (!rootElem) {
        throw new Error('Could not find root element');
    }
    const root = ReactDOM.createRoot(rootElem);

    root.render(
        // <App />
        <React.StrictMode>
            <App />
        </React.StrictMode>
    );
} catch (error) {
    console.error(error);
}