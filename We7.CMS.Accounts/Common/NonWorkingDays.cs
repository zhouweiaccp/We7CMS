using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// 非工作日处理类
    /// </summary>
    [Serializable]
    public class NonWorkingDays
    {
        System.DayOfWeek[] weekends;

        public System.DayOfWeek[] Weekends
        {
            get { return weekends; }
            set { weekends = value; }
        }

        ExceptionDays[] workingDays;

        public ExceptionDays[] WorkingDays
        {
            get { return workingDays; }
            set { workingDays = value; }
        }

        ExceptionDays[] nonworkingDays;

        public ExceptionDays[] NonworkingDays
        {
            get { return nonworkingDays; }
            set { nonworkingDays = value; }
        }

        public static NonWorkingDays LoadNonWorkingDays(string fileName)
        {
            FileStream fs = null;
            try
            {
                NonWorkingDays mydays = new NonWorkingDays();
                XmlSerializer xml = new XmlSerializer(typeof(NonWorkingDays));
                if (File.Exists(fileName))
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    mydays = (NonWorkingDays)xml.Deserialize(fs);
                }
                else
                {
                    List<DayOfWeek> myWeekends = new List<DayOfWeek>();
                    myWeekends.Add(DayOfWeek.Saturday);
                    myWeekends.Add(DayOfWeek.Sunday);

                    mydays.Weekends = myWeekends.ToArray();

                    List<ExceptionDays> myNonWorkingDays = new List<ExceptionDays>();
                    ExceptionDays myExceptionDays = new ExceptionDays();
                    myExceptionDays.StartTime = Convert.ToDateTime("2009-10-01");
                    myExceptionDays.EndTime = Convert.ToDateTime("2009-10-01");
                    myNonWorkingDays.Add(myExceptionDays);
                    myExceptionDays.StartTime = Convert.ToDateTime("2009-05-01");
                    myExceptionDays.EndTime = Convert.ToDateTime("2009-05-01");
                    myNonWorkingDays.Add(myExceptionDays);

                    mydays.NonworkingDays = myNonWorkingDays.ToArray();

                    fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    xml.Serialize(fs, mydays);
                }
                return mydays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }       
        }

    }

    /// <summary>
    /// 时间区域（工作或非工作连续区域）
    /// </summary>
    [Serializable]
    public class ExceptionDays
    {
        DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

    }
}
