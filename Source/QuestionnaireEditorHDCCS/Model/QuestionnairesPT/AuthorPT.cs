using AProtskiv.Questionnaires;
using Newtonsoft.Json;
using PropertyTools.DataAnnotations;

namespace QuestionnaireEditorHDCCS.Model.QuestionnairesPT
{
    public class AuthorPT : Author
    {
        public AuthorPT()
        {
        }

        public AuthorPT(string name, string contact) : base(name, contact)
        {
        }

        [DisplayName("Name")]
        [Width(200)]
        [JsonIgnore]
        public string PT_Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        [DisplayName("Contact")]
        [Width(200)]
        [JsonIgnore]
        public string PT_Contact
        {
            get
            {
                return base.Contact;
            }
            set
            {
                base.Contact = value;
            }
        }
    }

    
}