using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for GamePadView.xaml
    /// </summary>
    public partial class GamePadView : UserControl
    {
        public GamePadView()
        {
            InitializeComponent();
            Thread GamePadState = new Thread(UpdateGamePadView);
            GamePadState.IsBackground = true;
            GamePadState.Start();
        }

        private void UpdateGamePadView()
        {
            while(true)
            {

            }
        }
    }
}
