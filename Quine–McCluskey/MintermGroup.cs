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

        public IEnumerable<Minterm> Minterms
        {
            get
            {
                return _minterms;
            }
        }

        public int NumberOfOnes { get; private set; }

        #endregion

        #region Constructors

        public MintermGroup(int numberOfOnes)
        {
            NumberOfOnes = numberOfOnes;
        }

        #endregion

        #region Methods

        public void Add(Minterm minterm)
        {
            if (minterm.NumberOfOnes != NumberOfOnes)
                throw new ArgumentException("Invalid number of ones in given minimal.");

            if(!_minterms.Exists(item => BitComparer.Compare(item.Binary, minterm.Binary)))
                _minterms.Add(minterm);
        }

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
