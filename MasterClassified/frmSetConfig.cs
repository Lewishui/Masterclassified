﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MC.Buiness;
using MC.DB;
using System.Collections;

namespace MasterClassified
{
    public partial class frmSetConfig : Form
    {
        List<int> newlist;
        List<string> showSuijiResultlist = new List<string>();
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;
        public int listbox3indexnum = 0;

        private frmAddFanAnName frmAddFanAnName;


        public frmSetConfig()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.listBox1.SelectedIndex = 0;
            InitialSystemInfo();

        }
        private void InitialSystemInfo()
        {
            try
            {
                #region 初始化配置
                ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
                ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
                ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());
                #endregion


                this.listBox1.DisplayMember = "Name";
                clsAllnew BusinessHelp = new clsAllnew();
                List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAnName();
                List<FangAnLieBiaoDATA> filtered = Result.FindAll(s => s.Name != null);
                this.listBox1.DataSource = filtered;
                List<FangAnLieBiaoDATA> Result12 = BusinessHelp.Read_FangAn("YES");
                int index = 0;
                ProcessLogger.Fatal("68013 Read FangAN " + DateTime.Now.ToString());
                foreach (FangAnLieBiaoDATA ite in filtered)
                {

                    if (Result12 != null && Result12.Count > 0 && Result12[0].Name == ite.Name)
                        break;
                    index++;
                }
                listBox1.SelectedIndex = index;
                #region 显示默认方案到 显示栏中
                ProcessLogger.Fatal("68014 Read FangAN " + DateTime.Now.ToString());
                List<FangAnLieBiaoDATA> moreResult = BusinessHelp.Read_FangAn(this.listBox1.Text.ToString());

                showSuijiResultlist = new List<string>();

                foreach (FangAnLieBiaoDATA item in moreResult)
                {
                    if (item.Data == null)
                        continue;

                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.Data, "\r\n");

                    for (int i = 1; i < temp1.Length; i++)
                    {
                        showSuijiResultlist.Add(temp1[i]);
                    }

                }
                ProcessLogger.Fatal("68015 Read FangAN " + DateTime.Now.ToString());
                if (moreResult[0].MorenDuanShu != null && moreResult.Count > 0 && moreResult[0].MorenDuanShu != "")
                    this.comboBox1.Text = moreResult[0].MorenDuanShu;
                else
                    this.comboBox1.SelectedIndex = 0;

                if (moreResult[0].Mobanleibie != null && moreResult.Count > 0 && moreResult[0].Mobanleibie != "")
                    this.comboBox3.Text = moreResult[0].Mobanleibie;
                else
                    this.comboBox3.SelectedIndex = 0;
                ProcessLogger.Fatal("68016 Read FangAN " + DateTime.Now.ToString());
                this.listBox3.DataSource = showSuijiResultlist;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误" + ex + "请到备份界面点击左侧【初始化默认方案】按钮", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                throw;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                NewMethod1();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);

