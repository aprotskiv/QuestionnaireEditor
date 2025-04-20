using System.Reflection;
using System.Runtime.Serialization;

namespace QuestionnaireEditorHDCCS.Helpers
{
    public class EnumHelper
    {
        public static T MatchByEnumMemberValue<T>(string? enumMemberValue, T defaultValue)
            where T : struct, IConvertible
        {
            if (string.IsNullOrEmpty(enumMemberValue))
            {
                return defaultValue;
            }

            var enumType = typeof(T);
            var memberInfo = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static)
                            .FirstOrDefault( mi => {
                                var enumMemberAttr = mi.GetCustomAttribute(typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                                return enumMemberAttr?.Value == enumMemberValue;
                            });
            if (memberInfo == null)
            {
                return defaultValue;
            }
            return Enum.Parse<T>(memberInfo.Name);
        }

        public static string? GetEnumMemberValue<T>(T enumValue)
             where T : struct, IConvertible
        {
            var enumType = typeof(T);
            var memberInfo = enumType.GetMember(enumValue.ToString(), BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault();

            return (memberInfo?.GetCustomAttribute(typeof(EnumMemberAttribute)) as EnumMemberAttribute)?.Value;                            
        }
    }
}
