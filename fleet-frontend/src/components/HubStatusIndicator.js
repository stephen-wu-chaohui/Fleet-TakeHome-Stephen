import { Box, Typography } from "@mui/material";

export default function HubStatusIndicator({ status }) {
  const { color, text } = mapStatus(status);

  return (
    <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
      <Box
        sx={{
          width: 10,
          height: 10,
          borderRadius: "50%",
          backgroundColor: color,
          transition: "background-color 0.3s ease",
        }}
      />
      <Typography variant="body2">{text}</Typography>
    </Box>
  );
}

function mapStatus(status) {
  switch (status) {
    case "connected":
      return { color: "#4caf50", text: "Connected" }; // green
    case "reconnecting":
      return { color: "#ffb300", text: "Reconnecting…" }; // amber
    case "connecting":
      return { color: "#29b6f6", text: "Connecting…" }; // blue
    case "disconnected":
      return { color: "#f44336", text: "Disconnected" }; // red
    case "error":
      return { color: "#d32f2f", text: "Error" };
    default:
      return { color: "#9e9e9e", text: "Unknown" }; // grey
  }
}
