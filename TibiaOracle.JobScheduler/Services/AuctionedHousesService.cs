using Connector_TibiaData.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;
using TibiaOracle.Logic.Services;

namespace TibiaOracle.JobScheduler.Services
{
    public class AuctionedHousesService : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _client;
        private readonly Timer _scheduledTimer;

        private const string LINE_SEPARATOR = "━━━━━━━━━━━━━━━━━━━━━━━\n";
        private const string CHANNEL_NAME = "house-auctions";
        private const string WORLD_NAME = "antica";

        public AuctionedHousesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
            });

            _client.Log += Log;
            _client.Ready += OnReady;
            _client.MessageReceived += MessageReceived;

            // Set up a scheduled task to run every 5 minutes (example)
            _scheduledTimer = new Timer(PerformScheduledTasks, null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
        }

        // This is called when the service starts
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("DISCORD_TOKEN environment variable is missing.");

            // Log in and start the bot
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Keep the bot running indefinitely
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[Discord] {msg}");
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            Console.WriteLine($"✅ Logged in as {_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }

        // Message handler for the bot
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("🏓 Pong!");
            }
        }

        private async void PerformScheduledTasks(object state)
        {
            using var scope = _serviceProvider.CreateScope();

            var houseLogic = scope.ServiceProvider.GetRequiredService<HouseLogic>();
            foreach (var guild in _client.Guilds)
            {
                try
                {
                    var auctionedHouses = await houseLogic.GetAllAuctionedHouses(WORLD_NAME);
                    if (!auctionedHouses.Any()) return;

                    var formattedMessage = FormatMessage(auctionedHouses);

                    var channel = guild.Channels.FirstOrDefault(c => c.Name == CHANNEL_NAME);
                    var channelId = channel?.Id;

                    if (channelId == null)
                    {
                        var result = await guild.CreateTextChannelAsync(CHANNEL_NAME)
                            ?? throw new Exception("Failed to create channel.");

                        channelId = result.Id;
                    }

                    if (channelId == null)
                    {
                        throw new Exception("Could not find channel.");
                    }

                    var actualChannel = guild.GetTextChannel(channelId.Value);
                    await actualChannel.SendMessageAsync(formattedMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in scheduled task: {ex.Message}");
                }
            }
        }

        private string FormatMessage(IEnumerable<House> auctionedHouses) =>
            string.Join("\n", SortByTimeLeft(auctionedHouses).Select(aH =>
            $"{aH.Name}\n- Bid: {(aH.Auction.CurrentBid == 0 ? "0" : aH.Auction.CurrentBid / 1000000 + "kk")}\n- Time left: {aH.Auction.TimeLeft}\n{LINE_SEPARATOR}"));

        private static IEnumerable<House> SortByTimeLeft(IEnumerable<House> houses) =>
            houses.OrderBy(t =>
            {
                var parts = t.Auction.TimeLeft.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) return int.MaxValue;

                var value = int.TryParse(parts[0], out var num) ? num : int.MaxValue;
                var unit = parts[1].ToLower();

                return unit switch
                {
                    "day" or "days" => value * 24,
                    "hour" or "hours" => value,
                    _ => int.MaxValue
                };
            }).ThenBy(t => t.Name);

        // Ensure cleanup on stop
        public override void Dispose()
        {
            _client.Dispose();
            _scheduledTimer.Dispose();
            base.Dispose();
        }
    }
}