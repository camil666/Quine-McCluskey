using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quine_McCluskey
{
    public class MintermGroup
    {
        #region Fields

        private List<Minterm> _minterms = new List<Minterm>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the minterms.
        /// </summary>
        /// <value>
        /// The minterms.
        /// </value>
        public IEnumerable<Minterm> Minterms
        {
            get
            {
                return _minterms;
            }
        }

        /// <summary>
        /// Gets the number of ones (1) in minterms.
        /// </summary>
        /// <value>
        /// The number of ones.
        /// </value>
        public int NumberOfOnes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MintermGroup"/> class.
        /// </summary>
        /// <param name="numberOfOnes">The number of ones.</param>
        public MintermGroup(int numberOfOnes)
        {
            NumberOfOnes = numberOfOnes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified minterm to the group.
        /// </summary>
        /// <param name="minterm">The minterm.</param>
        /// <exception cref="System.ArgumentException">Invalid number of ones in given minimal.</exception>
        public void Add(Minterm minterm)
        {
            if (minterm.NumberOfOnes != NumberOfOnes)
                throw new ArgumentException("Invalid number of ones in given minimal.");

            if(!_minterms.Exists(item => BitComparer.Equals(item.Binary, minterm.Binary)))
                _minterms.Add(minterm);
        }

        /// <summary>
        /// Combines the specified group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>Combined group.</returns>
        public MintermGroup Combine(MintermGroup group)
        {
            var newGroup = new MintermGroup(NumberOfOnes);

            foreach (var minterm in Minterms)
            {
                foreach (var other in group.Minterms)
                {
                    Minterm combined = minterm.Combine(other);
                    if (combined != null)
                        newGroup.Add(combined);
                }
            }

            return newGroup.Minterms.Count() > 0 ? newGroup : null;
        }

        #endregion
    }
}
