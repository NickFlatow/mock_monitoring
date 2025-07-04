import { useEffect, useRef, useState } from "react";

export function useInfiniteScroll() {
    const bottom = useRef<HTMLDivElement>(null);
    const observer = useRef<IntersectionObserver | null>(null);

    const [offset, setOffset] = useState(0);
    const [maxOffset, setMaxOffset] = useState(0);


    useEffect(() => {
        observer.current = new IntersectionObserver((entries) => {
            if (entries[0].isIntersecting && offset < maxOffset) {
                console.log('Reached bottom of page');
                setOffset((prevOffset) => {
                    console.log("offset is", prevOffset);
                    return prevOffset + 10;
                });
            }
        });

        if (bottom.current) {
            observer.current.observe(bottom.current); // Start observing the bottom element
        }

        return () => {
            if (observer.current) {
                observer.current.disconnect();
            }
        };
    }, [maxOffset]);

    return {
        bottom,
        offset,
        maxOffset,
        setMaxOffset,
    }

}