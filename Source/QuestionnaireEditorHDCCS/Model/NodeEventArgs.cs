namespace QuestionnaireEditorHDCCS.Model
{
    public class NodeEventArgs : EventArgs
    {
        public NodeEventArgs(Node source)
        {
            this.Source = source;
        }

        public Node Source { get; }
    }
}
