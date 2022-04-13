using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;

namespace Victory.Network.Infrastructure.Resources
{
    public static class ResourceManager
    {
        public static string GetText(Type type, string key)
        {
            var options = Options.Create(new LocalizationOptions());

            var stringLocalizerFactory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localizer = stringLocalizerFactory.Create(type);
            var translation = localizer.GetString(key);

            return translation.ToString();
        }

        public static string GetText(string key)
        {
            var translation = GetText(typeof(SharedResource), key);
            return translation.ToString();
        }
    }
}
