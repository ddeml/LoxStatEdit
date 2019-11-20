using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LoxStatEdit
{
    public class LoxStatFile : LoxStatObject
    {
        /// <summary>The original Loxone file name. Contains UUID and datecode of the statfile. 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>Offset 0, Length 2, number of values per data point
        /// </summary>
        public ushort ValueCount { get; set; }
        /// <summary>Offset 2, Length 2, always 0x0000
        /// </summary>
        public ushort Unknown0x02 { get; set; }
        /// <summary>Offset 4, Length 4, Flags?
        /// </summary>
        public uint Unknown0x04 { get; set; }
        /// <summary>Offset 8, Length 4
        /// </summary>
        public int TextLength { get; set; }
        /// <summary>Offset 12, Length LabelLength
        /// </summary>
        public byte[] TextBytes { get; set; }
        /// <summary>Offset 12+TextLength, Text zero terminator
        /// </summary>
        public byte TextTerminator { get; set; }
        /// <summary>Offset 13+TextLength, Length varying, Padding with 0 for 16-byte-alignment
        /// </summary>
        public byte[] Padding { get; set; }
        /// <summary>Data Points
        /// </summary>
        public List<LoxStatDataPoint> DataPoints { get; set; }
        /// <summary>Exception during load (if any)
        /// </summary>
        public Exception LoadException { get; private set; }

        public Guid Guid
        {
            get
            {
                Guid value;
                Guid.TryParseExact
                (
                    Path.GetFileNameWithoutExtension(FileName).
                        Replace("-", ""),
                    "N",
                    out value
                );
                return value;
            }
        }

        public DateTime YearMonth
        {
            get
            {
                return GetYearMonth(FileName);
            }
        }

        public string Text
        {
            get
            {
                return Encoding.UTF8.GetString(TextBytes);
            }
            set
            {
                Encoding.UTF8.GetBytes(value);
                TextLength = TextBytes.Length;
                Padding = new byte[GetPaddingLength(12 + TextLength)];
            }
        }

        public static DateTime GetYearMonth(string fileName)
        {
            DateTime value;
            DateTime.TryParseExact
            (
                Path.GetExtension(fileName).Substring(1),
                "yyyyMM",
                CultureInfo.InvariantCulture,
                DateTimeStyles.NoCurrentDateDefault,
                out value
            );
            return value;
        }

        /// <summary>Finds problems in that file
        /// </summary>
        /// <param name="collection"></param>
        public override void AddProblems(ICollection<LoxStatProblem> collection)
        {
            try
            {
                if (Guid == Guid.Empty)
                    AddProblem(collection, "File name is not a Guid");
                if (YearMonth == DateTime.MinValue)
                    AddProblem(collection, "File name extension is not a year/month");
                if (ValueCount < 1)
                    AddProblem(collection, "Unexpected number of values per data point.");
                //Intentionally not checking Unknown bytes
                if (TextLength == 0)
                    AddProblem(collection, "Unexpected text length");
                //Intentionally not checking Padding
                foreach (var dataPoint in DataPoints)
                    dataPoint.AddProblems(collection);
            }
            catch (Exception ex)
            {
                AddProblem(collection, ex);
            }
        }

        public override string ToString()
        {
            try
            {
                return Path.GetFileName(FileName);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public int GetPaddingLength(int offset)
        {
            var pageWidth = 8 + ValueCount * 8 + (ValueCount > 1 ? 8 : 0);

            return (pageWidth - (offset % pageWidth)) % pageWidth;
        }

        public static LoxStatFile Load(string fileName, bool headerOnly = false)
        {
            var loxStatFile = new LoxStatFile();
            loxStatFile.FileName = fileName;
            try
            {
                using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    loxStatFile.ValueCount = reader.ReadUInt16();
                    loxStatFile.Unknown0x02 = reader.ReadUInt16();
                    loxStatFile.Unknown0x04 = reader.ReadUInt32();
                    loxStatFile.TextLength = reader.ReadInt32();
                    loxStatFile.TextBytes = reader.ReadBytes(loxStatFile.TextLength);
                    loxStatFile.TextTerminator = reader.ReadByte();
                    loxStatFile.Padding = reader.ReadBytes(
                        loxStatFile.GetPaddingLength(13 + loxStatFile.TextLength));

                    var dataPoints = new List<LoxStatDataPoint>();
                    loxStatFile.DataPoints = dataPoints;
                    if (!headerOnly)
                        LoxStatDataPoint.FromBinaryReader(loxStatFile, reader);
                }
            }
            catch (Exception ex)
            {
                loxStatFile.LoadException = ex;
            }
            return loxStatFile;
        }

        public void Save()
        {
            using (var writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
            {
                writer.Write(ValueCount);
                writer.Write(Unknown0x02);
                writer.Write(Unknown0x04);
                writer.Write(TextLength);
                writer.Write(TextBytes);
                writer.Write(TextTerminator);
                writer.Write(Padding);
                if (DataPoints != null)
                    foreach (var dataPoint in DataPoints)
                        dataPoint.Save(writer);
            }
        }

    }
}
