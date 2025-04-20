using AProtskiv.Questionnaires;
using Newtonsoft.Json;
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
    public class QuestionPT : Question, INotifyPropertyChanged
    {
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
                base.Id = value;
            }
        }

        [SelectorStyle(SelectorStyle.ComboBox)]
        [DisplayName("Kind")]
        [JsonIgnore]
        public QuestionKind PT_Kind
        {
            get
            {
                return base.Kind;
            }
            set
            {
                if (base.Kind != value)
                {
                    base.Kind = value;
                    OnPropertyChanged();
                }                
            }
        }

        [DisplayName("Text")]
        [JsonIgnore]
        public string PT_Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    OnPropertyChanged();
                }                
            }
        }

        public override AnswerOption[] AnswerOptions
        {
            get
            {
                return AnswerOptionsWrapper.Options.ToArray();
            }
            set
            {
                AnswerOptionsWrapper.Options.Clear();

                if (value != null)
                {
                    AnswerOptionsWrapper.Options.AddRange(
                        value.Select(x => ParentClassConverter.Convert<AnswerOptionPT>(x))
                    );
                }
            }
        }


        [Browsable(false)]
        [JsonIgnore]
        public AnswerOptionsWrapper AnswerOptionsWrapper { get; set; } = new AnswerOptionsWrapper();


        public override QuestionOption[] QuestionOptions
        {
            get
            {
                return QuestionOptionsWrapper.Options.ToArray();
            }
            set
            {
                QuestionOptionsWrapper.Options.Clear();

                if (value != null)
                {
                    QuestionOptionsWrapper.Options.AddRange(
                        value.Select(x => ParentClassConverter.Convert<QuestionOptionPT>(x))
                    );
                }
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public QuestionOptionsWrapper QuestionOptionsWrapper { get; set; } = new QuestionOptionsWrapper();
    }

    public class QuestionOptionsWrapper
    {
        [Browsable(false)]
        public IEnumerable<Column> OptionsColumns { get; } = new[]
        {            
            new Column(nameof(QuestionOptionPT.PT_Text), "Text", null, "*", 'L'),
            new Column(nameof(QuestionOptionPT.PT_Id), "OptionId", null, "*", 'L'),
        };

        [Category(null)]
        [ColumnsProperty(nameof(OptionsColumns))]
        [HeaderPlacement(HeaderPlacement.Above)]
        [DisplayName(null)]
        [JsonIgnore]
        public List<QuestionOptionPT> Options { get; set; } = new List<QuestionOptionPT> { };
    }

    public class AnswerOptionsWrapper
    {
        [Browsable(false)]
        public IEnumerable<Column> OptionsColumns { get; } = new[]
        {
            new Column(nameof(QuestionOptionPT.PT_Text), "Text", null, "*", 'L'),
            new Column(nameof(QuestionOptionPT.PT_Id), "OptionId", null, "*", 'L'),
        };

        [Category(null)]
        [ColumnsProperty(nameof(OptionsColumns))]
        [HeaderPlacement(HeaderPlacement.Above)]
        [DisplayName(null)]
        [JsonIgnore]
        public List<AnswerOptionPT> Options { get; set; } = new List<AnswerOptionPT> { };
    }
}