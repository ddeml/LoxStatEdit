using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LoxStatEdit
{
    public partial class LoxStatFileForm: Form
    {
        const int _valueColumnOffset = 2;
        readonly string[] _args;
        LoxStatFile _loxStatFile;
        IList<LoxStatProblem> _problems;

        public LoxStatFileForm(params string[] args)
        {
            _args = args;
            InitializeComponent();
        }

        private void LoadFile()
        {
            _fileNameTextBox.Text = Path.GetFullPath(_fileNameTextBox.Text);
            _loxStatFile = LoxStatFile.Load(_fileNameTextBox.Text);
            _dataGridView.RowCount = 0;
            var columns = _dataGridView.Columns;
            while(columns.Count > (_valueColumnOffset + 1))
                columns.RemoveAt(columns.Count - 1);
            _fileInfoTextBox.Text = string.Format("{0}: {1} data point(s) á {2} value(s)",
                _loxStatFile.Text, _loxStatFile.DataPoints.Count, _loxStatFile.ValueCount);
            for(int i = 2; i <= _loxStatFile.ValueCount; i++)
            {
                var newColumn = (DataGridViewColumn)columns[_valueColumnOffset].Clone();
                newColumn.HeaderText = string.Format("{0} {1}", newColumn.HeaderText, i);
                columns.Add(newColumn);
            }
            _dataGridView.RowCount = _loxStatFile.DataPoints.Count;
            _dataGridView.MultiSelect = true;

            RefreshProblems();
            RefreshChart();
        }

        private void RefreshProblems()
        {
            _problems = LoxStatProblem.GetProblems(_loxStatFile);
            _problemButton.Enabled = _problems.Any();
        }

        private void RefreshChart()
        {
            // Now we can set up the Chart:
            List<Color> colors = new List<Color> { Color.Green, Color.Red, Color.Black, Color.Blue, Color.Violet, Color.Turquoise, Color.YellowGreen, Color.AliceBlue, Color.Beige, Color.Chocolate, Color.CornflowerBlue, Color.Firebrick };

            _chart.Series.Clear();

            int skipHeaderColumns = 2;

            for (int i = skipHeaderColumns; i < _dataGridView.Columns.Count; i++)
            {
                Series S = _chart.Series.Add(_dataGridView.Columns[i].HeaderText);
                S.ChartType = SeriesChartType.Spline;
                S.Color = colors[i];
            }


            // and fill in all the values from the dgv to the chart:
            for (int i = 0; i < _dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < _chart.Series.Count; j++)
                {
                    int p = _chart.Series[j].Points.AddXY(_dataGridView[1, i].Value, _dataGridView[j+ skipHeaderColumns, i].Value);
                }
            }
        }

        private void ShowProblems()
        {
            MessageBox.Show
            (
                this,
                string.Format
                (
                    "{1} Problems (showing max 50):{0}{2}",
                    Environment.NewLine,
                    _problems.Count,
                    string.Join(Environment.NewLine, _problems.Take(50))
                ),
                Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = _fileNameTextBox.Text;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                _fileNameTextBox.Text = openFileDialog.FileName;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                var rowIndex = e.RowIndex;
                var dataPoint = _loxStatFile.DataPoints[rowIndex];
                var columnIndex = e.ColumnIndex;
                e.Value = (columnIndex == 0) ? (object)dataPoint.Index :
                    (columnIndex == 1) ? (object)dataPoint.Timestamp :
                    (object)dataPoint.Values[e.ColumnIndex - _valueColumnOffset];
            }
            catch
            {
                e.Value = null;
            }
        }

        private void DataGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            var columnIndex = e.ColumnIndex;
            var dataPoint = _loxStatFile.DataPoints[e.RowIndex];
            if(columnIndex == 1)
                dataPoint.Timestamp = Convert.ToDateTime(e.Value);
            else
                dataPoint.Values[columnIndex - _valueColumnOffset] =
                    Convert.ToDouble(e.Value.ToString());
            RefreshProblems();
            RefreshChart();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if((_args != null) && (_args.Length > 0))
            {
                _fileNameTextBox.Text = _args[0];
                LoadFile();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            RefreshProblems();
            if(_problems.Any())
                ShowProblems();
            if(MessageBox.Show(this, "I will not (and can not) take any " +
                "responsibility for any issues of your Loxone installation whatsoever! " +
                "Loxone will most likely not support any issues if you use the files " +
                "generated by this tool either!" +
                "I am just a desparate Loxone user in need of a solution for a problem " +
                "and lucky enough to be a professional coder! " +
                "Loxone did not provide me a full file format documentation and I used " +
                "Hexinator (thank you!) to decode the file format to some degree! " +
                "Would you still like to save the file?",
                "DISCLAIMER! USE AT YOUR OWN RISK!", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            _loxStatFile.FileName = Path.GetFullPath(_fileNameTextBox.Text);
            _loxStatFile.Save();
            MessageBox.Show(this, "Save successful!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ProblemButton_Click(object sender, EventArgs e)
        {
            ShowProblems();
        }

        private void _dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void _dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {

                string CopiedContent = Clipboard.GetText();
                string[] Lines = CopiedContent.Split('\n');
                int StartingRow = _dataGridView.CurrentCell.RowIndex;
                int StartingColumn = _dataGridView.CurrentCell.ColumnIndex;
                foreach (var line in Lines)
                {
                    if (StartingRow <= (_dataGridView.Rows.Count - 1))
                    {
                        string[] cells = line.Split('\t');
                        int ColumnIndex = StartingColumn;
                        for (int i = 0; i < cells.Length && ColumnIndex <= (_dataGridView.Columns.Count - 1); i++)
                        {
                            if (!String.IsNullOrEmpty(cells[i]))
                            {
                                var columnIndex = ColumnIndex++;
                                var dataPoint = _loxStatFile.DataPoints[StartingRow];
                                dataPoint.Values[columnIndex - _valueColumnOffset] = Convert.ToDouble(cells[i].ToString());
                            }
                        }
                        StartingRow++;
                    }
                }
                RefreshProblems();
                RefreshChart();
                _dataGridView.Refresh();
            }
        }
    }
}
