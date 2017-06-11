using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Science_Base
{
	class BaseMain
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello. This is science at base. Check back later!");
            MainWindow Main = new MainWindow();
            Application.EnableVisualStyles();
            Application.Run(Main);
		}
	}
}
