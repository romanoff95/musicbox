using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.OleDb;
using System.Threading;
using Sanford.Multimedia.Midi;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace musicbox
{
    [Serializable]
    public enum MediaTypes
    {
        Audio = 0,
        Video,
        Karaoke
    }

   
    static partial class Program
    {


        public static void SerializeToBinary(String filePath, Object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using (stream)
            {
                formatter.Serialize(stream, obj);
            }
        }
        public static bool DeserializeFromBinary<T>(String filePath,out T deserializedObject)  where T :class
        {
            bool ret = false;
            Object retObject;
            deserializedObject = null;
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(filePath))
            {
                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        retObject = formatter.Deserialize(stream);
                    }
                    catch (SerializationException )
                    {
                        return ret;
                    }
                }
                deserializedObject = retObject as T;
                if (deserializedObject != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        /*public static Boolean TryDeserializeObjectFromBinary(String filePath, out Object obj)
        {
            Boolean retFlag = true;
            obj = null;
            try
            {
                obj = DeserializeFromBinary(filePath);
            }
            catch (Exception e)
            {
                retFlag = false;
                throw new Exception(e.Message);
            }

            return retFlag;
        }*/
        /*public static Boolean TryGetPlayItemInfoByExtension(String extension,out MediaItemInfo itemInfo)
        {
            extension = extension.ToLower();
            return _MediaSettings.Instance.MeidaItemInfoDict.TryGetValue(extension, out itemInfo);
        }*/
        public static Boolean SendDataToInet(String strUrl, String param)
        {
            Boolean bRetFlag = false;
            String SuccessResponse = "success";

            if (!strUrl.StartsWith("http://"))
                strUrl = String.Concat("http://", strUrl);
            HttpWebRequest httpwebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
            //webReq.ContentType = "text/html; charset=windows-1251";
            //webReq.ContentEn
            //webReq.Headers.Add("ContentEncoding:windows-1251");
            Encoding encoding = Encoding.GetEncoding(1251);
            Byte[] data = encoding.GetBytes(param);
            httpwebRequest.Method = "POST";
            httpwebRequest.ContentType = "application/x-www-form-urlencoded";
            httpwebRequest.ContentLength = data.Length;
            Stream requestStream;
            try
            {
                requestStream = httpwebRequest.GetRequestStream();
            }
            catch (WebException)
            {
                return bRetFlag;
            }
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            HttpWebResponse httpwebResponse;

            try
            {
                httpwebResponse = (HttpWebResponse)httpwebRequest.GetResponse();
            }
            catch (WebException)
            {
                return bRetFlag;
            }

            using (StreamReader responseStream = new StreamReader(httpwebResponse.GetResponseStream(), encoding))
            {
                String responseString = responseStream.ReadToEnd();
                if (responseString.StartsWith(SuccessResponse))
                    bRetFlag = true;
                Debug.WriteLine(strUrl + " resp : " + responseString + " " + DateTime.Now.ToString());

            }
            httpwebResponse.Close();
            return bRetFlag;
        }

        private static byte m_BillStatus = BillValidatorManager.VALIDATOR_UNITIALIZED;

        public static byte ValidatorState
        {
            get { return Program.m_BillStatus; }
            set { Program.m_BillStatus = value; }
        }

        //private static BillValidatorManager billValidatorManager;
        //private static InternetLinkManager internetLinkManager;
        //internal static SendInfoManager sendInfoManager;
        //internal static SongInfoManager songInfoManager;
        //internal static MediaItemsSettings mediaItemsSettings = new MediaItemsSettings();


        private static readonly string m_ConfigCmdArg = "/config";
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(false);
            
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException +=new ThreadExceptionEventHandler(Application_ThreadException);
            //---------------------------------------------

            if (args.Length > 0)
            {
                if (String.Compare(args[0], m_ConfigCmdArg) == 0)
                {
                    if (new Sett.SettForm().ShowDialog() != DialogResult.OK)
                        return;
                }
                    //Application.Run(new Sett.SettForm());
            }
            //internetLinkManager = new InternetLinkManager();
           // billValidatorManager = new BillValidatorManager();
            //sendInfoManager = new SendInfoManager();
            //songInfoManager = new SongInfoManager();
            

            /*if (Properties.Settings.Default.ManageInternetConnectionState)
            {
                internetLinkManager.Start();
            }*/

            /*billValidatorManager.Incas += new EventHandler(billValidatorManager_Incas);
            billValidatorManager.BillIn += new EventHandler<BillValidatorManager.BillInEventArgs>(billValidatorManager_BillIn);*/

            Application.Run(new Main.MainWnd());

            //songInfoManager.Dispose();
            //sendInfoManager.Dispose();
           // billValidatorManager.Dispose();
            //internetLinkManager.Dispose();
            //MessageBox.Show(args.Length > 0 ? args[0] : "empty");
            /*Main.MainWnd mainWnd = new Main.MainWnd();
            Application.ApplicationExit += delegate
            {
                Final();
            };
            Sett.SettCtx settCtx = new musicbox.Sett.SettCtx(mainWnd);
            Application.Run(settCtx);*/
            
        }

        /*static void billValidatorManager_BillIn(object sender, BillValidatorManager.BillInEventArgs e)
        {
            CBInfo.Instance.BillsCount.Increment();
            CBInfo.Instance.Sum.Add((Int32)e.Bill);

            //CBInfo.Instance.BillsCountNow.Increment();
            CBInfo.Instance.SumNow.Add((Int32)e.Bill);

            CBInfo.Instance.BillsCountCommon.Increment();
            CBInfo.Instance.SumCommon.Add((Int32)e.Bill);

            CBInfo.Instance.Serialize();
        }
        static void billValidatorManager_Incas(object sender, EventArgs e)
        {
            //Incas
            //Debug.WriteLine("Incaszzzzzzzz");
            /*sendInfoManager.IncasInfoSendManager.Add(new IncasInfoSendManager.IncasInfo[] { new
                        IncasInfoSendManager.IncasInfo(
                        CBInfo.Instance.SumCommon.Value.ToString(),
                        CBInfo.Instance.Sum.Value.ToString(), CBInfo.Instance.BillsCount.Value.ToString(),
                        DateTime.Now) });*/
            /*CBInfo.Instance.BillsCount.Exchange(0);
            CBInfo.Instance.Sum.Exchange(0);
            CBInfo.Instance.Serialize();
        }*/

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = ((Exception)e.ExceptionObject);
            MessageBox.Show(exception.Message + " " + exception.StackTrace
                , "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            /*if (Screen.AllScreens.Length > 1)
            {
                musicbox.Main.ExceptionForm exceptionForm = new musicbox.Main.ExceptionForm();
                exceptionForm.WindowState = FormWindowState.Normal;
                Rectangle tempRect = Screen.AllScreens[1].Bounds;
                exceptionForm.DesktopLocation = tempRect.Location;
                exceptionForm.Size = tempRect.Size;
                exceptionForm.Show();
                
            }*/
            Final();
            (new musicbox.Main.ExceptionForm()).ShowDialog();
            MessageBox.Show(e.Exception.Message);
            

            Application.Exit();
        }
        static void Final()
        {
            //musicbox.Main.Player.Instance.Serialize();
            //musicbox.Main.Player.Instance.Stop();

            //musicbox.MenuSettings.Serialize();
        }

    }
}