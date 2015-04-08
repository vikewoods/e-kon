using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SimpleBrowser;
using Cookie = System.Net.Cookie;

namespace EKonsulatBotService
{
    public class Checker
    {
        private Thread[] threads;

        public Object Sync = new Object();

        public Browser CapthaBrowser = new Browser();



        // Функция чтобы получить куки для обычного HttpRequest или моего SimpleBrowser - проблема в том что на сайте посольства очень интересная проверка на подленость юзера
        public void GetCookiesFromDrive()
        {

            using (var Driver = new ChromeDriver())
            {
                Globals.IdService = 1;
                Globals.IdCity = 92;
                Driver.Manage().Window.Maximize();
                Driver.Navigate().GoToUrl("https://secure.e-konsulat.gov.pl/Uslugi/RejestracjaTerminu.aspx?IDUSLUGI=" + Globals.IdService + "&IDPlacowki=" + Globals.IdCity);

                if (Globals.IsElementDisplayed(Driver, By.Id("cp_KomponentObrazkowy_CaptchaImageID")))
                {
                    var cookies = Driver.Manage().Cookies.AllCookies;

                    StreamWriter streamWriter = new StreamWriter(@"Cookies\" + Globals.cookieFileNameGuid + ".txt");

                    foreach (var result in cookies)
                    {
                        var cookieExpires = result.Expiry;
                        var cookieDomain = result.Domain;
                        var cookiePath = result.Path;
                        var cookieName = result.Name;
                        var cookieValue = result.Value;
    
                        var cookieString = cookieName + ";" + cookieValue + ";" + cookiePath + ";" + cookieDomain + ";" + cookieExpires;
           
                        streamWriter.WriteLine(cookieString);
                        Console.WriteLine(cookieString);

                        Cookie cookieNew = new Cookie(cookieName, cookieValue, cookiePath, cookieDomain);
                        if (cookieExpires != null) cookieNew.Expires = (DateTime)cookieExpires;
                        CapthaBrowser.Cookies.Add(cookieNew);
                    }
                    streamWriter.Close();
                    Driver.Close();
                    MessageBox.Show("Cookies has been saved!");
                }
                else
                {
                    MessageBox.Show("Error!");
                    Driver.Close();
                }
            }

        }

        public void DoRequest()
        {
            // Just testing
            this.GetCookiesFromDrive();

            Globals.IdService = 1;
            Globals.IdCity = 92;

            CapthaBrowser.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0";
            CapthaBrowser.Accept = "image/png,image/*;q=0.8,*/*;q=0.5";
            CapthaBrowser.SetHeader("Accept-Language: en-US,en;q=0.5");
            CapthaBrowser.SetHeader("Accept-Encoding: gzip, deflate");
            CapthaBrowser.AutoRedirect = false;
            CapthaBrowser.RefererMode = Browser.RefererModes.Never;

            CapthaBrowser.Navigate("https://secure.e-konsulat.gov.pl/Uslugi/RejestracjaTerminu.aspx?IDUSLUGI=" +
                                   Globals.IdService + "&IDPlacowki=" + Globals.IdCity);

            if (LastRequestFailed(CapthaBrowser)) return;

            var capthaImage = CapthaBrowser.Find("cp_KomponentObrazkowy_CaptchaImageID").GetAttribute("src");

            // Гавно-код
            var properCapthaImage = "https://secure.e-konsulat.gov.pl" + capthaImage.Substring(2);

            //Console.WriteLine(CapthaBrowser.CurrentHtml);
            Console.WriteLine("Captha value: {0}", properCapthaImage);

        }

        private void Checking()
        {
            //CheckDate();
        }

        public void CreateThreads()
        {
            if (Globals.CountThreads > 100)
            {
                throw new Exception("Threads are overload!");
            }

            for (int i = 0; i < Globals.CountThreads; i++)
            {
                threads[i] = new Thread(Checking);
                threads[i].IsBackground = true;
                threads[i].Start();
            }
        }

        public void StopThreads()
        {
            for (int i = 0; i < Globals.CountThreads; i++)
            {
                threads[i].Abort();
            }
        }

        static bool LastRequestFailed(Browser browser)
        {
            if (browser.LastWebException != null)
            {
                browser.Log("There was an error loading the page: " + browser.LastWebException.Message);
                return true;
            }
            return false;
        }


    }
}
