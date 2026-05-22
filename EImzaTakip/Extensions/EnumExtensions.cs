using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EImzaTakip.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// enum içerisindeki dipslay değerleri içerisindeki değerlerin gözükmesini sağlar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            var memberInfo = value.GetType()
                                  .GetMember(value.ToString())
                                  .FirstOrDefault();

            if (memberInfo != null)
            {
                var displayAttribute = memberInfo
                    .GetCustomAttribute<DisplayAttribute>();

                if (displayAttribute != null)
                {
                    return displayAttribute.Name;
                }
            }

            return value.ToString();
        }
    }
}
