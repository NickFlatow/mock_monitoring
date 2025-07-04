import React from 'react';

import { BaseLog } from '../components/logs/BaseLog';

interface SensorLogProps {
    sensorId: string;
}

export function SensorLog({ sensorId }: SensorLogProps) {
    return (
        <BaseLog
            sensorId={sensorId}

            // very lazy
            // should be replaced with dal repo 
            fetchLogs={ async (sensorId: string, offset: number) => {
                const response = await fetch(`http://localhost:5132/api/sensor/${sensorId}?offset=${offset}`);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            }}
            />
        )
}
