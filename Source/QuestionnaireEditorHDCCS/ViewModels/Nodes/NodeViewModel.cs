using GalaSoft.MvvmLight;
using PropertyTools;
using QuestionnaireEditorHDCCS.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace QuestionnaireEditorHDCCS.ViewModels.Nodes
{
    public class NodeViewModel : ViewModelBase, IDragSource, IDropTarget
    {
        public object? Tag
        {
            get
            {
                return Node.Tag;
            }
        }

        public NodeViewModel? Parent { get; private set; }

        public bool HasItems
        {
            get
            {
                return Children.Count > 0;
            }
        }

        #region IDropTarget

        public bool CanDrop(IDragSource node, DropPosition mode, DragDropEffect effect)
        {
            return node is NodeViewModel && (mode == DropPosition.Add || Parent != null);
        }

        public void Drop(IEnumerable<IDragSource> nodes, DropPosition mode, DragDropEffect effect, DragDropKeyStates initialKeyStates)
        {
            foreach (var node in nodes)
            {
                Drop(node, mode, effect == DragDropEffect.Copy);
            }
        }

        public void Drop(IDragSource node, DropPosition mode, bool copy)
        {
            var cvm = node as NodeViewModel;
            if (cvm == null)
                return;

            if (copy)
            {
                var targetNode = cvm.Node;
                if (cvm.GetRoot() is IRootNodeViewModel rootNodeViewModel)
                {
                    if (rootNodeViewModel.TryDropCopyCreate(cvm.Node, out Node? newTarget))
                    {
                        targetNode = newTarget;
                    }
                }

                cvm = new NodeViewModel(targetNode, cvm.Parent);
            }


            switch (mode)
            {
                case DropPosition.Add:
                    Children.Add(cvm);
                    cvm.Parent = this;
                    IsExpanded = true;
                    break;

                case DropPosition.InsertBefore:
                    if (Parent != null)
                    {
                        int index = Parent.Children.IndexOf(this);
                        Parent.Children.Insert(index, cvm);
                        cvm.Parent = Parent;
                    }

                    break;

                case DropPosition.InsertAfter:
                    if (Parent != null)
                    {
                        int index2 = Parent.Children.IndexOf(this);
                        Parent.Children.Insert(index2 + 1, cvm);
                        cvm.Parent = Parent;
                    }
                    break;
            }
        }

        #endregion


        #region IDragSource

        public bool IsDraggable
        {
            get
            {
                return Parent != null;
            }
        }

        public void Detach()
        {
            if (Parent != null)
                Parent.Children.Remove(this);
            Parent = null;
        }

        #endregion

        private NodeViewModel GetRoot()
        {
            return Parent == null
                ? this
                : Parent.GetRoot();
        }

        private Node Node;

        private ObservableCollection<NodeViewModel>? children;

        public ObservableCollection<NodeViewModel> Children
        {
            get
            {
                if (children == null)
                {
                    children = new ObservableCollection<NodeViewModel>();

                    // load children
                    var cc = Node as CompositeNode;
                    if (cc != null)
                    {
                        foreach (var child in cc.Children)
                        {
                            children.Add(new NodeViewModel(child, this));                            
                        }
                    }
                }

                return children;
            }
        }

        public string? Name
        {
            get
            {
                return Node.Name;
            }
            set
            {
                Node.Name = value;
                RaisePropertyChanged();
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (isExpanded == value) return;
                isExpanded = value;

                RaisePropertyChanged();
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value) return;
                isSelected = value;

                RaisePropertyChanged();
            }
        }

        public int Level { get; set; }

        private bool isEditing;
        public bool IsEditing
        {
            get
            {
                return isEditing;
            }
            set
            {
                isEditing = value;
                RaisePropertyChanged();
                Debug.WriteLine(Name + ".IsEditing = " + value);
            }
        }

        public NodeViewModel(Node Node, NodeViewModel? parent)
        {
            this.Node = Node;
            Parent = parent;
            IsExpanded = true;
        }

        public override string? ToString()
        {
            return Name;
        }


        public NodeViewModel? AddChild(object tag, string nodeName = "New node")
        {
            var cn = Node as CompositeNode;
            if (cn == null)
            {
                return null;
            }

            var justGetChildren = this.Children; // prevents duplicate of first node 

            var newChild = new CompositeNode(nodeName) { Tag = tag };
            cn.Children.Add(newChild);

            var vm = new NodeViewModel(newChild, this);
            Children.Add(vm);
            return vm;
        }

        public void ExpandParents()
        {
            if (Parent != null)
            {
                Parent.ExpandParents();
                Parent.IsExpanded = true;
            }
        }

        public void ExpandAll()
        {
            IsExpanded = true;
            foreach (var child in Children)
            {
                child.ExpandAll();
            }
        }
    }
}
