using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quine_McCluskey
{
    public class MintermCollection
    {
        #region Fields

        private List<MintermGroup> _mintermGroups = new List<MintermGroup>();
        private List<Minterm> _primeImplicants;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified minterm to the collection.
        /// </summary>
        /// <param name="minterm">The minterm.</param>
        public void Add(Minterm minterm)
        {
            MintermGroup selectedGroup = _mintermGroups.FirstOrDefault(group => group.NumberOfOnes == minterm.NumberOfOnes);
            if (selectedGroup == null)
            {
                selectedGroup = new MintermGroup(minterm.NumberOfOnes);
                _mintermGroups.Add(selectedGroup);
            }

            selectedGroup.Add(minterm);
        }

        /// <summary>
        /// Adds the specified group to the collection.
        /// </summary>
        /// <param name="group">The group.</param>
        public void Add(MintermGroup group)
        {
            // todo: check if group with same number of ones already exists and merge if nessesary
            _mintermGroups.Add(group);
        }

        /// <summary>
        /// Gets the prime implicants.
        /// </summary>
        /// <returns>The prime implicants</returns>
        public IEnumerable<Minterm> GetPrimeImplicants()
        {
            if (_primeImplicants == null)
            {
                _primeImplicants = new List<Minterm>();
                Combine(_primeImplicants);
            }

            return _primeImplicants;
        }

        private void Combine(IList<Minterm> primeImplicants)
        {
            var newCollection = new MintermCollection();
            bool wasCombined = false;

            for (int i = 0; i < _mintermGroups.Count - 1; ++i)
            {
                var newGroup = _mintermGroups[i].Combine(_mintermGroups[i + 1]);
                if (newGroup != null)
                {
                    newCollection.Add(newGroup);
                    wasCombined = true;
                }

                foreach (var minterm in _mintermGroups[i].Minterms.Where(item => !item.WasCombined))
                    primeImplicants.Add(minterm);
            }

            // the last group
            foreach (var minterm in _mintermGroups[_mintermGroups.Count - 1].Minterms.Where(item => !item.WasCombined))
                primeImplicants.Add(minterm);

            if (wasCombined)
                newCollection.Combine(primeImplicants);
        }

        #endregion
    }
}
