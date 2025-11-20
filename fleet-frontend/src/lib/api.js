import { httpGet } from "./httpClient";

export const getCars = (make) => {
  const q = make ? `?make=${encodeURIComponent(make)}` : "";
  return httpGet(`/api/cars${q}`);
};

export const getRegistrationStatuses = () =>
  httpGet("/api/registration");
