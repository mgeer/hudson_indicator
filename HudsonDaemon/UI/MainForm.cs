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
//        private const int NameIndex = 1;
//        private const int UrlIndex = 2;
        private Button btnShowJobs;
        private Button btnWatch;
        private IContainer components;
        private ListView jobsView;
        private Label label1;
        private TextBox urls;
        private Button loginButton;

        private HudsonApiGraber hudsonApiGraber = new HudsonApiGraber(new CommonSpider());
        private TextBox textLog;
        private readonly Schedule daemonSchedule;
        public MainForm()
        {
            InitializeComponent();
            InitJobsView();
            LoadViewAsJsonIfExists();
            daemonSchedule = new Schedule(new TextBoxLogger(textLog));
//            Hidable();
        }

        private void BtnShowJobs_Click(object sender, EventArgs e)
        {
            try
            {
                var jobs = hudsonApiGraber.GetJobs(TryGetHudsonUrls());
                ShowJobs(jobs);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void BtnWatch_Click(object sender, EventArgs e)
        {
            var items = from item in GetSelectedItems(jobsView.Items).ToArray()
                                         select new JobItem {name = item.SubItems[1].Text, url = item.SubItems[2].Text};
            daemonSchedule.Refresh(items, hudsonApiGraber);
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
            SizeChanged += MainForm_SizeChanged;
//            notifyIcon.Click += notifyIcon_Click;
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.urls = new System.Windows.Forms.TextBox();
            this.btnShowJobs = new System.Windows.Forms.Button();
            this.jobsView = new System.Windows.Forms.ListView();
            this.btnWatch = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.textLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入要监控的Hudson地址（一行一个）：";
            // 
            // urls
            // 
            this.urls.Location = new System.Drawing.Point(14, 36);
            this.urls.Multiline = true;
            this.urls.Name = "urls";
            this.urls.Size = new System.Drawing.Size(801, 58);
            this.urls.TabIndex = 1;
            this.urls.Text = "http://db-spi-hudson0.db01.baidu.com:8235/hudson/view/lc/";
            // 
            // btnShowJobs
            // 
            this.btnShowJobs.Location = new System.Drawing.Point(740, 100);
            this.btnShowJobs.Name = "btnShowJobs";
            this.btnShowJobs.Size = new System.Drawing.Size(75, 23);
            this.btnShowJobs.TabIndex = 2;
            this.btnShowJobs.Text = "显示Jobs";
            this.btnShowJobs.UseVisualStyleBackColor = true;
            this.btnShowJobs.Click += new System.EventHandler(this.BtnShowJobs_Click);
            // 
            // jobsView
            // 
            this.jobsView.CheckBoxes = true;
            this.jobsView.FullRowSelect = true;
            this.jobsView.GridLines = true;
            this.jobsView.Location = new System.Drawing.Point(14, 129);
            this.jobsView.Name = "jobsView";
            this.jobsView.Size = new System.Drawing.Size(801, 297);
            this.jobsView.TabIndex = 3;
            this.jobsView.UseCompatibleStateImageBehavior = false;
            this.jobsView.View = System.Windows.Forms.View.Details;
            // 
            // btnWatch
            // 
            this.btnWatch.Location = new System.Drawing.Point(740, 432);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(75, 23);
            this.btnWatch.TabIndex = 2;
            this.btnWatch.Text = "监控";
            this.btnWatch.UseVisualStyleBackColor = true;
            this.btnWatch.Click += new System.EventHandler(this.BtnWatch_Click);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(554, 8);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(261, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "啊！我需要登录！！";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(12, 461);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(801, 181);
            this.textLog.TabIndex = 5;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(831, 654);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.jobsView);
            this.Controls.Add(this.btnWatch);
            this.Controls.Add(this.btnShowJobs);
            this.Controls.Add(this.urls);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "神灯";
            this.ResumeLayout(false);
            this.PerformLayout();

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
//                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
//            notifyIcon.Visible = false;
        }

        private void Persist()
        {
            string[] lines = urls.Lines;
            var source = JobViewToItemList();
            var view = new DaemonView
                {
                    Urls = lines,
                    JobLines = source.ToArray()
                };
            new Repository().Save(DaemonView.Serialize(view));
        }

        private void ShowJobs(IEnumerable<JobItem> jobs)
        {
            jobsView.Items.Clear();
            var items = jobs.Select(CreateListViewItem).ToArray();
            jobsView.Items.AddRange(items);
        }

        private IEnumerable<HudsonUrl> TryGetHudsonUrls()
        {
            var lines = urls.Lines;
            EnsureUrlsAreValid(lines);
            return (from url in lines select new HudsonUrl(url));
        }

        private bool credendialMode;
        private void button1_Click(object sender, EventArgs e)
        {
            if (credendialMode)
            {
                Logout();
            }
            else
            {
                Login();
            }
        }

        private void Login()
        {
            var userForm = new UserForm();
            var dialogResult = userForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var userName = userForm.userName.Text;
                var password = userForm.password.Text;
                var credentialSpider = new CredentialSpider(userName, password);
                hudsonApiGraber = new HudsonApiGraber(credentialSpider);
                loginButton.Text = string.Format("hi, 我是 {0}, {1}", userName, "点击注销！");
                credendialMode = true;
            }
        }

        private void Logout()
        {
            loginButton.Text = "啊！我需要登录！！";
            hudsonApiGraber = new HudsonApiGraber(new CommonSpider());
            credendialMode = false;
        }
    }
}