import { useEffect } from "react";

function App() {
    useEffect(() => {
        fetch("https://localhost:7129/api/projects", { method: "GET" });
    }, []);

    return <></>;
}

export default App;
