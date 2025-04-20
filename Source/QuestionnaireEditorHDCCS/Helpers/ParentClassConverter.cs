using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace QuestionnaireEditorHDCCS.Helpers
{
    public class ParentClassConverter
    {
        public static TChild Convert<TChild>([NotNull] object parent)
        {
            return JsonConvert.DeserializeObject<TChild>(
                JsonConvert.SerializeObject(parent)
            );
        }
    }
}