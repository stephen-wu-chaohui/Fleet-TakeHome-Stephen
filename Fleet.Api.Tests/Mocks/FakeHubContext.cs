using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fleet.Api.Tests.Mocks
{
    public class FakeHubContext<THub> : IHubContext<THub> where THub : Hub
    {
        public IHubClients Clients { get; }
        public IGroupManager Groups => throw new System.NotImplementedException();

        public FakeHubContext()
        {
            Clients = new FakeHubClients();
        }

        public class FakeHubClients : IHubClients
        {
            public IClientProxy All { get; } = new FakeClientProxy();

            public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds) => All;
            public IClientProxy Client(string connectionId) => All;
            public IClientProxy Clients(IReadOnlyList<string> connectionIds) => All;
            public IClientProxy Group(string groupName) => All;
            public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => All;
            public IClientProxy Groups(IReadOnlyList<string> groupNames) => All;
            public IClientProxy User(string userId) => All;
            public IClientProxy Users(IReadOnlyList<string> userIds) => All;
        }

        public class FakeClientProxy : IClientProxy
        {
            public object? LastMessage { get; private set; }
            public string? LastMethod { get; private set; }

            public Task SendCoreAsync(string method, object?[] args, CancellationToken cancellationToken = default)
            {
                LastMethod = method;
                LastMessage = args;
                return Task.CompletedTask;
            }
        }
    }
}
