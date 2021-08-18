using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shiv_test_bot.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Victoria;

namespace shiv_test_bot.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService service;
        private readonly IConfiguration configuration;
        private readonly LavaNode lavanode;
        public static List<Mute> Mutes = new List<Mute>();
        

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration configuration, LavaNode lavanode)
        {
            this.provider = provider;
            this.client = client;
            this.service = service;
            this.configuration = configuration;
            this.lavanode = lavanode;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            this.client.MessageReceived += OnMessageReceived;
            this.service.CommandExecuted += OnCommandExecuted;
            this.client.Ready += OnReadyAsync;

            var newTask = new Task(async () => await MuteHandler());
            newTask.Start();

            await this.service.AddModulesAsync(Assembly.GetEntryAssembly(), this.provider);
        }

        private async Task MuteHandler()
        {
            List<Mute> Remove = new List<Mute>();

            foreach(var mute in Mutes)
            {
                if (DateTime.Now < mute.End)
                    continue;

                var guild = client.GetGuild(mute.Guild.Id);

                if(guild.GetRole(mute.Role.Id) == null)
                {
                    Remove.Add(mute);
                    continue;
                }

                var role = guild.GetRole(mute.Role.Id);

                if(guild.GetUser(mute.User.Id) == null)
                {
                    Remove.Add(mute);
                    continue;
                }

                var user = guild.GetUser(mute.User.Id);

                if(role.Position > guild.CurrentUser.Hierarchy)
                {
                    Remove.Add(mute);
                    continue;
                }

                await user.RemoveRoleAsync(mute.Role);
                Remove.Add(mute);
            }
            Mutes = Mutes.Except(Remove).ToList();

            await Task.Delay(1 * 60 * 1000);
            await MuteHandler();
        }

        private async Task OnReadyAsync()
        {        
            if (!lavanode.IsConnected)
            {
                await lavanode.ConnectAsync();
            }          
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result)
        {
            if(result.IsSuccess)
            {
                return;
            }

            await commandContext.Channel.SendMessageAsync(result.ErrorReason);
        }

        private async Task OnMessageReceived(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(this.configuration["Prefix"], ref argPos) && !message.HasMentionPrefix(this.client.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(this.client, message);
            await this.service.ExecuteAsync(context, argPos, this.provider);
        }

        
    }
}
