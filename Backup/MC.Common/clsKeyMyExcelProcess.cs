using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MC.Common
{
    public class clsKeyMyExcelProcess
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            try
            {
                //得到这个句柄，具体作用是得到这块内存入口
                IntPtr t = new IntPtr(excel.Hwnd);
                //得到本进程唯一标志k   
                int k = 0;
                GetWindowThreadProcessId(t, out k);
                //得到对进程k的引用

                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                //关闭进程k
                p.Kill();
            }
            catch (Exception ex)
            {
                // clsLogPrint.WriteLog("System Exception Close Excel:" + ex.Message + ";Inner Message:" + ex.InnerException.Message);
                throw ex;
            }
        }
    }
}
