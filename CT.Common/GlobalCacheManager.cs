using CT.DL;
using CT.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CT.Common
{
    public class GlobalCacheManager
    {

        private static Timer myTimer;


        public static void Init()
        {
            GlobalCache.UpdateSystemDate();
            GlobalCache.LoadData();
            // Create a Timer with a callback function and set it to trigger every 1000 milliseconds (1 second)


            //for each 5 minutes
            myTimer = new Timer(OnTimedEvent, null, 0, 300000);

        }

        // This method will be called every time the timer elapses
        private static void OnTimedEvent(object state)
        {
            GlobalCache.UpdateSystemDate();
            GlobalCache.LoadData();
        }

    }
}
