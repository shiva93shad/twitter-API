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


namespace ConsoleApplication5
{
    class Program
    {
        static void Main()
        {
            string CONSUMER_KEY = "okIbrwDiJY0cLOEL6SmonCVZ3";
            string CONSUMER_SECRET = "oZ8KO1tKR4I4OqNna1S7rKixOeelrHtekujrkmyaJmLDngFsqo";
            string ACCESS_TOKEN_KEY = "802387812599365632-DJIse3LlJaeaWBdEKYtGqIWhTraRdN2";
            string ACCESS_TOKEN_SECRET = "DZs1OlfqTxxrEYOpWc7wob84K748v28SUyfDUhrRgT1Ey";

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

            long maxID = 812570866257186816;
            
            var searchParameter = Search.CreateTweetSearchParameter("#Trump");
            searchParameter.MaximumNumberOfResults = 100;
            searchParameter.MaxId = maxID;
          //  searchParameter.Since = new DateTime(2016, 12, 1);
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
                        maxID = t.Id;
                        searchParameter.MaxId = maxID;
                        if (tweets == null)
                        {
                            break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(50000);
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
    }
}
