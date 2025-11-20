import React, { useEffect, useState } from "react";
import {
  TextField,
  Button,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Stack,
} from "@mui/material";

import { getCars } from "../lib/api";
import PageLayout from "../layout/PageLayout";

// Icons
import SearchIcon from "@mui/icons-material/Search";

function CarsPage() {
  const [cars, setCars] = useState([]);
  const [makeFilter, setMakeFilter] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadCars = async (make) => {
    setLoading(true);
    setError("");

    try {
      const data = await getCars(make);
      setCars(data);
    } catch (err) {
      console.error(err);
      setError(err.message || "Failed to load cars.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCars("");
  }, []);

  return (
    <PageLayout title="Cars" right={
        <Stack direction="row" spacing={2}>
          <TextField
            label="Filter by Make"
            variant="outlined"
            size="small"
            value={makeFilter}
            onChange={(e) => setMakeFilter(e.target.value)}
            sx={{ width: 250 }}
          />

          <Button
            variant="contained"
            startIcon={<SearchIcon />}
            onClick={() => loadCars(makeFilter)}
          >
            Search
          </Button>
        </Stack>}>
      <Table stickyHeader>
        <TableHead>
          <TableRow>
            <TableCell>Id</TableCell>
            <TableCell>Make</TableCell>
            <TableCell>Model</TableCell>
            <TableCell>Registration #</TableCell>
            <TableCell>Expiry</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {cars.map((car) => (
            <TableRow key={car.id}>
              <TableCell>{car.id}</TableCell>
              <TableCell>{car.make}</TableCell>
              <TableCell>{car.model}</TableCell>
              <TableCell>{car.registrationNumber}</TableCell>
              <TableCell>
                {new Date(car.registrationExpiry).toLocaleString()}
              </TableCell>
            </TableRow>
          ))}

          {cars.length === 0 && !loading && (
            <TableRow>
              <TableCell colSpan={5} align="center">
                No cars found.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </PageLayout>
  );
}

export default CarsPage;
