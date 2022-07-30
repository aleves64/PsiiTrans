using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Script.Serialization;
using System.Collections;
using System.IO;

namespace JMdict
{
    class Conjugator
    {
        Dictionary<string, Conjugation> conjugations;
        Dictionary<string, ConjugationTrie> index;
        public static HashSet<string> knownPOS;

        public void load()
        {
            conjugations = new Dictionary<string, Conjugation>();
            knownPOS = new HashSet<string>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            string jsonRaw = File.ReadAllText("data/Conjugations.txt");
            IList data = (IList)json.DeserializeObject(jsonRaw);
            foreach (var it in data)
            {
                IDictionary conjJson = (IDictionary)it;
                List<ConjugationVariant> tenses = new List<ConjugationVariant>();
                var jsonTenses = (IList)conjJson["Tenses"];
                foreach (var form in jsonTenses)
                {
                    IDictionary formJson = (IDictionary)form;
                    ConjugationVariant conjVar = new ConjugationVariant
                    {
                        Formal = (bool)formJson["Formal"],
                        Negative = (bool)formJson["Negative"],
                        Suffix = (string)formJson["Suffix"],
                        Tense = (string)formJson["Tense"],
                        NextType = (string)formJson["Next Type"] ?? ((string)formJson["Tense"] == "Te-form" ? "te-form" : null),
                        Ignore = formJson["Ignore"] != null
                    };
                    tenses.Add(conjVar);
                }
                var conj = new Conjugation
                {
                    name = (string)conjJson["Name"],
                    POS = (string)conjJson["Part of Speech"],
                    tenses = tenses
                };
                conj.addBaseFormSuffix(tenses[0].Suffix);
                Conjugation old;
                if (conjugations.TryGetValue(conj.name, out old))
                {
                    old.addTenses(conj.tenses);
                    old.addBaseFormSuffix(tenses[0].Suffix);
                } else
                {
                    knownPOS.Add(conj.name);
                    conjugations.Add(conj.name, conj);
                }
            }
            rebuildIndex();
        }

        public string getBaseFormSuffix(string pos)
        {
            Conjugation conj;
            if (conjugations.TryGetValue(pos, out conj))
            {
                return conj.baseFormSuffixes[0];
            }
            return "";
        }

        private void rebuildIndex()
        {
            index = new Dictionary<string, ConjugationTrie>();
            foreach (Conjugation conj in conjugations.Values)
            {
                ConjugationTrie trie = new ConjugationTrie();
                foreach (ConjugationVariant form in conj.tenses)
                {
                    trie.addForm(form, conjugations);
                }
                index[conj.name] = trie;
            }
        }
        
        public IEnumerable<ConjugationState> findMatching(List<string> POS, string text, int position)
        {
            List<ConjugationState> results = new List<ConjugationState>();
            List<Tuple<ConjugationTrie, ConjugationState, string>> cur = new List<Tuple<ConjugationTrie, ConjugationState, string>>();

            foreach (string pos in POS)
            {
                ConjugationTrie trie;
                if (index.TryGetValue(pos, out trie))
                {
                    cur.Add(Tuple.Create<ConjugationTrie, ConjugationState, string>(trie, null, pos));
                }
            }

            int offset = 0;
            bool hasEmptySuf = false;

            while (cur.Count > 0 && position + offset <= text.Length)
            {
                List<Tuple<ConjugationTrie, ConjugationState, string>> added = new List<Tuple<ConjugationTrie, ConjugationState, string>>();
                List<Tuple<ConjugationTrie, ConjugationState, string>> added2 = new List<Tuple<ConjugationTrie, ConjugationState, string>>();
                List<Tuple<ConjugationTrie, ConjugationState, string>> next = new List<Tuple<ConjugationTrie, ConjugationState, string>>();
                HashSet<string> addedPOS = new HashSet<string>();

                foreach (var it in cur)
                {
                    foreach (ConjugationVariant link in it.Item1.linkForms)
                    {
                        if (addedPOS.Add(link.NextType))
                        {
                            ConjugationState linked = new ConjugationState("", link, it.Item2 == null ? null : it.Item2.tense);
                            added.Add(Tuple.Create(index[link.NextType], linked, it.Item3));
                        }
                    }
                }
                foreach (var it in added)
                {
                    foreach (var link in it.Item1.linkForms)
                    {
                        if (addedPOS.Add(link.NextType))
                        {
                            ConjugationState linked = new ConjugationState("", link, it.Item2 == null ? null : it.Item2.tense);
                            added2.Add(Tuple.Create(index[link.NextType], linked, it.Item3));
                        }
                    }
                }
                ConjugationState newState = null;
                char c;
                if (position + offset < text.Length)
                {
                    c = text[position + offset];
                }
                else
                {
                    c = '\0';
                }
                var tmp = cur.Concat(added).Concat(added2);
                foreach (var it in tmp)
                {
                    foreach (var form in it.Item1.forms)
                    {
                        if (newState == null)
                        {
                            newState = new ConjugationState(text.Substring(position, offset), form, it.Item2 == null ? null : it.Item2.tense);
                        }
                        else
                        {
                            newState.updateTense(form.Tense);
                        }
                        newState.addPOS(it.Item3);
                    }
                    ConjugationTrie nextTrie;
                    if (it.Item1.children.TryGetValue(c, out nextTrie))
                    {
                        next.Add(Tuple.Create(nextTrie, it.Item2, it.Item3));
                    }
                }
                if (newState != null)
                {
                    if (!newState.suffix.EndsWith("てい") && !newState.suffix.EndsWith("でい")
                        && !newState.suffix.EndsWith("るた"))
                    {
                        if (newState.suffix == "")
                        {
                            hasEmptySuf = true;
                        }
                        results.Add(newState);
                    }
                }
                cur = next;
                offset += 1;
            }
            if (!hasEmptySuf && (POS == null || !knownPOS.IsSupersetOf(POS)))
            {
                ConjugationState state = new ConjugationState("");
                results.Add(state);
            }
            return results;
        }

        public string getStem(string baseForm, List<string> POS)
        {
            foreach (string pos in POS)
            {
                Conjugation conj;
                if (conjugations.TryGetValue(pos, out conj))
                {
                    foreach (string suffix in conj.baseFormSuffixes)
                    {
                        if (baseForm.EndsWith(suffix))
                        {
                            string res = baseForm.Substring(0, baseForm.Length - suffix.Length);
                            return res;
                        }
                    }
                    return baseForm;
                }
            }
            return baseForm;
        }
    }
}
