using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.CMS.Common;
using Thinkment.Data;
using We7.Framework.Util;
using System.IO;

namespace We7.CMS.Helpers
{
    [Serializable]
    [Helper("We7.CMS.Helpers.ChannelModuleHelper")]
    public class ChannelModuleHelper : BaseHelper
    {

        public static string ModuleCofigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Admin/Addins/config/Module.config");

        public ColumnModuleCollection LoadModules()
        {
            ColumnModuleCollection result;
            try
            {
                result=SerializationHelper.Load<ColumnModuleCollection>(ModuleCofigPath);
            }
            catch
            {
                result = new ColumnModuleCollection();
                SerializationHelper.Save(result, ModuleCofigPath);
            }
            return result;
        }

        public void SaveModules(ColumnModuleCollection cols)
        {
            SerializationHelper.Save(cols, ModuleCofigPath);
        }

        public void CreateModule(ColumnModule module)
        {
            module.ID = We7Helper.CreateNewID();
            ColumnModuleCollection cols = LoadModules();
            cols.Add(module);
            SaveModules(cols);
        }

        public void UpdateModule(ColumnModule module, string[] field)
        {
            ColumnModuleCollection cols = LoadModules();
            foreach (ColumnModule m in cols)
            {
                if (module.ID == m.ID)
                {
                    foreach (string s in field)
                    {
                        if (s == "Title")
                            m.Title = module.Title;
                        if (s == "Desc")
                            m.Desc = module.Desc;
                        if (s == "ParamIntro")
                            m.ParamIntro = module.ParamIntro;
                        if (s == "Path")
                            m.Path = module.Path;
                    }
                }
            }
            SaveModules(cols);
        }

        public ColumnModule GetModule(string id)
        {
            ColumnModuleCollection cols = LoadModules();
            foreach (ColumnModule m in cols)
            {
                if (m.ID == id)
                    return m;
            }
            return null;
        }

        public List<ColumnModule> GetAllModule()
        {
            ColumnModuleCollection cols = LoadModules();
            return new List<ColumnModule>(cols);
        }

        public void DeleteModule(string id)
        {
            ColumnModuleCollection cols = LoadModules();
            ColumnModule module = null;
            foreach (ColumnModule m in cols)
            {
                if (m.ID == id)
                {
                    module = m;
                }
            }
            if (module != null)
                cols.Remove(module);
            SaveModules(cols);
        }

        public void CreateMapping(string channelID, string moduleID, string parameter)
        {
            ChannelModuleMapping mapping = new ChannelModuleMapping();
            mapping.ID = We7Helper.CreateNewID();
            mapping.ChannelID = channelID;
            mapping.ModuleID = moduleID;
            mapping.Parameter = parameter;
            Assistant.Insert(mapping);
        }

        public ChannelModuleMapping GetMapping(string id)
        {
            List<ChannelModuleMapping> list = Assistant.List<ChannelModuleMapping>(new Criteria(CriteriaType.Equals, "ID", id), null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        public List<ChannelModuleMapping> GetMappingByChannelID(string oid)
        {
            return Assistant.List<ChannelModuleMapping>(new Criteria(CriteriaType.Equals, "ChannelID", oid),null);
        }

        public void DeleteMappingByChannelID(string id)
        {
            Assistant.DeleteList<ChannelModuleMapping>(new Criteria(CriteriaType.Equals, "ChannelID", id));
        }

        public void UpdateMapping(string id, string parameter)
        {
            Assistant.Update(new ChannelModuleMapping { Parameter = parameter }, new string[] { "Parameter" }, new Criteria(CriteriaType.Equals, "ID", id));
        }

        public void DeleteMapping(string id)
        {
            ChannelModuleMapping mapping = GetMapping(id);

            if (mapping != null)
            {
                Assistant.Delete(mapping);
            }
        }
    }
}
