# 📘 Fleet Manager — Full-Stack Take-Home Project

A small full-stack system for managing vehicles and their registration status.  
The solution includes:

- A **.NET backend** that exposes car data and pushes live registration updates via **SignalR**
- A **React + Vite + Tailwind** frontend with a clean Azure-style UI
- A **background service** checking for expired registrations
- A modern, polished UX demonstrating good engineering and design principles

This project is built to showcase code quality, structure, and real-world UI/UX polish.

---

## 🚗 Features

### Cars Page (`/`)
- Fetches a list of cars from the API  
- Optional `make` filter (triggered on **Enter** key)  
- Responsive, scrollable table with sticky header  
- License plates rendered in fixed-width font for clarity  
- Clean, Azure-themed UI

### Registration Page (`/registration`)
- Shows the *current* registration status of all cars  
- Displays **Valid / Expired** status with visual badges  
- Live updates via **SignalR**  
- Updated rows briefly highlight for visibility  
- Responsive scrollable table with sticky header  

### UI/UX
- Fixed top navigation with active page highlight  
- Fixed footer  
- Soft blue background (Azure-inspired)  
- Page fade-in transitions  
- Modern **Inter** font  
- Clean spacing, consistent line heights  
- Tailwind CSS v4 (no custom config required)

---

## 🏗️ Tech Stack

### Frontend
- **React 19**
- **Vite 5**
- **Tailwind CSS v4**
- **Inter font (Google Fonts)**
- **React Router 6**
- **SignalR client (`@microsoft/signalr`)**

### Backend
- **.NET**
- **Minimal APIs**
- **SignalR**
- **Background worker** to detect expirations
- **DTO-based API**
- CORS + HTTPS support

---

## 📂 Folder Structure (Frontend)

```
src/
  ├── App.jsx
  ├── AppLayout.jsx
  ├── CarsPage.jsx
  ├── RegistrationPage.jsx
  ├── index.css
  └── main.jsx
public/
README.md
vite.config.js
package.json
```

---

## ▶️ Running the Project

### Backend
Start your existing .NET backend:

```
dotnet run
```

Serves:

```
https://localhost:5001/api/cars
https://localhost:5001/api/registration
https://localhost:5001/hubs/registration
```

Ensure HTTPS certificates are trusted.

---

### Frontend

Install dependencies:

```
npm install
```

Run dev server:

```
npm run dev
```

Frontend:

```
http://localhost:5173
```

Vite proxy automatically forwards API calls to the backend.

---

## 🔧 Configuration

Proxy configuration in `vite.config.js`:

```js
server: {
  proxy: {
    "/api": "https://localhost:5001",
    "/hubs": "https://localhost:5001"
  }
}
```

Frontend uses relative URLs:

```js
fetch("/api/cars")
fetch("/api/registration")
new HubConnectionBuilder().withUrl("/hubs/registration")
```

---

## 🎨 Design Notes

This UI focuses on:

- Clean Azure dashboard aesthetic  
- White cards over soft blue background  
- Sticky headers and consistent row height  
- Monospace styling for plate numbers  
- Modern typography with Inter  
- Lightweight fade transitions  
- Professional table layout with responsive scroll  

---

## 📡 SignalR Live Updates

- Background service checks for expired registrations  
- Backend pushes **deltas only**  
- Frontend merges updates into existing list  
- Status badges update instantly  

---

## 👤 Author

**Stephen Wu**  
Full Stack Developer — Perth, Australia  
Experience in .NET, React, fleet optimisation, e-commerce, and platform modernisation.
