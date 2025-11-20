# 🚚 Fleet Frontend  
*A modern React + Material UI + SignalR interface for real-time fleet monitoring.*

## 📌 Overview

Fleet Frontend is the React-based user interface for the **Fleet** project.  
It provides:

- A Cars listing page (`/`)
- A real-time Registration Monitoring page (`/registration`)
- A clean layout using Material UI (MUI)
- Live updates from the backend via **SignalR**
- A reusable page layout with a scrollable content area and persistent footer

This project demonstrates a production-style React architecture that you might see in a real fleet or logistics dashboard.

---

## ✨ Features

### 🚗 Cars Page (`/`)

- Displays a list of all vehicles returned by the backend (`/api/cars`)
- Shows basic details such as ID, make, model, and registration number
- Uses a **sticky table header** and a **scrollable table body**
- Renders inside a shared `<PageLayout>` shell for consistent spacing and layout

### 🕒 Registration Monitoring Page (`/registration`)

- Displays **live registration expiry status** for cars
- Consumes the `/api/registration` HTTP endpoint for initial state
- Subscribes to real-time updates from the backend **SignalR hub** (`/hubs/registration`)
- Merges incremental updates (deltas) into the current table
- Shows a **HubStatusIndicator** that reflects the SignalR connection state  
  (`connecting`, `connected`, `reconnecting`, `disconnected`, `error`)

### 🎨 Layout & Styling

- Uses Material UI v5 with the `sx={{ }}` styling system
- Global app shell handled by `TopBar` + routing in `App.js`
- Per-page layout handled by a reusable `<PageLayout>` component:
  - Header row with title and optional right-hand content (e.g., hub status)
  - Scrollable table/content area
  - Optional persistent footer

### 🔌 SignalR Integration

- Custom `useHubConnection` hook encapsulates SignalR connection lifecycle
- Automatically starts the connection, handles reconnect behaviour, and exposes `connection` + `status`
- `RegistrationPage` subscribes to `"RegistrationStatusUpdated"` events and merges updates into local state
- Hub status is surfaced via `<HubStatusIndicator />` visually in the page header

---

## 🗂 Folder Structure

```text
src/
 ├── app/
 │    └── App.js                # Root routing + layout shell
 ├── pages/
 │    ├── CarsPage.js           # "/" – list of cars
 │    └── RegistrationPage.js   # "/registration" – live expiry status
 ├── layout/
 │    ├── PageLayout.js         # Reusable page layout (header + scrollable body + footer)
 │    └── TopBar.js             # Application top navigation bar
 ├── components/
 │    └── HubStatusIndicator.js # Connection status dot + label
 ├── hooks/
 │    └── useHubConnection.js   # SignalR connection lifecycle hook
 ├── lib/
 │    ├── api.js                # Typed API helper functions (getCars, getRegistrationStatuses, etc.)
 │    ├── httpClient.js         # Shared fetch wrapper (base URL + error handling)
 │    ├── signalrClient.js      # SignalR hub connection factory (if used)
 │    └── config.js             # Frontend configuration (API base URL, hub URL, etc.)
 ├── theme/
 │    └── theme.js              # MUI theme configuration
 ├── index.js                   # React entry point
 └── index.css                  # Global styles
```

This structure keeps pages, layout, core logic, and theme cleanly separated.

---

## ▶️ Getting Started

### 1. Install dependencies

```bash
npm install
```

### 2. Run the development server

```bash
npm start
```

By default the app runs at:

```text
http://localhost:3000
```

Make sure the backend (**Fleet.Api**) is running and that CORS is configured to allow `http://localhost:3000` (or whatever URL you use).

---

## ⚙️ Configuration

Frontend configuration is centralized under `src/lib/config.js` (or similar).  
A common pattern is:

```js
// src/lib/config.js
const API_BASE_URL =
  process.env.REACT_APP_API_URL || "https://localhost:5001";

const HUB_URL = `${API_BASE_URL}/hubs/registration`;

export default {
  apiBaseUrl: API_BASE_URL,
  registrationHubUrl: HUB_URL,
};
```

You can override `REACT_APP_API_URL` via environment variables for different environments (dev/staging/prod).

API helpers (in `lib/api.js`) use this configuration so that pages never hard-code URLs.

---

## 🔌 SignalR Usage Overview

The frontend uses a `useHubConnection` hook to manage SignalR connections.  
Typical usage in `RegistrationPage` looks like:

```js
const { connection, status } = useHubConnection(config.registrationHubUrl);

useEffect(() => {
  if (!connection) return;

  connection.on("RegistrationStatusUpdated", (updates) => {
    // `updates` is a list of changed vehicles
    setStatuses((prev) => mergeStatusUpdates(prev, updates));
  });

  return () => {
    connection.off("RegistrationStatusUpdated");
  };
}, [connection]);
```

`HubStatusIndicator` then receives the `status` string and renders a color-coded dot + label.

---

## 🧪 Testing

If you’ve added Jest + React Testing Library tests, run them with:

```bash
npm test
```

You can add tests for:

- `CarsPage` (renders data from `getCars`)  
- `RegistrationPage` (merges SignalR updates)  
- `httpClient` (successful + error responses)  
- `useHubConnection` (mock SignalR connection lifecycle)  

---

## 🏁 Production Build

To build a production-optimized bundle:

```bash
npm run build
```

This creates a `build/` directory that can be served via:

- A static file host (Netlify, Vercel, etc.)
- As static files from a .NET / Node backend
- From a container in Kubernetes / Docker environments

Make sure `API_BASE_URL` / `REACT_APP_API_URL` is configured correctly for your deployed backend endpoint.

---

## 🧩 Notes on Styling & Layout

- Uses MUI `sx` prop for component-level styles, which is considered best practice for small-to-medium codebases using MUI v5+.
- Page layout responsibilities are centralized in `PageLayout` so that individual page components (Cars, Registration) stay focused on data + rendering logic.
- The scrollable area is encapsulated in the layout rather than making the whole page scroll, avoiding header/footer jitter and scrollbar reflow issues.

---

## 📄 License

This frontend is provided under the MIT License.  
You are free to use it for learning, personal projects, or as a reference for your own production applications.
