using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
// REST API
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

// STREAM API
using Tweetinvi.Streaming;
using Stream = Tweetinvi.Stream;

// Others
using Tweetinvi.Exceptions; // Handle Exceptions
using Tweetinvi.Core.Extensions; // Extension methods provided by Tweetinvi
using Tweetinvi.Models.DTO; // Data Transfer Objects for Serialization
using Tweetinvi.Json; // JSON static classes to get json from Twitter.
using System.IO;

namespace twitterAPI
{
    class Program
    {
        static void Main()
        {
            string CONSUMER_KEY = "okIbrwDiJY0cLOEL6SmonCVZ3";
            string CONSUMER_SECRET = "oZ8KO1tKR4I4OqNna1S7rKixOeelrHtekujrkmyaJmLDngFsqo";
            string ACCESS_TOKEN_KEY = "802387812599365632-DJIse3LlJaeaWBdEKYtGqIWhTraRdN2";
            string ACCESS_TOKEN_SECRET = "DZs1OlfqTxxrEYOpWc7wob84K748v28SUyfDUhrRgT1Ey";

           // File.AppendAllText(@"d:\tweets.txt", "hi");

            Auth.SetUserCredentials(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN_KEY, ACCESS_TOKEN_SECRET);
            var user = User.GetAuthenticatedUser();
            Console.WriteLine(user);
            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                var queryRateLimit = RateLimit.GetQueryRateLimit(args.QueryURL);
                Console.WriteLine(queryRateLimit);
                Console.WriteLine(args.QueryURL);
            };


            //var tweet = Tweet.PublishTweet("hi");
            //var tweet = Search.SearchTweetsWithMetadata("pizza");
            //Console.WriteLine(tweet);
            //813172596451770368
            long maxID = 815300283228127233;
            long count = 0;
            var searchParameter = Search.CreateTweetSearchParameter("Man. City  Liverpool");
            searchParameter.MaxId = maxID;
            searchParameter.Since = new DateTime(2016, 12, 31);
            searchParameter.Until = new DateTime(2017, 01, 01);
            searchParameter.Lang = LanguageFilter.English;
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;
           
            //Search.
            //  searchParameter.TweetSearchType = twee.OriginalTweetsOnly;
            //searchParameter.TweetSearchType= TweetSearchType.
            while(true)
            {
              

                try
                {
                    // var tokenRateLimits = RateLimit.GetCurrentCredentialsRateLimits();

                    //Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);



                    var tweets = Search.SearchTweets(searchParameter);

                   
                    foreach (var t in tweets)
                    {
                        
                        Console.WriteLine(t.FullText);
                        Console.WriteLine(t.CreatedBy);
                        Console.WriteLine(t.CreatedAt);
                        count = count + 1;
                        Console.WriteLine(count);
                        maxID = t.Id;
                        string csv = null;
                        string tweetUser = null;
                        searchParameter.MaxId = maxID;
                         csv = tweet_to_string(t);
                        csv = csv + "\r\n";
                         tweetUser = user_to_string(t.CreatedBy);
                        tweetUser = tweetUser + "\r\n";
                        File.AppendAllText(@"C:\Users\j.amini\OneDrive\twitter API\tweets-man city vs liverpool.csv", csv);
                        File.AppendAllText(@"C:\Users\j.amini\OneDrive\twitter API\tweetUser-man city vs liverpool.csv", tweetUser);

                        if (tweets == null)
                        {
                            break;
                        }
                    }
                   // System.Threading.Thread.Sleep(2000);

                }
                catch (Exception ex)
                {
                  //  System.Threading.Thread.Sleep(2000);
                    //  Console.WriteLine(ex);
                    //RateLimit.QueryAwaitingForRateLimit += (sender, args) =>
                    //{
                    //    Console.WriteLine("{0} is awaiting {1}ms for RateLimit to be available", args.Query, args.ResetInMilliseconds);
                    //};
                }

               

               
            }
            Console.WriteLine("The END");
            Console.ReadKey();
        }

        private static string user_to_string(IUser createdBy)
        {
            string u=null;
            u += createdBy.Id;
            u += ";" + createdBy.Name;
            u += ";" + createdBy.CreatedAt.ToString();
            u += ";" + createdBy.Location;
            u += ";" + createdBy.Language;
            u += ";" + createdBy.ListedCount;
            u += ";" + createdBy.Protected;
            u += ";" + createdBy.Retweets;
            u += ";" + createdBy.StatusesCount;
            u += ";" + createdBy.Url;
            u += ";" + createdBy.FollowersCount;
            u += ";" + createdBy.FavouritesCount;
            u += ";" + createdBy.Following;
            u += ";" + createdBy.FriendsCount;
            return u;


        }

        public static string tweet_to_string(ITweet t)
        {
            string s;
            s = t.Id.ToString();
            s = s + ";" + t.FullText;
            s = s + ";" + t.CreatedBy.Id.ToString();
            s = s + ";" + t.CreatedAt;
            s = s + ";" + t.Hashtags.Count;
            s = s + ";";
            foreach (var k in t.Hashtags)
            {
                s = s + "," + k.Text;
            }
               
            s = s + ";" + t.Media.Count;
            s = s + ";" + t.Entities.Symbols.Count;
            s = s + ";";
            foreach (var k in t.Entities.Symbols)
            {
                s = s + "," + k.Text;
            }

            s = s + ";" + t.Urls.Count;
            s = s + ";";
            foreach (var k in t.Urls)
            {
                s = s + "," + k.URL;
            }
            s = s + ";" + t.UserMentions.Count();
            s = s + ";";
            foreach (var k in t.UserMentions)
            {
                s = s + "," + k.Name;
            }

            s = s + ";" + t.FavoriteCount;
            s = s + ";" + t.InReplyToUserId;
            s += ";" + t.InReplyToUserIdStr;
            s = s + ";" + t.IsRetweet;
            s = s + ";" + t.Retweeted;
            s += ";" + t.RetweetCount;
            s += ";" + t.IsTweetDestroyed;
            s += ";" + t.IsTweetPublished;
            s += ";" + t.Language;
            s += ";" + t.QuotedTweet;
            s += ";" + t.PublishedTweetLength;

            s.Replace("\n","\t");

            return s;

        }
    }
}
