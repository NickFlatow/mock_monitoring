import React, { useState, useEffect, useRef } from 'react';
import { useInfiniteScroll } from '../../hooks/useInfiniteScroll';


type APIResponse = {
    totalResults: number;
    logs: any[];
}


export interface BaseLogProps {
    sensorId: string;
    fetchLogs: (sensorId: string, offset: number) => Promise<APIResponse>;
}



export function BaseLog({ sensorId, fetchLogs }: BaseLogProps) {
    const limit = 10;
    //todo declare client-side interface for logs
    const [logs, setLogs] = useState<any[]>([]);
    const { bottom, offset, maxOffset, setMaxOffset } = useInfiniteScroll();

    useEffect(() => {
        const loadLogs = async () => {
            try {
                const logResults = await fetchLogs(sensorId, offset);

                if (logResults.totalResults != maxOffset) {
                    setMaxOffset(logResults.totalResults)
                }
                
                setLogs((prevLogs) => [...prevLogs, ...logResults.logs]);
                
                // setMaxOffset(logResults.Tot > 0 ? maxOffset + 10 : maxOffset);
            } catch (error) {
                console.error('Error fetching logs:', error);
            }
        };
        if (offset < maxOffset || offset === 0) {
            loadLogs();
        }
    }, [offset]); //update when offset changes

    return (
        <div>
            <h2>Sensor Logs for {sensorId}</h2>
            <ul>
                {logs.map((log, index) => (
                    <li key={index}>{JSON.stringify(log)}</li>
                ))}
            </ul>
            {/* I am the bottom reference for the observer in useInfiniteScroll */}
            <div ref={bottom} style={{ height: '20px' }} />
        </div>
    );
}