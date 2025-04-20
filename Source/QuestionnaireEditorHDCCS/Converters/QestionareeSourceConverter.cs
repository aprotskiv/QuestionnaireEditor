using QuestionnaireEditorHDCCS.Model.QuestionnairesPT;
using System.Globalization;
using System.Windows.Data;

namespace QuestionnaireEditorHDCCS.Converters
{
    public class QestionareeSourceConverter : IValueConverter
    {
        public string? QestionareeImage { get; set; }
        
        public string? QuestionImage { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuestionnairePT)
            { 
                return QestionareeImage;
            }
            else if (value is QuestionPT)
            {
                return QuestionImage;
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
