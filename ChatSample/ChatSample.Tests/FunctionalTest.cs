using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Sockets;
using Microsoft.AspNetCore.TestHost;
using SignalRChatSample;
using Xunit;

namespace ChatSample.Tests
{
    public class FunctionalTest
    {
        [Theory]
        [InlineData(TransportType.LongPolling)]
        [InlineData(TransportType.ServerSentEvents)]
        public async Task SendMessage_WithTestServer_RecieveMessage(TransportType transportType)
        {
            var expectedUser = "idubnori";
            var expectedMessage = "hello world!";

            string recievedUser = null;
            string recievedMessage = null;

            var webHostBuilder = WebHost.CreateDefaultBuilder().UseStartup<Startup>();
            using (var testServer = new TestServer(webHostBuilder))
            {
                var hubConnection = testServer.CreateHubConnection("/chat", transportType);
                await hubConnection.StartAsync();

                hubConnection.On("broadcastMessage", new[] {typeof(string), typeof(string)}, (p, s) =>
                {
                    recievedUser = (string)p.First();
                    recievedMessage = (string)p.Last();
                    return Task.CompletedTask;
                }, null);

                await hubConnection.SendAsync("Send", new object[] { expectedUser, expectedMessage });

                await Task.Delay(500);

                Assert.Equal(expectedUser, recievedUser);
                Assert.Equal(expectedMessage, recievedMessage);
            }
        }
    }
}
