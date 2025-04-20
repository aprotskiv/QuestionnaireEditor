using AProtskiv.Questionnaires;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QuestionnaireEditorHDCCS.Controls;
using QuestionnaireEditorHDCCS.Model;
using QuestionnaireEditorHDCCS.Model.QuestionnairesPT;
using QuestionnaireEditorHDCCS.ViewModels.Nodes;
using QuestionnaireEditorHDCCS.ViewModels.Tree;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace QuestionnaireEditorHDCCS.ViewModels
{

    public class QuestionnaireTreeModel : ViewModelBase, ITreeViewModel
    {
        #region ITreeViewModel Properties

        private CompositeNode? Model { get; set; }

        private NodeViewModel? _rootModel;
        public NodeViewModel? RootModel 
        {
            get
            {
                return _rootModel;
            }
            set
            {
                if (_rootModel != value)
                {
                    _rootModel = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(Root));
                }
            }
        }

        public IEnumerable? Root
        {
            get
            {
                yield return RootModel;
            }
        }

        public IEnumerable? Children
        {
            get
            {
                return RootModel?.Children;
            }
        }

        public string? Title { get; set; }

        public int Count { get; set; }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public QuestionnaireTreeModel()
        {
            AddQuestionCommand = new RelayCommand(AddQuestionCommadHandler);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestionCommadHandler, CanRemoveQuestion);
        }

        public void Initialize(QuestionnairePT questionnaire)
        {
            var questionnaireModel = new CompositeNode(BuildRootName(questionnaire))
            {
                Tag = questionnaire
            };

            this.Model = questionnaireModel;

            var questionnaireVM = new RootNodeViewModel(this.Model, parent: null);
            questionnaireVM.DropCopyCreate += QuestionnaireVM_DropCopyCreate;

            questionnaire.PropertyChanged += (object? sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(QuestionnairePT.PT_Name):
                    case nameof(QuestionnairePT.PT_Language):
                        var senderQPT = sender as QuestionnairePT;
                        questionnaireVM.Name = BuildRootName(senderQPT);
                        break;
                }                
            };
            

            foreach (var question in questionnaire.PT_Questions)
            {
                AddQuestionNode(question, parentNode: questionnaireVM);
            }

            this.Title = "TreeListBox (N=" + this.Count + ")";
            this.RootModel = questionnaireVM;
        }

        private string BuildRootName(Questionnaire q)
        {
            return string.Format("{0} [{1}]", q.Name, q.LanguageCode);
        }

        private void QuestionnaireVM_DropCopyCreate(object? sender, DropCopyCreateNodeEventArgs e)
        {
            // e.Copy = new Node();
        }

        #endregion

        #region Bind NodeVM methods

        /// <param name="parentNode">if NULL - the <see cref="RootModel"/> will beused</param>
        private void AddQuestionNode(QuestionPT question, NodeViewModel? parentNode = null)
        {
            var nodeVM = (parentNode ?? RootModel)?.AddChild(tag: question, nodeName: question.Text);
            if (nodeVM != null)
            {
                question.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(QuestionPT.PT_Text) 
                        && s is QuestionPT questionPT)
                    {
                        nodeVM.Name = questionPT.PT_Text;
                    }
                };
            }
        }

        #endregion

        #region Commands

        public ICommand AddQuestionCommand { get;  }
        public ICommand RemoveQuestionCommand { get; }

        private void AddQuestionCommadHandler()
        {
            if (RootModel?.Tag is QuestionnairePT qnPT)
            {
                var question = new QuestionPT
                {
                    PT_Id = Guid.NewGuid(),
                    Kind = QuestionKind.NumericalQuestion,
                    PT_Text = "<Please enter question>"
                };

                qnPT.PT_Questions.Add(question);
                AddQuestionNode(question);

                StatusMessage = "Question has been added. Please set its Kind, Text, Question and/or Answer options";
            }
        }


        private bool CanRemoveQuestion()
        {
            if (SelectedNode?.Tag is QuestionPT)
            { 
                return true;
            }

            return false;
        }

        private void RemoveQuestionCommadHandler()
        {
            var model = SelectedNode;

            var q1 = SelecteQuestion;
            if (q1 != null &&
                        MessageBoxResult.Yes == MessageBox.Show(
                            $"Do you want to remove question:\n\n'{q1.Text}'\n\n?", 
                            "Remove Question",
                            MessageBoxButton.YesNo,                    
                            MessageBoxImage.Question
                        )
                )
            {
                

                var questionnaireVM = (RootNodeViewModel?)this.RootModel;
                if (questionnaireVM != null && questionnaireVM.Tag is QuestionnairePT qnPT && model != null)
                {
                    qnPT.PT_Questions.Remove(q1);
                    questionnaireVM.Children.Remove(model);

                    StatusMessage = "Question has been removed.";
                }
            }
        }


        #endregion



        public void Select(int count)
        {
            var children = this.RootModel?.Children as IList<NodeViewModel>;
            for (int i = 0; i < count; i++)
            {
                children[i].IsSelected = true;
            }
        }

        private NodeViewModel? _selectedNode;
        public NodeViewModel? SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                _selectedNode = value;
                RaisePropertyChanged();

                SelecteQuestion = SelectedNode?.Tag as QuestionPT;
                
            }
        }

        private QuestionPT? _selecteQuestion;
        public QuestionPT? SelecteQuestion
        {
            get
            {
                return _selecteQuestion;
            }
            set
            {
                if (_selecteQuestion != value)
                {
                    var oldQuestion = _selecteQuestion;
                    // unsubscribe from old
                    if (oldQuestion != null)
                    {
                        oldQuestion.PropertyChanged -= SelecteQuestion_PropertyChanged;
                    }                    

                    _selecteQuestion = value;

                    // subscribe to new 
                    if (_selecteQuestion != null)
                    {
                        _selecteQuestion.PropertyChanged += SelecteQuestion_PropertyChanged;
                    }

                    RaisePropertyChanged(nameof(SelecteQuestion));
                    RaisePropertyChanged(nameof(SelecteQuestion_QuestionOptionsVisibility));
                    RaisePropertyChanged(nameof(SelecteQuestion_AnswerOptionsVisibility));
                }                
            }
        }

        private void SelecteQuestion_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(QuestionPT.PT_Kind))
            {
                RaisePropertyChanged(nameof(SelecteQuestion_QuestionOptionsVisibility));
                RaisePropertyChanged(nameof(SelecteQuestion_AnswerOptionsVisibility));
            }
        }

        public Visibility SelecteQuestion_AnswerOptionsVisibility
        {
            get
            {
                return GetOptionsVisibility(SelecteQuestion, OptionsKind.AnswersOptions);
            }
        }

        public Visibility SelecteQuestion_QuestionOptionsVisibility
        {
            get
            {
                return GetOptionsVisibility(SelecteQuestion, OptionsKind.QuestionOptions);
            }
        }

        private Visibility GetOptionsVisibility(QuestionPT? questionPT, OptionsKind optionsKind)
        {
            if (questionPT == null)
            {
                // no question (both AnswersOptions & QuestionOptions must be collapsed)
                return Visibility.Collapsed;
            }

            var questionKind = questionPT.PT_Kind;

            switch (optionsKind)
            {
                case OptionsKind.AnswersOptions:
                    {
                        switch (questionKind)
                        {
                            case QuestionKind.NumericalQuestion:
                            case QuestionKind.SingleEntryQuestion:
                            case QuestionKind.MultipleEntryQuestion:
                            case QuestionKind.FreeResponseQuestion:

                                return Visibility.Collapsed;

                            case QuestionKind.MultipleChoiceQuestion:
                            case QuestionKind.MultipleSelectQuestion:
                            case QuestionKind.RankingQuestion:
                            case QuestionKind.MatchingMatrixQuestion:
                            case QuestionKind.DragAndDropQuestion:
                                return Visibility.Visible;
                        }
                    }
                    break;

                case OptionsKind.QuestionOptions:
                    {
                        switch (questionKind)
                        {
                            // no question options
                            case QuestionKind.NumericalQuestion:
                            case QuestionKind.MultipleChoiceQuestion:
                            case QuestionKind.MultipleSelectQuestion:
                            case QuestionKind.RankingQuestion:
                            case QuestionKind.SingleEntryQuestion:
                            case QuestionKind.MultipleEntryQuestion:
                            case QuestionKind.FreeResponseQuestion:
                            case QuestionKind.DragAndDropQuestion:
                                return Visibility.Collapsed;

                            // has question options                                
                            case QuestionKind.MatchingMatrixQuestion:
                                return Visibility.Visible;
                        }
                    }
                    break;
            }

            return Visibility.Visible;
        }
    }
}
