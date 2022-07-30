using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMdict
{
    class Conjugation
    {
        public string name;
        public string POS;
        public List<string> baseFormSuffixes = new List<string>();
        public List<ConjugationVariant> tenses;

        public void addBaseFormSuffix(string p)
        {
            if (!baseFormSuffixes.Contains(p))
            {
                baseFormSuffixes.Add(p);
            }
        }
        public void addTenses(List<ConjugationVariant> newTenses)
        {
            foreach (ConjugationVariant tense in newTenses)
            {
                if (!tenses.Any((t) => t.Suffix == tense.Suffix))
                {
                    tenses.Add(tense);
                }
            }
        }
    }
}
