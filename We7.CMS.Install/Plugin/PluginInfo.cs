using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Common
{
	/// <summary>
	/// 插件信息类
	/// </summary>
	[Serializable]
	public class PluginInfo
	{
		#region Fields

		private static string PluginTemplateXmlPath = "Install/Plugin/Plugin.xml.tpl";

		private string xmlPath;

		private List<string> snapshot = new List<string>();

		private PluginType pluginType;

		#endregion

		#region Constructor

		public PluginInfo(string filePath, PluginType plugintype)
			: this()
		{
			xmlPath = filePath;

			pluginType = plugintype;

			LoadXml();
		}

		public PluginInfo(PluginType plugintype)
		{
			pluginType = plugintype;
		}

		public PluginInfo(string filePath)
			: this()
		{
			xmlPath = filePath;

			LoadXml();
		}

		public PluginInfo()
		{
			Pages = new List<UrlItem>();
			Deployment = new Deployment();
		}

		#endregion

		#region Properties

		public PluginType PluginType
		{
			get { return pluginType; }
			set { pluginType = value; }
		}

		public string Directory { get; set; }

		/// <summary>
		/// 文件的路径
		/// </summary>
		public string FilePath { get; set; }

		public string Name { get; set; }

		public string Author { get; set; }

		public string Version { get; set; }

		public bool Enable { get; set; }

		public bool IsInstalled { get; set; }

		public string Url { get; set; }

		public string Description { get; set; }

		public string Summary { get; set; }

		public string DefaultPage { get; set; }

		public List<UrlItem> Pages { get; set; }

		public List<UrlItem> Controls { get; set; }

		public Deployment Deployment { get; set; }

		public string Thumbnail { get; set; }

		public List<string> Snapshot { get { return snapshot; } }

		public string Others { get; set; }

		public DateTime UpdateTime { get; set; }

		public DateTime CreateTime { get; set; }

		public String Compatible { get; set; }

		public bool IsSpecial { get; set; }

		public int Clicks { get; set; }

		public bool IsLocal { get; set; }

		/// <summary>
		/// Bin文件夹的绝对路径
		/// </summary>
		public string BinDirectory
		{
			get
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
			}
		}

		/// <summary>
		/// 程序根路径
		/// </summary>
		public string BaseDircotry
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		/// <summary>
		/// 客户端插件存放的目录名
		/// </summary>
		public string PluginsClientPath
		{
			get
			{
				string path = "Plugins";
				switch (pluginType)
				{
					case PluginType.PLUGIN:
						path = "Plugins";
						break;
					case PluginType.RESOURCE:
						path = "Plugins";
						break;
				}
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
			}
		}

		/// <summary>
		/// 当前插件的根目录（绝对路径）
		/// </summary>
		public string PluginClientPath
		{
			get
			{
				return Path.Combine(PluginsClientPath, Directory);
			}
		}

		/// <summary>
		/// 服务器端插件存放的完整目录
		/// </summary>
		public string PluginLocalGalleryPath
		{
			get
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginGallery);
			}
		}

		/// <summary>
		/// 远程插件目录
		/// </summary>
		public string PluginServerGaplleryPath
		{
			get
			{
				return string.Format("{0}/{1}", PluginServer.TrimEnd(new char[] { '/', '\\' }), PluginGallery);
			}
		}


		/// <summary>
		/// 插件服务器地址
		/// </summary>
		public string PluginServer
		{
			get
			{
				return GeneralConfigs.GetConfig().PluginServer;
			}
		}

		/// <summary>
		/// 服务器端插件存放的目录
		/// </summary>
		public string PluginGallery
		{
			get
			{
				string pluginGallery = "PluginCollage";
				switch (pluginType)
				{
					case PluginType.PLUGIN:
						pluginGallery = "PluginCollage";
						break;
					case PluginType.RESOURCE:
						pluginGallery = "ControlCollage";
						break;
				}
				return pluginGallery;
			}
		}

		/// <summary>
		/// 插件的服务地址
		/// </summary>
		public string PluginService
		{
			get
			{
				return String.Format("{0}/Admin/Plugin/PluginInfomation.asmx", PluginServer);
			}
		}

		/// <summary>
		/// 插件的菜单数目
		/// </summary>
		public int HasMenu
		{
			get
			{
				PluginInfo pinfo = this;
				int result = 0;
				if (pinfo.Pages.Count > 0)
					foreach (UrlItem ui in pinfo.Pages)
					{
						if (ui.EntityID != "System.User")
							result++;
					}
				else
				{
					XmlDocument installdoc = new XmlDocument();
					foreach (string install in pinfo.Deployment.Install)
					{
						string installpath = string.Format("{0}/{1}/Data/{2}", this.PluginsClientPath, pinfo.Directory, install);
						if (File.Exists(installpath))
						{
							installdoc.Load(installpath);
							foreach (XmlNode node in installdoc.SelectNodes("//Sql"))
							{
								string insert = node.InnerText.Trim().ToLower();
								if (insert.Contains("insert") && insert.Contains("menu"))
									result++;
							}
						}
					}
				}
				return result;
			}
		}

		#endregion

		#region Methods

		public void LoadXml(string filePath)
		{
			xmlPath = filePath;

			LoadXml();
		}

		public void LoadXml()
		{
			if (File.Exists(xmlPath))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(xmlPath);
				LoadXml(doc);
			}
			else
			{
				PluginType = PluginType.RESOURCE;
				Directory = Path.GetFileNameWithoutExtension(xmlPath);
				Version = "V3.0";
				XmlDocument doc = new XmlDocument();
				doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginTemplateXmlPath));
				doc.Save(xmlPath);
				Save();
			}
		}

		public void LoadXml(Stream stream)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(stream);
			LoadXml(doc);
		}

		protected virtual void LoadXml(XmlDocument doc)
		{

			XmlNode node = doc.DocumentElement.SelectSingleNode("Name");
			Name = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Author");
			Author = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Version");
			Version = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Compatible");
			Compatible = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("UpdateTime");
			string udt = node != null ? node.InnerText.Trim() : String.Empty;
			DateTime dt;
			DateTime.TryParse(udt, out dt);
			UpdateTime = dt;

			node = doc.DocumentElement.SelectSingleNode("Enable");
			Enable = node == null ? false : node.InnerText.Trim().ToLower() == "true";

			node = doc.DocumentElement.SelectSingleNode("IsInstalled");
			IsInstalled = node == null ? false : node.InnerText.Trim().ToLower() == "true";

			node = doc.DocumentElement.SelectSingleNode("Url");
			Url = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Description");
			Description = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Summary");
			Summary = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Pages");
			if (node != null)
			{
				XmlAttribute attr = node.Attributes["defaultPage"];
				Url = attr != null ? attr.Value : String.Empty;

				XmlNodeList nodes = node.SelectNodes("Add");
				Pages.Clear();
				foreach (XmlNode n in nodes)
				{
					string id = n.Attributes["ID"] != null ? n.Attributes["ID"].Value : String.Empty;
					string name = n.Attributes["Name"] != null ? n.Attributes["Name"].Value : String.Empty;
					string title = n.Attributes["Title"] != null ? n.Attributes["Title"].Value : String.Empty;
					string url = n.Attributes["Url"] != null ? n.Attributes["Url"].Value : String.Empty;
					bool main = n.Attributes["Main"] != null ? n.Attributes["Main"].Value.ToLower() == "true" ? true : false : false;
					string entityid = n.Attributes["EntityID"] != null ? n.Attributes["EntityID"].Value : string.Empty;
					Pages.Add(new UrlItem(id, name, title, url, main, entityid));
				}
			}

			node = doc.DocumentElement.SelectSingleNode("Controls");
			if (node != null)
			{
				Controls = new List<UrlItem>();
				XmlNodeList nodes = node.SelectNodes("Add");

				foreach (XmlNode n in nodes)
				{
					string name = n.Attributes["Name"] != null ? n.Attributes["Name"].Value : String.Empty;
					string url = n.Attributes["Url"] != null ? n.Attributes["Url"].Value : String.Empty;

					Controls.Add(new UrlItem("", name, "", url, false, ""));
				}
			}

			node = doc.DocumentElement.SelectSingleNode("Deployment");

			if (node != null)
			{

				XmlNodeList nodes = doc.DocumentElement.SelectNodes("Deployment/Install/Item");
				foreach (XmlNode n in nodes)
				{
					if (!Deployment.Install.Contains(n.InnerText.Trim()))
						Deployment.Install.Add(n.InnerText.Trim());
				}

				nodes = doc.DocumentElement.SelectNodes("//Deployment/Update/Item");
				foreach (XmlNode n in nodes)
				{
					if (!Deployment.Update.Contains(n.InnerText.Trim()))
						Deployment.Update.Add(n.InnerText.Trim());
				}

				nodes = doc.DocumentElement.SelectNodes("//Deployment/Unstall/Item");
				foreach (XmlNode n in nodes)
				{
					if (!Deployment.Unstall.Contains(n.InnerText.Trim()))
						Deployment.Unstall.Add(n.InnerText.Trim());
				}

				node = node.SelectSingleNode("Description");
				Deployment.Introduction = node != null ? node.InnerText.Trim() : String.Empty;
			}

			node = doc.DocumentElement.SelectSingleNode("Picture");
			if (node != null)
			{
				Thumbnail = node.Attributes["Thumbnail"] != null ? node.Attributes["Thumbnail"].Value.Trim() : String.Empty;
				XmlNodeList nodes = node.SelectNodes("Item");
				foreach (XmlNode n in nodes)
				{
					Snapshot.Add(n.InnerText.Trim());
				}
				//Snapshot = node.Attributes["Snapshot"] != null ? node.Attributes["Snapshot"].Value.Trim() : String.Empty;
			}

			node = doc.DocumentElement.SelectSingleNode("Directory");
			Directory = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("Others");
			Others = node != null ? node.InnerText.Trim() : String.Empty;

			node = doc.DocumentElement.SelectSingleNode("IsLocal");
			IsLocal = node != null ? (node.InnerText.Trim() == "1") : false;
		}

		/// <summary>
		/// 保存插件的配置信息
		/// </summary>
		public void Save()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(xmlPath);

			XmlNode node = doc.DocumentElement.SelectSingleNode("Enable");
			if (node == null)
			{
				node = doc.CreateElement("Enable");
				doc.DocumentElement.AppendChild(node);
			}
			node.InnerText = Enable ? "true" : "false";

			node = doc.DocumentElement.SelectSingleNode("IsInstalled");
			if (node == null)
			{
				node = doc.CreateElement("IsInstalled");
				doc.DocumentElement.AppendChild(node);
			}
			node.InnerText = IsInstalled ? "true" : "false";

			node = doc.DocumentElement.SelectSingleNode("UpdateTime");
			if (node == null)
			{
				node = doc.CreateElement("UpdateTime");
				doc.DocumentElement.AppendChild(node);
			}
			node.InnerText = UpdateTime.ToString();

			node = doc.DocumentElement.SelectSingleNode("IsLocal");
			if (node == null)
			{
				node = doc.CreateElement("IsLocal");
				doc.DocumentElement.AppendChild(node);
			}
			node.InnerText = IsLocal ? "1" : "0";

			node = doc.DocumentElement.SelectSingleNode("Pages");
			if (node == null)
			{
				node = doc.CreateElement("Pages");
				doc.DocumentElement.AppendChild(node);

			}
			node.RemoveAll();
			foreach (UrlItem ui in this.Pages)
			{
				XmlElement ele = doc.CreateElement("Add");
				node.AppendChild(ele);
				ele.SetAttribute("ID", ui.ID);
				ele.SetAttribute("Name", ui.Name);
				ele.SetAttribute("Title", ui.Title);
				ele.SetAttribute("Url", ui.Url);
				ele.SetAttribute("Main", ui.Main.ToString());
				ele.SetAttribute("EntityID", ui.EntityID);
			}
			doc.Save(xmlPath);
		}

		#endregion

	}

	#region Property Class
	/// <summary>
	/// URL描述项（用于插件）
	/// </summary>
	[Serializable]
	public class UrlItem
	{
		#region constructor
		public UrlItem() { }

		public UrlItem(string id, string name, string title, string url, bool main, string entityID)
		{
			ID = id;
			Name = name;
			Title = title;
			Url = url;
			Main = main;
			EntityID = entityID;
		}

		#endregion

		#region Properties
		/// <summary>
		/// 菜单id
		/// </summary>
		public string ID { get; set; }
		/// <summary>
		/// 菜单描述
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 菜单链接地址
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 是否一级菜单
		/// </summary>
		public bool Main { get; set; }

		public string EntityID { get; set; }

		#endregion
	}

	/// <summary>
	/// 插件动作类
	/// </summary>
	[Serializable]
	public class Deployment
	{

		#region Fields

		private List<string> install = new List<string>();
		private List<string> update = new List<string>();
		private List<string> unstall = new List<string>();

		#endregion


		#region Properties

		public List<string> Install
		{
			get
			{
				return install;
			}
		}

		public List<string> Update
		{
			get
			{
				return update;
			}
		}

		public List<string> Unstall
		{
			get
			{
				return unstall;
			}
		}

		public string Introduction { get; set; }

		#endregion
	}

	#endregion

	#region SubClass
	/// <summary>
	/// 远程插件信息
	/// </summary>
	[Serializable]
	public class RemotePluginInfo : PluginInfo
	{

		protected override void LoadXml(XmlDocument doc)
		{
			base.LoadXml(doc);
		}

	}

	#endregion
}
