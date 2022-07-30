using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMdict
{
    class ConjugationState
    {
        public readonly string suffix;
        public readonly ConjugationVariant form;
        public string tense;
        public readonly ISet<string> POS;
        private bool hasOwnTense;

        public ConjugationState(string suffix, ConjugationVariant form = null, string prevTense = null)
        {
            this.suffix = suffix;
            this.form = form;
            string tense = null;

            if (form != null && !string.IsNullOrEmpty(form.Tense) && form.Tense != "Remove" && form.Tense != "Stem")
            {
                tense = form.Tense;
                hasOwnTense = true;
            }
            else
            {
                hasOwnTense = false;
            }
            if (!string.IsNullOrEmpty(prevTense) && prevTense != "Remove" && prevTense != "Stem")
            {
                if (tense == null)
                {
                    tense = prevTense;
                }
                else
                {
                    tense = prevTense + " → " + tense;
                }
            }
            this.tense = tense;
            this.POS = new HashSet<string>();
        }

        public void updateTense(string t)
        {
            if (!hasOwnTense && !string.IsNullOrEmpty(t) && t != "Remove" && t != "Stem" && form.Tense != "Te-form")
            {
                if (tense == null)
                {
                    tense = t;
                }
                else
                {
                    tense = tense + " → " + t;
                }
                hasOwnTense = true;
            }
        }

        public int length
        {
            get
            {
                return suffix.Length;
            }
        }

        public void addPOS(string aPOS)
        {
            POS.Add(aPOS);
        }
    }
}
