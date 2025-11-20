import React from "react";
import { Routes, Route, useLocation } from "react-router-dom";
import { AnimatePresence } from "framer-motion";

import TopBar from "../layout/TopBar";
import CarsPage from "../pages/CarsPage";
import RegistrationPage from "../pages/RegistrationPage";
import PageTransition from "../components/PageTransition";

import { Box } from "@mui/material";

function App() {
  const location = useLocation();

  return (
    <Box 
      sx={{
        display: "flex",
        flexDirection: "column",
        height: "100vh", // full viewport height
        overflow: "hidden", // prevent document scrollbar
        bgcolor: "background.default"
      }}
    >
      <TopBar />

      <Box
        sx={{
          flexGrow: 1,
          padding: 2,
        }}
      >
        <AnimatePresence mode="wait">
          <Routes location={location} key={location.pathname}>
            <Route
              path="/"
              element={
                <PageTransition>
                  <CarsPage />
                </PageTransition>
              }
            />
            <Route
              path="/registration"
              element={
                <PageTransition>
                  <RegistrationPage />
                </PageTransition>
              }
            />
          </Routes>
        </AnimatePresence>
      </Box>
    </Box>
  );
}

export default App;
