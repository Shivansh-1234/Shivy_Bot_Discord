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

namespace shiv_test_bot.Modules
{
    public class Fun : ModuleBase
    {

        private readonly Random _random = new Random();

        [Command("meme")]
        [Alias("reddit")]
        public async Task Meme(string subreddit = null)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync($"https://reddit.com/r/{subreddit ?? "memes"}/random.json?limit=1");
            if(!result.StartsWith("["))
            {
                await Context.Channel.SendMessageAsync("This subreddit doesnt exist!");
                return;
            }
            JArray arr = JArray.Parse(result);
            JObject post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString());

            await Context.Channel.TriggerTypingAsync();
            var builder = new EmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithColor(new Color(33, 176, 252))
                .WithTitle(post["title"].ToString())
                .WithUrl("https://reddit.com" + post["permalink"].ToString())
                .WithFooter($"🗨 {post["num_comments"]} ⬆️ {post["ups"]}");
            var embed = builder.Build();

            await ReplyAsync(embed: embed);

        }

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        [Command("anime")]
        [Alias("reddit")]
        public async Task Anime()
        {
            int rand = RandomNumber(1, 10);
            await Context.Channel.TriggerTypingAsync();

            // Not the best way ik but its 3 in the morning :/
            if (rand == 1)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Steins;Gate")
                    .WithUrl("https://myanimelist.net/anime/9253/Steins_Gate")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/5/73199.jpg")
                    .AddField("Description", "The self-proclaimed mad scientist Rintarou Okabe rents out a room in a rickety old building in Akihabara, where he indulges himself in his hobby of inventing " +
                    "prospective future gadgets with fellow lab members: Mayuri Shiina, his air-headed childhood friend, and Hashida Itaru, a perverted hacker nicknamed Daru." +
                    "The three pass the time by tinkering with their most promising contraption yet, a machine dubbed the Phone Microwave " +
                    "which performs the strange function of morphing bananas into piles of green gel.", false)
                    .AddField("Aired: ", "Apr 06, 2011 to Sep 14, 2011", false)
                    .AddField("Rating: ", "Ranked #4  Popularity #11   Members 2,022,272 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/5/73199.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 2)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Fullmetal Alchemist: Brotherhood")
                    .WithUrl("https://myanimelist.net/anime/5114/Fullmetal_Alchemist__Brotherhood")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/1223/96541.jpg")
                    .AddField("Description", "Alchemy is bound by this Law of Equivalent Exchange—something the young brothers Edward and Alphonse Elric only realize after attempting " +
                    "human transmutation: the one forbidden act of alchemy. They pay a terrible price for their transgression—Edward loses his left leg, Alphonse his physical body. " +
                    "It is only by the desperate sacrifice of Edward's right arm that he is able to affix Alphonse's soul to a suit of armor. Devastated and alone, it is the hope " +
                    "that they would both eventually return to their original bodies that gives Edward the inspiration to obtain metal limbs called " +
                    "automail and become a state alchemist, the Fullmetal Alchemist.", false)
                    .AddField("Aired: ", "Apr 05, 2009 to Jul 04, 2010", false)
                    .AddField("Rating: ", "Ranked #1  Popularity #3  Members 2,580,656 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/1223/96541.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 3)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Jujutsu Kaisen")
                    .WithUrl("https://myanimelist.net/anime/40748/Jujutsu_Kaisen_TV")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/1171/109222.jpg")
                    .AddField("Description", "Idly indulging in baseless paranormal activities with the Occult Club, high schooler Yuuji Itadori spends his days at either the clubroom or the hospital, " +
                    "where he visits his bedridden grandfather. However, this leisurely lifestyle soon takes a turn for the strange when he unknowingly encounters a cursed item. Triggering a chain of supernatural occurrences, " +
                    "Yuuji finds himself suddenly thrust into the world of Curses—dreadful beings formed from human malice and negativity—after swallowing the said item, revealed to be a finger belonging to the demon Sukuna Ryoumen, " +
                    "the King of Curses.", false)
                    .AddField("Aired: ", "Oct 03, 2020 until now", false)
                    .AddField("Rating: ", "Ranked #37  Popularity #52  Members 1,374,500 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/1171/109222.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 4)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Haikyuu!!")
                    .WithUrl("https://myanimelist.net/anime/20583/Haikyuu?q=haikyuu&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/7/76014.jpg")
                    .AddField("Description", "Inspired after watching a volleyball ace nicknamed Little Giant in action, small-statured Shouyou Hinata revives the volleyball club at his middle school." +
                    "The newly-formed team even makes it to a tournament; however, their first match turns out to be their last when they are brutally squashed by the King of the Court,  " +
                    "Tobio Kageyama. Hinata vows to surpass Kageyama, and so after graduating from middle school, he joins Karasuno High School's volleyball team—only to find that his sworn rival, " +
                    "Kageyama, is now his teammate.", false)
                    .AddField("Aired: ", "Jan 01, 2014 until now", false)
                    .AddField("Rating: ", "Ranked #122  Popularity #37  Members 1,465,950 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/7/76014.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 5)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Monster")
                    .WithUrl("https://myanimelist.net/anime/19/Monster?q=monster&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/10/18793.jpg")
                    .AddField("Description", "Dr. Kenzou Tenma, an elite neurosurgeon recently engaged to his hospital director's daughter, is well on his way to ascending the hospital hierarchy. That is until one night, " +
                    "a seemingly small event changes Dr. Tenma's life forever. While preparing to perform surgery on someone, he gets a call from the hospital director telling him to switch patients and instead perform life-saving " +
                    "brain surgery on a famous performer. His fellow doctors, fiancée, and the hospital director applaud his accomplishment; but because of the switch, a poor immigrant worker is dead, causing " +
                    "Dr. Tenma to have a crisis of conscience.", false)
                    .AddField("Aired: ", "Apr 07, 2004 to Sep 28, 2005", false)
                    .AddField("Rating: ", "Ranked #31  Popularity #161  Members 728,797 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/10/18793.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 6)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Kimetsu no Yaiba")
                    .WithUrl("https://myanimelist.net/anime/38000/Kimetsu_no_Yaiba?q=demon%20slaye&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/1286/99889.jpg")
                    .AddField("Description", "Ever since the death of his father, the burden of supporting the family has fallen upon Tanjirou Kamado's shoulders. Though living impoverished on a remote mountain, " +
                    "the Kamado family are able to enjoy a relatively peaceful and happy life. One day, Tanjirou decides to go down to the local village to make a little money selling charcoal. On his way back, night falls, " +
                    "forcing Tanjirou to take shelter in the house of a strange man, who warns him of the existence of flesh-eating demons that lurk in the woods at night.", false)
                    .AddField("Aired: ", "Apr 06, 2019 to Sep 28, 2019", false)
                    .AddField("Rating: ", "Ranked #78  Popularity #15  Members 1,953,509 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/1286/99889.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 7)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Shingeki no Kyojin")
                    .WithUrl("https://myanimelist.net/anime/16498/Shingeki_no_Kyojin?q=shingeki%20no&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/10/47347.jpg")
                    .AddField("Description", "Centuries ago, mankind was slaughtered to near extinction by monstrous humanoid creatures called titans, forcing humans to hide in fear behind enormous concentric walls. What makes these " +
                    "giants truly terrifying is that their taste for human flesh is not born out of hunger but what appears to be out of pleasure. To ensure their survival, the remnants of humanity began living within defensive barriers, " +
                    "resulting in one hundred years without a single titan encounter. However, that fragile calm is soon shattered when a colossal titan manages to breach the supposedly impregnable outer wall, reigniting the fight for survival " +
                    "against the man-eating abominations.", false)
                    .AddField("Aired: ", "Apr 07, 2013 until now", false)
                    .AddField("Rating: ", "Ranked #106  Popularity #2  Members 3,009,488 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/10/47347.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 8)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Death Note")
                    .WithUrl("https://myanimelist.net/anime/1535/Death_Note?q=death%20note&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/9/9453.jpg")
                    .AddField("Description", "A shinigami, as a god of death, can kill any person—provided they see their victim's face and write their victim's name in a notebook called a Death Note. One day, Ryuk, " +
                    "bored by the shinigami lifestyle and interested in seeing how a human would use a Death Note, drops one into the human realm", false)
                    .AddField("Aired: ", "Oct 04, 2006 until now", false)
                    .AddField("Rating: ", "Ranked #62  Popularity #1  Members 3,028,020 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/9/9453.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 9)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Kimi no Na wa.")
                    .WithUrl("https://myanimelist.net/anime/32281/Kimi_no_Na_wa")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/5/87048.jpg")
                    .AddField("Description", "Mitsuha Miyamizu, a high school girl, yearns to live the life of a boy in the bustling city of Tokyo—a dream that stands in stark contrast to her present life in the countryside. " +
                    "Meanwhile in the city, Taki Tachibana lives a busy life as a high school student while juggling his part-time job and hopes for a future in architecture.", false)
                    .AddField("Aired: ", "Aug 26, 2016", false)
                    .AddField("Rating: ", "Ranked #20  Popularity #9  Members 2,070,179 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/5/87048.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 10)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Naruto")
                    .WithUrl("https://myanimelist.net/anime/20/Naruto?q=naruto&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/13/17405.jpg")
                    .AddField("Description", "Moments prior to Naruto Uzumaki's birth, a huge demon known as the Kyuubi, the Nine-Tailed Fox, attacked Konohagakure, the Hidden Leaf Village, and wreaked havoc. " +
                    "In order to put an end to the Kyuubi's rampage, the leader of the village, the Fourth Hokage, sacrificed his life and sealed the monstrous beast inside the newborn Naruto.", false)
                    .AddField("Aired: ", "Oct 3, 2002 to Feb 8, 2007", false)
                    .AddField("Rating: ", "Ranked #627  Popularity #8  Members 2,187,158 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/13/17405.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 11)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("One Piece")
                    .WithUrl("https://myanimelist.net/anime/21/One_Piece")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/6/73245.jpg")
                    .AddField("Description", "Gol D. Roger was known as the Pirate King,  the strongest and most infamous being to have sailed the Grand Line. The capture and execution of Roger by the World Government brought a change " +
                    "throughout the world. His last words before his death revealed the existence of the greatest treasure in the world, One Piece. It was this revelation that brought about the Grand Age of Pirates, men who dreamed of " +
                    "finding One Piece—which promises an unlimited amount of riches and fame—and quite possibly the pinnacle of glory and the title of the Pirate King.", false)
                    .AddField("Aired: ", "Oct 20, 1999 to Now", false)
                    .AddField("Rating: ", "Ranked #75  Popularity #31  Members 1,605,487 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/6/73245.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 12)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("One Punch Man")
                    .WithUrl("https://myanimelist.net/anime/30276/One_Punch_Man")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/12/76049.jpg")
                    .AddField("Description", "The seemingly ordinary and unimpressive Saitama has a rather unique hobby: being a hero. In order to pursue his childhood dream, he trained relentlessly for three years—and lost " +
                    "all of his hair in the process. Now, Saitama is incredibly powerful, so much so that no enemy is able to defeat him in battle. In fact, all it takes to defeat evildoers with just one punch has led to an " +
                    "unexpected problem—he is no longer able to enjoy the thrill of battling and has become quite bored.", false)
                    .AddField("Aired: ", "Oct 5, 2015 to Dec 21, 2015", false)
                    .AddField("Rating: ", "Ranked #95  Popularity #5  Members 2,476,596 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/12/76049.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 13)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Mo Dao Zu Shi: Xian Yun Pian")
                    .WithUrl("https://myanimelist.net/anime/38450/Mo_Dao_Zu_Shi__Xian_Yun_Pian")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/1404/106707.jpg")
                    .AddField("Description", "Continuing his masquerade as the deranged lunatic from the Lanling Jin Clan, Wei Wuxian resides in the Cloud Recesses while his former cultivation classmate, Lan Wangji, searches for answers " +
                    "about the demonic severed arm they have in custody. With an overwhelming dark energy emanating from the arm, the two are forced to work together in order to keep it contained. However, the demonic arm is not the only " +
                    "dark force lurking in the region, and as spiritual tensions rise in the mountains of the Gusu Lan Clan, it is up to the two of them to try and restore the natural order.", false)
                    .AddField("Aired: ", "Aug 3, 2019 to Sep 21, 2019", false)
                    .AddField("Rating: ", "Ranked #130  Popularity #2073  Members 67,069 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/1404/106707.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 14)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Berserk")
                    .WithUrl("https://myanimelist.net/anime/32379/Berserk?q=beserk&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/10/79352.jpg")
                    .AddField("Description", "Now branded for death and destined to be hunted by demons until the day he dies, Guts embarks on a journey to defy such a gruesome fate, as waves of beasts relentlessly pursue him. " +
                    "Steeling his resolve, he takes up the monstrous blade Dragonslayer and vows to exact vengeance on the one responsible, hunting down the very man he once looked up to and considered a friend.", false)
                    .AddField("Aired: ", "Jul 1, 2016 to Sep 16, 2016", false)
                    .AddField("Rating: ", "Ranked #6964  Popularity #574  Members 286,73 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/10/79352.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }

            else if (rand == 15)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Boku no Hero Academia")
                    .WithUrl("https://myanimelist.net/anime/31964/Boku_no_Hero_Academia?q=my%20hero%20aca&cat=anime")
                    .WithImageUrl("https://cdn.myanimelist.net/images/anime/10/78745.jpg")
                    .AddField("Description", "The appearance of quirks,  newly discovered super powers, has been steadily increasing over the years, with 80 percent of humanity possessing various abilities from manipulation of elements to " +
                    "shapeshifting. This leaves the remainder of the world completely powerless, and Izuku Midoriya is one such individual.", false)
                    .AddField("Aired: ", "Apr 3, 2016 to Jun 26, 2016", false)
                    .AddField("Rating: ", "Ranked #538  Popularity #6  Members 2,316,652 \n(According to myanimelist.com)", false)
                    .WithThumbnailUrl("https://cdn.myanimelist.net/images/anime/10/78745.jpg")
                    .WithColor(Color.Red)
                    .Build();

                await ReplyAsync(embed: embed);
            }


        }
    }
}
