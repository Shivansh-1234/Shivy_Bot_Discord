using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Victoria;
using shiv_test_bot.Services;
using Victoria.Enums;
using System.Linq;

namespace shiv_test_bot.Modules
{
    public class Music : ModuleBase<SocketCommandContext>
    {
        private readonly LavaNode lavanode;

        public Music(LavaNode lavanode)
        {
            this.lavanode = lavanode;
        }

        [Command("Join")]
        public async Task JoinAsync()
        {
            if (lavanode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm already connected to a voice channel!UwU");
                return;
            }

            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to a voice channel!UwU");
                return;
            }

            try
            {
                await lavanode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync($"Joined {voiceState.VoiceChannel.Name}!");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Play")]
        [Alias("p")]
        public async Task PlayAsync([Remainder] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                await ReplyAsync("Please provide search terms.UwU");
                return;
            }

            if (!lavanode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel.UwU");
                return;
            }


            var searchResponse = await lavanode.SearchYouTubeAsync(query);
            if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
                searchResponse.LoadStatus == LoadStatus.NoMatches)
            {
                await ReplyAsync($"I wasn't able to find anything for `{query}`.UwU");
                return;
            }

            var player = lavanode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {

                var track = searchResponse.Tracks[0];
                player.Queue.Enqueue(track);
                await ReplyAsync($"Enqueued: {track.Title}  UwU");

            }
            else
            {
                var track = searchResponse.Tracks[0];


                await player.PlayAsync(track);
                await ReplyAsync($"Now Playing: {track.Title}  UwU");
            }
            
        }

        [Command("Leave")]
        public async Task LeaveAsync()
        {
            if (!lavanode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAsync("I'm not connected to any voice channels!UwU");
                return;
            }

            var voiceChannel = (Context.User as IVoiceState).VoiceChannel ?? player.VoiceChannel;
            if (voiceChannel == null)
            {
                await ReplyAsync("Not sure which voice channel to disconnect from.UwU");
                return;
            }

            try
            {
                await lavanode.LeaveAsync(voiceChannel);
                await ReplyAsync($"I've left {voiceChannel.Name}  UwU!");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Pause")]
        public async Task PauseAsync()
        {
            if (!lavanode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAsync("I'm not connected to a voice channel.UwU");
                return;
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                await ReplyAsync("I cannot pause when I'm not playing anything!UwU");
                return;
            }

            try
            {
                await player.PauseAsync();
                await ReplyAsync($"Paused: {player.Track.Title}");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Resume")]
        public async Task ResumeAsync()
        {
            if (!lavanode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Paused)
            {
                await ReplyAsync("I cannot resume when I'm not playing anything!");
                return;
            }

            try
            {
                await player.ResumeAsync();
                await ReplyAsync($"Resumed: {player.Track.Title}  UwU");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Stop")]
        public async Task StopAsync()
        {
            if (!lavanode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAsync("I'm not connected to a voice channel.UwU");
                return;
            }

            if (player.PlayerState == PlayerState.Stopped)
            {
                await ReplyAsync("It's Already Stopped You Muggle");
                return;
            }

            try
            {
                await player.StopAsync();
                await ReplyAsync("No longer playing anything.");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Command("Skip")]
        public async Task Skip()
        {
            var voiceState = Context.User as IVoiceState;
            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAsync("You must be connected to a voice channel!UwU");
                return;
            }

            if (!lavanode.HasPlayer(Context.Guild))
            {
                await ReplyAsync("I'm not connected to a voice channel!UwU");
                return;
            }

            var player = lavanode.GetPlayer(Context.Guild);

            if (voiceState.VoiceChannel != player.VoiceChannel)
            {
                await ReplyAsync("You are in a different voice channel stupid muggle . Put me in right channel UwU");
                return;
            }

            if(player.Queue.Count == 0)
            {
                await ReplyAsync("No more song in the queue :SADFACE: UwU");
                return;
            }

            await player.SkipAsync();
            await ReplyAsync($"Skipped UWUWUW ! Now playing **{player.Track.Title}**!");
        }

    }
}
