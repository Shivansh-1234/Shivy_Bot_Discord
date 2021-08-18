using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace shiv_test_bot.Modules
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {

        [Command("about")]
        public async Task AboutAsync()
        {
            var embed = new EmbedBuilder()
                .WithTitle("About Me")
                .WithDescription(" Hello! This is Shivy Bot! My creator is the one and only Shivy#9999 " +
                "Thank you for choosing me! I am an All-in-One bot who can do many tasks .For a list of command use !commands " +
                "Stay tuned for future updates!")
                .WithColor(Color.Orange)
                .Build();

            await ReplyAsync(embed:embed);
        }

        [Command("Commands")]
        public async Task CommandsAsync()
        {
            var embed = new EmbedBuilder()
                .WithTitle("COMMANDS - use '?' prefix")
                .WithDescription(
                "General Commands " +
                "\n1.   about (@username)" +
                "\n2.   info" +
                "\n" +
                "\nMemes" +
                "\n3.   meme / reddit (name of subreddit if you want (optional))" +
                "\n" +
                "\nMute And Unmute" +
                "\n4.   mute (@username)(time in minutes)(reason (optional))" +
                "\n5.   unmute (@username)" +
                "\n" +
                "\nMUSIC COMMANDS " +
                "\n6.   play (name of song)" +
                "\n7.   pause" +
                "\n8.   resume" +
                "\n9.   stop" +
                "\n10.   join" +
                "\n11.  leave" +
                "\n" +
                "\nOther Commands" +
                "\n12.  anime -- Suggests a random anime" +
                "\n13.  joseph")
                .WithColor(Color.Red)
                .Build();

            await ReplyAsync(embed:embed);
        }

        [Command("joseph")]
        public async Task JosephAsync()
        {
            var embed = new EmbedBuilder()
                .WithImageUrl("https://previews.123rf.com/images/pivden/pivden1704/pivden170400006/75374369-middle-finger-sign-by-male-hand.jpg")
                .WithDescription("MAA CHUDA")
                .WithColor(Color.Red)
                .Build();


            await ReplyAsync(embed:embed);
        }

        [Command("info")]
        public async Task InfoAsync(SocketGuildUser sgu = null)
        {
            if(sgu == null)
            {
                sgu = Context.User as SocketGuildUser;
            }

            var embed = new EmbedBuilder()
                .WithTitle($"{sgu.Username}#{sgu.Discriminator}")
                .AddField("ID:", sgu.Id, true)
                .AddField("NAME:", $"{sgu.Username}#{sgu.Discriminator}", true)
                .AddField("CREATED AT:", sgu.CreatedAt, true)
                .WithColor(new Color(238,62,75))
                .WithThumbnailUrl(sgu.GetAvatarUrl() ?? sgu.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed:  embed);
        }

        [Command("server")]
        public async Task Server()
        {
            await Context.Channel.TriggerTypingAsync();
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithDescription("About This Server")
                .WithTitle($"{Context.Guild.Name} Information")
                .WithColor(Color.Red)
                .AddField("Created at:", Context.Guild.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Total Members:", (Context.Guild as SocketGuild).MemberCount + " members", true)
                .AddField("Users Currently Online:", (Context.Guild as SocketGuild).Users.Where(x => x.Status != UserStatus.Offline).Count() + " members", true);

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}
