using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMdict
{
    class JMdictEntry
    {
        public List<string> kanji = new List<string>();
        public List<string> kana = new List<string>();
        public List<string> POS = new List<string>();
        public List<JMdictSense> sense = new List<JMdictSense>();
    }
}
