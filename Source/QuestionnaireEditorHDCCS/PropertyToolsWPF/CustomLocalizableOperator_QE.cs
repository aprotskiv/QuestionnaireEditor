using PropertyTools.Wpf;
using PropertyTools.Wpf.Operators;
using QuestionnaireEditorHDCCS.Resources;

namespace QuestionnaireEditorHDCCS.PropertyToolsWPF
{
    public class CustomLocalizableOperator_QE : DefaultLocalizableOperator
    {
        public override string? GetLocalizedString(string key, Type declaringType)
        {
            var value = key;

            String? resourceKey = null;

            if (declaringType?.IsEnumOrNullableEnum() == true)
            {
                // resource strings for enum values are retrieved by composite key  "{enumType.FullName}.{enum member}"
                resourceKey = declaringType.FullName + "." + (key ?? "-"); // in case it is NULL in Nullable<EnumType>
            }
            else if (key != null)
            {
                resourceKey = key;
                if (declaringType != null)
                {
                    resourceKey = declaringType.FullName + "." + resourceKey;
                }
            }

            if (resourceKey != null)
            {
                var resourceValue = PropertyGrid_LocalizedStrings.ResourceManager.GetString(resourceKey);
                if (resourceValue != null)
                {
                    value = resourceValue;
                }
                else
                {
                    // not resource found
                    if (declaringType?.IsEnumOrNullableEnum() == true)
                    {
                        value = key; // for enums - show original text
                    }
                    else
                    {
                        value = $"[{key}]"; // otherwise non-translated text wrap in square brackets
                    }
                }
            }

            return value;
        }
    }
}
