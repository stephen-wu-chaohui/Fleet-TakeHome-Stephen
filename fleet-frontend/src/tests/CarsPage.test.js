import { render, screen } from "@testing-library/react";
import CarsPage from "../pages/CarsPage";
import * as api from "../lib/httpClient";

jest.mock("../lib/httpClient", () => ({
  httpGet: jest.fn(),
}));

describe("CarsPage", () => {
  it("renders cars returned by API", async () => {
    api.httpGet.mockResolvedValue([
      { id: 1, make: "Toyota", model: "Corolla", registrationNumber: "ABC123" },
      { id: 2, make: "Honda", model: "Civic", registrationNumber: "XYZ999" }
    ]);

    render(<CarsPage />);

    expect(await screen.findByText("Toyota")).toBeInTheDocument();
    expect(await screen.findByText("Honda")).toBeInTheDocument();
  });
});