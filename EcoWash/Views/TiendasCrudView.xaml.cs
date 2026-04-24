using EcoWash.ViewModels;
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

namespace EcoWash.Views
{
    /// <summary>
    /// Lógica de interacción para TiendasCrudView.xaml
    /// </summary>
    public partial class TiendasCrudView : UserControl
    {
        public TiendasCrudView()
        {
            InitializeComponent();
            DataContext = new TiendasCrudViewModel();
        }
    }
}
