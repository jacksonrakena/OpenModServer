using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using OpenModServer.Areas.Identity;

namespace OpenModServer.Structures;

public static class TempDataExtensions
{
    /// <summary>
    /// Puts an object into the TempData by first serializing it to JSON.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tempData"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void PutJson<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonConvert.SerializeObject(value);
    }

    /// <summary>
    /// Gets an object from the TempData by deserializing it from JSON.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tempData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T? GetJson<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        object o;
        tempData.TryGetValue(key, out o);
        return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
    }

    public static AuthorizationOptions AddPermissionPolicy(this AuthorizationOptions authorization,
        Permissions permission)
    {
        authorization
            .AddPolicy(permission.ToString("G"), auth =>
            {
                auth.RequireAssertion(ctx =>
                    ctx.User.HasClaim("Permission", permission.ToString("G")) ||
                    ctx.User.HasClaim("Permission", Permissions.Administrator.ToString("G")));
            });
        return authorization;
    }
}