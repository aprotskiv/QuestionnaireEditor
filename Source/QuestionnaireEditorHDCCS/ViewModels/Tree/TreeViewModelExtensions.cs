using QuestionnaireEditorHDCCS.Model;

namespace QuestionnaireEditorHDCCS.ViewModels.Tree
{
    public static class TreeViewModelExtensions
    {
        public static void AddRecursive(this ITreeViewModel treeViewModel, CompositeNode model, int n, int levels)
        {
            for (int i = 0; i < n; i++)
            {
                var m2 = new CompositeNode(
                    name: model.Name + (char)('A' + i)
                );
                model.Children.Add(m2);
                treeViewModel.Count++;

                if (levels > 0)
                {
                    treeViewModel.AddRecursive(m2, n, levels - 1);
                }
            }
        }
    }
}
