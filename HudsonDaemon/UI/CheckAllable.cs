namespace HudsonDaemon.UI
{
    using System;
    using System.Windows.Forms;

    internal class CheckAllable
    {
        private readonly ListView listView;

        public CheckAllable(ListView listView)
        {
            this.listView = listView;
            listView.ColumnClick += new ColumnClickEventHandler(this.ListViewColumnClick);
        }

        private bool AreAllItemChecked()
        {
            bool areAllChecked = true;
            this.IterateListViewItem(delegate (ListViewItem item) {
                if (!item.Checked)
                {
                    areAllChecked = false;
                }
            });
            return areAllChecked;
        }

        private void CheckAll()
        {
            this.IterateListViewItem(item => item.Checked = true);
        }

        private void IterateListViewItem(Action<ListViewItem> action)
        {
            this.listView.SuspendLayout();
            foreach (ListViewItem item in this.listView.Items)
            {
                action(item);
            }
            this.listView.ResumeLayout();
        }

        private void ListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                if (this.AreAllItemChecked())
                {
                    this.UncheckAll();
                }
                else
                {
                    this.CheckAll();
                }
            }
        }

        private void UncheckAll()
        {
            this.IterateListViewItem(item => item.Checked = false);
        }
    }
}

