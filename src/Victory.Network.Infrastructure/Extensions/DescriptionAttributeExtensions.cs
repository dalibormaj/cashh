using System;
using System.ComponentModel;
using Victory.Network.Infrastructure.Resources;

namespace Victory.Network.Infrastructure.Extensions
{
    public static class DescriptionAttributeExtensions
    {
        public static string GetDescription(this Enum e, bool tryTranslate = false)
        {
            var memberInfo = e.GetType().GetMember(e.ToString());
            var description = e.ToString();
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            if (tryTranslate)
                description = ResourceManager.GetText(description);

            return description;
        }
    }
}
