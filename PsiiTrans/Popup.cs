using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JMdict;

namespace PsiiTrans
{
    partial class Popup : FormBase
    {
        protected override bool ShowWithoutActivation { get { return true; } }
        private Font entryFont = new Font("Meiryo", 14.25f);
        private Font senseFont = new Font("Meiryo", 11.25f);

        public Popup()
        {
            InitializeComponent();

            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.Manual;
        }

        public void SetResults(List<LookupResult> res)
        {
            contentPanel.Controls.Clear();

            int nextY = 0;
            foreach (LookupResult lookupResult in res)
            {
                JMdictEntry entry = lookupResult.entry;
                Label entryLabel = new Label();
                Label readingLabel = new Label();
                Label conjugationLabel = new Label();

                entryLabel.AutoSize = true;
                entryLabel.Font = entryFont;
                entryLabel.ForeColor = Color.Firebrick;

                readingLabel.AutoSize = true;
                readingLabel.Font = entryFont;
                readingLabel.ForeColor = Color.DimGray;

                conjugationLabel.AutoSize = true;
                conjugationLabel.Font = senseFont;
                conjugationLabel.ForeColor = Color.IndianRed;

                if (entry.kanji.Count > 0)
                {
                    entryLabel.Text = String.Join(", ", entry.kanji);
                    readingLabel.Text = String.Join(", ", entry.kana);

                    Point entryLoc = entryLabel.Location;
                    entryLoc.Y = nextY;
                    entryLabel.Location = entryLoc;
                    contentPanel.Controls.Add(entryLabel);

                    Point readingLoc = readingLabel.Location;
                    readingLoc.Y = nextY;
                    readingLoc.X = entryLoc.X + entryLabel.Width;
                    readingLabel.Location = readingLoc;
                    contentPanel.Controls.Add(readingLabel);
                } else
                {
                    readingLabel.Text = String.Join(", ", entry.kana);

                    Point readingLoc = readingLabel.Location;
                    readingLoc.Y = nextY;
                    readingLabel.Location = readingLoc;
                    contentPanel.Controls.Add(readingLabel);
                }
                ConjugationState finalTense = lookupResult.tenses.Last();
                string tense = finalTense.tense;
                if (tense != "")
                {
                    conjugationLabel.Text = tense;
                    if (finalTense.form.Formal)
                    {
                        conjugationLabel.Text += " formal";
                    }
                    if (finalTense.form.Negative)
                    {
                        conjugationLabel.Text += " negative";
                    }
                    
                    Point conjugationLoc = conjugationLabel.Location;
                    conjugationLoc.X = readingLabel.Location.X + readingLabel.Width;
                    conjugationLoc.Y = nextY;
                    conjugationLabel.Location = conjugationLoc;
                    contentPanel.Controls.Add(conjugationLabel);
                }

                nextY += readingLabel.Height;
                int senseCounter = 1;
                foreach (JMdictSense sense in entry.sense)
                {
                    Label senseLabel = new Label();
                    senseLabel.AutoSize = true;
                    senseLabel.Font = senseFont;

                    string prefix = "";
                    if (entry.sense.Count > 1)
                    {
                        prefix = String.Format("{0}. ", senseCounter++);
                    }
                    string text = prefix + String.Join("; ", sense.glossary);
                    int width = TextRenderer.MeasureText(text, senseFont).Width;
                    while (width > this.Width)
                    {
                        int newlineIndex = text.Length - 1;
                        int subwidth = width;
                        while (subwidth > this.Width)
                        {
                            newlineIndex = text.LastIndexOf(" ", newlineIndex - 1);
                            subwidth = TextRenderer.MeasureText(text.Substring(0, newlineIndex + 1), senseFont).Width;
                        }
                        text = text.Substring(0, newlineIndex) + "\n" + text.Substring(newlineIndex + 1);
                        width = TextRenderer.MeasureText(text, senseFont).Width;
                    }

                    senseLabel.Text = text;
                    Point senseLoc = senseLabel.Location;
                    senseLoc.Y = nextY;
                    senseLabel.Location = senseLoc;
                    contentPanel.Controls.Add(senseLabel);

                    nextY += senseLabel.Height;
                }
            }
            this.Height = nextY;
        }
    }
}
