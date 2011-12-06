using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;

namespace We7.Model.Core
{
	/// <summary>
	/// 容器信息
	/// </summary>
	[Serializable]
	public class ContainerInfo
	{
		/// <summary>
		/// 容器路径
		/// </summary>
		[XmlAttribute("path")]
		public string Path { get; set; }

		/// <summary>
		/// 可见性
		/// </summary>
		[XmlAttribute("visible")]
		public bool Visible { get; set; }

		/// <summary>
		/// 容器名
		/// </summary>
		[XmlAttribute("name")]
		public string Name { get; set; }

	}

	/// <summary>
	/// 编辑容器信息
	/// </summary>
	[Serializable]
	public class EditInfo : ContainerInfo
	{
		private GroupCollection groups = new GroupCollection();
		/// <summary>
		/// 分组
		/// </summary>
		[XmlElement("group")]
		public GroupCollection Groups
		{
			get { return groups; }
			set { groups = value; }
		}

		private We7ControlCollection controls = new We7ControlCollection();
		/// <summary>
		/// 当前容器含有的控件
		/// </summary>
		[XmlIgnore]
		public We7ControlCollection Controls
		{
			get
			{
				if (controls == null || controls.Count == 0)
				{
					foreach (Group group in groups)
					{
						if (group.Enable)
						{
							foreach (We7Control ctr in group.Controls)
							{
								controls.Add(ctr);
							}
							break;
						}
					}
				}
				return controls;
			}
		}

		/// <summary>
		/// 是否是用户自定义控件
		/// </summary>
		public bool IsUCEditor
		{
			get
			{
				return !String.IsNullOrEmpty(Path) && Path.EndsWith(".ascx", true, CultureInfo.CurrentCulture);
			}
		}

		/// <summary>
		/// 是否自定义浏览控件
		/// </summary>
		public bool IsUCViewer
		{
			get { return !String.IsNullOrEmpty(ViewerPath) && ViewerPath.EndsWith(".ascx", true, CultureInfo.CurrentCulture); }
		}

		/// <summary>
		/// 布局控件
		/// </summary>
		[XmlAttribute("layout")]
		public string Layout { get; set; }

		[XmlAttribute("viewerPath")]
		public string ViewerPath { get; set; }

		[XmlAttribute("viewerLayout")]
		public string ViewerLayout { get; set; }

		[XmlAttribute("viewerCss")]
		public string ViewerCss { get; set; }

		[XmlAttribute("editCss")]
		public string EditCss { get; set; }

		[XmlAttribute("ucLayout")]
		public string UcLayout { get; set; }

		[XmlAttribute("ucCss")]
		public string UcCss { get; set; }
	}

	/// <summary>
	/// 列表容器
	/// </summary>
	[Serializable]
	public class ListInfo : ContainerInfo
	{
		private GroupCollection groups = new GroupCollection();
		/// <summary>
		/// 分组
		/// </summary>
		[XmlElement("group")]
		public GroupCollection Groups
		{
			get { return groups; }
			set { groups = value; }
		}
	}

	/// <summary>
	/// 条件容器
	/// </summary>
	[Serializable]
	public class ConditionInfo : ContainerInfo
	{
		private We7ControlCollection controls = new We7ControlCollection();
		/// <summary>
		/// 含有的查询控件
		/// </summary>
		[XmlElement("control")]
		public We7ControlCollection Controls
		{
			get { return controls; }
			set { controls = value; }
		}
	}

	/// <summary>
	/// 分页容器
	/// </summary>
	[Serializable]
	public class PagerInfo : ContainerInfo
	{
	}

	/// <summary>
	/// 命令容器
	/// </summary>
	[Serializable]
	public class CommandInfo : ContainerInfo
	{
	}
	/// <summary>
	/// 导航容器
	/// </summary>
	[Serializable]
	public class NavigationInfo : ContainerInfo
	{
	}
}
