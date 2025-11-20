using Microsoft.AspNetCore.SignalR;

namespace Fleet.Api.Hubs;

/// <summary>
/// Hub used to broadcast registration-expiry updates in real time.
/// 
/// The hub does not receive messages from clients; it acts only as
/// a server-to-client push channel used by RegistrationCheckService.
/// </summary>
public class RegistrationHub : Hub {
    // No methods needed for now – we just push from the server
}
