using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quine_McCluskey
{
    public class Minterm
    {
        #region Fields

        private int? _numerOfOnes;

        #endregion

        #region Properties

        public bool?[] Binary { get; private set; }

        public int NumberOfOnes
        {
            get
            {
                if (!_numerOfOnes.HasValue)
                    _numerOfOnes = Binary.Count(item => item.HasValue && item.Value);

                return _numerOfOnes.Value;
            }
        }

        #endregion

        #region Constructors

        public Minterm(int numeral, int numberOfBits)
        {
            var bitArray = new BitArray(new[] { numeral });
            Binary = new bool?[numberOfBits];

            for (int i = 0; i < numberOfBits; ++i)
                Binary[i] = bitArray[i];
        }

        private Minterm(bool?[] bits)
        {
            Binary = bits;
        }

        #endregion

        #region Methods

        public Minterm Combine(Minterm minterm)
        {
            var array = new bool?[Binary.Length];
            int mismatchCount = 0;

            for (int i = 0; i < Binary.Length; ++i)
            {
                if (Binary[i] == minterm.Binary[i])
                {
                    array[i] = Binary[i];
                }
                else
                {
                    array[i] = null;
                    ++mismatchCount;
                }

                if (mismatchCount > 1)
                    return null;
            }

            return new Minterm(array);
        }

        #endregion
    }
}
