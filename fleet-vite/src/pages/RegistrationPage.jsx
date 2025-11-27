import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export default function RegistrationPage() {
  const [statusList, setStatusList] = useState([]);
  const [loading, setLoading] = useState(true);

  //
  // Load initial snapshot of registration status
  //
  useEffect(() => {
    async function loadInitial() {
      const res = await fetch("/api/registration");
      const data = await res.json();
      setStatusList(data);
      setLoading(false);
    }
    loadInitial();
  }, []);

  //
  // SignalR live updates
  //
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/hubs/registration")
      .withAutomaticReconnect()
      .build();

    connection.on("registrationStatusUpdated", (data) => {
      setStatusList(prev => {
        const next = [...prev];
        data.forEach((update) => {
          const idx = next.findIndex(x => x.carId === update.carId);
          if (idx === -1) {
            next.push(update);
          } else {
            next[idx] = update;
          }
        });

        return next;
      });
    });

    connection.start();

    return () => connection.stop();
  }, []);

  return (
    <div className="grid gap-6 fade-page">
      <div className="h-14 flex items-center">
        <h2 className="text-2xl font-semibold text-gray-800">
          Registration Status
        </h2>
      </div>

      <div className="overflow-x-auto rounded-lg border bg-white shadow-sm">
        <table className="w-full text-left border-collapse min-w-[600px]">
          <thead className="bg-blue-100/60 border-b sticky top-0 z-10 backdrop-blur">
            <tr>
              <th className="px-3 font-medium">Car ID</th>
              <th className="px-3 font-medium">Plate #</th>
              <th className="px-3 font-medium">Expiry</th>
              <th className="px-3 font-medium">Status</th>
            </tr>
          </thead>

          <tbody>
            {loading ? (
              <tr>
                <td colSpan="4" className="p-3 text-gray-500">Loading...</td>
              </tr>
            ) : (
              statusList.map((item) => (
                <tr key={item.carId} className="border-t">
                  <td className="px-3 py-2.5 text-sm">{item.carId}</td>
                  <td className="px-3 py-2.5 font-mono text-sm">{item.plate}</td>
                  <td className="px-3 py-2.5 text-sm">
                    {new Date(item.registrationExpiry).toLocaleString()}
                  </td>
                  <td className="px-3 ">
                    {item.isExpired ? (
                      <span className="inline-block px-3 py-1 text-sm font-medium rounded-full bg-red-500/15 text-red-700 border border-red-300/50">
                        <span className="mr-1">✖</span> Expired
                      </span>
                    ) : (
                      <span className="inline-block px-3 py-1 text-sm font-medium rounded-full bg-green-500/15 text-green-700 border border-green-300/50">
                        <span className="mr-1">✔</span> Valid
                      </span>
                    )}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
