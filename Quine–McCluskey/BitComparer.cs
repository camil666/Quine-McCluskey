using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quine_McCluskey
{
    public static class BitComparer
    {
        #region methods

        public static bool Compare(bool?[] array1, bool?[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; ++i)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }

        #endregion
    }
}
