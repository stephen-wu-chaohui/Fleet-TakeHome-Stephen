import React from "react";
import { AppBar, Toolbar, Typography, Button } from "@mui/material";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import VerifiedUserIcon from "@mui/icons-material/VerifiedUser";
import { Link } from "react-router-dom";

function TopBar() {
  return (
    <AppBar position="static" color="primary">
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          Fleet
        </Typography>

        <Button
          color="inherit"
          component={Link}
          to="/"
          startIcon={<DirectionsCarIcon />}
        >
          Cars
        </Button>

        <Button
          color="inherit"
          component={Link}
          to="/registration"
          startIcon={<VerifiedUserIcon />}
        >
          Registration
        </Button>
      </Toolbar>
    </AppBar>
  );
}

export default React.memo(TopBar);
