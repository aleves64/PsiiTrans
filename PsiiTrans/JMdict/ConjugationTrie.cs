using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMdict
{
    class ConjugationTrie
    {
        public List<ConjugationVariant> forms = new List<ConjugationVariant>();
        public List<ConjugationVariant> linkForms = new List<ConjugationVariant>();
        public Dictionary<char, ConjugationTrie> children = new Dictionary<char, ConjugationTrie>();

        public void addForm(ConjugationVariant form, Dictionary<string, Conjugation> conjugations)
        {
            ConjugationTrie cur = this;
            if (!form.Ignore)
            {
                foreach (char c in form.Suffix)
                {
                    cur = cur.getOrAddChild(c);
                }
                cur.forms.Add(form);
            }
            if (form.NextType != null)
            {
                string baseSuf = getStem(conjugations, form.Suffix, form.NextType);
                cur = this;
                foreach (char c in baseSuf)
                {
                    cur = cur.getOrAddChild(c);
                }
                cur.linkForms.Add(form);
            }
        }

        private ConjugationTrie getOrAddChild(char c)
        {
            ConjugationTrie res;
            if (!children.TryGetValue(c, out res))
            {
                res = new ConjugationTrie();
                children.Add(c, res);
            }
            return res;
        }

        private static string getStem(Dictionary<string, Conjugation> conjugations, string baseForm, string pos)
        {
            Conjugation conj;
            if (conjugations.TryGetValue(pos, out conj))
            {
                foreach (string suffix in conj.baseFormSuffixes)
                {
                    if (baseForm.EndsWith(suffix))
                    {
                        return baseForm.Substring(0, baseForm.Length - suffix.Length);
                    }
                }
                return baseForm;
            } else
            {
                return baseForm;
            }
        }
    }
}
