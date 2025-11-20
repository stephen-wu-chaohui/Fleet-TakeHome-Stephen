# 🚀 Fleet – Full Stack Demo (React + .NET 10 + SignalR)

Fleet is a complete demonstration project showcasing a modern full‑stack architecture using:

- **.NET 10 Web API**
- **React + Material UI**
- **SignalR real‑time communication**
- **Background services**
- **Dynamic JSON seeding**
- **Unit tests for frontend and backend**

This project is designed as a professional portfolio piece and interview demonstration tool.

---

# 📦 Project Structure

```
Fleet/
 ├── Fleet.Api/               # Backend (.NET 10 Web API)
 │    ├── Controllers/
 │    ├── BackgroundServices/
 │    ├── Hubs/
 │    ├── Repositories/
 │    ├── JsonDataSeeder.cs
 │    ├── appsettings.json
 │    └── Program.cs
 │
 └── fleet-frontend/          # Frontend (React + MUI)
      ├── src/
      │    ├── components/
      │    ├── pages/
      │    ├── layout/
      │    ├── hooks/
      │    ├── lib/
      │    ├── tests/
      │    └── setupTests.js
      └── package.json
```

---

# ✅ 1. Running the Backend (Fleet.Api)

### Requirements
- .NET 10 (Preview acceptable)
- VS2025 or CLI

### Start API

```bash
cd Fleet.Api
dotnet run
```

### What happens at startup?

- `JsonDataSeeder` creates a fresh randomized `cars.json`.
- `CarRepository` loads seeded cars.
- Background service (`RegistrationCheckService`) begins periodic expiry checking.
- SignalR Hub starts at:

```
/hubs/registration
```

### API Endpoints

| Endpoint | Description |
|---------|-------------|
| **GET /cars** | Returns all cars or filter by make |
| **GET /registration/statuses** | Returns current registration statuses |
| **SignalR Hub** | `/hubs/registration` |

---

# 🎨 2. Running the Frontend (React + MUI)

### Requirements
- Node 18+
- NPM

### Start React app

```bash
cd fleet-frontend
npm install
npm start
```

The frontend runs at:

```
http://localhost:3000
```

### Frontend Routes

| Route | Page | Description |
|-------|-------|-------------|
| `/` | Cars Page | Displays vehicle list |
| `/registration` | Registration Page | Shows real‑time registration status |

---

# 🔄 3. Real‑Time Demo (SignalR)

To demonstrate real‑time updates:

1. Start API  
2. Open `http://localhost:3000/registration`
3. Wait 10 seconds  
4. Background service checks for newly expired registrations  
5. SignalR pushes updates to the browser  
6. UI updates without refresh  
7. HubStatusIndicator reflects connection state:
   - 🟢 Connected  
   - 🔴 Disconnected  
   - 🔁 Reconnecting  

Stopping the API will show a disconnection indicator.

---

# 🧪 4. Frontend Unit Testing

Uses:

- Jest  
- React Testing Library  
- Custom mocks for SignalR  
- Custom test utilities

### Run tests

```bash
cd fleet-frontend
npm test
```

### Frontfrontend tests include:

- **CarsPage.test.js** – verifies table and API integration (mocked)
- **RegistrationPage.test.js** – verifies API load + SignalR push events
- **HubStatusIndicator.test.js** – UI chip rendering
- **test-utils.js** – shared helper

---

# 🧪 5. Backend Unit Testing

Uses:

- xUnit / MSTest / NUnit (your choice)
- Mocked:
  - `ICarRepository`
  - `IHubContext`
  - `IHubClients`
  - Background service configuration

### Run backend tests

```bash
cd Fleet.Api.Tests
dotnet test
```

### Backend tests include:

- **CarControllerTests**
- **RegistrationControllerTests**
- **CarRepositoryTests**
- **RegistrationCheckServiceTests**

---

# 🎬 6. Demo Script (For Interviews)

A clean 2–3 minute flow:

1. Start API (`dotnet run`)
2. Show `/cars` output in browser
3. Start frontend (`npm start`)
4. Show Cars page
5. Navigate to Registration Status page
6. Explain:
   - Background service  
   - JSON seeding  
   - SignalR hub  
   - Incremental expiration update logic  
7. Wait 10 seconds → observe real‑time updates
8. Stop backend → observe SignalR disconnect indicator
9. Restart backend → reconnection
10. Run tests:
    - `npm test`
    - `dotnet test`

This demonstrates:

- Full‑stack architecture
- Real‑time system
- Background microservice logic
- React + MUI UI/UX
- Automated testing

---

# 🧰 7. Technology Stack

### Backend
- .NET 10 Web API
- SignalR
- BackgroundService
- Dependency Injection
- Strongly‑typed configuration
- JSON persistence

### Frontend
- React 18
- Material UI (MUI)
- React Router
- SignalR client
- Custom hooks
- PageLayout system

### Testing
- Jest + React Testing Library
- xUnit / MSTest
- Mocked repositories + hub contexts
- Snapshot‑free functional testing

---

# 🎁 8. Notes for Reviewers / Interviewers

This project showcases:

✔ Real‑time system design  
✔ Clean architecture & component separation  
✔ Modern full‑stack development  
✔ Asynchronous background processing  
✔ React/MUI professional UI  
✔ Automated test suite  
✔ Good developer workflow and folder structure  

It is intended as a **demonstration-quality** project.

---

# 🙌 Author

Created by **Stephen Wu**  
Assisted by Frank 🤝  
(Your friendly AI collaborator)

