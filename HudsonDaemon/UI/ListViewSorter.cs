namespace HudsonDaemon.UI
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ListViewSorter : IComparer
    {
        public ListViewSorter()
        {
            this.LastSort = 0;
            this.ByColumn = 0;
        }

        public int Compare(object o1, object o2)
        {
            if (!(o1 is ListViewItem))
            {
                return 0;
            }
            if (!(o2 is ListViewItem))
            {
                return 0;
            }
            ListViewItem item = (ListViewItem) o1;
            string text = item.SubItems[this.ByColumn].Text;
            ListViewItem item2 = (ListViewItem) o2;
            string strB = item2.SubItems[this.ByColumn].Text;
            int num = (item2.ListView.Sorting == SortOrder.Ascending) ? string.Compare(text, strB) : string.Compare(strB, text);
            this.LastSort = this.ByColumn;
            return num;
        }

        public int ByColumn { get; set; }

        public int LastSort { get; set; }
    }
}

