using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace LoxStatEdit
{
    public partial class MiniserverForm: Form
    {

        #region class FileItem
        class FileItem: IComparable<FileItem>
        {
            internal MsFileInfo MsFileInfo;
            internal FileInfo FileInfo;
            private string _name;

            public string FileName
            {
                get
                {
                    return (MsFileInfo != null) ? MsFileInfo.FileName : FileInfo.Name;
                }
            }

            public string Name
            {
                get
                {
                    if(_name == null)
                    {
                        try
                        {
                            if(FileInfo != null)
                                _name = LoxStatFile.Load(FileInfo.FullName, true).Text;
                        }
                        catch(Exception)
                        {
                            _name = null;
                        }
                        if(string.IsNullOrEmpty(_name))
                            _name = FileName;
                    }
                    return _name;
                }
            }

            public DateTime YearMonth
            {
                get
                {
                    return LoxStatFile.GetYearMonth(FileName);
                }
            }

            public string Status
            {
                get
                {
                    if(FileInfo == null)
                    {
                        return "Only on MS";
                    }
                    if(MsFileInfo == null)
                    {
                        return "Only on FS";
                    }
                    if(FileInfo.LastWriteTime > MsFileInfo.Date)
                    {
                        return "Newer on FS";
                    }
                    if(FileInfo.LastWriteTime < MsFileInfo.Date)
                    {
                        return "Newer on MS";
                    }
                    if(FileInfo.Length > MsFileInfo.Size)
                    {
                        return "Larger on FS";
                    }
                    if(FileInfo.Length < MsFileInfo.Size)
                    {
                        return "Larger on MS";
                    }
                    return "Same date/size";
                }
            }

            public override string ToString()
            {
                return string.Format("{0} ({1})", Name, Status);
            }

            public int CompareTo(FileItem other)
            {
                var c = StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name);
                return (c != 0) ? c : YearMonth.CompareTo(other.YearMonth);
            }
        }

        #endregion

        #region Fields
        readonly string[] _args;
        IList<MsFileInfo> _msFolder = new MsFileInfo[0];
        IList<FileInfo> _localFolder = new FileInfo[0];
        IList<FileItem> _fileItems;

        #endregion

        #region Constructor
        public MiniserverForm(params string[] args)
        {
            _args = args;
            InitializeComponent();
        }

        #endregion

        #region Methods
        private void RefreshGridView()
        {
            var msMap = _msFolder.ToLookup(e => e.FileName, StringComparer.OrdinalIgnoreCase);
            var localMap = _localFolder.ToLookup(e => e.Name, StringComparer.OrdinalIgnoreCase);
            _fileItems = msMap.Select(e => e.Key).Union(localMap.Select(e => e.Key)).
                Select(f => new FileItem
                {
                    MsFileInfo = msMap[f].FirstOrDefault(),
                    FileInfo = localMap[f].FirstOrDefault(),
                }).
                OrderBy(f => f).
                ToList();
            _dataGridView.RowCount = _fileItems.Count;
            _dataGridView.Refresh();
        }

        private void RefreshMs()
        {
            _msFolder = MsFileInfo.Load(GetMsUriBuilder().Uri);
        }

        private void RefreshLocal()
        {
            Directory.CreateDirectory(_folderTextBox.Text);
            _localFolder = Directory.EnumerateFiles(_folderTextBox.Text).
                Select(fileName => new FileInfo(fileName)).ToList();
        }

        private UriBuilder GetMsUriBuilder()
        {
            var uriBuilder = new UriBuilder(new Uri(_urlTextBox.Text).
                GetComponents(UriComponents.Scheme | UriComponents.UserInfo |
                UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped));
            uriBuilder.Path = "stats";
            return uriBuilder;
        }

        private Uri GetFileNameUri(string fileName)
        {
            var uriBuilder = GetMsUriBuilder();
            uriBuilder.Path += "/" + fileName;
            return uriBuilder.Uri;
        }

        private void Download(FileItem fileItem)
        {
            var ftpWebRequest = (FtpWebRequest)FtpWebRequest.
                Create(GetFileNameUri(fileItem.FileName));
            ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            var targetFileName = Path.Combine(_folderTextBox.Text, fileItem.FileName);
            using(var response = ftpWebRequest.GetResponse())
            using(var ftpStream = response.GetResponseStream())
            using(var fileStream = File.OpenWrite(targetFileName))
                ftpStream.CopyTo(fileStream);
            File.SetLastWriteTime(targetFileName, fileItem.MsFileInfo.Date);
        }

        private void Upload(FileItem fileItem)
        {
            var ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(GetFileNameUri(fileItem.FileName));
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            using(var fileStream = File.OpenRead(
                Path.Combine(_folderTextBox.Text, fileItem.FileInfo.FullName)))
            using(var ftpStream = ftpWebRequest.GetRequestStream())
                fileStream.CopyTo(ftpStream);
            using(var response = ftpWebRequest.GetResponse()) { }
        }

        #endregion

        #region Events
        private void RefreshMsButton_Click(object sender, EventArgs e)
        {
            RefreshMs();
            RefreshGridView();
        }

        private void RefreshFolderButton_Click(object sender, EventArgs e)
        {
            RefreshLocal();
            RefreshGridView();
        }

        private void MiniserverForm_Load(object sender, EventArgs e)
        {
            _folderTextBox.Text = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), "LoxStatEdit");
            if(_args.Length >= 1)
            {
                _urlTextBox.Text = _args[0];
                RefreshMs();
            }
            if(_args.Length >= 2)
                _folderTextBox.Text = Path.GetFullPath(_args[1]);
            RefreshLocal();
            RefreshGridView();
        }

        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                var fileItem = _fileItems[e.RowIndex];
                switch(e.ColumnIndex)
                {
                    case 0: e.Value = fileItem.Name; break;
                    case 1: e.Value = fileItem.YearMonth; break;
                    case 2: e.Value = fileItem.Status; break;
                    case 3: e.Value = "Edit"; break;
                    case 4: e.Value = "Download"; break;
                    case 5: e.Value = "Upload"; break;
                    default: e.Value = null; break;
                }
            }
            catch
            {
            #if DEBUG
                Debugger.Break();
            #endif
                e.Value = null;
            }
        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0) return; //When clicking the header row
            var fileItem = _fileItems[e.RowIndex];
            switch(e.ColumnIndex)
            {
                case 3: //Edit
                    using(var form = new LoxStatFileForm(fileItem.FileInfo.FullName))
                        form.ShowDialog(this);
                    break;
                case 4: //Download
                    Download(fileItem);
                    RefreshLocal();
                    RefreshGridView();
                    break;
                case 5: //Upload
                    Upload(fileItem);
                    RefreshMs();
                    RefreshGridView();
                    break;
            }
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in _dataGridView.SelectedRows)
                Download(_fileItems[row.Index]);
            RefreshLocal();
            RefreshGridView();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in _dataGridView.SelectedRows)
                Upload(_fileItems[row.Index]);
            RefreshMs();
            RefreshGridView();
        }

        #endregion

    }
}
