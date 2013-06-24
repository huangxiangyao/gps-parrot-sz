using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Parrot
{
    public class ListViewSort : IComparer
    {
        // Fields
        private int column;
        private bool desc;

        // Methods
        public ListViewSort()
        {
            this.column = 0;
        }

        public ListViewSort(int column, bool desc)
        {
            this.desc = (bool)desc;
            this.column = column;
        }

        public int Compare(object x, object y)
        {
            int num = string.Compare(((ListViewItem)x).SubItems[this.column].Text, ((ListViewItem)y).SubItems[this.column].Text);
            if (this.desc)
            {
                return (0 - num);
            }
            return num;
        }
    }
}