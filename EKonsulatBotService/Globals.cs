using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace EKonsulatBotService
{
    public class Globals
    {
        ArrayList Proxy = new ArrayList();
        ArrayList Output = new ArrayList();

        public static Guid cookieFileNameGuid = Guid.NewGuid();

        public static int IdService { get; set; }
        public static int IdCity { get; set; }

        public static int CountGood = 0;
        public static int CountBad = 0;

        public int CountProxy = 0;
        public static String Delimiter = ";";
        public static int ProxyType = 0;

        public static int CountThreads = 0;


        public static void InvokeEx(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }

        }

        public static bool IsElementDisplayed(IWebDriver driver, By element)
        {
            if (driver.FindElements(element).Count > 0)
            {
                if (driver.FindElement(element).Displayed)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
