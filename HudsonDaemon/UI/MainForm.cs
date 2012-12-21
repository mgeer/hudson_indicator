namespace HudsonDaemon.UI
{
    using Daemons;
    using Hudson;
    using Persistance;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class MainForm : Form
    {
        private Button btnShowJobs;
        private Button btnWatch;
        private IContainer components;
        private readonly Schedule daemonSchedule = new Schedule();
        private ListView jobsView;
        private Label label1;
        private const int NameIndex = 1;
        private NotifyIcon notifyIcon;
        private const int UrlIndex = 2;
        private TextBox urls;

        public MainForm()
        {
            this.InitializeComponent();
            this.InitJobsView();
            this.LoadViewAsJsonIfExists();
            this.Hidable();
        }

        private void BtnShowJobs_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<JobItem> jobs = HudsonApiGraber.GetJobs(this.TryGetHudsonUrls());
                this.ShowJobs(jobs);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void BtnWatch_Click(object sender, EventArgs e)
        {
            IEnumerable<JobItem> items = from item in GetSelectedItems(this.jobsView.Items).ToArray<ListViewItem>() select new JobItem { name = item.SubItems[1].Text, url = item.SubItems[2].Text };
            this.daemonSchedule.Refresh(items);
            this.Persist();
        }

        private static ColumnHeader CreateColumn(string text, int width)
        {
            return new ColumnHeader { Text = text, Width = width };
        }

        private static ListViewItem CreateListViewItem(JobItem job)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Add(job.name);
            item.SubItems.Add(job.url);
            return item;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static void EnsureUrlsAreValid(ICollection<string> urls)
        {
            if (urls.Count == 0)
            {
                throw new ApplicationException("请输入要监控的Url地址！");
            }
            foreach (string str in from url in urls
                where !UrlValidator.Validate(url)
                select url)
            {
                throw new AppDomainUnloadedException("Url 格式错误: " + str);
            }
        }

        private static IEnumerable<ListViewItem> GetSelectedItems(ListView.ListViewItemCollection items)
        {
            IEnumerator enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ListViewItem current = (ListViewItem) enumerator.Current;
                if (current.Checked)
                {
                    yield return current;
                }
            }
        }

        private void Hidable()
        {
            base.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
            this.notifyIcon.Click += new EventHandler(this.notifyIcon_Click);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(MainForm));
            this.label1 = new Label();
            this.urls = new TextBox();
            this.btnShowJobs = new Button();
            this.jobsView = new ListView();
            this.btnWatch = new Button();
            this.notifyIcon = new NotifyIcon(this.components);
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xdd, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入要监控的Hudson地址（一行一个）：";
            this.urls.Location = new Point(14, 0x18);
            this.urls.Multiline = true;
            this.urls.Name = "urls";
            this.urls.Size = new Size(0x321, 0x3a);
            this.urls.TabIndex = 1;
            this.urls.Text = "http://db-spi-hudson0.db01.baidu.com:8235/hudson/view/lc/";
            this.btnShowJobs.Location = new Point(740, 0x58);
            this.btnShowJobs.Name = "btnShowJobs";
            this.btnShowJobs.Size = new Size(0x4b, 0x17);
            this.btnShowJobs.TabIndex = 2;
            this.btnShowJobs.Text = "显示Jobs";
            this.btnShowJobs.UseVisualStyleBackColor = true;
            this.btnShowJobs.Click += new EventHandler(this.BtnShowJobs_Click);
            this.jobsView.CheckBoxes = true;
            this.jobsView.FullRowSelect = true;
            this.jobsView.GridLines = true;
            this.jobsView.Location = new Point(14, 0x75);
            this.jobsView.Name = "jobsView";
            this.jobsView.Size = new Size(0x321, 0x129);
            this.jobsView.TabIndex = 3;
            this.jobsView.UseCompatibleStateImageBehavior = false;
            this.jobsView.View = View.Details;
            this.btnWatch.Location = new Point(740, 420);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new Size(0x4b, 0x17);
            this.btnWatch.TabIndex = 2;
            this.btnWatch.Text = "监控";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new EventHandler(this.BtnWatch_Click);
            this.notifyIcon.Icon = (Icon) manager.GetObject("notifyIcon.Icon");
            this.notifyIcon.Text = "HudsonDaemon";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x33d, 0x1c7);
            base.Controls.Add(this.jobsView);
            base.Controls.Add(this.btnWatch);
            base.Controls.Add(this.btnShowJobs);
            base.Controls.Add(this.urls);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "MainForm";
            base.ShowInTaskbar = false;
            this.Text = "神灯";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitJobsView()
        {
            ColumnHeader header = CreateColumn("选择", 40);
            ColumnHeader header2 = CreateColumn("Job 名称", 200);
            ColumnHeader header3 = CreateColumn("Job URL", 500);
            this.jobsView.Columns.AddRange(new ColumnHeader[] { header, header2, header3 });
            new SortableListView(this.jobsView, new int[] { 1, 2 }).Sortable();
            new CheckAllable(this.jobsView);
        }

        private IEnumerable<JobLine> JobViewToItemList()
        {
            return this.jobsView.Items.Cast<ListViewItem>().Select<ListViewItem, JobLine>(delegate (ListViewItem item) {
                JobLine line = new JobLine {
                    Selected = item.Checked
                };
                JobItem item2 = new JobItem {
                    name = item.SubItems[1].Text,
                    url = item.SubItems[2].Text
                };
                line.Item = item2;
                return line;
            });
        }

        private void LoadViewAsJsonIfExists()
        {
            string str = new Repository().Load();
            if (!string.IsNullOrEmpty(str))
            {
                DaemonView view = DaemonView.Parse(str);
                this.urls.Lines = view.Urls;
                List<ListViewItem> list = new List<ListViewItem>();
                foreach (JobLine line in view.JobLines)
                {
                    ListViewItem item = new ListViewItem {
                        Checked = line.Selected
                    };
                    item.SubItems.Add(line.Item.name);
                    item.SubItems.Add(line.Item.url);
                    list.Add(item);
                }
                this.jobsView.Items.AddRange(list.ToArray());
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.Hide();
                this.notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            base.Visible = true;
            base.WindowState = FormWindowState.Normal;
            this.notifyIcon.Visible = false;
        }

        private void Persist()
        {
            string[] lines = this.urls.Lines;
            IEnumerable<JobLine> source = this.JobViewToItemList();
            DaemonView view = new DaemonView {
                Urls = lines,
                JobLines = source.ToArray<JobLine>()
            };
            new Repository().Save(DaemonView.Serialize(view));
        }

        private void ShowJobs(IEnumerable<JobItem> jobs)
        {
            ListViewItem[] items = jobs.Select<JobItem, ListViewItem>(new Func<JobItem, ListViewItem>(MainForm.CreateListViewItem)).ToArray<ListViewItem>();
            this.jobsView.Items.AddRange(items);
        }

        private IEnumerable<HudsonUrl> TryGetHudsonUrls()
        {
            string[] lines = this.urls.Lines;
            EnsureUrlsAreValid(lines);
            return (from url in lines select new HudsonUrl(url));
        }

    }
}

