using System;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using Microsoft.AspNetCore.TestHost;

namespace ChatSample.Tests
{
    public static class TestServerExtensions
    {
        public static HubConnection CreateHubConnection(this TestServer testServer, string hubPath, TransportType transportType = TransportType.LongPolling)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri("http://test/"), hubPath))
                .WithMessageHandler(testServer.CreateHandler())
                .WithTransport(transportType)
                .WithConsoleLogger()
                .Build();

            return hubConnection;
        }
    }
}