using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;

namespace MeasureAndCount
{
    /// <summary>
    /// Interaction logic for MeasureAndCountView.xaml
    /// </summary>
    public partial class MeasureAndCountView : Window
    {
        public MeasureAndCountView(List<string> textForTreeView)
        {

            InitializeComponent();
            // display view  
            foreach (var item in textForTreeView)
            {
                treeview.Items.Add(new TreeViewItem { Header = item });
            }
        }
        private void OnCloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

    }
}
