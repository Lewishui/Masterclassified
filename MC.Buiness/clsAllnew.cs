using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MC.DB;
using MC.Common;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;

namespace MC.Buiness
{
    public class clsAllnew
    {
        private BackgroundWorker bgWorker1;
        ////9.110.
        //230.55
        public log4net.ILog ProcessLogger { get; set; }
        public log4net.ILog ExceptionLogger { get; set; }
        #region 导入数据
        public clsAllnew()
        {

            InitialSystemInfo();

        }
        public List<inputCaipiaoDATA> InputclaimReport(ref BackgroundWorker bgWorker, string path)
        {
            try
            {
                List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
                ProcessLogger.Fatal("1006-- Input C Data end" + DateTime.Now.ToString());
                bgWorker1 = bgWorker;
                if (!path.Contains(".txt"))
                    Result = ReadMAPFile(path);
                else if (path.Contains(".txt"))
                    Result = ReadcaipiaoFile_TXT(path);
                if (Result.Count != 0)
                    SPInputclaimreport_Server(Result);

                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                return null;

            }
            return null;
        }
        public List<inputCaipiaoDATA> ReadMAPFile(string path)
        {

            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "Resources\\ALL MU.xls";
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook analyWK = excelApp.Workbooks.Open(path, Type.Missing, true, Type.Missing,
                "htc", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            Microsoft.Office.Interop.Excel.Worksheet WS = (Microsoft.Office.Interop.Excel.Worksheet)analyWK.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range rng;
            rng = WS.get_Range(WS.Cells[2, 1], WS.Cells[WS.UsedRange.Rows.Count, 30]);
            int rowCount = WS.UsedRange.Rows.Count - 1;
            object[,] o = new object[1, 1];
            o = (object[,])rng.Value2;
            clsCommHelp.CloseExcel(excelApp, analyWK);

            for (int i = 1; i <= rowCount; i++)
            {
                bgWorker1.ReportProgress(0, "Input claim Report Data   :  " + i.ToString() + "/" + rowCount.ToString());
                inputCaipiaoDATA temp = new inputCaipiaoDATA();

                temp.QiHao = "";
                if (o[i, 1] != null)
                    temp.QiHao = o[i, 1].ToString().Trim();
                if (temp.QiHao == "" || temp.QiHao == null)
                    continue;

                temp.KaiJianHaoMa = "";
                if (o[i, 2] != null)
                    temp.KaiJianHaoMa = o[i, 2].ToString().Trim();


                temp.Xuan = "";
                if (o[i, 3] != null)
                    temp.Xuan = o[i, 3].ToString().Trim();


                temp.Input_Date = DateTime.Now.ToString("yyyyMMdd-HHmm");
                Result.Add(temp);
            }
            return Result;

        }
        public List<inputCaipiaoDATA> ReadcaipiaoFile_TXT(string path)
        {
            try
            {
                List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();

                string[] fileText = File.ReadAllLines(path);
                for (int i = 1; i < fileText.Length; i++)
                {
                    inputCaipiaoDATA temp = new inputCaipiaoDATA();
                    string a = fileText[i].Trim();

                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(fileText[i], "\t");

                    temp.QiHao = temp1[0].ToString().Trim();
                    temp.KaiJianHaoMa = temp1[1].ToString().Trim();
                    temp.Xuan = temp1[2].ToString().Trim();
                    temp.KaiJianRiqi = clsCommHelp.objToDateTime(temp1[3]);
                    temp.Caipiaomingcheng = temp1[4].ToString().Trim();
                    temp.Input_Date = DateTime.Now.ToString("yyyyMMdd-HHmm");
                    Result.Add(temp);

                }
                return Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                throw;
            }


        }

        public List<CaipiaoZhongLeiDATA> Readcaipiaoleixing_TXT(string path)
        {
            try
            {
                List<CaipiaoZhongLeiDATA> Result = new List<CaipiaoZhongLeiDATA>();

                string[] fileText = File.ReadAllLines(path);
                for (int i = 1; i < fileText.Length; i++)
                {
                    CaipiaoZhongLeiDATA temp = new CaipiaoZhongLeiDATA();
                    string a = fileText[i].Trim();

                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(fileText[i], "\t");

                    temp.Name = temp1[0].ToString().Trim();
                    temp.Caipiaowenjianming = temp1[1].ToString().Trim();
                    temp.JiBenHaoMaS = temp1[2].ToString().Trim();
                    temp.JiBenHaoMaT = temp1[3].ToString().Trim();

                    temp.Check_TeBieHao = temp1[5].ToString().Trim();

                    temp.Xuan = temp1[4].ToString().Trim();
                    temp.TeBieHaoS = temp1[6].ToString().Trim();
                    temp.TeBieHaoT = temp1[7].ToString().Trim();
                    temp.Jiaxuantebiehao = temp1[8].ToString().Trim();


                    //temp.QiHao = temp1[0].ToString().Trim();
                    //temp.KaiJianHaoMa = temp1[1].ToString().Trim();
                    //temp.Xuan = temp1[2].ToString().Trim();
                    //temp.KaiJianRiqi = clsCommHelp.objToDateTime(temp1[3]);
                    //temp.Input_Date = DateTime.Now.ToString("yyyyMMdd-HHmm");
                    Result.Add(temp);

                }
                return Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                throw;
            }


        }

        public void SPInputclaimreport_Server(List<inputCaipiaoDATA> AddMAPResult)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterClassified_CaiPiaoData");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

            //  collection1.RemoveAll();
            if (AddMAPResult == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (inputCaipiaoDATA item in AddMAPResult)
            {
                var dd = Query.And(Query.EQ("QiHao", item.QiHao), Query.EQ("Caipiaomingcheng", item.Caipiaomingcheng));//同时满足多个条件

                // QueryDocument query = new QueryDocument("QiHao", item.QiHao);
                collection1.Remove(dd);

                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterClassified_CaiPiaoData");
                BsonDocument fruit_1 = new BsonDocument
                 { 
                 { "QiHao", item.QiHao },
                 { "Jihao", item.Jihao },
                 { "System_Time", DateTime.Now.ToString("MM/dd/yyyy/HH")}, 
                 { "KaiJianHaoMa", item.KaiJianHaoMa} ,
                  { "KaiJianRiqi", item.KaiJianRiqi} ,
                { "Xuan", item.Xuan} ,
                { "Caipiaomingcheng", item.Caipiaomingcheng} 
                 };
                collection.Insert(fruit_1);
            }
        }

        public void SPInputclaimreport_Server1(List<inputCaipiaoDATA> AddMAPResult, string zhiqianqianqi)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterClassified_CaiPiaoData");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

            //  collection1.RemoveAll();
            if (AddMAPResult == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (inputCaipiaoDATA item in AddMAPResult)
            {
                //var dd = Query.And(Query.GTE("shenheriqi_Valume", DateTime.Parse(findtext + "01")), Query.LTE("Input Date", DateTime.Parse(findtext + "31")));//同时满足多个条件
                var dd = Query.And(Query.EQ("QiHao", zhiqianqianqi), Query.EQ("Caipiaomingcheng", item.Caipiaomingcheng));//同时满足多个条件

                //   QueryDocument query = new QueryDocument("QiHao", zhiqianqianqi);
                collection1.Remove(dd);

                dd = Query.And(Query.EQ("QiHao", item.QiHao), Query.EQ("Caipiaomingcheng", item.Caipiaomingcheng));//同时满足多个条件              
                collection1.Remove(dd);

                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterClassified_CaiPiaoData");
                BsonDocument fruit_1 = new BsonDocument
                 { 
                 { "QiHao", item.QiHao },
                 { "Jihao", item.Jihao },
                 { "System_Time", DateTime.Now.ToString("MM/dd/yyyy/HH")}, 
                 { "KaiJianHaoMa", item.KaiJianHaoMa} ,
                  { "KaiJianRiqi", item.KaiJianRiqi} ,
                { "Xuan", item.Xuan} ,
                { "Caipiaomingcheng", item.Caipiaomingcheng} 
                 };
                collection.Insert(fruit_1);
            }
        }

        public List<inputCaipiaoDATA> InputCaipiaoleixing(ref BackgroundWorker bgWorker, string path)
        {
            try
            {
                List<CaipiaoZhongLeiDATA> Result = new List<CaipiaoZhongLeiDATA>();
                ProcessLogger.Fatal("2006-- Input Caipiaoleixing" + DateTime.Now.ToString());
                bgWorker1 = bgWorker;
                if (!path.Contains(".txt"))
                {
                    MessageBox.Show("错误导入类型文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (path.Contains(".txt"))
                    Result = Readcaipiaoleixing_TXT(path);
                if (Result.Count != 0)
                    Save_CaiPiaoZhongLei(Result);

                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                return null;

            }
            return null;
        }
        #endregion

        #region 读取彩票数据
        public List<inputCaipiaoDATA> ReadclaimreportfromServer()
        {

            #region Read  database info server
            try
            {
                List<inputCaipiaoDATA> ClaimReport_Server = new List<inputCaipiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterClassified_CaiPiaoData");
                MongoCollection<BsonDocument> employees = db.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

                foreach (BsonDocument emp in employees.FindAll())
                {
                    inputCaipiaoDATA item = new inputCaipiaoDATA();

                    #region 数据
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());
                    if (emp.Contains("QiHao"))
                        item.QiHao = (emp["QiHao"].AsString);
                    if (emp.Contains("Jihao"))
                        item.Jihao = (emp["Jihao"].ToString());
                    if (emp.Contains("KaiJianHaoMa"))
                        item.KaiJianHaoMa = (emp["KaiJianHaoMa"].AsString);
                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);
                    if (emp.Contains("JiShu1"))
                        item.JiShu1 = (emp["JiShu1"].AsString);
                    if (emp.Contains("JiShu2"))
                        item.JiShu2 = (emp["JiShu2"].AsString);
                    if (emp.Contains("JiShu3"))
                        item.JiShu3 = (emp["JiShu3"].AsString);
                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);

                    if (emp.Contains("Caipiaomingcheng"))
                        item.Caipiaomingcheng = (emp["Caipiaomingcheng"].AsString);
                    #endregion
                    ClaimReport_Server.Add(item);
                }
                return ClaimReport_Server;

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return null;

                throw ex;
            }
            #endregion
        }
        public List<inputCaipiaoDATA> ReadclaimreportfromServerBy_Xuan(string findtext)
        {

            #region Read  database info server
            try
            {
                List<inputCaipiaoDATA> ClaimReport_Server = new List<inputCaipiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterClassified_CaiPiaoData");
                MongoCollection<BsonDocument> employees = db.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

                var query = new QueryDocument("Caipiaomingcheng", findtext);

                foreach (BsonDocument emp in employees.Find(query))
                {
                    inputCaipiaoDATA item = new inputCaipiaoDATA();

                    #region 数据
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());
                    if (emp.Contains("QiHao"))
                        item.QiHao = (emp["QiHao"].AsString);
                    if (emp.Contains("Jihao"))
                        item.Jihao = (emp["Jihao"].ToString());
                    if (emp.Contains("KaiJianHaoMa"))
                        item.KaiJianHaoMa = (emp["KaiJianHaoMa"].AsString);
                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);
                    if (emp.Contains("JiShu1"))
                        item.JiShu1 = (emp["JiShu1"].AsString);
                    if (emp.Contains("JiShu2"))
                        item.JiShu2 = (emp["JiShu2"].AsString);
                    if (emp.Contains("JiShu3"))
                        item.JiShu3 = (emp["JiShu3"].AsString);
                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);
                    if (emp.Contains("KaiJianRiqi"))
                        item.KaiJianRiqi = clsCommHelp.objToDateTime(emp["KaiJianRiqi"].AsString);

