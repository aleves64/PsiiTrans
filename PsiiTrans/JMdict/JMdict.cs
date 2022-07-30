using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JMdict
{
    class JMdict
    {
        public Dictionary<string, JMdictMatch> dictionary;
        private Dictionary<string, string> xmlEntities;
        private Conjugator conjugator;

        public void load(Conjugator conjugator)
        {
            this.conjugator = conjugator;
            dictionary = new Dictionary<string, JMdictMatch>();
            xmlEntities = new Dictionary<string, string>();

            using (XmlTextReader xml = new XmlTextReader("data/JMdict_e"))
            {
                xml.DtdProcessing = DtdProcessing.Parse;
                xml.WhitespaceHandling = WhitespaceHandling.None;
                xml.EntityHandling = EntityHandling.ExpandCharEntities;
                while (xml.Read())
                {
                    switch (xml.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xml.Name == "entry")
                            {
                                readEntry(xml);
                            }
                            break;
                    }
                }
            }
        }

        public List<LookupResult> lookup(string text)
        {
            List<LookupResult> res = new List<LookupResult>();
            for (int i = 1; i <= text.Length; ++i)
            {
                string sub = text.Substring(0, i);
                JMdictMatch match = stemLookup(sub);
                if (match != null)
                {
                    var matchingConjugations = conjugator.findMatching(match.getAllPOS(), text, i);
                    foreach (JMdictEntry entry in match.entries)
                    {
                        List<ConjugationState> tenses = new List<ConjugationState>();
                        foreach (ConjugationState conjugation in matchingConjugations)
                        {
                            if (conjugation.POS.Intersect(entry.POS).Count() > 0)
                            {
                                tenses.Add(conjugation);
                            }
                        }
                        if (tenses.Count > 0)
                        {
                            LookupResult newResult = new LookupResult
                            {
                                entry = entry,
                                tenses = tenses
                            };
                            res.Add(newResult);
                        }
                    }
                }
            }
            return res;
        }
 
        public JMdictMatch stemLookup(string key)
        {
            JMdictMatch res;
            if (dictionary.TryGetValue(key, out res))
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        private void readEntry(XmlTextReader xml)
        {
            JMdictEntry entry = new JMdictEntry();
            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "entry") break;
                if (xml.NodeType == XmlNodeType.Element)
                {
                    switch (xml.Name)
                    {
                        case "k_ele":
                            readKanji(entry, xml);
                            break;
                        case "r_ele":
                            readReading(entry, xml);
                            break;
                        case "sense":
                            readSense(entry, xml);
                            break;
                    }
                }
            }
            addToDictionary(entry);
        }

        private void readKanji(JMdictEntry entry, XmlTextReader xml)
        {
            string text = null;

            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "k_ele") break;
                if (xml.NodeType == XmlNodeType.Element)
                {
                    string name = xml.Name;
                    switch (name)
                    {
                        case "keb":
                            text = xml.ReadString();
                            break;
                        case "ke_inf":
                            break;
                        case "ke_pri":
                            break;
                    }
                }
            }
            if (text != null)
            {
                entry.kanji.Add(text);
            }
        }
        private void readReading(JMdictEntry entry, XmlTextReader xml)
        {
            string text = null;
            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "r_ele") break;
                if (xml.NodeType == XmlNodeType.Element)
                {
                    string name = xml.Name;
                    switch (name)
                    {
                        case "reb":
                            text = xml.ReadString();
                            break;
                        case "re_nokanji":
                            break;
                        case "re_restr":
                            break;
                        case "re_inf":
                            break;
                        case "re_prio":
                            break;
                    }
                }
            }
            if (text != null)
            {
                entry.kana.Add(text);
            }
        }
        private void readSense(JMdictEntry entry, XmlTextReader xml)
        {
            JMdictSense sense = new JMdictSense();

            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.EndElement && xml.Name == "sense") break;
                if (xml.NodeType == XmlNodeType.Element)
                {
                    string name = xml.Name;
                    switch (name)
                    {
                        case "gloss":
                            string value = xml.ReadString();
                            sense.glossary.Add(value);
                            break;
                        case "pos":
                            string POS = fromEntity(xml);
                            entry.POS.Add(POS);
                            break;
                        case "field":
                        case "misc":
                        case "dial":
                            string v = fromEntity(xml);
                            sense.misc.Add(v);
                            break;
                    }
                }
            }
            entry.sense.Add(sense);
        }
        private void addToDictionary(string key, JMdictEntry entry)
        {
            JMdictMatch entries;
            if (dictionary.TryGetValue(key, out entries))
            {
                entries.addEntry(entry);
            }
            else
            {
                entries = new JMdictMatch(key);
                entries.addEntry(entry);
                dictionary.Add(key, entries);
            }
        }
        private void addToDictionary(JMdictEntry entry)
        {
            foreach (string key in entry.kanji)
            {
                string stem = conjugator.getStem(key, entry.POS);
                addToDictionary(stem, entry);
            }
            foreach (string key in entry.kana)
            {
                string stem = conjugator.getStem(key, entry.POS);
                addToDictionary(stem, entry);
            }
        }
        private string fromEntity(XmlReader xr)
        {
            xr.Read();
            if (xr.NodeType == XmlNodeType.EntityReference)
            {
                string name = xr.Name;
                if (!xmlEntities.ContainsKey(name))
                {
                    xr.ResolveEntity();
                    xmlEntities.Add(name, xr.ReadString());
                }
                return name;
            }
            else
            {
                throw new Exception("Wtf?" + xr.NodeType.ToString() + ": " + xr.Name + ": " + xr.ReadString());
            }
        }
    }
}
