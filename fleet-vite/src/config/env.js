export const env = {
  appName: import.meta.env.VITE_APP_NAME ?? 'Fleet',
  apiBaseUrl: '',               // SAME ORIGIN
  signalRHubUrl: '/hubs/registration'
};
