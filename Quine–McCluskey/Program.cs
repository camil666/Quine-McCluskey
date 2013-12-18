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

            //var numbers = new int[] { 0, 1, 2, 4, 6, 11, 12 };
            //var fis = new int[] { 3, 9 };

            //var numbers = new int[] { 0, 4, 5, 6, 11, 15 };
            //var fis = new int[] { 2, 7, 9, 12 };

            var numbers = new int[] { 0, 1, 2, 4, 9, 12, 13, 19, 24, 27 };
            var fis = new int[] { 3, 6, 10, 11, 14, 18, 23, 26, 31 };

            var allNumbers = numbers.Concat(fis);

            double temp = allNumbers.Max();

            int bitCount = 0;

            while (temp > 1)
            {
                ++bitCount;
                temp /= 2.0;
            }

            foreach (int number in allNumbers)
            {
                col.Add(new Minterm(new Numeral(number, fis.Contains(number)), bitCount));
            }

            var primeImplicants = col.GetPrimeImplicants();

            var table = new MintermTable(primeImplicants);

            var results = table.Reduce();
        }
    }
}
