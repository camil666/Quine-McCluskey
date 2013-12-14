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

        /// <summary>
        /// Gets the binary representation of the minterm.
        /// </summary>
        /// <value>
        /// The binary representation of the minterm.
        /// </value>
        public bool?[] Binary { get; private set; }

        public ICollection<Numeral> Numerals { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this minterm was combined with another minterm.
        /// </summary>
        /// <value>
        ///   <c>true</c> if minterm was combined; otherwise, <c>false</c>.
        /// </value>
        public bool WasCombined { get; private set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Minterm"/> class.
        /// </summary>
        /// <param name="numeral">The numeral reprezentation.</param>
        /// <param name="numberOfBits">The number of bits needed in binary representation.</param>
        public Minterm(Numeral numeral, int numberOfBits)
        {
            Numerals = new List<Numeral>();
            Numerals.Add(numeral);
            var bitArray = new BitArray(new[] { numeral.Number });
            Binary = new bool?[numberOfBits];

            for (int i = 0; i < numberOfBits; ++i)
                Binary[i] = bitArray[i];
        }

        private Minterm(bool?[] bits)
        {
            Numerals = new List<Numeral>();
            Binary = bits;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Combines the specified minterm with another minterm.
        /// </summary>
        /// <param name="minterm">The minterm.</param>
        /// <returns>Combined minterm.</returns>
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

            WasCombined = true;
            minterm.WasCombined = true;

            var newMinterm = new Minterm(array);

            foreach (Numeral numeral in Numerals)
                newMinterm.Numerals.Add(numeral);

            foreach (Numeral numeral in minterm.Numerals)
            {
                if (!newMinterm.Numerals.Contains(numeral))
                    newMinterm.Numerals.Add(numeral);
            }

            return newMinterm;
        }

        #endregion
    }
}
