using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MC.Common
{
    public class clsCommHelp
    {
        #region NullToString
        public static string NullToString(object obj)
        {
            string strResult = "";
            if (obj != null)
            {
                strResult = obj.ToString().Trim();
            }
            return strResult;
        }
        #endregion

        #region StringToDecimal
        /// <summary>
        /// 转换字符串，将字符串转换成数字，并且将空字符串转换成0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal StringToDecimal(string s)
        {
            decimal result = 0;

            if (s != null && s != "")
            {
                result = Decimal.Parse(s);
            }
            return result;
        }
        #endregion

        #region StringToInt
        /// <summary>
        /// 转换字符串，将字符串转换成数字，并且将空字符串转换成0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int StringToInt(string s)
        {
            int result = 0;

            if (s != null && s != "")
            {
                result = Convert.ToInt32(s.Trim());
            }
            return result;
        }
        #endregion

        #region 日期转换(objToDateTime)
        /// <summary>
        /// 将excel里取得的日期转化成String数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string objToDateTime<T>(T t)
        {
            string strResult = "";
            object obj = t;

            try
            {
                if (obj != null)
                {
                    strResult = DateTime.FromOADate((double)obj).ToString("MM/dd/yyyy");
                }
            }
            catch
            {
                try
                {
                    strResult = Convert.ToDateTime(obj.ToString()).ToString("MM/dd/yyyy");
                }
                catch
                {
                    try
                    {
                        if (obj.ToString().Length == 8)
                        {
                            strResult = DateTime.Parse(obj.ToString().Substring(0, 4) + "-" +
                                                       obj.ToString().Substring(4, 2) + "-" +
                                                       obj.ToString().Substring(6, 2)).ToString("MM/dd/yyyy");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return strResult;
        }

        public static string objToDateTime1<T>(T t)
        {
            string strResult = "";
            object obj = t;

            try
            {
                if (obj != null)
                {
                    strResult = DateTime.FromOADate((double)obj).ToString("yyyy/MM/dd");
                }
            }
            catch
            {
                try
                {
                    strResult = Convert.ToDateTime(obj.ToString()).ToString("yyyy/MM/dd");
                }
                catch
                {
                    try
                    {
                        if (obj.ToString().Length == 8)
                        {
                            strResult = DateTime.Parse(obj.ToString().Substring(4, 4) + "-" +
                                                       obj.ToString().Substring(0, 2) + "-" +
                                                       obj.ToString().Substring(2, 2)).ToString("yyyy/MM/dd");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return strResult;
        }
        #endregion

        #region 字符串简单加密解密

        /// <summary>
        /// 简单加密解密

        /// </summary>
        /// <param name="str">需要加密、解密的字符串</param>
        /// <returns>加密、解密后的字符串</returns>
        public static string encryptString(string str)
        {
            string strResult = "";
            char[] charMessage = str.ToCharArray();
            foreach (char c in charMessage)
            {
                char newChar = changerChar(c);
                strResult += newChar.ToString();
            }
            return strResult;
        }

        private static char changerChar(char c)
        {
            char resutlt;
            int intStrLength = 0;
            string twoString = Convert.ToString(c, 2).PadLeft(8, '0');
            if (twoString.Length > 8)
            {
                twoString = Convert.ToString(c, 2).PadLeft(16, '0');
            }
            intStrLength = twoString.Length;
            string newTwoString = twoString.Substring(intStrLength / 2) + twoString.Substring(0, intStrLength / 2);
            resutlt = Convert.ToChar(Convert.ToInt32(newTwoString, 2));
            return resutlt;
        }
        #endregion

        #region 将字符串日期转换为时间类型

        public static DateTime GetDateByString(string dateString)
        {
            return DateTime.Parse(dateString.Substring(0, 4) + "-" + dateString.Substring(4, 2) + "-" + dateString.Substring(6, 2));
        }
        #endregion

        #region 关闭打开的Excel
        public static void CloseExcel(Microsoft.Office.Interop.Excel.Application ExcelApplication, Microsoft.Office.Interop.Excel.Workbook ExcelWorkbook)
        {
            ExcelWorkbook.Close(false, Type.Missing, Type.Missing);
            ExcelWorkbook = null;
            ExcelApplication.Quit();
            GC.Collect();
            clsKeyMyExcelProcess.Kill(ExcelApplication);
        }
        #endregion

        #region 得到Sap连接字符串

        #endregion

        #region 判断2个日期是否是整年

        public static bool CheckThroughoutTheYear(string data1, string date2)
        {
            bool blnResult = false;
            string dtStart = "";
            string dtEnd = "";
            if (Convert.ToDateTime(date2).CompareTo(Convert.ToDateTime(data1)) > 0)
            {
                dtStart = data1;
                dtEnd = date2;
            }
            else
            {
                dtStart = date2;
                dtEnd = data1;
            }
            string strTheoryDate = Convert.ToDateTime(dtEnd).ToString("yyyy")
                                 + Convert.ToDateTime(dtStart).ToString("MMdd");
            strTheoryDate = Convert.ToDateTime(objToDateTime<string>(strTheoryDate)).AddDays(-1).ToString("MM/dd/yyyy");
            if (objToDateTime<string>(strTheoryDate) == objToDateTime<string>(dtEnd))
            {
                blnResult = true;
            }
            return blnResult;
        }

        #endregion



    }
}
