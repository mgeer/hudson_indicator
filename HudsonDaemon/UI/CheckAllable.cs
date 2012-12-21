using System;
using System.Windows.Forms;

namespace HudsonIndicator.HudsonDaemon.UI
{
    internal class CheckAllable
    {
        private readonly ListView listView;

        public CheckAllable(ListView listView)
        {
            this.listView = listView;
            listView.ColumnClick += ListViewColumnClick;
        }

        private bool AreAllItemChecked()
        {
            var areAllChecked = true;
            IterateListViewItem(delegate (ListViewItem item) {
                if (!item.Checked)
                {
                    areAllChecked = false;
                }
            });
            return areAllChecked;
        }

        private void CheckAll()
        {
            IterateListViewItem(item => item.Checked = true);
        }

        private void IterateListViewItem(Action<ListViewItem> action)
        {
            listView.SuspendLayout();
            foreach (ListViewItem item in listView.Items)
            {
                action(item);
            }
            listView.ResumeLayout();
        }

        private void ListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                if (AreAllItemChecked())
                {
                    UncheckAll();
                }
                else
                {
                    CheckAll();
                }
            }
        }

        private void UncheckAll()
        {
            IterateListViewItem(item => item.Checked = false);
        }
    }
}

