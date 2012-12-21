using System.Collections;
using System.Windows.Forms;

namespace HudsonIndicator.HudsonDaemon.UI
{
    public class ListViewSorter : IComparer
    {
        public ListViewSorter()
        {
            LastSort = 0;
            ByColumn = 0;
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
            var item = (ListViewItem) o1;
            var text = item.SubItems[ByColumn].Text;
            var item2 = (ListViewItem) o2;
            var strB = item2.SubItems[ByColumn].Text;
            var num = (item2.ListView.Sorting == SortOrder.Ascending) ? System.String.CompareOrdinal(text, strB) : System.String.CompareOrdinal(strB, text);
            LastSort = ByColumn;
            return num;
        }

        public int ByColumn { get; set; }

        public int LastSort { get; set; }
    }
}

