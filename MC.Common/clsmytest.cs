
using MC.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Threading.Tasks;
//using System.Windows.Forms;

namespace MC.Common
{
    public class clsmytest
    {
        public List<softTime_info> list_Server;
        public  bool checkname(string user,string pass)
        {
            #region Noway
            //bool success = NewMySqlHelper.DbConnectable();

            //if (success == false)
            //{
            //    MessageBox.Show("系统网络异常,请保持网络畅通或联系开发人员 !");
            //    return;
            //}
           
            string strSelect = "select * from control_soft_time where name='" + user + "'"+ " And password = '" + pass + "'";;
            list_Server = new List<softTime_info>();
            list_Server = findsoftTime(strSelect);
            DateTime oldDate = DateTime.Now;
            DateTime dt3;
            string endday = DateTime.Now.ToString("yyyy/MM/dd");
            dt3 = Convert.ToDateTime(endday);
            DateTime dt2;
            if (list_Server.Count == 0 || list_Server[0].endtime == null || list_Server[0].endtime == "")
            {
                MessageBox.Show("系统网络异常,请保持网络畅通或联系开发人员 !");
                return false;
            }
            else
                dt2 = Convert.ToDateTime(list_Server[0].endtime);

            TimeSpan ts = dt2 - dt3;
            int timeTotal = ts.Days;

            if (timeTotal > 0 && timeTotal < 10)
            {
              //  MessageBox.Show("本系统【HTmail】服务即将到期,请及时续费以免影响使用 !\r\n\r\n温馨提示：联系方式网址：www.yhocn.com\r\nQQ：512250428\r\n微信：bqwl07910", "服务到期", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            if (timeTotal < 0)
            {
              //  MessageBox.Show("本系统【HTmail】服务到期,请及时续费 !\r\n\r\n温馨提示：联系方式网址：www.yhocn.com\r\nQQ：512250428\r\n微信：bqwl07910", "服务到期", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Application.Exit();

                //return;
                return false;

            }
            return true;

            #endregion
        }
        public List<softTime_info> findsoftTime(string findtext)
        {
            //    findtext = sqlAddPCID(findtext);
            MySql.Data.MySqlClient.MySqlDataReader reader = NewMySqlHelper.ExecuteReader(findtext);
            List<softTime_info> ClaimReport_Server = new List<softTime_info>();

            while (reader.Read())
            {
                softTime_info item = new softTime_info();
                if (reader.GetValue(0) != null && Convert.ToString(reader.GetValue(0)) != "")
                    item._id = Convert.ToString(reader.GetValue(0));

                if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                    item.name = reader.GetString(1);
                if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                    item.starttime = reader.GetString(2);
                if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                    item.endtime = reader.GetString(3);

                if (reader.GetValue(4) != null && Convert.ToString(reader.GetValue(4)) != "")
                    item.soft_name = reader.GetString(4);

                if (reader.GetValue(5) != null && Convert.ToString(reader.GetValue(5)) != "")
                    item.denglushijian = reader.GetString(5);

                if (reader.GetValue(6) != null && Convert.ToString(reader.GetValue(6)) != "")
                    item.password = reader.GetString(6);

                if (reader.GetValue(7) != null && Convert.ToString(reader.GetValue(7)) != "")
                    item.pid = reader.GetString(7);

                if (reader.GetValue(8) != null && Convert.ToString(reader.GetValue(8)) != "")
                    item.mark1 = reader.GetString(8);

                if (reader.GetValue(9) != null && Convert.ToString(reader.GetValue(9)) != "")
                    item.mark2 = reader.GetString(9);

                if (reader.GetValue(10) != null && Convert.ToString(reader.GetValue(10)) != "")
                    item.mark3 = reader.GetString(10);

                if (reader.GetValue(11) != null && Convert.ToString(reader.GetValue(11)) != "")
                    item.mark4 = reader.GetString(11);

                if (reader.GetValue(12) != null && Convert.ToString(reader.GetValue(12)) != "")
                    item.mark5 = reader.GetString(12);

                ClaimReport_Server.Add(item);

                //这里做数据处理....
            }
            reader.Dispose();
            reader.Close();

           // cmd.Dispose();
            return ClaimReport_Server;
        }
    }
}
