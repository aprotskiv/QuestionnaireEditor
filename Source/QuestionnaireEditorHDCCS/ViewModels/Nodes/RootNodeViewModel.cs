using QuestionnaireEditorHDCCS.Model;

namespace QuestionnaireEditorHDCCS.ViewModels.Nodes
{
    public interface IRootNodeViewModel
    {
        event EventHandler<DropCopyCreateNodeEventArgs> DropCopyCreate;

        bool TryDropCopyCreate(Node source, out Node? target);
    }

    public class RootNodeViewModel : NodeViewModel, IRootNodeViewModel
    {
        public RootNodeViewModel(Node Node, NodeViewModel parent) : base(Node, parent)
        {
        }

        #region IRootNodeViewModel

        public event EventHandler<DropCopyCreateNodeEventArgs>? DropCopyCreate;

        public bool TryDropCopyCreate(Node source, out Node? target)
        {
            var args = new DropCopyCreateNodeEventArgs(source);

            DropCopyCreate?.Invoke(this, args);

            target = args.Copy;
            return target != null;
        }

        #endregion
    }
}
