using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMdict
{
    class JMdictMatch
    {
        public readonly List<JMdictEntry> entries = new List<JMdictEntry>();
        public readonly string stem;
        private List<string> allPOS;

        public JMdictMatch(string stem)
        {
            this.stem = stem;
        }

        public List<string> getAllPOS()
        {
            if (allPOS == null)
            {
                HashSet<string> tmp = new HashSet<string>();
                foreach (JMdictEntry ee in entries)
                {
                    tmp.UnionWith(ee.POS);
                }
                allPOS = tmp.ToList();
            }
            return allPOS;
        }
        private void updatePOS()
        {
            allPOS = null;
        }
        public void addEntry(JMdictEntry entry)
        {
            entries.Add(entry);
            updatePOS();
        }
    }
}
