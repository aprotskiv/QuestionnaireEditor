using System.Collections.ObjectModel;

namespace QuestionnaireEditorHDCCS.Model
{
    public class CompositeNode : Node
    {
        public ObservableCollection<Node> Children { get; private set; }

        public CompositeNode(string name) 
            : base(name)
        {
            Children = new ObservableCollection<Node>();
        }
    }
}
