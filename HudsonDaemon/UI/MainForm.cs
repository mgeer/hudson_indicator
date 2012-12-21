using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HudsonIndicator.HudsonDaemon.Daemons;
using HudsonIndicator.HudsonDaemon.Hudson;
using HudsonIndicator.HudsonDaemon.Persistance;

namespace HudsonIndicator.HudsonDaemon.UI
{
    public class MainForm : Form
    {
        private const int NameIndex = 1;
        private const int UrlIndex = 2;
        private readonly Schedule daemonSchedule = new Schedule();
        private Button btnShowJobs;
        private Button btnWatch;
        private IContainer components;
        private ListView jobsView;
        private Label label1;
        private NotifyIcon notifyIcon;
        private TextBox urls;

        public MainForm()
        {
            InitializeComponent();
            InitJobsView();
            LoadViewAsJsonIfExists();
            Hidable();
        }

        private void BtnShowJobs_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<JobItem> jobs = HudsonApiGraber.GetJobs(TryGetHudsonUrls());
                ShowJobs(jobs);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void BtnWatch_Click(object sender, EventArgs e)
        {
            IEnumerable<JobItem> items = from item in GetSelectedItems(jobsView.Items).ToArray()
                                         select new JobItem {name = item.SubItems[1].Text, url = item.SubItems[2].Text};
            daemonSchedule.Refresh(items);
            Persist();
        }

        private static ColumnHeader CreateColumn(string text, int width)
        {
            return new ColumnHeader {Text = text, Width = width};
        }

        private static ListViewItem CreateListViewItem(JobItem job)
        {
            var item = new ListViewItem();
            item.SubItems.Add(job.name);
            item.SubItems.Add(job.url);
            return item;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
                var current = (ListViewItem) enumerator.Current;
                if (current.Checked)
                {
                    yield return current;
                }
            }
        }

        private void Hidable()
        {
            base.SizeChanged += MainForm_SizeChanged;
            notifyIcon.Click += notifyIcon_Click;
        }

        private void InitializeComponent()
        {
            components = new Container();
            var manager = new ComponentResourceManager(typeof (MainForm));
            label1 = new Label();
            urls = new TextBox();
            btnShowJobs = new Button();
            jobsView = new ListView();
            btnWatch = new Button();
            notifyIcon = new NotifyIcon(components);
            SuspendLayout();
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(0xdd, 12);
            label1.TabIndex = 0;
            label1.Text = "输入要监控的Hudson地址（一行一个）：";
            urls.Location = new Point(14, 0x18);
            urls.Multiline = true;
            urls.Name = "urls";
            urls.Size = new Size(0x321, 0x3a);
            urls.TabIndex = 1;
            urls.Text = "http://db-spi-hudson0.db01.baidu.com:8235/hudson/view/lc/";
            btnShowJobs.Location = new Point(740, 0x58);
            btnShowJobs.Name = "btnShowJobs";
            btnShowJobs.Size = new Size(0x4b, 0x17);
            btnShowJobs.TabIndex = 2;
            btnShowJobs.Text = "显示Jobs";
            btnShowJobs.UseVisualStyleBackColor = true;
            btnShowJobs.Click += BtnShowJobs_Click;
            jobsView.CheckBoxes = true;
            jobsView.FullRowSelect = true;
            jobsView.GridLines = true;
            jobsView.Location = new Point(14, 0x75);
            jobsView.Name = "jobsView";
            jobsView.Size = new Size(0x321, 0x129);
            jobsView.TabIndex = 3;
            jobsView.UseCompatibleStateImageBehavior = false;
            jobsView.View = View.Details;
            btnWatch.Location = new Point(740, 420);
            btnWatch.Name = "btnWatch";
            btnWatch.Size = new Size(0x4b, 0x17);
            btnWatch.TabIndex = 2;
            btnWatch.Text = "监控";
            btnWatch.UseVisualStyleBackColor = true;
            btnWatch.Click += BtnWatch_Click;
            notifyIcon.Icon = (Icon) manager.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "HudsonDaemon";
            AutoScaleDimensions = new SizeF(6f, 12f);
            AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x33d, 0x1c7);
            base.Controls.Add(jobsView);
            base.Controls.Add(btnWatch);
            base.Controls.Add(btnShowJobs);
            base.Controls.Add(urls);
            base.Controls.Add(label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "MainForm";
            base.ShowInTaskbar = false;
            Text = "神灯";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitJobsView()
        {
            ColumnHeader header = CreateColumn("选择", 40);
            ColumnHeader header2 = CreateColumn("Job 名称", 200);
            ColumnHeader header3 = CreateColumn("Job URL", 500);
            jobsView.Columns.AddRange(new[] {header, header2, header3});
            new SortableListView(jobsView, new[] {1, 2}).Sortable();
            new CheckAllable(jobsView);
        }

        private IEnumerable<JobLine> JobViewToItemList()
        {
            return jobsView.Items.Cast<ListViewItem>().Select<ListViewItem, JobLine>(delegate(ListViewItem item)
                {
                    var line = new JobLine
                        {
                            Selected = item.Checked
                        };
                    var item2 = new JobItem
                        {
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
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            DaemonView view = DaemonView.Parse(str);
            urls.Lines = view.Urls;
            var list = new List<ListViewItem>();
            foreach (JobLine line in view.JobLines)
            {
                var item = new ListViewItem
                    {
                        Checked = line.Selected
                    };
                item.SubItems.Add(line.Item.name);
                item.SubItems.Add(line.Item.url);
                list.Add(item);
            }
            jobsView.Items.AddRange(list.ToArray());
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void Persist()
        {
            string[] lines = urls.Lines;
            IEnumerable<JobLine> source = JobViewToItemList();
            var view = new DaemonView
                {
                    Urls = lines,
                    JobLines = source.ToArray()
                };
            new Repository().Save(DaemonView.Serialize(view));
        }

        private void ShowJobs(IEnumerable<JobItem> jobs)
        {
            ListViewItem[] items = jobs.Select(CreateListViewItem).ToArray();
            jobsView.Items.AddRange(items);
        }

        private IEnumerable<HudsonUrl> TryGetHudsonUrls()
        {
            string[] lines = urls.Lines;
            EnsureUrlsAreValid(lines);
            return (from url in lines select new HudsonUrl(url));
        }
    }
}