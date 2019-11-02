using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace LoxStatEdit
{
    public class MsFileInfo
    {
        public string FileName { get; private set; }
        public DateTime Date { get; private set; }
        public int Size { get; private set; }

        public static IList<MsFileInfo> Load(Uri uri)
        {
            var list = new List<MsFileInfo>();
            var ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(uri);
            ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            using(var response = ftpWebRequest.GetResponse())
            using(var ftpStream = response.GetResponseStream())
            using(var streamReader = new StreamReader(ftpStream))
                while(!streamReader.EndOfStream)
                {
                    //Hacky but works fair enough in our particular use case (I hope...)
                    var line = streamReader.ReadLine();
                    int datePos = line.IndexOf(' ', 24);
                    int size;
                    if(!int.TryParse(line.Substring(18, datePos - 18), out size))
                        size = -1;
                    datePos++;
                    int fileNamePos;
                    DateTime dateTime;
                    if(DateTime.TryParseExact(line.Substring(datePos, 12), "MMM dd HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                        fileNamePos = datePos + 13;
                    else if(DateTime.TryParseExact(line.Substring(datePos, 11), "MMM dd yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                        fileNamePos = datePos + 12;
                    else fileNamePos = line.LastIndexOf(' ') + 1;
                    list.Add(new MsFileInfo
                    {
                        FileName = line.Substring(fileNamePos),
                        Date = dateTime,
                        Size = size,
                    });
                }
            return list;
        }
    }
}
