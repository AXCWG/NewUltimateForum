namespace UltimateForum.Server;

public static class GlobalStatic
{
    public static IConfiguration? ApplicationConfiguration { get; set; }

    public static bool GetConfiguredValueOrThrow(this IConfiguration? conf, string key)
    {
        return bool.Parse(conf?[key]??throw new Exception(
            $"Whether it's called before app build or {key} is null. If the latter, please download a new appsettings.json from the repository. "));
    }

    extension(ISession s)
    {
        public long? GetLong(string key)
        {
            var str =  s.GetString(key);
            if (str is null)
            {
                return null; 
            }
            return long.Parse(str); 
        }
    }
}