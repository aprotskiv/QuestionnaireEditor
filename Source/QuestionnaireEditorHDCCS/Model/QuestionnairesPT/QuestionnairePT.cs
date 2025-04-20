using AProtskiv.Questionnaires;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyTools.DataAnnotations;
using QuestionnaireEditorHDCCS.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using BrowsableAttribute = System.ComponentModel.BrowsableAttribute;
using CategoryAttribute = System.ComponentModel.CategoryAttribute;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using ReadOnlyAttribute = System.ComponentModel.ReadOnlyAttribute;

namespace QuestionnaireEditorHDCCS.Model.QuestionnairesPT
{
    public class QuestionnairePT : Questionnaire, INotifyPropertyChanged
    {
        public QuestionnairePT()
        {
            base.LanguageCode = EnumHelper.GetEnumMemberValue(DEFAULT_LANGUAGE);
        }

        const SpecificCulturesCodes DEFAULT_LANGUAGE = SpecificCulturesCodes.English__United_States_;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        [DisplayName("Id")]
        [ReadOnly(true)]
        [JsonIgnore]
        public Guid PT_Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                if (base.Id != value)
                {
                    base.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        [DisplayName("Name")]
        [JsonIgnore]
        public string PT_Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                if (base.Name != value)
                {
                    base.Name = value;
                    OnPropertyChanged();
                }
            }
        }


        [DisplayName("Language")]
        [SelectorStyle(SelectorStyle.ComboBox)]
        [JsonIgnore]
        public SpecificCulturesCodes PT_Language
        {
            get
            {
                if (string.IsNullOrEmpty(base.LanguageCode))
                {
                    base.LanguageCode = EnumHelper.GetEnumMemberValue(DEFAULT_LANGUAGE);
                }

                return EnumHelper.MatchByEnumMemberValue<SpecificCulturesCodes>(base.LanguageCode, DEFAULT_LANGUAGE);
            }
            set
            {
                base.LanguageCode = EnumHelper.GetEnumMemberValue(value);
                OnPropertyChanged();
            }
        }


        [DisplayName("Description")]
        [JsonIgnore]
        public string PT_Description
        {
            get
            {
                return base.Description;
            }
            set
            {
                if (base.Description != value)
                {
                    base.Description = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public override Author[] Authors
        {
            get
            {
                return PT_Authors.ToArray();
            }
            set
            {
                PT_Authors.Clear();

                if (value != null)
                {
                    PT_Authors.AddRange(
                       value.Select(x => ParentClassConverter.Convert<AuthorPT>(x))
                    );
                }
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public IEnumerable<Column> AuthorsColumns { get; } = new[]
        {
            new Column(nameof(AuthorPT.PT_Name), "Name", null, "*", 'L'),
            new Column(nameof(AuthorPT.PT_Contact), "Contact", null, "*", 'L')
        };

        [Category("Additional|")]
        [ColumnsProperty(nameof(AuthorsColumns))]
        [HeaderPlacement(HeaderPlacement.Above)]
        [DisplayName("Authors")]
        [JsonIgnore]
        public List<AuthorPT> PT_Authors { get; set; } = new List<AuthorPT>();

        public override Question[] Questions
        {
            get
            {
                return PT_Questions.ToArray();
            }
            set
            {
                PT_Questions.Clear();

                if (value != null)
                {
                    PT_Questions.AddRange(
                        value.Select(x => ParentClassConverter.Convert<QuestionPT>(x))
                    );
                }
            }
        }

        [Browsable(false)] // in separate node
        [DisplayName("Questions")]
        [JsonIgnore]
        public List<QuestionPT> PT_Questions { get; set; } = new List<QuestionPT> { };

        #endregion
    }
}