using System;
using System.Collections.Generic;
using System.IO;

namespace LoxStatEdit
{
    public class LoxStatDataPoint: LoxStatObject
    {
        public static readonly DateTime TimestampBias = new DateTime(2009, 1, 1);

        /// <summary>The file to which this data point belongs
        /// </summary>
        public LoxStatFile LoxStatFile { get; private set; }
        /// <summary>The index of the data point within the file
        /// </summary>
        public int Index { get; private set; }
        /// <summary>Part 1 of the loxone object uuid (nnnnnnnn-nnnn-XXXX-nnnn-nnnnnnnnnnnn)
        /// </summary>
        public ushort ObjectUidPart1 { get; set; }
        /// <summary>Part 2 of the loxone object uuid (nnnnnnnn-XXXX-nnnn-nnnn-nnnnnnnnnnnn)
        /// </summary>
        public ushort ObjectUidPart2 { get; set; }
        /// <summary>Timestamp in seconds from 2009-01-01
        /// </summary>
        public uint TimestampOffset { get; set; }
        /// <summary>Values 1/2/… (depending on Number of Values per Data Point x)
        /// </summary>
        public double[] Values { get; set; }
        /// <summary>Padding to the next 16 byte aligned offset
        /// </summary>
        public byte[] Padding { get; set; }

        public DateTime Timestamp
        {
            get
            {
                return TimestampBias.AddSeconds(TimestampOffset);
            }
            set
            {
                TimestampOffset = (uint)((value - TimestampBias).TotalSeconds);
            }
        }

        public override void AddProblems(ICollection<LoxStatProblem> collection)
        {
            try
            {
                if(LoxStatFile == null)
                {
                    AddProblem(collection, "Data point does not belong to a LoxStatFile");
                    return;
                }
                bool isLastDataPoint = false;
                var dataPoints = LoxStatFile.DataPoints;
                if(dataPoints != null)
                {
                    isLastDataPoint = dataPoints.Count == (Index + 1);
                    if((Index < 0) || (Index >= dataPoints.Count) ||
                        (dataPoints[Index] != this))
                        AddProblem(collection, "Invalid data point index");
                    else if((Index > 0) &&
                        (dataPoints[Index - 1].TimestampOffset > TimestampOffset))
                        AddProblem(collection, "Timestamp order mismatch");
                }
                var guid = LoxStatFile.Guid;
                if(guid != Guid.Empty)
                {
                    var guidBytes = guid.ToByteArray();
                    if((BitConverter.ToUInt16(guidBytes, 6) != ObjectUidPart1) ||
                        (BitConverter.ToUInt16(guidBytes, 4) != ObjectUidPart2))
                        AddProblem(collection, "Uid part mismatch");
                }
                var yearMonth = LoxStatFile.YearMonth;
                if(yearMonth != DateTime.MinValue)
                {
                    //It may happen that the last data point has a
                    //Timestamp in the next month
                    //Probably to for interpolation?
                    var timestamp = Timestamp;
                    if((timestamp < yearMonth) ||
                        (timestamp >= yearMonth.AddMonths(isLastDataPoint ? 2 : 1)))
                        AddProblem(collection, "Timestamp not within year/month of StatFile");
                }
                if((Values == null) || (Values.Length != LoxStatFile.ValueCount))
                    AddProblem(collection, "Value count mismatch");
                //Intentionally not checking Padding
            }
            catch(Exception ex)
            {
                AddProblem(collection, ex);
            }
        }

        public override string ToString()
        {
            try
            {
                return string.Format("{0}[{1}]", LoxStatFile, Index);
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        internal static void FromBinaryReader(LoxStatFile loxStatFile, BinaryReader reader)
        {
            var list = loxStatFile.DataPoints;
            var valueCount = loxStatFile.ValueCount;
            int index = 0;
            while(true)
            {
                var bytes = reader.ReadBytes(4);
                if(bytes.Length == 0) return; //End of file
                var dataPoint = new LoxStatDataPoint
                {
                    LoxStatFile = loxStatFile,
                    Index = index++,
                };
                list.Add(dataPoint);
                dataPoint.ObjectUidPart1 = BitConverter.ToUInt16(bytes, 0);
                dataPoint.ObjectUidPart2 = BitConverter.ToUInt16(bytes, 2);
                dataPoint.TimestampOffset = reader.ReadUInt32();
                dataPoint.Values = new double[valueCount];
                for(int i = 0; i < valueCount; i++)
                    dataPoint.Values[i] = reader.ReadDouble();
                dataPoint.Padding = reader.ReadBytes(
                    LoxStatFile.GetPaddingLength(8 + valueCount * 8));
            }
        }

        internal void Save(BinaryWriter writer)
        {
            writer.Write(ObjectUidPart1);
            writer.Write(ObjectUidPart2);
            writer.Write(TimestampOffset);
            foreach(var value in Values)
                writer.Write(value);
            writer.Write(Padding);
        }
    }
}
