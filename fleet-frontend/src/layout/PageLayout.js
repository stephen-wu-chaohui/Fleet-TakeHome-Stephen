import { Box, Typography, Paper } from "@mui/material";
import TableContainer from "@mui/material/TableContainer";

export default function PageLayout({ title, right, children }) {
  return (
    <Box sx={{ display: "flex", flexDirection: "column", height: "100vh" }}>
      
      {/* Header Row */}
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          mb: 2,
        }}
      >
        <Typography variant="h4">{title}</Typography>
        {right && <Box>{right}</Box>}
      </Box>

      {/* Scrollable content area */}
      <Paper sx={{ flex: 1, overflow: "hidden" }}>
        {/* Ensure sticky Header tables work */}
        <TableContainer sx={{ maxHeight: "calc(100vh - 150px)", overflowY: "auto" }}>
          {children}
        </TableContainer>
      </Paper>
    </Box>
  );
}