                throw;
            }

        }

        private void NewMethod1()
        {
            try
            {
                ArrayList CharList = new ArrayList();
                newlist = new List<int>();
                showSuijiResultlist = new List<string>();

                newlist.Add(0);
                newlist.Add(1);
                newlist.Add(2);
                newlist.Add(3);
                newlist.Add(4);
                newlist.Add(5);
                newlist.Add(6);
                newlist.Add(7);
                newlist.Add(8);
                newlist.Add(9);
                //
                //newlist.Add(10);
                if (this.comboBox3.Text != "" && this.comboBox3.Text.Contains("_"))
                {
                    //要i求1-33  不含0 
                    newlist = new List<int>();
                    newlist.Add(1);
                    newlist.Add(2);
                    newlist.Add(3);
                    newlist.Add(4);
                    newlist.Add(5);
                    newlist.Add(6);
                    newlist.Add(7);
                    newlist.Add(8);
                    newlist.Add(9);

                    newlist.Add(10);
                    newlist.Add(11);
                    newlist.Add(12);
                    newlist.Add(13);
                    newlist.Add(14);
                    newlist.Add(15);
                    newlist.Add(16);
                    newlist.Add(17);
                    newlist.Add(18);
                    newlist.Add(19);
                    newlist.Add(20);
                    newlist.Add(21);
                    newlist.Add(22);
                    newlist.Add(23);
                    newlist.Add(24);
                    newlist.Add(25);
                    newlist.Add(26);
                    newlist.Add(27);
                    newlist.Add(28);
                    newlist.Add(29);
                    newlist.Add(30);
                    newlist.Add(31);
                    newlist.Add(32);
                    newlist.Add(33);
                }
                newlist = newlist.Select(a => new { a, newID = Guid.NewGuid() }).OrderBy(b => b.newID).Select(c => c.a).ToList();

                for (int i = 0; i < newlist.Count; i++)
                {
                    CharList.Add(newlist[i].ToString());

                }
                //for (int i = 0; i < newlist.Count; i++)
                //{
                //    Binding mybd9 = new Binding("text", newlist, newlist[i].ToString());
                //    this.textBox1.DataBindings.Add(mybd9);
                //}

                int duan = Convert.ToInt32(comboBox1.Text);
                int evertduan = 10 / duan;
                int ilast = 0;
                ilast = duan * evertduan;


                #region   //锁定自定以的数字在相应的段数中
                List<string> SelfNo = new List<string>();
                string[] temp1 = System.Text.RegularExpressions.Regex.Split(this.textBox1.Text, "\r\n");
                for (int iq = 0; iq < temp1.Length; iq++)
                {
                    if (temp1[iq].Length > 4 && temp1[iq] != "十一段=\t" && temp1[iq] != "十二段=\t" && temp1[iq] != "十三段=\t" && temp1[iq] != "十四段=\t" && temp1[iq] != "十五段=\t" && temp1[iq] != "十六段=\t" && temp1[iq] != "十七段=\t")
                    {
                        SelfNo.Add(temp1[iq]);
                        //差分自定义的数字
                        string[] temp2 = System.Text.RegularExpressions.Regex.Split(temp1[iq], "\t");
                        string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                        for (int iq1 = 0; iq1 < temp3.Length; iq1++)
                        {
                            int Pointer = CharList.IndexOf(temp3[iq1]);
                            newlist.RemoveAt(Pointer);
                            CharList.RemoveAt(Pointer);
                        }
                    }
                }

                #endregion

                #region 判断自定义段位模板按其分配每段的数字个数

                if (this.comboBox3.Text != "" && this.comboBox3.Text != "默认")
                {
                    List<int> EverDuanList = ZidingyiMeiDuanGeshu();
                    string first = "";
                    showSuijiResultlist = new List<string>();
                    //for (int iq = 1; iq <= duan; iq++)
                    {
                        int iq = 0;

                        for (int i = 0; i < EverDuanList.Count; i++)
                        {
                            string num = "";
                            int ago = 0;
                            //如果有自定义的数字则重新计算当前段数的所添加数字个数
                            int newEverDuanList = EverDuanList[i];
                            string newaddselfn0 = "";
                            #region  //如果有自定义的数字则重新计算当前段数的所添加数字个数

                            for (int ii = 0; ii < SelfNo.Count; ii++)
                            {

                                if (SelfNo[ii].Contains("一") && i == 0)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[0] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("二") && i == 1)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[1] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("三") && i == 2)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[2] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("四") && i == 3)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[3] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("五") && i == 4)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[4] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("六") && i == 5)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[5] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("七") && i == 6)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[6] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("八") && i == 7)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[7] - temp3.Length;
                                    break;

                                }
                                else if (SelfNo[ii].Contains("九") && i == 8)
                                {
                                    string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                    newaddselfn0 = temp2[1];

                                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                    newEverDuanList = EverDuanList[8] - temp3.Length;
                                    break;

                                }


                            }
                            #endregion


                            //for (int j = 0; j <= EverDuanList[i]; j++)
                            for (int j = 0; j <= newEverDuanList; j++)
                            {
                                ago++;
                                if (ago > newEverDuanList)
                                    break;
                                num = num + " " + newlist[0];
                                newlist.RemoveAt(0);
                            }
                            iq = i + 1;
                            if (newEverDuanList != EverDuanList[i])
                                num = newaddselfn0 + num;

                            first = first + "\r\n" + iq.ToString() + "段=" + " " + num;

                            showSuijiResultlist.Add(iq.ToString() + " 段= " + " " + num);
                        }


                    }
                    List<string> showSuijiResultlist1 = new List<string>();
                    for (int ii = 0; ii < showSuijiResultlist.Count; ii++)
                    {
                        for (int i = 0; i < newlist.Count; i++)
                        {
                            showSuijiResultlist[ii] = showSuijiResultlist[ii] + " " + newlist[i];
                            newlist.RemoveAt(i);

                            break;
                        }
                    }
                }
                #endregion
                else
                {
                    //if (ilast > 0)
                    //{

                    string first = "";
                    showSuijiResultlist = new List<string>();
                    for (int iq = 1; iq <= duan; iq++)
                    {
                        string num = "";
                        int ago = 0;

                        //如果有自定义的数字则重新计算当前段数的所添加数字个数
                        int newEverDuanList = evertduan;
                        string newaddselfn0 = "";
                        #region  //如果有自定义的数字则重新计算当前段数的所添加数字个数

                        for (int ii = 0; ii < SelfNo.Count; ii++)
                        {

                            if (SelfNo[ii].Contains("一") && iq == 1)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("二") && iq == 2)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("三") && iq == 3)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("四") && iq == 4)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("五") && iq == 5)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("六") && iq == 6)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("七") && iq == 7)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("八") && iq == 8)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }
                            else if (SelfNo[ii].Contains("九") && iq == 9)
                            {
                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(SelfNo[ii], "\t");
                                newaddselfn0 = temp2[1];

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp2[1], " ");
                                newEverDuanList = evertduan - temp3.Length;
                                break;

                            }


                        }
                        #endregion




                        //  for (int i = 0; i <= newlist.Count; i++)
                        for (int i = 0; i <= newEverDuanList; i++)
                        {
                            ago++;
                            if (ago > newEverDuanList)
                                break;

                            num = num + " " + newlist[0];
                            newlist.RemoveAt(0);

                        }
                        if (newEverDuanList != evertduan)
                            num = newaddselfn0 + num;
                        first = first + "\r\n" + iq.ToString() + "段=" + " " + num;

                        showSuijiResultlist.Add(iq.ToString() + " 段= " + " " + num);

                    }
                    List<string> showSuijiResultlist1 = new List<string>();

                    for (int ii = 0; ii < showSuijiResultlist.Count; ii++)
                    {
                        for (int i = 0; i < newlist.Count; i++)
                        {
                            showSuijiResultlist[ii] = showSuijiResultlist[ii] + " " + newlist[i];
                            newlist.RemoveAt(i);
                            //  showSuijiResultlist1.Add(showSuijiResultlist[ii]);

                            break;
                        }
                    }
                }

                //    showSuijiResultlist.Add(first);

                //}


                this.listBox3.DataSource = showSuijiResultlist;



                //int[,] rst = new int[3, 2];
                //int times = 0;
                //for (int i = 0, j = 0; i < 1800; i++)
                //{
                //    int newIndex = (new Random()).Next(0, newlist.Count - 1);
                //    int newValue = newlist[newIndex];
                //    newlist.RemoveAt(newIndex);
                //    times++;
                //    if (times % 2 == 0)
                //    {
                //        j++;
                //    }
                //    rst[j, times % 2] = newValue;
                //}
                //     string[] array = mySplit(1000, 11);

                // this.listBox3.DataSource = array;
                //int number = 10;//人数

                //int groups = Convert.ToInt32(comboBox1.Text);  //组数
                //  NewMethod();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }
        }

        private List<int> ZidingyiMeiDuanGeshu()
        {
            List<int> EverDuanList = new List<int>();

            if (this.comboBox3.Text != "")
            {
                if (this.comboBox1.Text == "2")
                {
                    //if (this.comboBox3.Text == "46 模板")
                    //{
                    //    EverDuanList.Add(4);
                    //    EverDuanList.Add(6);
                    //}
                    //else if (this.comboBox3.Text == "28 模板")
                    //{
                    //    EverDuanList.Add(2);
                    //    EverDuanList.Add(8);
                    //}
                    //else if (this.comboBox3.Text == "37 模板")
                    //{
                    //    EverDuanList.Add(3);
                    //    EverDuanList.Add(7);
                    //}

                    //20230427 新变更

                    if (this.comboBox3.Text == "64 模板")
                    {
                        EverDuanList.Add(6);
                        EverDuanList.Add(4);

                    }
                    else if (this.comboBox3.Text == "82 模板")
                    {
                        EverDuanList.Add(8);
                        EverDuanList.Add(2);

                    }
                    else if (this.comboBox3.Text == "73 模板")
                    {
                        EverDuanList.Add(7);
                        EverDuanList.Add(3);

                    }

                    else if (this.comboBox3.Text == "17_16 模板")
                    {
                        EverDuanList.Add(17);
                        EverDuanList.Add(16);

                    }
                }
                else if (this.comboBox1.Text == "3")
                {
                    if (this.comboBox3.Text == "532 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                    }
                    else if (this.comboBox3.Text == "622 模板")
                    {
                        EverDuanList.Add(6);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }
                    else if (this.comboBox3.Text == "442 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(2);
                    }
                    //0409
                    else if (this.comboBox3.Text == "541 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(4);
                        EverDuanList.Add(1);
                    }

                    else if (this.comboBox3.Text == "631 模板")
                    {
                        EverDuanList.Add(6);
                        EverDuanList.Add(3);
                        EverDuanList.Add(1);
                    }
                    //new 0621
                    else if (this.comboBox3.Text == "721 模板")
                    {
                        EverDuanList.Add(7);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                    }
                    else if (this.comboBox3.Text == "11_11_11 模板")
                    {
                        EverDuanList.Add(11);
                        EverDuanList.Add(11);
                        EverDuanList.Add(11);
                    }

                }
                else if (this.comboBox1.Text == "4")
                {
                    if (this.comboBox3.Text == "4222 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }

                    //new 0409
                    if (this.comboBox3.Text == "4411 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "3331 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "4321 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                    }
                    //new  20230427  
                    if (this.comboBox3.Text == "5221 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                    }

                    if (this.comboBox3.Text == "5311 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(3);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    //new 0621
                    if (this.comboBox3.Text == "7111 模板")
                    {
                        EverDuanList.Add(7);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "6211 模板")
                    {
                        EverDuanList.Add(6);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "9_8_8_8 模板")
                    {
                        EverDuanList.Add(9);
                        EverDuanList.Add(8);
                        EverDuanList.Add(8);
                        EverDuanList.Add(8);
                    }
                }
                //new 0621
                else if (this.comboBox1.Text == "5")
                {
                    //new 0621
                    if (this.comboBox3.Text == "52111 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "61111 模板")
                    {
                        EverDuanList.Add(6);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }

                    if (this.comboBox3.Text == "42211 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "32221 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "43111 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(3);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "7_7_7_6_6 模板")
                    {
                        EverDuanList.Add(7);
                        EverDuanList.Add(7);
                        EverDuanList.Add(7);
                        EverDuanList.Add(6);
                        EverDuanList.Add(6);
                    }
                }
                //new 0621
                else if (this.comboBox1.Text == "6")
                {
                    //new 0621
                    if (this.comboBox3.Text == "511111 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "421111 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "331111 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "666_555 模板")
                    {
                        EverDuanList.Add(6);
                        EverDuanList.Add(6);
                        EverDuanList.Add(6);
                        EverDuanList.Add(5);
                        EverDuanList.Add(5);
                        EverDuanList.Add(5);
                    }
                }
                //new 0621
                else if (this.comboBox1.Text == "7")
                {
                    if (this.comboBox3.Text == "4111111 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "55555_44 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(5);
                        EverDuanList.Add(5);
                        EverDuanList.Add(5);
                        EverDuanList.Add(5);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                    }

                }
                //new 0621
                else if (this.comboBox1.Text == "8")
                {
                    //new 0621
                    if (this.comboBox3.Text == "31111111 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                        EverDuanList.Add(1);
                    }
                    if (this.comboBox3.Text == "5_4444444 模板")
                    {
                        EverDuanList.Add(5);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                    }
                }
                //new 20240311
                else if (this.comboBox1.Text == "9")
                {
                    //new 0621
                    if (this.comboBox3.Text == "444444_333 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "10")
                {
                    //new 0621
                    if (this.comboBox3.Text == "444_3333333 模板")
                    {
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(4);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "11")
                {
                    //new 0621
                    if (this.comboBox3.Text == "33333333333_ 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "12")
                {
                    //new 0621
                    if (this.comboBox3.Text == "333333333_222 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "13")
                {
                    //new 0621
                    if (this.comboBox3.Text == "3333333_222222 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "14")
                {
                    //new 0621
                    if (this.comboBox3.Text == "33333_222222222 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "15")
                {
                    //new 0621
                    if (this.comboBox3.Text == "333_222222222222 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "16")
                {
                    //new 0621
                    if (this.comboBox3.Text == "3_222222222222222 模板")
                    {
                        EverDuanList.Add(3);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                    }

                }
                //new 20240311
                else if (this.comboBox1.Text == "17")
                {
                    //new 0621
                    if (this.comboBox3.Text == "2222222222222222_1 模板")
                    {
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(2);
                        EverDuanList.Add(1);
                    }

                }
            }
            return EverDuanList;

        }

        private void NewMethod()
        {
            int number = 10;//人数

            int groups = 7;//组数
            string[] strArr = Group(number, groups);

            for (int i = 0; i < strArr.Length; i++)
            {
                showSuijiResultlist.Add(i + 1 + "段=" + strArr[i]);
                //Console.WriteLine("第" + (i + 1) + "组 " + strArr[i]);
            }
            this.listBox3.DataSource = showSuijiResultlist;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {


            this.Close();

        }

        static string mySplit(int num, int n)//num为要拆分的数字，n为分段数
        {
            n = 5;

            int[] rules = new int[] { 5, 3, 2, 1 };
            string temp = num.ToString();
            int count = 0;
            int j = Array.IndexOf(rules, int.Parse(temp[0].ToString()));//取第一位
            if (j == -1 || !System.Text.RegularExpressions.Regex.IsMatch(temp, @"^\d0*$"))
                return "Error";
            else
            {
                string zero = temp.Substring(1);
                List<string> list = new List<string>();
                while (count < n)
                {
                    list.Add(temp);
                    j++;
                    if (j >= rules.Length)
                    {
                        j = 0;
                        if (zero.Length > 0) zero = zero.Substring(0, zero.Length - 1);
                        else break;
                    }
                    temp = rules[j].ToString() + zero;
                    count++;
                }
                string[] array = list.ToArray();
                //  this.listBox3.DataSource = array;
                // Array.Reverse(array);
                return String.Join(",", array);
            }
        }
        //static void Main(string[] args)
        //{
        //    Console.WriteLine(mySplit(1000, 11));//测试，10000分11段
        //    Console.ReadKey();
        //}

        static string[] Group(int number, int groups)
        {

            List<int> list = new List<int>();

            int num = number / groups;

            string[] strArr = new string[groups];

            for (int i = 1; i <= number; i++)

                list.Add(i);

            for (int i = 0; i < groups; i++)
            {

                for (int j = 0; j < num; j++)
                {

                    int value = list[new Random((int)DateTime.Now.Ticks).Next(0, list.Count)];

                    list.Remove(value);

                    strArr[i] += value.ToString("D2") + " ";

                    Thread.Sleep(20);

                }

            }

            return strArr;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;


            }

            List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
            FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


            for (int i = 0; i < showSuijiResultlist.Count; i++)
            {
                string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                if (i == 0)
                    item.DuanWei1 = temp1[1].Trim();
                else if (i == 1)
                    item.DuanWei2 = temp1[1].Trim();
                else if (i == 2)
                    item.DuanWei3 = temp1[1].Trim();
                else if (i == 3)
                    item.DuanWei4 = temp1[1].Trim();
                else if (i == 4)
                    item.DuanWei5 = temp1[1].Trim();
                else if (i == 5)
                    item.DuanWei6 = temp1[1].Trim();
                else if (i == 6)
                    item.DuanWei7 = temp1[1].Trim();
                else if (i == 7)
                    item.DuanWei8 = temp1[1].Trim();
                else if (i == 8)
                    item.DuanWei9 = temp1[1].Trim();
                else if (i == 9)
                    item.DuanWei10 = temp1[1].Trim();


                 //20240311
                else if (i == 10)
                    item.DuanWei11 = temp1[1].Trim();
                else if (i == 11)
                    item.DuanWei12 = temp1[1].Trim();
                else if (i == 12)
                    item.DuanWei13 = temp1[1].Trim();
                else if (i == 13)
                    item.DuanWei14 = temp1[1].Trim();
                else if (i == 14)
                    item.DuanWei15 = temp1[1].Trim();
                else if (i == 15)
                    item.DuanWei16 = temp1[1].Trim();
                else if (i == 16)
                    item.DuanWei17 = temp1[1].Trim();


                item.Data = item.Data + "\r\n" + showSuijiResultlist[i];
            }
            if (this.checkBox1.Checked == true)
                item.ZhuJian = "YES";

            if (this.checkBox2.Checked == true)
                item.MorenDuanShu = comboBox1.Text;

            if (this.comboBox3.Text != "")
                item.Mobanleibie = comboBox3.Text;

            item.Name = this.listBox1.Text.ToString();//保存名称
            item.DuanShu = showSuijiResultlist.Count.ToString();

            if (this.checkBox3.Checked == true)
                item.xiangtongxingfenxi = "YES";


            Result.Add(item);
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.Save_FangAn(Result);

            MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {



            clsAllnew BusinessHelp = new clsAllnew();
            List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAn(this.listBox1.Text.ToString());

            showSuijiResultlist = new List<string>();

            foreach (FangAnLieBiaoDATA item in Result)
            {
                if (item.Data == null)
                    continue;

                string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.Data, "\r\n");

                for (int i = 1; i < temp1.Length; i++)
                {
                    showSuijiResultlist.Add(temp1[i]);
                }
                //  
            }


            this.listBox3.DataSource = showSuijiResultlist;

        }

        private void 增加ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName("", 0);
                frmAddFanAnName.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName("", 0);
            }
            frmAddFanAnName.Show();


        }

        void FrmOMS_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is frmAddFanAnName)
            {
                InitialSystemInfo();
                frmAddFanAnName = null;
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            {
                if (this.listBox1.Text == null)
                {
                    MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


                }

                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


                for (int i = 0; i < showSuijiResultlist.Count; i++)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                    if (i == 0)
                        item.DuanWei1 = temp1[1].Trim();
                    else if (i == 1)
                        item.DuanWei2 = temp1[1].Trim();
                    else if (i == 2)
                        item.DuanWei3 = temp1[1].Trim();
                    else if (i == 3)
                        item.DuanWei4 = temp1[1].Trim();
                    else if (i == 4)
                        item.DuanWei5 = temp1[1].Trim();
                    else if (i == 5)
                        item.DuanWei6 = temp1[1].Trim();
                    else if (i == 6)
                        item.DuanWei7 = temp1[1].Trim();
                    else if (i == 7)
                        item.DuanWei8 = temp1[1].Trim();
                    else if (i == 8)
                        item.DuanWei9 = temp1[1].Trim();
                    else if (i == 9)
                        item.DuanWei10 = temp1[1].Trim();

                     //20240311
                    else if (i == 10)
                        item.DuanWei11 = temp1[1].Trim();
                    else if (i == 11)
                        item.DuanWei12 = temp1[1].Trim();
                    else if (i == 12)
                        item.DuanWei13 = temp1[1].Trim();
                    else if (i == 13)
                        item.DuanWei14 = temp1[1].Trim();
                    else if (i == 14)
                        item.DuanWei15 = temp1[1].Trim();
                    else if (i == 15)
                        item.DuanWei16 = temp1[1].Trim();
                    else if (i == 16)
                        item.DuanWei17 = temp1[1].Trim();



                    item.Data = item.Data + "\r\n" + showSuijiResultlist[i];
                }
                if (this.checkBox1.Checked == true)
                    item.ZhuJian = "YES";
                if (this.checkBox2.Checked == true)
                    item.MorenDuanShu = comboBox1.Text;

                item.Name = DateTime.Now.ToString("yyyyMMddHHmmss");//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                InitialSystemInfo();
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择方案名称！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.delete_FangAn(this.listBox1.Text);
            MessageBox.Show("删除{0}成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InitialSystemInfo();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            {
                if (this.listBox1.Text == null)
                {
                    MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


                }

                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


                for (int i = 0; i < showSuijiResultlist.Count; i++)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                    if (i == 0)
                        item.DuanWei1 = "";
                    else if (i == 1)
                        item.DuanWei2 = "";
                    else if (i == 2)
                        item.DuanWei3 = "";
                    else if (i == 3)
                        item.DuanWei4 = "";
                    else if (i == 4)
                        item.DuanWei5 = "";
                    else if (i == 5)
                        item.DuanWei6 = "";
                    else if (i == 6)
                        item.DuanWei7 = "";
                    else if (i == 7)
                        item.DuanWei8 = "";
                    else if (i == 8)
                        item.DuanWei9 = "";
                    else if (i == 9)
                        item.DuanWei10 = "";

                     //20240311
                    else if (i == 10)
                        item.DuanWei11 = "";
                    else if (i == 11)
                        item.DuanWei12 = "";
                    else if (i == 12)
                        item.DuanWei13 = "";
                    else if (i == 13)
                        item.DuanWei14 = "";
                    else if (i == 14)
                        item.DuanWei15 = "";
                    else if (i == 15)
                        item.DuanWei16 = "";
                    else if (i == 16)
                        item.DuanWei17 = "";

                    item.Data = "";
                }
                if (this.checkBox1.Checked == true)
                    item.ZhuJian = "YES";

                if (this.checkBox2.Checked == true)
                    item.MorenDuanShu = comboBox1.Text;

                item.Name = this.listBox1.Text.ToString();//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }

        }

        private void 改名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName(this.listBox1.Text, 0);
                frmAddFanAnName.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName(this.listBox1.Text, 0);
            }
            frmAddFanAnName.Show();


        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (this.listBox1.Text == null)
                {
                    MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


                }

                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


                for (int i = 0; i < showSuijiResultlist.Count; i++)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                    if (i == 0)
                        item.DuanWei1 = "";
                    else if (i == 1)
                        item.DuanWei2 = "";
                    else if (i == 2)
                        item.DuanWei3 = "";
                    else if (i == 3)
                        item.DuanWei4 = "";
                    else if (i == 4)
                        item.DuanWei5 = "";
                    else if (i == 5)
                        item.DuanWei6 = "";
                    else if (i == 6)
                        item.DuanWei7 = "";
                    else if (i == 7)
                        item.DuanWei8 = "";
                    else if (i == 8)
                        item.DuanWei9 = "";
                    else if (i == 9)
                        item.DuanWei10 = "";

                   //20240311
                    else if (i == 10)
                        item.DuanWei11 = "";
                    else if (i == 11)
                        item.DuanWei12 = "";
                    else if (i == 12)
                        item.DuanWei13 = "";
                    else if (i == 13)
                        item.DuanWei14 = "";
                    else if (i == 14)
                        item.DuanWei15 = "";
                    else if (i == 15)
                        item.DuanWei16 = "";
                    else if (i == 16)
                        item.DuanWei17 = "";
                    item.Data = "";
                }
                if (this.checkBox1.Checked == true)
                    item.ZhuJian = "YES";

                if (this.checkBox2.Checked == true)
                    item.MorenDuanShu = comboBox1.Text;

                item.Name = this.listBox1.Text.ToString();//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            comboBox3.Items.Add("默认");
            if (this.comboBox1.Text == "2")
            {
                // comboBox3.Items.Add("46 模板");
                // comboBox3.Items.Add("28 模板");
                // comboBox3.Items.Add("37 模板");
                //20230427变更
                comboBox3.Items.Add("64 模板");
                comboBox3.Items.Add("82 模板");
                comboBox3.Items.Add("73 模板");
                //20240311
                comboBox3.Items.Add("17_16 模板");

            }
            else if (this.comboBox1.Text == "3")
            {

                comboBox3.Items.Add("532 模板");
                comboBox3.Items.Add("622 模板");
                comboBox3.Items.Add("442 模板");
                //new 0409
                comboBox3.Items.Add("541 模板");
                comboBox3.Items.Add("631 模板");
                //new 0619
                comboBox3.Items.Add("721 模板");
                //20240311
                comboBox3.Items.Add("11_11_11 模板");

            }
            else if (this.comboBox1.Text == "4")
            {

                comboBox3.Items.Add("4222 模板");
                //new 0409
                comboBox3.Items.Add("4411 模板");
                comboBox3.Items.Add("3331 模板");
                comboBox3.Items.Add("4321 模板");
                //new  20230427 
                comboBox3.Items.Add("5221 模板");

                comboBox3.Items.Add("5311 模板");
                //new 0619
                comboBox3.Items.Add("6211 模板");
                comboBox3.Items.Add("7111 模板");
                //20240311
                comboBox3.Items.Add("9_8_8_8 模板");

            }
            //new 0619
            else if (this.comboBox1.Text == "5")
            {
                //new 0619
                comboBox3.Items.Add("52111 模板");

                comboBox3.Items.Add("61111 模板");
                comboBox3.Items.Add("42211 模板");
                comboBox3.Items.Add("32221 模板");
                comboBox3.Items.Add("43111 模板");
                //20240311
                comboBox3.Items.Add("7_7_7_6_6 模板");


            }
            //new 0619
            else if (this.comboBox1.Text == "6")
            {
                //new 0619
                comboBox3.Items.Add("511111 模板");

                comboBox3.Items.Add("421111 模板");
                comboBox3.Items.Add("331111 模板");
                //20240311
                comboBox3.Items.Add("666_555 模板");
            }
            //new 0619
            else if (this.comboBox1.Text == "7")
            {
                //new 0619
                comboBox3.Items.Add("4111111 模板");

                //20240311
                comboBox3.Items.Add("55555_44 模板");



            }
            //new 0619
            else if (this.comboBox1.Text == "8")
            {
                //new 0619
                comboBox3.Items.Add("31111111 模板");
                //20240311
                comboBox3.Items.Add("5_4444444 模板");


            }
            //20240311
            else if (this.comboBox1.Text == "9")
            {
                //20240311
                comboBox3.Items.Add("444444_333 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "10")
            {
                //20240311
                comboBox3.Items.Add("444_3333333 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "11")
            {
                //20240311
                comboBox3.Items.Add("33333333333_ 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "12")
            {
                //20240311
                comboBox3.Items.Add("333333333_222 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "13")
            {
                //20240311
                comboBox3.Items.Add("3333333_222222 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "14")
            {
                //20240311
                comboBox3.Items.Add("33333_222222222 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "15")
            {
                //20240311
                comboBox3.Items.Add("333_222222222222 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "16")
            {
                //20240311
                comboBox3.Items.Add("3_222222222222222 模板");

            }
            //20240311
            else if (this.comboBox1.Text == "17")
            {
                //20240311
                comboBox3.Items.Add("2222222222222222_1 模板");

            }


            this.comboBox3.SelectedIndex = 0;
            string amewi = this.textBox1.Text;//\t\r\n
            this.textBox1.Text = "";

            for (int i = 1; i <= Convert.ToInt32(comboBox1.Text); i++)
            {
                if (i == 1)
                    this.textBox1.Text = "一段=";
                if (i == 2)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "二段=";
                if (i == 3)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "三段=";
                if (i == 4)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "四段=";
                if (i == 5)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "五段=";
                if (i == 6)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "六段=";
                if (i == 7)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "七段=";
                if (i == 8)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "八段=";
                if (i == 9)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "九段=";
                if (i == 10)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十段=";
                if (i == 11)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十一段=";
                if (i == 12)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十二段=";
                if (i == 13)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十三段=";
                if (i == 14)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十四段=";
                if (i == 15)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十五段=";
                if (i == 16)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十六段=";
                if (i == 17)
                    this.textBox1.Text = this.textBox1.Text + "\t\r\n" + "十七段=";


            }
            this.textBox1.Text = this.textBox1.Text + "\t\r\n";

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
            FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


            for (int i = 0; i < showSuijiResultlist.Count; i++)
            {
                string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                if (i == 0)
                    item.DuanWei1 = temp1[1].Trim();
                else if (i == 1)
                    item.DuanWei2 = temp1[1].Trim();
                else if (i == 2)
                    item.DuanWei3 = temp1[1].Trim();
                else if (i == 3)
                    item.DuanWei4 = temp1[1].Trim();
                else if (i == 4)
                    item.DuanWei5 = temp1[1].Trim();
                else if (i == 5)
                    item.DuanWei6 = temp1[1].Trim();
                else if (i == 6)
                    item.DuanWei7 = temp1[1].Trim();
                else if (i == 7)
                    item.DuanWei8 = temp1[1].Trim();
                else if (i == 8)
                    item.DuanWei9 = temp1[1].Trim();
                else if (i == 9)
                    item.DuanWei10 = temp1[1].Trim();

                //20240311
                else if (i == 10)
                    item.DuanWei11 = temp1[1].Trim();
                else if (i == 11)
                    item.DuanWei12 = temp1[1].Trim();
                else if (i == 12)
                    item.DuanWei13 = temp1[1].Trim();
                else if (i == 13)
                    item.DuanWei14 = temp1[1].Trim();
                else if (i == 14)
                    item.DuanWei15 = temp1[1].Trim();
                else if (i == 15)
                    item.DuanWei16 = temp1[1].Trim();
                else if (i == 16)
                    item.DuanWei17 = temp1[1].Trim();

                item.Data = item.Data + "\r\n" + showSuijiResultlist[i];
            }
            // if (this.checkBox1.Checked == true)
            item.ZhuJian = "YES";

            // if (this.checkBox2.Checked == true)
            item.MorenDuanShu = comboBox1.Text;

            if (this.comboBox3.Text != "")
                item.Mobanleibie = comboBox3.Text;

            item.Name = this.listBox1.Text.ToString();//保存名称
            item.DuanShu = showSuijiResultlist.Count.ToString();

            if (this.checkBox3.Checked == true)
                item.xiangtongxingfenxi = "YES";


            Result.Add(item);
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.Save_FangAn(Result);

            MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void 自选方案ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            //   this.listBox3.DataSource = showSuijiResultlist;
          //  string nem = listBox3.SelectedItems.ToString();
            string nem = this.listBox3.Text.ToString();
         //   nem = nem.Replace("", "");
            //string list1Remove =  listBox3.SelectedItem.ToString();
            Clipboard.SetText(nem);
        }
        public static void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }

        private void listBox3_Click(object sender, EventArgs e)
        {
            listbox3indexnum = listBox3.SelectedIndex;       
        }

        private void 相同性分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nem = ""; 
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                nem = nem + listBox3.Items[i].ToString();                            
            }
            Clipboard.SetText(nem);
        }
    }
}
