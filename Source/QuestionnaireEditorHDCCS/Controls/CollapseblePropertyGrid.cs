using PropertyTools.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuestionnaireEditorHDCCS.Controls
{
    public class CollapseblePropertyGrid : PropertyGrid
    {
        public CollapseblePropertyGrid() : base()
        {
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SelectedObjectProperty)
            {
                if (e.NewValue == null)
                {
                    this.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
