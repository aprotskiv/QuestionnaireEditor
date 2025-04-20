using System.Globalization;
using System.Reflection;
using System.Text;

namespace CulturesCodesEnumGenerator
{
    internal class Program
    {
        static bool IsBrowsable(CultureInfo x)
        {
            return x.Name.StartsWith("en-")
                             ? (x.Name == "en-GB" || x.Name == "en-US")
                             :
                                 x.Name.StartsWith("es-")
                                     ? (x.Name == "es-ES" || x.Name == "es-MX")
                                     :
                                         x.Name.StartsWith("fr-")
                                         ? (x.Name == "fr-FR")
                                         :
                                             x.Name.StartsWith("ru-")
                                                 ? (x.Name == "ru-RU")
                                                 :
                                                    x.Name.StartsWith("pt-")
                                                        ? (x.Name == "pt-PT" || x.Name == "pt-BR")
                                                        :
                                                            x.Name.StartsWith("nl-")
                                                                ? (x.Name == "nl-NL")
                                                                :
                                                                    x.Name.StartsWith("it-")
                                                                        ? (x.Name == "it-IT")
                                                                        : true;

            
        }

        static void Main(string[] args)
        {


            var sb = new StringBuilder();
            sb.AppendLine("public enum SpecificCulturesCodes");
            sb.AppendLine("{");

            var allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Where(x => x.Name != "en-US-POSIX");

            foreach (var ci in allCultures)
            {
                if (!IsBrowsable(ci))
                {
                    // [BrowsableAttribute(false)]
                    sb.Append("\t\t")
                        .AppendFormat("[BrowsableAttribute(false)]")
                        .AppendLine();
                }

                // [EnumMember(Value = "en")]
                sb.Append("\t\t")
                    .AppendFormat("[EnumMember(Value = \"{0}\")]", ci.Name)
                    .AppendLine();

                // [Description("English")]
                sb.Append("\t\t")
                    .AppendFormat("[Description(\"{0}\")]", ci.EnglishName)
                    .AppendLine();

                // English,
                sb.Append("\t\t")
                    .Append(ci.EnglishName
                        .Replace('-', '_')
                        .Replace(' ', '_')
                        .Replace('(', '_')
                        .Replace(')', '_')
                        .Replace('[', '_')
                        .Replace(']', '_')
                        .Replace('ʼ', '_')
                        .Replace('’', '_')
                        .Replace(',', '_')
                        .Replace('&', '_')
                        .Replace(".", "")
                    )
                    .Append(",")
                    .AppendLine().AppendLine();
            }


            sb.AppendLine("}");

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Directory.Exists(dir))
            {
                File.WriteAllText(
                    Path.Combine(dir, "CulturesCodesEnum.cs"),
                    sb.ToString()
                );

                Console.WriteLine("File created!");
            }
        }
    }
}
