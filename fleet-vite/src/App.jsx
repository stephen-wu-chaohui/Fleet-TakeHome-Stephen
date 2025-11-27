import { BrowserRouter, Routes, Route } from "react-router-dom";
import AppLayout from "./layouts/AppLayout";
import CarsPage from "./pages/CarsPage";
import RegistrationPage from "./pages/RegistrationPage";

export default function App() {
  return (
    <BrowserRouter>
      <AppLayout>
        <Routes>
          <Route path="/" element={<CarsPage />} />
          <Route path="/registration" element={<RegistrationPage />} />
        </Routes>
      </AppLayout>
    </BrowserRouter>
  );
}
