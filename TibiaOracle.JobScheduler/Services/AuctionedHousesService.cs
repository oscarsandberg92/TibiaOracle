﻿using Connector_TibiaData.Models;
using Cronos;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private const string CATEGORY_NAME = "bot-channels";
        private const string WORLD_NAME = "antica";
        private CronExpression _cronExpression;

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

            _cronExpression = CronExpression.Parse("0 9,20 * * *");
            _scheduledTimer = new Timer(PerformScheduledTasks, null, TimeSpan.Zero, GetNextCronInterval());
        }

        private TimeSpan GetNextCronInterval()
        {
            var next = _cronExpression.GetNextOccurrence(DateTime.UtcNow);
            return next.HasValue ? next.Value - DateTime.UtcNow : TimeSpan.Zero;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("DISCORD_TOKEN environment variable is missing.");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

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

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("🏓 Pong!");
            }

            if (message.Content == "!houses")
            {
                using var scope = _serviceProvider.CreateScope();
                var houseLogic = scope.ServiceProvider.GetRequiredService<HouseLogic>();

                var auctionedHouses = await houseLogic.GetAllAuctionedHouses(WORLD_NAME);
                var formattedMessage = FormatMessage(auctionedHouses);
                
                await message.Channel.SendMessageAsync(formattedMessage);
            }

            if (message.Content == "!makemoney")
            {
                using var scope = _serviceProvider.CreateScope();
                var marketLogic = scope.ServiceProvider.GetRequiredService<MarketLogic>();

                var result = await marketLogic.RunAsync();
                await message.Channel.SendMessageAsync($"{string.Join('\n', result.Take(20))}");
            }
        }

        private async void PerformScheduledTasks(object state)
        {
            await SendHouseMessages();
            var nextInterval = GetNextCronInterval();

            if (nextInterval > TimeSpan.Zero)
            {
                _scheduledTimer.Change(nextInterval, Timeout.InfiniteTimeSpan);
            }
        }

        private async Task<SocketCategoryChannel> GetCategoryAsync(SocketGuild guild)
        {
            var category = guild.CategoryChannels.FirstOrDefault(category => category.Name == CATEGORY_NAME);
            var categoryId = category?.Id;

            if (categoryId is null)
            {
                var result = await guild.CreateCategoryChannelAsync(CATEGORY_NAME)
                    ?? throw new Exception("Failed to create category.");

                categoryId = result.Id;
            }

            if (categoryId == null)
            {
                throw new Exception("Could not find category.");
            }

            return guild.GetCategoryChannel(categoryId.Value);
        }

        private string FormatMessage(IEnumerable<HouseDetails> auctionedHouses) =>
            string.Join("\n", auctionedHouses.OrderBy(aH => aH.Status.Auction.AuctionEnd).Select(aH =>
            $"{aH.Name}. [{aH.Town}]\n" +
            $"- Bid: {(aH.Status.Auction.CurrentBid== 0 ? "None" : aH.Status.Auction.CurrentBid / 1000000 + $"kk ({aH.Status.Auction.CurrentBidder})")}\n" +
            $"- Size: {aH.Size}sqm\n" +
            $"- Beds: {aH.Beds}\n" +
            $"- Link: https://www.tibia.com/community/?subtopic=houses&page=view&world={aH.World}&houseid={aH.Id}\n" +
            $"- Ends: {aH.Status.Auction.AuctionEnd}\n" +
            $"{LINE_SEPARATOR}"));

        // Ensure cleanup on stop
        public override void Dispose()
        {
            _client.Dispose();
            _scheduledTimer.Dispose();
            base.Dispose();
        }

        private async Task<SocketTextChannel> GetTextChannelAsync(SocketGuild guild)
        {
            var category = await GetCategoryAsync(guild);

            var channel = guild.Channels.FirstOrDefault(c => c.Name == CHANNEL_NAME);
            var channelId = channel?.Id;

            if (channelId == null)
            {
                var result = await guild.CreateTextChannelAsync(CHANNEL_NAME, properties =>
                {
                    properties.CategoryId = category.Id;
                }) ?? throw new Exception("Failed to create channel.");

                channelId = result.Id;
            }

            if (channelId == null)
            {
                throw new Exception("Could not find channel.");
            }

            return guild.GetTextChannel(channelId.Value);
        }

        private async Task SendHouseMessages()
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
                    var channel = await GetTextChannelAsync(guild);

                    await channel.SendMessageAsync(formattedMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in scheduled task: {ex.Message}");
                }
            }
        }
    }
}