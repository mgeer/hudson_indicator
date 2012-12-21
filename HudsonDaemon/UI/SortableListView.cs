namespace HudsonDaemon.UI
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    internal class SortableListView
    {
        private readonly ListView listView;
        private readonly int[] sortableColumnIndexs;

        public SortableListView(ListView listView, int[] sortableColumnIndexs)
        {
            this.listView = listView;
            this.sortableColumnIndexs = sortableColumnIndexs;
        }

        public void Sort(int column)
        {
            ListViewSorter listViewItemSorter = (ListViewSorter) this.listView.ListViewItemSorter;
            if (listViewItemSorter.LastSort == column)
            {
                this.listView.Sorting = (this.listView.Sorting == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                this.listView.Sorting = SortOrder.Descending;
            }
            listViewItemSorter.ByColumn = column;
            this.listView.Sort();
        }

        public void Sortable()
        {
            this.listView.Sorting = SortOrder.Ascending;
            ListViewSorter sorter = new ListViewSorter();
            this.listView.ListViewItemSorter = sorter;
            this.listView.ColumnClick += new ColumnClickEventHandler(this.SortJobs);
        }

        private void SortJobs(object sender, ColumnClickEventArgs e)
        {
            int column = e.Column;
            if (!this.sortableColumnIndexs.All<int>(i => (i != column)))
            {
                this.Sort(column);
            }
        }
    }
}

