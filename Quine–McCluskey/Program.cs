using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quine_McCluskey
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
            var col = new MintermCollection();
            col.Add(new Minterm(1, 4));
            col.Add(new Minterm(2, 4));
            col.Add(new Minterm(3, 4));
            col.Add(new Minterm(7, 4));
            col.Add(new Minterm(9, 4));
            col.Add(new Minterm(10, 4));
            col.Add(new Minterm(11, 4));
            col.Add(new Minterm(13, 4));
            col.Add(new Minterm(15, 4));

            var aaa = col.Combine();
        }
    }
}
