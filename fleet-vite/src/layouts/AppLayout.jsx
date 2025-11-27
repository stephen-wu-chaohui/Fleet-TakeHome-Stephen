import { NavLink, useLocation } from "react-router-dom";

export default function AppLayout({ children }) {
  const location = useLocation();

  return (
    <div className="h-screen flex flex-col bg-blue-50">

      {/* Top Navigation */}
      <nav className="bg-white border-b shadow-sm shadow-blue-100 fixed top-0 left-0 right-0 z-50">
        <div className="max-w-6xl mx-auto px-6 py-3 grid grid-cols-2 items-center">

          <div className="text-lg font-semibold text-blue-600">
            Fleet Manager
          </div>

          <div className="justify-self-end flex gap-6">
            <NavLink
              to="/"
              className={({ isActive }) =>
                isActive
                  ? "text-blue-600 font-semibold border-b-2 border-blue-600 pb-1"
                  : "text-gray-700 hover:text-blue-600"
              }
            >
              Cars
            </NavLink>

            <NavLink
              to="/registration"
              className={({ isActive }) =>
                isActive
                  ? "text-blue-600 font-semibold border-b-2 border-blue-600 pb-1"
                  : "text-gray-700 hover:text-blue-600"
              }
            >
              Registration
            </NavLink>
          </div>

        </div>
      </nav>

      {/* Main Scrollable Content */}
      <main className="flex-1 overflow-auto pt-[70px] pb-[50px] px-6">
        <div key={location.pathname} className="max-w-6xl mx-auto fade-page">
          {children}
        </div>
      </main>

      {/* Footer */}
      <footer className="bg-white border-t fixed bottom-0 left-0 right-0 z-50 shadow-blue-100 shadow-sm">
        <div className="max-w-6xl mx-auto px-6 py-2 text-sm text-gray-500">
          © {new Date().getFullYear()} Fleet Manager — Demo Interface by Stephen Wu
        </div>
      </footer>

    </div>
  );
}
