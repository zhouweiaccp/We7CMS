using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using We7.Framework;
using We7.Model.Core;
using Thinkment.Data;

namespace We7.CMS
{
    public class MoudelMonitoring
    {
        private int TimeoutMillis = 2000;
        FileSystemWatcher fsw =new FileSystemWatcher(); 
        WatcherTimer watcher = null;
        public MoudelMonitoring()
        {
           // fsw = new FileSystemWatcher();
            watcher = new WatcherTimer(fsw_Changed,TimeoutMillis);
            Initial();
        }
        private void Initial()
        {
            fsw.Path = AppDomain.CurrentDomain.BaseDirectory + "\\Models";
            fsw.IncludeSubdirectories = true;
            //监控变化类型 
            fsw.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime;
            // 所有文件
            fsw.Filter = "*.*";

            // Add event handlers.所有的事件均添加进来,通知定时器
            fsw.Created += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.Changed += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.Deleted += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.Renamed += new RenamedEventHandler(watcher.OnFileChanged);

            // Begin watching.
            fsw.EnableRaisingEvents = true;
        }

        void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.fuck));
            var hf=AppCtx.Cache.RetrieveObject<HelperFactory>(HelperFactory.CacheKey);
            if (hf!=null)
            {
                MoudelMonitoring.SetModelDataDic(hf.Assistant);
                //更新缓存
                AppCtx.Cache.RemoveObject(HelperFactory.CacheKey);
                AppCtx.Cache.AddObject(HelperFactory.CacheKey, hf);
                //更新Application
                if (WatcherTimer.context != null)
                {
                    WatcherTimer.context.Application.Remove(HelperFactory.ApplicationID);
                    WatcherTimer.context.Application.Add(HelperFactory.ApplicationID, hf);
                }
            }
        }
        /// <summary>
        /// 设置内容模型字典
        /// </summary>
        /// <param name="assistat"></param>
        public static void SetModelDataDic(ObjectAssistant assistat)
        {
            if (assistat==null)
            {
                return;
            }
            ContentModelCollection cmc = ModelHelper.GetAllContentModel();
            foreach (ContentModel item in cmc)
            {
                ModelInfo info = ModelHelper.GetModelInfo(item.Name);
                foreach (var table in info.DataSet.Tables)
                {
                    ObjectManager om = new ObjectManager();
                    EntityObject eo = new EntityObject();
                    eo.TypeForDT = typeof(TableInfo);
                    eo.IdentityName = "ID";
                    eo.IsDataTable = true;
                    eo.PrimaryKeyName = "ID";
                    eo.TableName = table.Name;
                    Dictionary<string, Property> diccolumn = new Dictionary<string, Property>(StringComparer.OrdinalIgnoreCase);
                    foreach (var column in table.Columns)
                    {
                        Property pt = new Property();
                        pt.Description = column.Label;
                        pt.Field = column.Name;
                        pt.Nullable = column.Nullable;
                        pt.Size = column.MaxLength;
                        pt.Name = column.Name;
                        //pt.Type = column.DataType;
                        diccolumn.Add(column.Name, pt);
                    }

                    eo.PropertyDict = diccolumn;
                    om.CurObject = eo;
                    DataBase db = new DataBase();
                    db.DbDriver = assistat.GetDatabases()["We7.CMS.Common"].DbDriver;
                    db.Name = assistat.GetDatabases()["We7.CMS.Common"].Name;
                    db.ConnectionString = assistat.GetDatabases()["We7.CMS.Common"].ConnectionString;
                    om.CurDatabase = db;
                    om.ObjType = eo.TypeForDT;
                    if (assistat.DicForTable.ContainsKey(table.Name))  //如果存在此KEY
                    {
                        assistat.DicForTable.Remove(table.Name);  //移除此项
                    }
					if (!assistat.DicForTable.ContainsKey(table.Name))
					{
						assistat.DicForTable.Add(table.Name, om);  //添加
					}
                }
            }
        }

    }
}
