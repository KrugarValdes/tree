using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Суффиксное_дерево
{
    public partial class Form1 : Form
    {
        class Node
        {
            public string branches = "";
            public List<int> freq = new List<int>();
            public List<Node> links = new List<Node>();
        };
        Node root = new Node();
        List<string> dump;

        public Form1()
        {
            InitializeComponent();
        }

        private void загрузитьТекстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() != DialogResult.OK) return;

            StreamReader sr = new StreamReader(d.FileName, Encoding.Default);
            int nodes = 1;
            while(!sr.EndOfStream)
            {
                string word = sr.ReadLine().ToLower();
                Node current = root;
                foreach(char c in word)
                    if (current.branches.Contains(c))
                    {
                        int i = current.branches.IndexOf(c);
                        current.freq[i]++;
                        current = current.links[i];
                    }
                    else
                    {
                        current.branches += c;
                        current.freq.Add(1);
                        Node x = new Node();
                        nodes++;
                        current.links.Add(x);
                        current = x;
                    }
            }
            sr.Close();

            textBox1.Text += "Файл " + d.SafeFileName + " обработан. В суффиксном дереве " + nodes + " узлов.\r\n\r\n";
        }

        void dump_tree(Node next, int index)
        {
            int sum = 0;
            foreach (int f in next.freq)
                sum += f;

            for(int i=0; i<next.branches.Length; i++)
            {
                dump.Add(string.Format("{0}\t{1}\t{2}\t{3:0.###}", index, index+i+1, next.branches[i], (double)next.freq[i]/sum));
                dump_tree(next.links[i], index + i + 1);
            }
        }

        private void сохранитьСтатистикуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() != DialogResult.OK) return;

            StreamWriter sw = new StreamWriter(d.FileName);

            dump = new List<string>();
            dump_tree(root, 0);
            //textBox1.Lines = dump.ToArray();

            foreach (string s in dump)
                sw.WriteLine(s);
            sw.Close();

            textBox1.Text += "Граф сохранён в " + d.FileName + ".\r\n\r\n";
        }
    }
}
