import React, { useEffect, useState } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Chip,
} from "@mui/material";

import { API_BASE } from "../lib/config";
import { getRegistrationStatuses } from "../lib/api";
import useHubConnection from "../hooks/useHubConnection";
import HubStatusIndicator from "../components/HubStatusIndicator";
import PageLayout from "../layout/PageLayout";

import CheckCircleOutlineIcon from "@mui/icons-material/CheckCircleOutline";
import HighlightOffIcon from "@mui/icons-material/HighlightOff";


export default function RegistrationPage() {
  const [statuses, setStatuses] = useState([]);

  // SignalR connection with status tracking
  const { connection, status } = useHubConnection(
    `${API_BASE}/hubs/registration`
  );

  // Initial load from API
  useEffect(() => {
    loadStatuses();
  }, []);

  const loadStatuses = async () => {
    try {
      const data = await getRegistrationStatuses();
      setStatuses(data);
    } catch (err) {
      console.error("Failed to fetch registration statuses", err);
    }
  };

  // Listen for hub updates
  useEffect(() => {
    if (!connection) return;

    connection.on("RegistrationStatusUpdated", (list) => {
      console.log("Received registration status update", list);
      
      setStatuses(prev => {
        const next = [...prev];
        list.forEach((update) => {
          const idx = next.findIndex(x => x.carId === update.carId);
          if (idx === -1) {
            next.push(update);
          } else {
            next[idx] = update;
          }
        });

        return next;
      });
    });

    return () => {
      connection?.off("RegistrationStatusUpdated");
    };
  }, [connection]);


  return (
    <PageLayout title="Registration Status" right={<HubStatusIndicator status={status} />}>
      <Table stickyHeader>
       <TableHead>
          <TableRow>
            <TableCell>Car ID</TableCell>
            <TableCell>Registration #</TableCell>
            <TableCell>Expiry</TableCell>
            <TableCell>Status</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {statuses.map((s) => (
            <TableRow key={s.carId}>
              <TableCell>{s.carId}</TableCell>
              <TableCell>{s.registrationNumber}</TableCell>
              <TableCell>
                {new Date(s.registrationExpiry).toLocaleString()}
              </TableCell>

              <TableCell>
                {s.isExpired ? (
                  <Chip
                    icon={<HighlightOffIcon />}
                    label="Expired"
                    color="error"
                    variant="outlined"
                  />
                ) : (
                  <Chip
                    icon={<CheckCircleOutlineIcon />}
                    label="Valid"
                    color="success"
                    variant="outlined"
                  />
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </PageLayout>
  );
}
