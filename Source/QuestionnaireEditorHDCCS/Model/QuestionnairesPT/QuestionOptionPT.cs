using AProtskiv.Questionnaires;
using Newtonsoft.Json;
using PropertyTools.DataAnnotations;

namespace QuestionnaireEditorHDCCS.Model.QuestionnairesPT
{
    public class QuestionOptionPT : QuestionOption
    {
        [DisplayName("Text")]
        [Width(200)]
        [SortIndex(0)]
        [JsonIgnore]
        public string PT_Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DisplayName("OptionId")]
        [SortIndex(1)]
        [JsonIgnore]
        public string PT_Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }
    }    
}