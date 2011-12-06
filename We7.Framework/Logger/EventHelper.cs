using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace We7.Framework
{
    /// <summary>
    /// 事件日志处理类
    /// </summary>
	public class EventHelper
	{
		string source;
		string log;

		public EventHelper()
		{
		}

		public string Source
		{
			get { return source; }
			set { source = value; }
		}

		public string Log
		{
			get { return log; }
			set { log = value; }
		}

		public void Write(EventLogEntryType type, string e)
		{
			if (!EventLog.SourceExists(source))
			{
				EventLog.CreateEventSource(source, log);
			}
			EventLog.WriteEntry(source, e, type);
		}		
	}
}
