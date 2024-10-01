import React, { useState } from "react";
import axios from "axios";

const App: React.FC = () => {
  const [originalUrl, setOriginalUrl] = useState("");
  const [shortenedUrl, setShortenedUrl] = useState("");

  const handleShortenUrl = async () => {
    try {
      const response = await axios.post(
        "http://localhost:5000/api/urlshortener/shorten",
        originalUrl,
        {
          headers: { "Content-Type": "application/json" },
        }
      );
      setShortenedUrl(response.data.shortenedUrl);
    } catch (error) {
      console.error("Error shortening URL", error);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 flex flex-col items-center justify-center">
      <div className="w-1/3 p-4 bg-white shadow-md rounded-lg">
        <input
          type="text"
          value={originalUrl}
          onChange={(e) => setOriginalUrl(e.target.value)}
          className="w-full p-2 border border-gray-300 rounded-md mb-4"
          placeholder="Enter URL"
        />
        <button
          onClick={handleShortenUrl}
          className="w-full bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        >
          Shorten URL
        </button>
        {shortenedUrl && (
          <p className="mt-4">
            Shortened URL:{" "}
            <a
              href={`https://${shortenedUrl}`}
              target="_blank"
              rel="noopener noreferrer"
            >
              {shortenedUrl}
            </a>
          </p>
        )}
      </div>
    </div>
  );
};

export default App;
