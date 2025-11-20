import { render, screen } from "@testing-library/react";
import HubStatusIndicator from "../components/HubStatusIndicator";

describe("HubStatusIndicator", () => {
  it("shows connected status", () => {
    render(<HubStatusIndicator status="connected" />);
    expect(screen.getByText(/connected/i)).toBeInTheDocument();
  });

  it("shows disconnected status", () => {
    render(<HubStatusIndicator status="disconnected" />);
    expect(screen.getByText(/disconnected/i)).toBeInTheDocument();
  });
});