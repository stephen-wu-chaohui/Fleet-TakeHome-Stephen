import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export default function useHubConnection(hubUrl, options = {}) {
  const [connection, setConnection] = useState(null);
  const [status, setStatus] = useState("connecting");

  useEffect(() => {
    const conn = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    setConnection(conn);

    conn.onreconnecting(() => {
      setStatus("reconnecting");
    });

    conn.onreconnected(() => {
      setStatus("connected");
    });

    conn.onclose(() => {
      setStatus("disconnected");
    });

    const start = async () => {
      try {
        await conn.start();
        setStatus("connected");
      } catch {
        setStatus("error");
        setTimeout(start, 2000);
      }
    };

    start();

    return () => {
      conn.stop();
    };
  }, [hubUrl]);

  return { connection, status };
}
