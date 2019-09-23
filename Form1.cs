using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimalMismatch
{
    public partial class Form1 : Form
    {
        List<string> listFiles = new List<string>();

        public Form1()
        {
            InitializeComponent();

        }

        //string Sourcee, Patternn;
        OptimalMismatch.Result s;
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            { 
            if (listBox1.Items.Count == 0)
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();


                OptimalMismatch.InitOptimalSearch(textBox1.Text, richTextBox1.Text);
                OptimalMismatch.Result result = OptimalMismatch.FindAll();
                ArrayList arrayList = new ArrayList(result.Indexes);
                foreach (var index in arrayList)
                {
                    listBox1.Items.Add(index);
                }

                int i = 0;
                i = listBox1.Items.Count;
                textBox2.Text = i.ToString();

                sw.Stop();
                label7.Text = sw.Elapsed.ToString() + " miliseconds";
            }

            else
            {
                listBox1.Items.Clear();
                Stopwatch sw = new Stopwatch();
                sw.Start();


                OptimalMismatch.InitOptimalSearch(textBox1.Text, richTextBox1.Text);
                OptimalMismatch.Result result = OptimalMismatch.FindAll();
                ArrayList arrayList = new ArrayList(result.Indexes);
                foreach (var index in arrayList)
                {
                    listBox1.Items.Add(index);
                }

                int i = 0;
                i = listBox1.Items.Count;
                textBox2.Text = i.ToString();

                sw.Stop();
                label7.Text = sw.Elapsed.ToString() + " miliseconds";
            }
        }
            else
            {
                if (listBox1.Items.Count == 0)
                {

                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    OptimalMismatch.InitOptimalSearch(textBox1.Text, richTextBox2.Text);
                    OptimalMismatch.Result result = OptimalMismatch.FindAll();
                    ArrayList arrayList = new ArrayList(result.Indexes);
                    foreach (var index in arrayList)
                    {
                        listBox1.Items.Add(index);
                    }

                    int i = 0;
                    i = listBox1.Items.Count;
                    textBox2.Text = i.ToString();

                    sw.Stop();
                    label7.Text = sw.Elapsed.ToString() + " miliseconds";
                }

                else
                {
                    listBox1.Items.Clear();
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    OptimalMismatch.InitOptimalSearch(textBox1.Text, richTextBox2.Text);
                    OptimalMismatch.Result result = OptimalMismatch.FindAll();
                    ArrayList arrayList = new ArrayList(result.Indexes);
                    foreach (var index in arrayList)
                    {
                        listBox1.Items.Add(index);
                    }

                    int i = 0;
                    i = listBox1.Items.Count;
                    textBox2.Text = i.ToString();

                    sw.Stop();
                    label7.Text = sw.Elapsed.ToString() + " miliseconds";
                }
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Sourcee = richTextBox1.Text;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            listBox1.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            label6.Text = "";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Stream myStream;
            OpenFileDialog openFD = new OpenFileDialog();

            if (openFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                openFD.Filter = "txt (*.txt)|*.txt|html (*.html)|*.html|Hepsi (*.*)|*.*";
                if ((myStream = openFD.OpenFile()) != null)
                {
                    textBox3.Text = openFD.FileName;
                    richTextBox2.Text = File.ReadAllText(openFD.FileName);
                    //        //richTextBox1.LoadFile(openFD.FileName,RichTextBoxStreamType.PlainText);
                    //        string fileName = openFD.FileName;
                    //        string fileText = File.ReadAllText(fileName);
                    //        //richTextBox1.Text = fileText;
                }
            }
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Patternn = textBox1.Text;
        }
    }

    class OptimalMismatch
    {
        private static char[] _x, _y;
        private static int _m, _n;
        private static int[] _adaptedGs, _qsBc, _frequency;
        private static Pattern[] _pattern;

        private static void OrderPattern(char[] x, Pattern[] pat, int[] freq)
        {
            for (int i = 0; i < x.Length; ++i)
            {
                Pattern ptrn = new Pattern();
                ptrn.Location = i;
                ptrn.Character = x[i];
                pat[i] = ptrn;
            }

            QuickSortPattern(pat, 0, x.Length - 1, freq);
        }

        private static void QuickSortPattern(Pattern[] pat, int low, int n, int[] freq)
        {
            int lo = low;
            int hi = n;

            if (lo >= n) return;

            Pattern mid = pat[(lo + hi) / 2];

            while (lo < hi)
            {
                while (lo < hi && OptimalPatternCompare(pat[lo], mid, freq) < 0) lo++;
                while (lo < hi && OptimalPatternCompare(pat[hi], mid, freq) > 0) hi--;

                if (lo < hi)
                {
                    Pattern temp = pat[lo];
                    pat[lo] = pat[hi];
                    pat[hi] = temp;
                }
            }

            if (hi < lo)
            {
                int temp = hi;
                hi = lo;
                lo = temp;
            }

            QuickSortPattern(pat, low, lo, freq);
            QuickSortPattern(pat, lo == low ? lo + 1 : lo, n, freq);
        }

        private static int OptimalPatternCompare(Pattern pat1, Pattern pat2, int[] freq)
        {
            int fx = freq[pat1.Character] - freq[pat2.Character];
            return (fx != 0 ? (fx > 0 ? 1 : -1) : (pat2.Location - pat1.Location));
        }

        private static int MatchShift(char[] x, int ploc, int lShift, Pattern[] pat)
        {
            int i, j;
            for (; lShift < x.Length; ++lShift)
            {
                i = ploc;
                while (--i >= 0)
                {
                    if ((j = (pat[i].Location - lShift)) < 0) continue;
                    if (pat[i].Character != x[j]) break;
                    
                }
                if (i < 0) break;
            }

            return (lShift);
        }

        private static void PreAdaptedGs(char[] x, int[] adaptedGs, Pattern[] pat)
        {
            int lShift, i, pLoc;
            adaptedGs[0] = lShift = 1;

            for (pLoc = 1; pLoc <= x.Length; ++pLoc)
            {
                lShift = MatchShift(x, pLoc, lShift, pat);
                adaptedGs[pLoc] = lShift;
            }

            for (pLoc = 0; pLoc < x.Length; ++pLoc)
            {
                lShift = adaptedGs[pLoc];
                while (lShift < x.Length)
                {
                    i = pat[pLoc].Location - lShift;
                    if (i < 0 || pat[pLoc].Character != x[i]) break;
                    ++lShift;
                    lShift = MatchShift(x, pLoc, lShift, pat);
                }
                adaptedGs[pLoc] = lShift;
            }
        }

        private static int[] CalculateCharFrequency(char[] x, char[] y, int z)
        {
            int i;
            int[] freq = new int[z];
            for (i = 0; i < x.Length; i++) freq[x[i]]++;
            for (i = 0; i < y.Length; i++) freq[y[i]]++;
            return freq;
        }

        private static void PreQsBc(char[] x, int[] qsBc)
        {
            int i, m = x.Length;

            for (i = 0; i < qsBc.Length; ++i)
                qsBc[i] = m + 1;

            for (i = 0; i < m; ++i)
                qsBc[x[i]] = m - i;
        }

        private static void SetupOptimalSearch()
        {
            OrderPattern(_x, _pattern, _frequency);
            PreQsBc(_x, _qsBc);
            PreAdaptedGs(_x, _adaptedGs, _pattern);
        }

        public static void InitOptimalSearch(string pattern, string source)
        {
            _x = pattern.ToCharArray();
            _y = source.ToCharArray();
            _m = _x.Length;
            _n = _y.Length;
            _adaptedGs = new int[_m + 1];
            _qsBc = new int[65536];
            _frequency = CalculateCharFrequency(_x, _y, 65536);
            _pattern = new Pattern[_m];
        }

        public static Result FindAll()
        {
            int i, j;
            List<int> result = new List<int>();
            SetupOptimalSearch();

            j = 0;
            int jOld = 0;
            while (j <= _n - _m)
            {
                i = 0;
                while (i < _m && _pattern[i].Character == _y[j + _pattern[i].Location]) ++i;

                if (i >= _m)
                    result.Add(j);

                jOld = j;
                if (j < _n - _m)
                    j += Math.Max(_adaptedGs[i], _qsBc[_y[j + _m]]);
                else
                    j += _adaptedGs[i];
            }

            return new Result(jOld, result);
        }

        public struct Result
        {
            public int Comp;
            public List<int> Indexes;

            public Result(int comp, List<int> indexes)
            {
                Comp = comp;
                Indexes = indexes;
            }
        }

        public struct Pattern
        {
            public int Location;
            public char Character;
        }

    }

}
