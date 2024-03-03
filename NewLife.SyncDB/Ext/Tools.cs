using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLife.SyncDB.Ext
{
    public static class Tools
    {
        public static int Unit { get; private set; } = 1000;

        /// <summary>
        /// 信息采集间隔
        /// </summary>
        public static int CollectInterval { get; private set; } = 5 * Unit;



        public static string LogFormat(this string str, bool isShotTime = true)
        {
            var time = string.Empty;
            if (isShotTime)
                time = DateTime.Now.ToString("MM-dd HH:mm:ss");
            else
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return $"{time}   {str}";
        }

        public static int FindIndex<T>(this IList<T> source, Predicate<T> match, int startIndex = 0)
        {
            for (int i = startIndex; i < source.Count; i++)
            {
                if (match(source[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static void For(this int count, Action<int> action)
        {
            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }

        public static void Foreach<T>(this IList<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

    }
}
