import { render, screen, waitFor, act } from "@testing-library/react";
import RegistrationPage from "../pages/RegistrationPage";
import * as api from "../lib/api";
import useHubConnection from "../hooks/useHubConnection";

// --- Mock API module ---
jest.mock("../lib/api", () => ({
  getRegistrationStatuses: jest.fn(),
}));

// --- Mock SignalR hook ---
jest.mock("../hooks/useHubConnection");

describe("RegistrationPage", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it("renders initial registration statuses from API", async () => {
    api.getRegistrationStatuses.mockResolvedValue([
      {
        carId: 1,
        registrationNumber: "ABC123",
        isExpired: false,
        registrationExpiry: "2025-01-01T00:00:00Z",
      },
    ]);

    useHubConnection.mockReturnValue({
      connection: null,   // no SignalR events in this test
      status: "connected",
    });

    render(<RegistrationPage />);

    // Data from initial API call
    expect(await screen.findByText("ABC123")).toBeInTheDocument();
    expect(await screen.findByText(/2025/)).toBeInTheDocument();
    expect(await screen.findByText(/Valid/i)).toBeInTheDocument();
  });

  it("updates rows when SignalR pushes new registration status", async () => {
    // Initial API response
    api.getRegistrationStatuses.mockResolvedValue([
      {
        carId: 1,
        registrationNumber: "ABC123",
        isExpired: false,
        registrationExpiry: "2025-01-01T00:00:00Z",
      },
    ]);

    // Fake SignalR connection
    const fakeOn = jest.fn();
    const fakeOff = jest.fn();

    useHubConnection.mockReturnValue({
      connection: {
        on: fakeOn,
        off: fakeOff,
      },
      status: "connected",
    });

    render(<RegistrationPage />);

    // Ensure event handler registration
    expect(fakeOn).toHaveBeenCalledWith(
      "RegistrationStatusUpdated",
      expect.any(Function)
    );

    // Extract the handler function
    const updateHandler = fakeOn.mock.calls[0][1];
    console.log({updateHandler});

    await act(async () => {
    });

    await act(async () => {
      updateHandler([
        {
          carId: 1,
          registrationNumber: "ABC123",
          isExpired: true,
          registrationExpiry: "2025-12-15T00:00:00Z",
        }
      ]);
    });
    
    // Verify that the old status is no longer present
    // expect(screen.queryByText(/Valid/i)).not.toBeInTheDocument();
    

    // UI should now show “Expired”
    await waitFor(() =>
      expect(screen.getByText(/Expired/i)).toBeInTheDocument()
    );
  });
});
