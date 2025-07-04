import React, { createContext, useEffect, useState } from 'react';
import { SensorLog } from './Pages/SensorLog';



export default function App() {
    return (
        <main>
            <SensorLog
                sensorId='1'
            />
        </main>
    );
}