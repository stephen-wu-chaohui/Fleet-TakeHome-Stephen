import { useState, useEffect } from "react";
import { env } from "../config/env";

export default function CarsPage() {
  const [cars, setCars] = useState([]);
  const [make, setMake] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function load() {
      setLoading(true);
      const url = make
        ? `${env.apiBaseUrl}/api/cars?make=${encodeURIComponent(make)}`
        : `${env.apiBaseUrl}/api/cars`;
      const res = await fetch(url);
      const data = await res.json();
      setCars(data);
      setLoading(false);
    }
    load();
  }, [make]);

  return (
    <div className="grid gap-6 fade-page">
      <div className="grid grid-cols-2 h-14 items-center">
        <h2 className="text-2xl font-semibold text-gray-800">Cars</h2>

        <input
          className="justify-self-end border rounded-md px-3 py-2 w-64 shadow-sm 
                    focus:ring-2 focus:ring-blue-300 focus:outline-none"
          placeholder="Filter by make..."
          onKeyDown={(e) => {
            if (e.key === 'Enter') {
              setMake(e.target.value.trim());
            }
          }}
        />
      </div>

      {/* Table */}
      <div className="overflow-x-auto rounded-lg border bg-white shadow-sm">
        <table className="w-full text-left border-collapse min-w-[600px]">
          <thead className="bg-blue-100/60 border-b sticky top-0 z-10 backdrop-blur">
            <tr>
              <th className="px-3 font-medium">ID</th>
              <th className="px-3 font-medium">Make</th>
              <th className="px-3 font-medium">Model</th>
              <th className="px-3 font-medium">Plate #</th>
              <th className="px-3 font-medium">Expiry</th>
            </tr>
          </thead>

          <tbody>
            {loading ? (
              <tr>
                <td colSpan="4" className="p-4 text-gray-500">
                  Loading...
                </td>
              </tr>
            ) : (
              cars.map((car, index) => (
                <tr
                  key={car.id}
                  className={`border-b ${
                    index % 2 === 0 ? "bg-white" : "bg-gray-50"
                  } hover:bg-blue-50 transition-colors`}
                >
                  <td className="px-3 py-2.5 text-sm">{car.id}</td>
                  <td className="px-3 py-2.5 text-sm">{car.make}</td>
                  <td className="px-3 py-2.5 text-sm">{car.model}</td>
                  <td className="px-3 py-2.5 font-mono text-sm tracking-wider">{car.plate}</td>
                  <td className="px-3 py-2.5 text-sm">{new Date(car.registrationExpiry).toLocaleString()}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>    
  );
}
