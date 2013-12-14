using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quine_McCluskey
{
    public class Numeral
    {
        #region Properties

        public int Number { get; private set; }

        public bool IsFi { get; private set; }

        #endregion

        #region Constructors

        public Numeral(int number, bool isFi = false)
        {
            Number = number;
            IsFi = isFi;
        }

        #endregion
    }
}
