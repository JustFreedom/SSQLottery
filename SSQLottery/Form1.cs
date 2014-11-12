﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Statistic;
using Statistic.Init;
using Statistic.StatisticOrgNumber;

namespace SSQLottery
{
    public partial class Form1 : Form
    {
        private string StatisticDetailKeyThreadName = "StatisticDetailKeyThread";
        private string StatisticOrgNumThreadName = "StatisticOrgNumThread";
        /// <summary>
        /// 创建的线程都缓存在这里
        /// </summary>
        private List<Thread> _threads = new List<Thread>(); 
        private delegate void SetTextCallback(Control control, string text);

        private BaseStatistic baseStatistic = StatisticFactory.TaobaoBaseStatisticInstance();
        StatisticInit statisticInit = StatisticFactory.TaobaoStatisticInitInstance();
        private IStatisticDetailOrderKey statisticDetailOrderKey = StatisticFactory.StatisticDetailOrderKeyInstance();
        private IStatisticOrgNumber statisticOrgNumber = StatisticFactory.TaobaoStatisticOrgNumberInstance();
        public Form1()
        {
            InitializeComponent();
            statisticInit.InitDataBaseAndTable();
            InitStatisticDetailOrder();
            InitStatisticOriginalNum();
            StatisticMonitor.NewStatisticPageCount += OnStatisticNewPage;
            StatisticMonitor.NewStatisticOriginalNumberCount += OnStatisticOriginalNumber;
        }

        private void OnStatisticNewPage(int newStatisticPageCount)
        {
            int pageCount;
            int.TryParse(this.TaobaoStatisticedPageLbl.Text, out pageCount);
            var text = TaobaoStatisticedPageLbl.Text;
            SetControlText(this.TaobaoStatisticGrp,this.TaobaoStatisticedPageLbl.Name, (pageCount + newStatisticPageCount).ToString());
        }

        private void OnStatisticOriginalNumber(int newStatisticOriginalNumberCount)
        {
            int pageCount;
            int.TryParse(this.TaobaoOrgStatisticedNumLbl.Text, out pageCount);
            SetControlText(this.TaobaoOrgNumberGrp, this.TaobaoOrgStatisticedNumLbl.Name, (pageCount + newStatisticOriginalNumberCount).ToString());
        }

        private void InitStatisticDetailOrder()
        {
            string respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage);
            
            string curIssueNumber = baseStatistic.GetCurrentIssueNumber(respStr);
            this.IssueNumberTxt.Text = curIssueNumber;
            int totalItemsCount = baseStatistic.GetTotalItemsCount(respStr);
            this.TaobaoTotalItemLbl.Text = totalItemsCount.ToString();
            
            int orgProcessedPageCount = statisticInit.GetDetailOrderKeyProgressCount(curIssueNumber);
            this.TaobaoStatisticedPageLbl.Text = orgProcessedPageCount.ToString();

            int totalPageCount = baseStatistic.GetTotalPageCount(respStr);
            this.TaobaoStatisticTotalPageLbl.Text = totalPageCount.ToString();
        }

        private void InitStatisticOriginalNum()
        {
            string respStr = baseStatistic.GetResponseByUrl(UrlConst.UnitedListByPage);
            string curIssueNumber = baseStatistic.GetCurrentIssueNumber(respStr);
            this.IssueNumberTxt.Text = curIssueNumber;
            int totalItemsCount = baseStatistic.GetTotalItemsCount(respStr);
            this.TaobaoOrgTotalNumLbl.Text = totalItemsCount.ToString();

            int orgProcessedPageCount = statisticInit.GetOriginalProgressCount(curIssueNumber);
            this.TaobaoOrgStatisticedNumLbl.Text = orgProcessedPageCount.ToString();

        }

        private void TaobaoDetailOrderStatisticStartBtn_Click(object sender, EventArgs e)
        {
            string currentIssueNumber = this.IssueNumberTxt.Text.Trim();
            int orgProcessedPageCount = statisticInit.GetDetailOrderKeyProgressCount(currentIssueNumber);
            //this.TaobaoStatisticPageLbl.Text = orgProcessedPageCount.ToString();
            //statisticDetailOrderKey.StatisticDetailOrderKey(IssueNumberTxt.Text,orgProcessedPageCount);
            var define = new StatisticDetailOrderKeyDefine(IssueNumberTxt.Text, orgProcessedPageCount);
            Thread thread = new Thread(define.Statistic);
            thread.Name = StatisticDetailKeyThreadName;
            _threads.Add(thread);
            thread.Start();
        }

        private void TaobaoDetailOrderStatisticStopBtn_Click(object sender, EventArgs e)
        {
            AbortThread(StatisticDetailKeyThreadName);
        }

        private void AbortThread(string threadName)
        {
            foreach (Thread thread in _threads)
            {
                if (thread.Name != null && thread.Name.Equals(threadName) && thread.IsAlive)
                    thread.Abort();
            }
        }

        private void SetControlText(string controlName, string value)
        {
            Control[] textBox = this.Controls.Find(controlName, true);
            if (textBox.Length > 0)
                SetText(textBox[0], value);
           
        }
        private void SetControlText(Control control, string value)
        {
            control.Text = value;
        }
        private void SetControlText(Control parentControl, string controlName, string value)
        {
            Control[] textBox = parentControl.Controls.Find(controlName, true);
            if (textBox.Length > 0)
                SetText(textBox[0], value);
        }

        private void SetText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }

        private void TaobaoOrgStatisticStartBtn_Click(object sender, EventArgs e)
        {
            string currentIssueNumber = this.IssueNumberTxt.Text.Trim();
            int orgProcessedPageCount = statisticInit.GetDetailOrderKeyProgressCount(currentIssueNumber);
            //this.TaobaoStatisticPageLbl.Text = orgProcessedPageCount.ToString();
            //statisticDetailOrderKey.StatisticDetailOrderKey(IssueNumberTxt.Text,orgProcessedPageCount);
            var define = new StatisticOriginalNumDefine(IssueNumberTxt.Text, orgProcessedPageCount);
            Thread thread = new Thread(define.Statistic);
            thread.Name = StatisticOrgNumThreadName;
            _threads.Add(thread);
            thread.Start();
        }

        private void TaobaoOrgStatisticStopBtn_Click(object sender, EventArgs e)
        {
            AbortThread(StatisticOrgNumThreadName);
        }

      
    }
}