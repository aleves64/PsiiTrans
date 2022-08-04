using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using JMdict;

namespace PsiiTrans
{
    public partial class MainForm : Form
    {
        private const int MAX_TEXT_LENGTH = 200;

        private bool isSelectWindow = false;
        private string selectedExe;
        private int lastTextIndex = -1;

        private Dictionary<long, string> handleToStringDict = new Dictionary<long, string>(); // Stores string output from each text thread
        private Dictionary<int, long> indexToHandleDict = new Dictionary<int, long>(); // Associates each combo box index with a text thread handle

        private OutputCallback Output;
        private ProcessEventHandler Connect;
        private ProcessEventHandler Disconnect;
        private ThreadEventCallback Create;
        private ThreadEventCallback Destroy;

        private JMdict.JMdict edict;
        private Popup resultPopup = new Popup();

        public MainForm()
        {
            InitializeComponent();

            // Init textbox
            outputTextbox.ReadOnly = true;
            outputTextbox.BackColor = SystemColors.Window;
            outputTextbox.MouseMove += WordUnderCursor;

            // Other event handlers
            base.Deactivate += PidFromSelectedWindow;
            ttCombo.SelectedIndexChanged += OnComboSelectionChanged;

            // Init JMdict
            Conjugator conjugator = new Conjugator();
            conjugator.load();
            edict = new JMdict.JMdict();
            edict.load(conjugator);

            // Init texthook
            Output = output;
            Connect = connect;
            Disconnect = disconnect;
            Create = create;
            Destroy = destroy;
            TextInterop.Start(Connect, Disconnect, Create, Destroy, Output);
        }


        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            if (outputTextbox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                outputTextbox.Text = text;
            }
        }

        private void AppendText(string text)
        {
            if (outputTextbox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                outputTextbox.AppendText(text);
            }
        }

        private void SetSelectedExeLabel(string text)
        {
            if (selectedExeLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetSelectedExeLabel);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                selectedExeLabel.Text = text;
            }
        }
        public bool output(OutputInfo outinfo, string message)
        {
            if (this.ttCombo.InvokeRequired)
            {
                OutputCallback d = new OutputCallback(output);
                this.Invoke(d, new object[] { outinfo, message });
                return false;
            }
            else
            {
                string txt = handleToStringDict[outinfo.handle];
                txt += message + System.Environment.NewLine;
                handleToStringDict[outinfo.handle] = txt;

                if (indexToHandleDict[ttCombo.SelectedIndex] == outinfo.handle)
                {
                    AppendText(message + System.Environment.NewLine);
                }
                return false;
            }
        }

        public void create(OutputInfo outinfo)
        {
            if (this.ttCombo.InvokeRequired)
            {
                ThreadEventCallback d = new ThreadEventCallback(create);
                this.Invoke(d, new object[] { outinfo });
            }
            else
            {
                string ttstr = String.Format("[{0:X}:{1:X}:{2:X}:{3:X}:{4:X}:{5}]", outinfo.handle, outinfo.processId, outinfo.addr, outinfo.ctx, outinfo.ctx2, outinfo.name);
                int index = ttCombo.Items.Add(ttstr);
                indexToHandleDict.Add(index, outinfo.handle);
                handleToStringDict.Add(outinfo.handle, "");
                ttCombo.SelectedIndex = index;
            }
        }

        void connect(uint dword)
        {
            if (this.connectedLabel.InvokeRequired)
            {
                ProcessEventHandler d = new ProcessEventHandler(connect);
                this.Invoke(d, new object[] { dword });
            }
            else
            {
                connectedLabel.Text = "connected";
            }
        }

        void disconnect(uint dword)
        {
        }

        void destroy(OutputInfo outinfo)
        {
        }

        private void WordUnderCursor(Object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int cursorTextIndex = textBox.GetCharIndexFromPosition(e.Location);

            if (cursorTextIndex == lastTextIndex) return;
            lastTextIndex = cursorTextIndex;

            int selectionLength = Math.Min(MAX_TEXT_LENGTH, textBox.Text.Length - cursorTextIndex);
            string selection = textBox.Text.Substring(cursorTextIndex, selectionLength);

            List<LookupResult> res = edict.lookup(selection);
            res.Reverse();

            if (res.Count > 0)
            {
                resultPopup.SetResults(res);

                Point wordPoint = textBox.GetPositionFromCharIndex(cursorTextIndex);
                wordPoint.X += 20; // arbitrary numbers to offset the result box from the text box
                wordPoint.Y += 60;
                wordPoint = PointToScreen(wordPoint);
                resultPopup.Location = wordPoint;
                resultPopup.Visible = true;
            } else
            {
                resultPopup.Visible = false;
            }
        }

        private void OnComboSelectionChanged(Object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int selectedIndex = comboBox.SelectedIndex;
            if (indexToHandleDict.ContainsKey(selectedIndex))
            {
                string txt = handleToStringDict[indexToHandleDict[selectedIndex]];
                SetText(txt);
                lastTextIndex = -1;
            }
        }

        private void PidFromSelectedWindow(Object sender, EventArgs e)
        {
            if (isSelectWindow)
            {
                isSelectWindow = false;
                Task.Factory.StartNew(() =>
                {
                    IntPtr newWindow = Winapi.GetForegroundWindow();
                    if (newWindow != IntPtr.Zero)
                    {
                        uint pid;
                        Winapi.GetWindowThreadProcessId(newWindow, out pid);
                        Process process = Process.GetProcessById((int)pid);
                        selectedExe = process.MainModule.FileName;
                        process.Dispose();
                        SetSelectedExeLabel(selectedExe);
                    }
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void selectProcessButton_Click(object sender, EventArgs e)
        {
            isSelectWindow = true;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileNameWithoutExtension(selectedExe);
            Process[] processes = Process.GetProcessesByName(filename);
            foreach (Process process in processes)
            {
                uint pid = (uint)process.Id;
                TextInterop.InjectProcess(pid);
                process.Dispose();
            }
        }
    }
}
