using Newtonsoft.Json.Linq;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using static ServiceStack.OrmLite.Dapper.SqlMapper;

namespace ClipHost;

public static class TwitchHelper
{
    public static TwitchAPI GetTwitchApi(this IAppSettings settings, string accessToken)
    {

        var ConsumerKey = settings.Get<string>("Twitch:Id");
        var ConsumerSecret = settings.Get<string>("Twitch:Secret");
        return new TwitchAPI(settings: new ApiSettings()
        {
            AccessToken = accessToken,
            ClientId = ConsumerKey,
            Secret = ConsumerSecret,
        });
    }

    public static async Task<TwitchAPI> GetRefreshedApi(this IAppSettings settings, string accessToken, string refreshToken)
    {
        var ConsumerSecret = settings.Get<string>("Twitch:Secret");
        var api = settings.GetTwitchApi(accessToken);
        TwitchLib.Api.Auth.RefreshResponse refreshAttempt = await api.Auth.RefreshAuthTokenAsync(refreshToken, ConsumerSecret);
        return settings.GetTwitchApi(refreshAttempt.AccessToken);
    }

    public static async Task<TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream?> GetLiveStreamAsync(this TwitchAPI api, string twitchUserName)
    {

        var items = await api.Helix.Streams.GetStreamsAsync(userLogins: new List<string>() { twitchUserName });
        return items.Streams.Length != 0 ? items.Streams[0] : null;
    }
    public static async Task<AuthId?> VerifyTwitchAccessTokenAsync(string consumerKey,
    string consumerSecret, string accessToken, string accessTokenSecret, CancellationToken token = default)
    {
        var api = new TwitchAPI(settings: new ApiSettings()
        {
            AccessToken = accessToken,
            ClientId = consumerKey,
            Secret = consumerSecret,
        });
        try
        {
            var usr = await api.Helix.Users.GetUsersAsync();
            if (usr == null) return null;
            return new AuthId()
            {
                Email = usr.Users[0].Login + "@twitch.tv",
                UserId = usr.Users[0].Login
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}