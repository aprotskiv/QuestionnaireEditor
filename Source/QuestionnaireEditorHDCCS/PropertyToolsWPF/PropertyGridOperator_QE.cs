using PropertyTools.Wpf;
using QuestionnaireEditorHDCCS.Resources;

namespace QuestionnaireEditorHDCCS.PropertyToolsWPF
{
    public class PropertyGridOperator_QE : PropertyGridOperator
    {
        protected override string? GetLocalizedString(string key, Type declaringType)
        {
            var key2 = key;

            if (declaringType != null)
            {
                if (declaringType.IsEnum == true || IsNullableEnum(declaringType))
                {
                    var enumMemberResourceKey = declaringType.Name + "." + (key ?? "-"); // in case it is NULL in Nullable<EnumType>
                    key2 = enumMemberResourceKey;
                }
            }

            if (key2 == null)
                return null;

            var value = PropertyGrid_LocalizedStrings.ResourceManager.GetString(key2);
            if (value == null)
            {
                if (declaringType?.IsEnum == true)
                {
                    value = key; // for enums - show original text
                }
                else
                {
                    value = $"[{key}]"; // otherwise non-translated text wrap in square brackets
                }
            }

            return value;
        }
    }
}
