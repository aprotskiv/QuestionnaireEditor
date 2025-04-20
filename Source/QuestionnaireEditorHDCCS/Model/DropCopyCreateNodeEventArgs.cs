namespace QuestionnaireEditorHDCCS.Model
{
    public class DropCopyCreateNodeEventArgs : NodeEventArgs
    {
        public DropCopyCreateNodeEventArgs(Node source) : base(source)
        {
        }

        public Node? Copy { get; set; }
    }
}
