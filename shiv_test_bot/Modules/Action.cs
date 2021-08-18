using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using shiv_test_bot.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace shiv_test_bot.Modules
{
    public class Action : ModuleBase<SocketCommandContext>
    {
        [Command("mute")]
        [RequireUserPermissionAttribute(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.ManageRoles)]

        public async Task Mute(SocketGuildUser user, int minutes, [Remainder]string reason = null)
        {
            if(user.Hierarchy > Context.Guild.CurrentUser.Hierarchy)
            {
                await Context.Channel.SendMessageAsync("Invalid User - That user is apparently more powerful than me smh");
                return;
            }

            var role = (Context.Guild as IGuild).Roles.FirstOrDefault(x => x.Name == "Muted");
            if (role == null)
                role = await Context.Guild.CreateRoleAsync("Muted", new GuildPermissions(sendMessages: false), Color.Red, false, null);

            if(role.Position > Context.Guild.CurrentUser.Hierarchy)
            {
                await Context.Channel.SendMessageAsync("Invalid permission - That user is apparently more powerful than me smh");
                return;
            }

            if(user.Roles.Contains(role))
            {
                await Context.Channel.SendMessageAsync("Already Muted - HE/SHE is already muted ffs . Leave him alone!");
                return;
            }

            await role.ModifyAsync(x => x.Position = Context.Guild.CurrentUser.Hierarchy);

            foreach(var channel in Context.Guild.TextChannels)
            {
                if (!channel.GetPermissionOverwrite(role).HasValue || channel.GetPermissionOverwrite(role).Value.SendMessages == PermValue.Allow)
                {
                    await channel.AddPermissionOverwriteAsync(role, new OverwritePermissions(sendMessages: PermValue.Deny));
                }
            }

            CommandHandler.Mutes.Add(new Helper.Mute { Guild = Context.Guild, User = user, End = DateTime.Now + TimeSpan.FromMinutes(minutes), Role = role });
            await user.AddRoleAsync(role);
            await Context.Channel.SendMessageAsync($"Muted! OwO - Duration: {minutes} minute/s\nReason: {reason ?? "None LMFAO"} ");
        }

        [Command("unmute")]
        [RequireUserPermissionAttribute(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Unmute(SocketGuildUser user)
        {
            var role = (Context.Guild as IGuild).Roles.FirstOrDefault(x => x.Name == "Muted");
            if (role == null)
            {
                await Context.Channel.SendMessageAsync("Invalid permission - This person is note muted yet smh");
                return;
            }

            if (role.Position > Context.Guild.CurrentUser.Hierarchy)
            {
                await Context.Channel.SendMessageAsync("Invalid permission - That user is apparently more powerful than me smh");
                return;
            }

            if (!user.Roles.Contains(role))
            {
                await Context.Channel.SendMessageAsync("Invalid permission - This person is note muted yet smh");
                return;
            }

            await user.RemoveRoleAsync(role);
            await Context.Channel.SendMessageAsync($"Unmuted! {user.Username}  UwU - Enjoy Freedom!");
        }

    }
}
