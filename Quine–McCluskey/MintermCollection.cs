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

        #endregion

        #region Methods

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

        public void Add(MintermGroup group)
        {
            // todo: check if group with same number of ones already exists and merge if nessesary
            _mintermGroups.Add(group);
        }

        public MintermCollection Combine()
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
            }

            return wasCombined ? newCollection.Combine() : this;
        }

        #endregion
    }
}
