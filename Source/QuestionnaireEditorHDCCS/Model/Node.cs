using GalaSoft.MvvmLight;

namespace QuestionnaireEditorHDCCS.Model
{
    public class Node : ViewModelBase
    {
        public Node(string name)
        {
            this.Name = name;
        }

        private string? _name;
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public object? Tag { get; set; }
    }
}
