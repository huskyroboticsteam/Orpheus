using HuskyRobotics.Arm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using Scarlet.Utilities;

namespace HuskyRobotics.UI
{
    /// <summary>
    /// Interaction logic for SideArmView.xaml
    /// </summary>
    public partial class ArmSideView : ArmView
    {
        
        public override string ViewName { get => "Arm Side"; }

        protected override (float, float) ProjectCanvas((float, float, float) input)
        {
            return (input.Item1, input.Item2);
        }

        protected override (float, float, float) UnprojectCanvas((float, float) input, (float, float, float) setpoint)
        {
            return (input.Item1, input.Item2, setpoint.Item3);
        }
    }
}