                    if (emp.Contains("Caipiaomingcheng"))
                        item.Caipiaomingcheng = (emp["Caipiaomingcheng"].AsString);
                    #endregion
                    ClaimReport_Server.Add(item);
                }
                return ClaimReport_Server;

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return null;

                throw ex;
            }
            #endregion
        }


        public List<inputCaipiaoDATA> ReadCaiPiaoData_One(string findtext, string Caipiaomingcheng)
        {

            #region Read  database info server
            try
            {
                List<inputCaipiaoDATA> ClaimReport_Server = new List<inputCaipiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterClassified_CaiPiaoData");
                MongoCollection<BsonDocument> employees = db.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

                //     var query = new QueryDocument("QiHao", findtext);
                var dd = Query.And(Query.EQ("QiHao", findtext), Query.EQ("Caipiaomingcheng", Caipiaomingcheng));//同时满足多个条件


                foreach (BsonDocument emp in employees.Find(dd))

                //foreach (BsonDocument emp in employees.Find(query))
                {
                    inputCaipiaoDATA item = new inputCaipiaoDATA();

                    #region 数据
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());
                    if (emp.Contains("QiHao"))
                        item.QiHao = (emp["QiHao"].AsString);
                    if (emp.Contains("Jihao"))
                        item.Jihao = (emp["Jihao"].ToString());
                    if (emp.Contains("KaiJianHaoMa"))
                        item.KaiJianHaoMa = (emp["KaiJianHaoMa"].AsString);
                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);
                    if (emp.Contains("JiShu1"))
                        item.JiShu1 = (emp["JiShu1"].AsString);
                    if (emp.Contains("JiShu2"))
                        item.JiShu2 = (emp["JiShu2"].AsString);
                    if (emp.Contains("JiShu3"))
                        item.JiShu3 = (emp["JiShu3"].AsString);
                    if (emp.Contains("KaiJianRiqi"))
                        item.KaiJianRiqi = (emp["KaiJianRiqi"].AsString);

                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);


                    if (emp.Contains("Caipiaomingcheng"))
                        item.Caipiaomingcheng = (emp["Caipiaomingcheng"].AsString);
                    #endregion
                    ClaimReport_Server.Add(item);
                }
                return ClaimReport_Server;

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return null;

                throw ex;
            }
            #endregion
        }

        public List<FangAnLieBiaoDATA> Read_AllFangAn()
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");


                foreach (BsonDocument emp in employees1.FindAll())
                {

                    FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();

                    #region 数据聚合
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);
                    if (emp.Contains("DuanShu"))
                        item.DuanShu = (emp["DuanShu"].ToString());

                    if (emp.Contains("Data"))
                        item.Data = (emp["Data"].AsString);

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);

                    if (emp.Contains("DuanWei1"))
                        item.DuanWei1 = (emp["DuanWei1"].AsString);

                    if (emp.Contains("DuanWei2"))
                        item.DuanWei2 = (emp["DuanWei2"].AsString);

                    if (emp.Contains("DuanWei3"))
                        item.DuanWei3 = (emp["DuanWei3"].AsString);

                    if (emp.Contains("DuanWei4"))
                        item.DuanWei4 = (emp["DuanWei4"].AsString);

                    if (emp.Contains("DuanWei5"))
                        item.DuanWei5 = (emp["DuanWei5"].AsString);

                    if (emp.Contains("DuanWei6"))
                        item.DuanWei6 = (emp["DuanWei6"].AsString);

                    if (emp.Contains("DuanWei7"))
                        item.DuanWei7 = (emp["DuanWei7"].AsString);

                    if (emp.Contains("DuanWei8"))
                        item.DuanWei8 = (emp["DuanWei8"].AsString);

                    if (emp.Contains("DuanWei9"))
                        item.DuanWei9 = (emp["DuanWei9"].AsString);

                    if (emp.Contains("DuanWei10"))
                        item.DuanWei10 = (emp["DuanWei10"].AsString);

                    if (emp.Contains("ZhuJian"))
                        item.ZhuJian = (emp["ZhuJian"].AsString);
                    if (emp.Contains("MorenDuanShu"))
                        item.MorenDuanShu = (emp["MorenDuanShu"].AsString);

                    if (emp.Contains("Mobanleibie"))
                        item.Mobanleibie = (emp["Mobanleibie"].AsString);
                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public List<FangAnLieBiaoDATA> Read_Piliang_AllFangAn()
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterPiLiangFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterPiLiangFangAn");


                foreach (BsonDocument emp in employees1.FindAll())
                {

                    FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();

                    #region 数据聚合
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);
                    if (emp.Contains("DuanShu"))
                        item.DuanShu = (emp["DuanShu"].ToString());

                    if (emp.Contains("Data"))
                        item.Data = (emp["Data"].AsString);

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);

                    if (emp.Contains("DuanWei1"))
                        item.DuanWei1 = (emp["DuanWei1"].AsString);

                    if (emp.Contains("DuanWei2"))
                        item.DuanWei2 = (emp["DuanWei2"].AsString);

                    if (emp.Contains("DuanWei3"))
                        item.DuanWei3 = (emp["DuanWei3"].AsString);

                    if (emp.Contains("DuanWei4"))
                        item.DuanWei4 = (emp["DuanWei4"].AsString);

                    if (emp.Contains("DuanWei5"))
                        item.DuanWei5 = (emp["DuanWei5"].AsString);

                    if (emp.Contains("DuanWei6"))
                        item.DuanWei6 = (emp["DuanWei6"].AsString);

                    if (emp.Contains("DuanWei7"))
                        item.DuanWei7 = (emp["DuanWei7"].AsString);

                    if (emp.Contains("DuanWei8"))
                        item.DuanWei8 = (emp["DuanWei8"].AsString);

                    if (emp.Contains("DuanWei9"))
                        item.DuanWei9 = (emp["DuanWei9"].AsString);

                    if (emp.Contains("DuanWei10"))
                        item.DuanWei10 = (emp["DuanWei10"].AsString);

                    if (emp.Contains("ZhuJian"))
                        item.ZhuJian = (emp["ZhuJian"].AsString);
                    if (emp.Contains("MorenDuanShu"))
                        item.MorenDuanShu = (emp["MorenDuanShu"].AsString);

                    if (emp.Contains("Mobanleibie"))
                        item.Mobanleibie = (emp["Mobanleibie"].AsString);
                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public List<FangAnLieBiaoDATA> Read_FangAn(string findtext)
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");

                var query = new QueryDocument("Name", findtext);
                if (findtext == "YES")
                    query = new QueryDocument("ZhuJian", findtext);
                foreach (BsonDocument emp in employees1.Find(query))
                {

                    FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);
                    if (emp.Contains("DuanShu"))
                        item.DuanShu = (emp["DuanShu"].ToString());

                    if (emp.Contains("Data"))
                        item.Data = (emp["Data"].AsString);

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);

                    if (emp.Contains("DuanWei1"))
                        item.DuanWei1 = (emp["DuanWei1"].AsString);

                    if (emp.Contains("DuanWei2"))
                        item.DuanWei2 = (emp["DuanWei2"].AsString);

                    if (emp.Contains("DuanWei3"))
                        item.DuanWei3 = (emp["DuanWei3"].AsString);

                    if (emp.Contains("DuanWei4"))
                        item.DuanWei4 = (emp["DuanWei4"].AsString);

                    if (emp.Contains("DuanWei5"))
                        item.DuanWei5 = (emp["DuanWei5"].AsString);

                    if (emp.Contains("DuanWei6"))
                        item.DuanWei6 = (emp["DuanWei6"].AsString);

                    if (emp.Contains("DuanWei7"))
                        item.DuanWei7 = (emp["DuanWei7"].AsString);

                    if (emp.Contains("DuanWei8"))
                        item.DuanWei8 = (emp["DuanWei8"].AsString);

                    if (emp.Contains("DuanWei9"))
                        item.DuanWei9 = (emp["DuanWei9"].AsString);

                    if (emp.Contains("DuanWei10"))
                        item.DuanWei10 = (emp["DuanWei10"].AsString);

                    if (emp.Contains("ZhuJian"))
                        item.ZhuJian = (emp["ZhuJian"].AsString);
                    if (emp.Contains("MorenDuanShu"))
                        item.MorenDuanShu = (emp["MorenDuanShu"].AsString);

                    if (emp.Contains("Mobanleibie"))
                        item.Mobanleibie = (emp["Mobanleibie"].AsString);
                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public List<FangAnLieBiaoDATA> Read_Piliang_FangAn(string findtext)
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterPiLiangFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterPiLiangFangAn");

                var query = new QueryDocument("Name", findtext);
                if (findtext == "YES")
                    query = new QueryDocument("ZhuJian", findtext);
                foreach (BsonDocument emp in employees1.Find(query))
                {

                    FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);
                    if (emp.Contains("DuanShu"))
                        item.DuanShu = (emp["DuanShu"].ToString());

                    if (emp.Contains("Data"))
                        item.Data = (emp["Data"].AsString);

                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);

                    if (emp.Contains("DuanWei1"))
                        item.DuanWei1 = (emp["DuanWei1"].AsString);

                    if (emp.Contains("DuanWei2"))
                        item.DuanWei2 = (emp["DuanWei2"].AsString);

                    if (emp.Contains("DuanWei3"))
                        item.DuanWei3 = (emp["DuanWei3"].AsString);

                    if (emp.Contains("DuanWei4"))
                        item.DuanWei4 = (emp["DuanWei4"].AsString);

                    if (emp.Contains("DuanWei5"))
                        item.DuanWei5 = (emp["DuanWei5"].AsString);

                    if (emp.Contains("DuanWei6"))
                        item.DuanWei6 = (emp["DuanWei6"].AsString);

                    if (emp.Contains("DuanWei7"))
                        item.DuanWei7 = (emp["DuanWei7"].AsString);

                    if (emp.Contains("DuanWei8"))
                        item.DuanWei8 = (emp["DuanWei8"].AsString);

                    if (emp.Contains("DuanWei9"))
                        item.DuanWei9 = (emp["DuanWei9"].AsString);

                    if (emp.Contains("DuanWei10"))
                        item.DuanWei10 = (emp["DuanWei10"].AsString);

                    if (emp.Contains("ZhuJian"))
                        item.ZhuJian = (emp["ZhuJian"].AsString);
                    if (emp.Contains("MorenDuanShu"))
                        item.MorenDuanShu = (emp["MorenDuanShu"].AsString);

                    if (emp.Contains("Mobanleibie"))
                        item.Mobanleibie = (emp["Mobanleibie"].AsString);
                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        public List<FangAnLieBiaoDATA> Read_FangAnName()
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");


                foreach (BsonDocument emp in employees1.FindAll())
                {

                    FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);


                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public List<FangAnLieBiaoDATA> Read_Piliang_FangAnName()
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterPiLiangFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterPiLiangFangAn");


                foreach (BsonDocument emp in employees1.FindAll())
                {

                    FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);


                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public List<CaipiaoZhongLeiDATA> Read_CaiPiaoZhongLei()
        {
            #region Read  database info server
            try
            {
                List<CaipiaoZhongLeiDATA> Result = new List<CaipiaoZhongLeiDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");


                foreach (BsonDocument emp in employees1.FindAll())
                {

                    CaipiaoZhongLeiDATA item = new CaipiaoZhongLeiDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("MoRenXuanzhong"))
                        item.MoRenXuanzhong = (emp["MoRenXuanzhong"].AsString);
                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        public List<CaipiaoZhongLeiDATA> Read_CaiPiaoZhongLei_Moren(string findtext)
        {
            #region Read  database info server
            try
            {
                List<CaipiaoZhongLeiDATA> Result = new List<CaipiaoZhongLeiDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");

                var query = new QueryDocument("MoRenXuanzhong", findtext);

                foreach (BsonDocument emp in employees1.Find(query))
                {

                    CaipiaoZhongLeiDATA item = new CaipiaoZhongLeiDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);

                    if (emp.Contains("MoRenXuanzhong"))
                        item.MoRenXuanzhong = (emp["MoRenXuanzhong"].AsString);



                    if (emp.Contains("JiBenHaoMaS"))
                        item.JiBenHaoMaS = (emp["JiBenHaoMaS"].AsString);

                    if (emp.Contains("JiBenHaoMaT"))
                        item.JiBenHaoMaT = (emp["JiBenHaoMaT"].AsString);

                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);

                    if (emp.Contains("Check_TeBieHao"))
                        item.Check_TeBieHao = (emp["Check_TeBieHao"].AsString);

                    if (emp.Contains("TeBieHaoS"))
                        item.TeBieHaoS = (emp["TeBieHaoS"].AsString);

                    if (emp.Contains("TeBieHaoT"))
                        item.TeBieHaoT = (emp["TeBieHaoT"].AsString);

                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);

                    if (emp.Contains("Caipiaowenjianming"))
                        item.Caipiaowenjianming = (emp["Caipiaowenjianming"].ToString());

                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        public List<CaipiaoZhongLeiDATA> Find_CaipiaoZhongLei_(string findtext)
        {
            #region Read  database info server
            try
            {
                List<CaipiaoZhongLeiDATA> Result = new List<CaipiaoZhongLeiDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");

                var query = new QueryDocument("Name", findtext);

                foreach (BsonDocument emp in employees1.Find(query))
                {

                    CaipiaoZhongLeiDATA item = new CaipiaoZhongLeiDATA();

                    #region 数据聚合
                    if (emp.Contains("Name"))
                        item.Name = (emp["Name"].AsString);
                    if (emp.Contains("Caipiaowenjianming"))
                        item.Caipiaowenjianming = (emp["Caipiaowenjianming"].ToString());

                    if (emp.Contains("JiBenHaoMaS"))
                        item.JiBenHaoMaS = (emp["JiBenHaoMaS"].AsString);

                    if (emp.Contains("JiBenHaoMaT"))
                        item.JiBenHaoMaT = (emp["JiBenHaoMaT"].AsString);

                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);

                    if (emp.Contains("Check_TeBieHao"))
                        item.Check_TeBieHao = (emp["Check_TeBieHao"].AsString);

                    if (emp.Contains("TeBieHaoS"))
                        item.TeBieHaoS = (emp["TeBieHaoS"].AsString);

                    if (emp.Contains("TeBieHaoT"))
                        item.TeBieHaoT = (emp["TeBieHaoT"].AsString);

                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);

                    #endregion
                    Result.Add(item);
                }
                return Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        private void InitialSystemInfo()
        {
            #region 初始化配置
            ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
            ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
            ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());
            #endregion
        }

        public List<inputCaipiaoDATA> Fast_FindData(string findtext, string Caipiaomingcheng)
        {

            try
            {
                List<inputCaipiaoDATA> ClaimReport_Server = new List<inputCaipiaoDATA>();
                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterClassified_CaiPiaoData");
                MongoCollection<BsonDocument> employees = db.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

                var query = new QueryDocument("QiHao", findtext);

                ///模糊查询
                //   var query1 = Query<inputCaipiaoDATA>.Matches(c => c.QiHao, new BsonRegularExpression(new Regex(findtext)));
                var dd = Query.And(Query.EQ("QiHao", findtext), Query.EQ("Caipiaomingcheng", Caipiaomingcheng));//同时满足多个条件

                //  var data = db.GetCollection("MasterClassified_CaiPiaoData").Find(dd);

                //foreach (var emp in data)
                foreach (BsonDocument emp in employees.Find(dd))
                {
                    inputCaipiaoDATA item = new inputCaipiaoDATA();

                    #region 数据
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());
                    if (emp.Contains("QiHao"))
                        item.QiHao = (emp["QiHao"].AsString);
                    if (emp.Contains("Jihao"))
                        item.Jihao = (emp["Jihao"].ToString());
                    if (emp.Contains("KaiJianHaoMa"))
                        item.KaiJianHaoMa = (emp["KaiJianHaoMa"].AsString);
                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);
                    if (emp.Contains("JiShu1"))
                        item.JiShu1 = (emp["JiShu1"].AsString);
                    if (emp.Contains("JiShu2"))
                        item.JiShu2 = (emp["JiShu2"].AsString);
                    if (emp.Contains("JiShu3"))
                        item.JiShu3 = (emp["JiShu3"].AsString);
                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);
                    if (emp.Contains("KaiJianRiqi"))
                        item.KaiJianRiqi = clsCommHelp.objToDateTime(emp["KaiJianRiqi"].AsString);

                    if (emp.Contains("Caipiaomingcheng"))
                        item.Caipiaomingcheng = (emp["Caipiaomingcheng"].AsString);
                    #endregion
                    ClaimReport_Server.Add(item);
                }
                query = new QueryDocument("KaiJianHaoMa", findtext);

                ///模糊查询
                //query1 = Query<inputCaipiaoDATA>.Matches(c => c.KaiJianHaoMa, new BsonRegularExpression(new Regex(findtext)));
                dd = Query.And(Query.EQ("KaiJianHaoMa", findtext), Query.EQ("Caipiaomingcheng", Caipiaomingcheng));//同时满足多个条件

                // data = db.GetCollection("MasterClassified_CaiPiaoData").Find(dd);

                //foreach (var emp in data)
                foreach (BsonDocument emp in employees.Find(dd))
                {
                    inputCaipiaoDATA item = new inputCaipiaoDATA();

                    #region 数据
                    if (emp.Contains("_id"))
                        item._id = (emp["_id"].ToString());
                    if (emp.Contains("QiHao"))
                        item.QiHao = (emp["QiHao"].AsString);
                    if (emp.Contains("Jihao"))
                        item.Jihao = (emp["Jihao"].ToString());
                    if (emp.Contains("KaiJianHaoMa"))
                        item.KaiJianHaoMa = (emp["KaiJianHaoMa"].AsString);
                    if (emp.Contains("Input_Date"))
                        item.Input_Date = (emp["Input_Date"].AsString);
                    if (emp.Contains("JiShu1"))
                        item.JiShu1 = (emp["JiShu1"].AsString);
                    if (emp.Contains("JiShu2"))
                        item.JiShu2 = (emp["JiShu2"].AsString);
                    if (emp.Contains("JiShu3"))
                        item.JiShu3 = (emp["JiShu3"].AsString);
                    if (emp.Contains("Xuan"))
                        item.Xuan = (emp["Xuan"].AsString);
                    if (emp.Contains("KaiJianRiqi"))
                        item.KaiJianRiqi = clsCommHelp.objToDateTime(emp["KaiJianRiqi"].AsString);

                    if (emp.Contains("Caipiaomingcheng"))
                        item.Caipiaomingcheng = (emp["Caipiaomingcheng"].AsString);
                    #endregion
                    ClaimReport_Server.Add(item);
                }
                return ClaimReport_Server;
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常:" + ex, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
                throw ex;
            }


        }


        #endregion

        #region 保存数据

        public void Save_FangAn(List<FangAnLieBiaoDATA> NEWResult)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterFangAn");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");

            if (NEWResult == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (FangAnLieBiaoDATA item in NEWResult)
            {
                QueryDocument query = new QueryDocument("Name", item.Name);
                collection1.Remove(query);

                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterFangAn");
                //更新主键
                if (item.ZhuJian == "YES")
                {
                    QueryDocument query1 = new QueryDocument("ZhuJian", "YES");
                    var update = Update.Set("ZhuJian", "");
                    collection.Update(query1, update);
                }
                if (item.MorenDuanShu != null && item.MorenDuanShu != "")
                {
                    QueryDocument query1 = new QueryDocument("ZhuJian", "YES");
                    var update = Update.Set("MorenDuanShu", "");
                    collection.Update(query1, update);
                }
                //插入新数据
                BsonDocument fruit_1 = new BsonDocument
                 { 
                 { "DuanShu", item.DuanShu },
                 { "Data", item.Data },    
                 { "Name", item.Name }, 
                { "DuanWei", item.DuanShu },
                { "DuanWei1", item.DuanWei1 },
                { "DuanWei2", item.DuanWei2 },
                { "DuanWei3", item.DuanWei3 },
                { "DuanWei4", item.DuanWei4 },
                { "DuanWei5", item.DuanWei5 },
                { "DuanWei6", item.DuanWei6 },
                { "DuanWei7", item.DuanWei7 },
                { "DuanWei8", item.DuanWei8 },
                { "DuanWei9", item.DuanWei9 },
                { "DuanWei10", item.DuanWei10 },
                 { "ZhuJian", item.ZhuJian },  
                { "MorenDuanShu", item.MorenDuanShu },  
                   { "Mobanleibie", item.Mobanleibie },  
                
                { "Input_Date", DateTime.Now.ToString("MM/dd/yyyy/HHss")}  
                 };
                collection.Insert(fruit_1);
            }




        }
        public void Save_Piliang_FangAn(List<FangAnLieBiaoDATA> NEWResult)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterPiLiangFangAn");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterPiLiangFangAn");

            if (NEWResult == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (FangAnLieBiaoDATA item in NEWResult)
            {
                QueryDocument query = new QueryDocument("Name", item.Name);
                collection1.Remove(query);

                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("MasterPiLiangFangAn");
                //更新主键
                if (item.ZhuJian == "YES")
                {
                    QueryDocument query1 = new QueryDocument("ZhuJian", "YES");
                    var update = Update.Set("ZhuJian", "");
                    collection.Update(query1, update);
                }
                if (item.MorenDuanShu != null && item.MorenDuanShu != "")
                {
                    QueryDocument query1 = new QueryDocument("ZhuJian", "YES");
                    var update = Update.Set("MorenDuanShu", "");
                    collection.Update(query1, update);
                }
                //插入新数据
                BsonDocument fruit_1 = new BsonDocument
                 { 
                 { "DuanShu", item.DuanShu },
                 { "Data", item.Data },    
                 { "Name", item.Name }, 
                { "DuanWei", item.DuanShu },
                { "DuanWei1", item.DuanWei1 },
                { "DuanWei2", item.DuanWei2 },
                { "DuanWei3", item.DuanWei3 },
                { "DuanWei4", item.DuanWei4 },
                { "DuanWei5", item.DuanWei5 },
                { "DuanWei6", item.DuanWei6 },
                { "DuanWei7", item.DuanWei7 },
                { "DuanWei8", item.DuanWei8 },
                { "DuanWei9", item.DuanWei9 },
                { "DuanWei10", item.DuanWei10 },
                 { "ZhuJian", item.ZhuJian },  
                { "MorenDuanShu", item.MorenDuanShu },  
                   { "Mobanleibie", item.Mobanleibie },                  
                { "Input_Date", DateTime.Now.ToString("MM/dd/yyyy/HHss")}  
                 };
                collection.Insert(fruit_1);
            }




        }

        public void Update_FangAn(string findtext, string newname)
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");

                var query = new QueryDocument("Name", findtext);

                foreach (BsonDocument emp in employees1.Find(query))
                {
                    {
                        QueryDocument query1 = new QueryDocument("Name", findtext);
                        var update = Update.Set("Name", newname);
                        collection1.Update(query1, update);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public void Update_PiLiang_FangAn(string findtext, string newname)
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("MasterPiLiangFangAn");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterPiLiangFangAn");

                var query = new QueryDocument("Name", findtext);

                foreach (BsonDocument emp in employees1.Find(query))
                {
                    {
                        QueryDocument query1 = new QueryDocument("Name", findtext);
                        var update = Update.Set("Name", newname);
                        collection1.Update(query1, update);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        public void Save_CaiPiaoZhongLei(List<CaipiaoZhongLeiDATA> NEWResult)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");

            if (NEWResult == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (CaipiaoZhongLeiDATA item in NEWResult)
            {
                QueryDocument query = new QueryDocument("Name", item.Name);
                collection1.Remove(query);

                MongoDatabase db = server.GetDatabase("MasterClassified");
                MongoCollection collection = db.GetCollection("CaiPiaoZhongLei");
                //更新主键
                //if (item.ZhuJian == "YES")
                //{
                //    QueryDocument query1 = new QueryDocument("ZhuJian", "YES");
                //    var update = Update.Set("ZhuJian", "");
                //    collection.Update(query1, update);
                //}
                //插入新数据
                BsonDocument fruit_1 = new BsonDocument
                 { 
                 { "Name", item.Name },
                 { "Caipiaowenjianming", item.Caipiaowenjianming },    
                 { "JiBenHaoMaT", item.JiBenHaoMaT }, 
                { "JiBenHaoMaS", item.JiBenHaoMaS },
                { "Xuan", item.Xuan },
                { "Check_TeBieHao", item.Check_TeBieHao },
                { "TeBieHaoS", item.TeBieHaoS },
                { "TeBieHaoT", item.TeBieHaoT },       
                { "Jiaxuantebiehao", item.Jiaxuantebiehao },       
                { "MoRenXuanzhong", "NO" },  
                { "Input_Date", DateTime.Now.ToString("MM/dd/yyyy/HHss")}  
                 };
                collection.Insert(fruit_1);
            }




        }

        public void Update_CaiPiaoZhongLei(string findtext, List<CaipiaoZhongLeiDATA> NEWResult)
        {
            #region Read  database info server
            try
            {
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();

                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");
                QueryDocument query = new QueryDocument("Name", findtext);
                collection1.Remove(query);
                foreach (CaipiaoZhongLeiDATA item in NEWResult)
                {
                    MongoDatabase db = server.GetDatabase("MasterClassified");
                    MongoCollection collection = db.GetCollection("CaiPiaoZhongLei");

                    BsonDocument fruit_1 = new BsonDocument
                 { 
                 { "Name", item.Name },
                 { "Caipiaowenjianming", item.Caipiaowenjianming },    
                 { "JiBenHaoMaT", item.JiBenHaoMaT }, 
                { "JiBenHaoMaS", item.JiBenHaoMaS },
                { "Xuan", item.Xuan },
                { "Check_TeBieHao", item.Check_TeBieHao },
                { "TeBieHaoS", item.TeBieHaoS },
                { "TeBieHaoT", item.TeBieHaoT },       
                 { "Jiaxuantebiehao", item.Jiaxuantebiehao },     
               { "MoRenXuanzhong", item.MoRenXuanzhong },  
                { "Input_Date", DateTime.Now.ToString("MM/dd/yyyy/HHss")}  
                 };
                    collection.Insert(fruit_1);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        public void MoRenUpdate_CaiPiaoZhongLei(string findtext)
        {
            #region Read  database info server
            try
            {
                string connectionString = "mongodb://127.0.0.1";
                MongoServer server = MongoServer.Create(connectionString);
                MongoDatabase db1 = server.GetDatabase("MasterClassified");
                MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
                MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");
                //去掉默认主键
                QueryDocument query1 = new QueryDocument("MoRenXuanzhong", "YES");
                var update = Update.Set("MoRenXuanzhong", "");
                collection1.Update(query1, update);
                query1 = new QueryDocument("MoRenXuanzhong", "NO");
                update = Update.Set("MoRenXuanzhong", "");
                collection1.Update(query1, update);

                //更新默认逐渐


                QueryDocument query11 = new QueryDocument("Name", findtext);
                var update11 = Update.Set("MoRenXuanzhong", "YES");
                collection1.Update(query11, update11);




            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }

        #endregion

        #region 删除数据
        public void delete_FangAn(string name)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterFangAn");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");



            {
                QueryDocument query = new QueryDocument("Name", name);
                collection1.Remove(query);

            }
        }
        public void delete_PiLiang_FangAn(string name)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterPiLiangFangAn");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterPiLiangFangAn");



            {
                QueryDocument query = new QueryDocument("Name", name);
                collection1.Remove(query);

            }
        }
        public void deleteID_FangAn(string name)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterFangAn");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterFangAn");

            if (name == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IMongoQuery query = Query.EQ("_id", new ObjectId(name));

            collection1.Remove(query);
        }




        public void delete_CaiPiaoZhongLei(string name)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("CaiPiaoZhongLei");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("CaiPiaoZhongLei");



            {
                QueryDocument query = new QueryDocument("Name", name);
                collection1.Remove(query);

            }




        }
        public void delete_CaiPiaoData(string name, string Caipiaomingcheng)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterClassified_CaiPiaoData");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

            if (name == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // QueryDocument query = new QueryDocument("QiHao", name);
            var dd = Query.And(Query.EQ("QiHao", name), Query.EQ("Caipiaomingcheng", Caipiaomingcheng));//同时满足多个条件

            collection1.Remove(dd);
        }
        public void delete_CaiPiaoData_Caipiaomingcheng(string Caipiaomingcheng)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterClassified_CaiPiaoData");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

            if (Caipiaomingcheng == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            QueryDocument query = new QueryDocument("Caipiaomingcheng", Caipiaomingcheng);

            collection1.Remove(query);
        }
        public void deleteID_CaiPiaoData(string name)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterClassified_CaiPiaoData");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

            if (name == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //   QueryDocument query = new QueryDocument("ID", name);
            IMongoQuery query = Query.EQ("_id", new ObjectId(name));

            collection1.Remove(query);
        }
        public void Clear_CaiPiaoData(string name)
        {
            string connectionString = "mongodb://127.0.0.1";
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db1 = server.GetDatabase("MasterClassified");
            MongoCollection collection1 = db1.GetCollection("MasterClassified_CaiPiaoData");
            MongoCollection<BsonDocument> employees1 = db1.GetCollection<BsonDocument>("MasterClassified_CaiPiaoData");

            if (name == null)
            {
                MessageBox.Show("No Data  input Sever", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            QueryDocument query = new QueryDocument("Xuan", name);
            collection1.Remove(query);
        }
        #endregion
    }
}
