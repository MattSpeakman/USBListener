using System;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace USBListener
{
    public static class SecondThreadConcern
    {
        public static void LongWork(IProgress<string> progress, IProgress<string> secondProgress, SerialPort sp)
        {
            string message;
            string msg;
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    if (!sp.IsOpen)
                    {
                        DateTime time = DateTime.Now;
                        time.ToShortTimeString();
                        sp.Open();
                        message = sp.ReadLine();
                        sp.Close();
                        msg = String.Format("Time: {0} Humidity: {1}" ,time, message);
                        progress.Report(msg);
                        secondProgress.Report(msg);
                    }
                    else
                    {
                        DateTime time = DateTime.Now;
                        time.ToShortTimeString();
                        message = sp.ReadLine();
                        sp.Close();
                        msg = String.Format("Time: {0} Humidity: {1}",time,message);
                        progress.Report(msg);
                        secondProgress.Report(msg);
                    }
                    ThirdThreadConcern.LongWork(message);
                }
                    
                catch (TimeoutException) { }
                catch (Exception e) 
                {
                    MessageBox.Show(e.Message); 
                }
                
                if (i > 998)
                    i = 0;
                Task.Delay(5000).Wait();
                progress.Report(i.ToString());
            }
        }
    }
}
