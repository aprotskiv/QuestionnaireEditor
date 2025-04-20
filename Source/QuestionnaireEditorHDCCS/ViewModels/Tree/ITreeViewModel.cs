using System.Collections;
using QuestionnaireEditorHDCCS.ViewModels.Nodes;

namespace QuestionnaireEditorHDCCS.ViewModels.Tree
{
    public interface ITreeViewModel
    {
        IEnumerable? Children { get; }

        int Count { get; set; }

        IEnumerable? Root { get; }

        NodeViewModel? RootModel { get; set; }

        NodeViewModel? SelectedNode { get; set; }

        string? Title { get; set; }

        void Select(int count);

        string StatusMessage { get; set; }
    }
}
