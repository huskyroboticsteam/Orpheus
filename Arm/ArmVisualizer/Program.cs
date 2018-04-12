using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HuskyRobotics.Arm;

namespace HuskyRobotics.Arm.Visualizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            const double DegToRad = Math.PI / 180.0;
            var a = new Armature((0, 0, Math.PI / 2, Math.PI / 2, 0, 0, 6.8),
                                 (0, 0, -76 * DegToRad, 100 * DegToRad, 0, 0, 28.0),
                                 (0, 0, -168.51 * DegToRad, -10 * DegToRad, 0, 0, 28.0),
                                 (0, 12.75, -Math.PI / 2, Math.PI / 2));
            var Vis = new ArmVisualizer(a);
            Vis.Show();
            Application.Run(Vis);
        }
    }
}
