import { API_BASE } from "./config";

export async function httpGet(path) {
  const res = await fetch(`${API_BASE}${path}`, {
    credentials: "include"
  });

  if (!res.ok) {
    throw new Error(`GET ${path} failed (${res.status})`);
  }

  return res.json();
}
