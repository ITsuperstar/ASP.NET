using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MetalsWebApplication
{
    public class OrderIdHelper
    {
        private static readonly object Locker = new object();
        private static int _sn = 0;

        //生成订单编号
        public static string GenerateId()
        {
            lock (Locker)   //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。
            {
                if (_sn == int.MaxValue)
                {
                    _sn = 0;
                }
                else
                {
                    _sn++;
                }
                Thread.Sleep(100);
                return "Yang" + DateTime.Now.ToString("yyyyMMddHHmmss") + _sn.ToString().PadLeft(10, '0');
            }
        }

    }
}