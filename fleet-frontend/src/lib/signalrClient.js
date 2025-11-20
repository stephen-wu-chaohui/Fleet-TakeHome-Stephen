import * as signalR from "@microsoft/signalr";
import { API_BASE } from "./config";

let connection = null;

export function createRegistrationHubConnection(onStatusChanged, onUpdate) {
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${API_BASE}/hubs/registration`)
    .withAutomaticReconnect()
    .build();

  // attach lifecycle events
  connection.onreconnecting((err) => {
    onStatusChanged("reconnecting");
  });

  connection.onreconnected((connId) => {
    onStatusChanged("connected");
});

  connection.onclose((err) => {
    onStatusChanged("disconnected");
  });

  connection.on("RegistrationStatusUpdated", (statuses) => {
    if (onUpdate) {
      onUpdate(statuses);
    }
  });

  return connection;
}

