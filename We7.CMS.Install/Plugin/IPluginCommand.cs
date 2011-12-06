using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using We7.CMS.Config;
using Thinkment.Data;
using System.Xml;
using We7.CMS.Common;
using We7.Framework.Zip;
using We7.Framework;
using We7.Model.Core;
using We7.CMS.WebControls.Core;
using We7.CMS.Accounts;

namespace We7.CMS.Plugin
{
	/// <summary>
	/// 用于执行Plugin的命令
	/// </summary>
	public interface IPluginCommand
	{
		/// <summary>
		/// 运行命令
		/// </summary>
		/// <param name="info">插件信息</param>
		/// <param name="action">当前执行的动作</param>
		/// <returns>执行的信息</returns>
		string Run(PluginInfo info, PluginAction action);
	}

	/// <summary>
	/// 生成插件命令的工厂
	/// </summary>
	public class PluginCommandFactory
	{
		/// <summary>
		/// 取得命令对象
		/// </summary>
		/// <param name="commandName">命令关键字</param>
		/// <returns>命令对象</returns>
		public static IPluginCommand CreateCommand(string commandName)
		{
			IPluginCommand command = null;
			switch (commandName.ToLower())
			{
				case "checkfile":
					command = new PluignCheckFileCommand();
					break;
				case "checkversion":
					command = new PluignCheckVersionCommand();
					break;
				case "download":
					command = new PluginDownLoadCommand();
					break;
				case "installskin":
					command = new PluginInstallSkinCommand();
					break;
				case "installmodels":
					command = new PluginInstallModelsCommand();
					break;
				case "installdbfile":
					command = new PluginInstallDBFileCommand();
					break;
				case "installcontrol":
					command = new PluginInstallWidgetAndControlCommand();
					break;

				case "installdll":
					command = new PluginInstallDllCommand();
					break;
				case "installdb":
					command = new PluginInstallDBCommand();
					break;
				case "delete":
					command = new PluginDeleteCommand();
					break;
				case "updateconfig":
					command = new PluginUpdatePluginFileCommand();
					break;
			}
			return command;
		}
	}

	/// <summary>
	/// 检测插件版本
	/// </summary>
	public class PluignCheckVersionCommand : IPluginCommand
	{
		public const string Version = "V2.0";

		#region IPluginCommand 成员
		public string Run(PluginInfo info, PluginAction action)
		{
			if (info.PluginType == PluginType.RESOURCE)
				return new PluginJsonResult(true).ToString();

			info.LoadXml();
			if (String.IsNullOrEmpty(info.Version) || String.Compare(info.Version, Version, true) < 0)
			{
				if (action == PluginAction.INSTALL || action == PluginAction.REMOTEINSTALL)
				{
					try
					{
						Directory.Delete(info.PluginClientPath, true);
					}
					catch (Exception ex)
					{
						Logger.Warn(info.PluginClientPath + "不能删除:\r\n" + ex.Message);
					}
				}
				return new PluginJsonResult(false, "当前插件版本低于" + Version + "。不能进行安装！").ToString();
			}
			return new PluginJsonResult(true).ToString();
		}

		#endregion
	}

	/// <summary>
	/// 检测插件的文件
	/// </summary>
	public class PluignCheckFileCommand : IPluginCommand
	{
		#region IPluginCommand 成员

		public string Run(PluginInfo info, PluginAction action)
		{
			if (info.PluginType == PluginType.RESOURCE)
				return new PluginJsonResult(true).ToString();

			try
			{
				string dataDir = Path.Combine(info.PluginClientPath, "Data");
				switch (action)
				{
					case PluginAction.INSTALL:
					case PluginAction.REMOTEINSTALL:
						CheckFileExits(info.Deployment.Install, dataDir);
						CheckFileExits(info.Deployment.Update, dataDir);
						break;
					case PluginAction.UPDATE:
					case PluginAction.REMOTEUPDATE:
						CheckFileExits(info.Deployment.Update, dataDir);
						break;
					case PluginAction.UNSTALL:
						CheckFileExits(info.Deployment.Update, dataDir);
						break;
				}
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}

		#endregion

		/// <summary>
		/// 检测布署文件是否存在
		/// </summary>
		/// <param name="files"></param>
		/// <param name="dir"></param>
		private void CheckFileExits(List<string> files, string dir)
		{
			foreach (string file in files)
			{
				string filePath = Path.Combine(dir, file);
				if (!File.Exists(filePath))
					throw new Exception("数据布署文件不存在！");
			}
		}
	}

	/// <summary>
	/// 更新插件的配置文件
	/// </summary>
	public class PluginUpdatePluginFileCommand : IPluginCommand
	{
		#region IPluginCommand 成员

		public string Run(PluginInfo info, PluginAction action)
		{
			if (info.PluginType == PluginType.RESOURCE)
				return new PluginJsonResult(true).ToString();

			try
			{
				switch (action)
				{
					case PluginAction.INSTALL:
					case PluginAction.REMOTEINSTALL:
					case PluginAction.UPDATE:
					case PluginAction.REMOTEUPDATE:
						info.IsInstalled = true;
						break;
					case PluginAction.UNSTALL:
						info.IsInstalled = false;
						break;
				}
				info.Save();
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}

		#endregion
	}


	/// <summary>
	/// 从服务器下载插件
	/// </summary>
	public class PluginDownLoadCommand : IPluginCommand
	{

		#region IPluginCommand 成员

		public string Run(PluginInfo info, PluginAction action)
		{
			int menuCount = 0;
			try
			{
				WebClient client = new WebClient();
				byte[] buffer = client.DownloadData(info.FilePath);
				string temppath = Path.Combine(info.PluginsClientPath, "_temp");
				if (!Directory.Exists(temppath))
				{
					Directory.CreateDirectory(temppath);
				}

				try
				{
					ExtractZipFile(buffer, temppath);

					string pluginCfg = Path.Combine(temppath, info.Directory);
					pluginCfg = Path.Combine(pluginCfg, "Plugin.xml");
					PluginInfo tempInfo = new PluginInfo(pluginCfg);
					if (String.IsNullOrEmpty(tempInfo.Version) || String.Compare(tempInfo.Version, PluignCheckVersionCommand.Version, true) < 0)
					{
						if (PluginAction.UPDATE == action || PluginAction.REMOTEUPDATE == action)
						{
							return new PluginJsonResult(false, "插件版本低于" + PluignCheckVersionCommand.Version + ",不能进行更新!").ToString();
						}
						else if (PluginAction.REMOTEINSTALL == action || PluginAction.INSTALL == action)
						{
							return new PluginJsonResult(false, "插件版本低于" + PluignCheckVersionCommand.Version + ",不能进行添加!").ToString();
						}
					}
					else
					{

						ExtractZipFile(buffer, info.PluginsClientPath);
						PluginInfo localInfo = PluginInfoCollection.CreateInstance(info.PluginType)[info.Directory];
						if (localInfo != null)
						{
							localInfo.IsLocal = false;
							localInfo.Save();
							if (PluginAction.UPDATE != action && PluginAction.REMOTEUPDATE != action)
								menuCount = localInfo.HasMenu;
						}
					}
				}
				catch (Exception ex)
				{
					return new PluginJsonResult(false, ex.Message).ToString();
				}
				finally
				{
					Directory.Delete(temppath, true);
				}
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true, menuCount.ToString()).ToString();
		}



		#endregion

		/// <summary>
		/// 解压ZIP文件
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="directory"></param>
		public void ExtractZipFile(byte[] buffer, string directory)
		{

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				ms.Position = 0;
				ZipUtils.ExtractZip(ms, directory);
			}
		}
	}


	/// <summary>
	/// 从服务器下载插件(资源平台使用)
	/// </summary>
	public class ResourcePlatformPluginDownLoadCommand
	{

		#region IPluginCommand 成员

		public string Run(PluginInfo info, PluginAction action, string pluginPath)
		{
			try
			{
				WebClient client = new WebClient();
				byte[] buffer;
				try
				{
					buffer = client.DownloadData(pluginPath);
				}
				catch
				{
					buffer = client.DownloadData(pluginPath);
				}
				string temppath = Path.Combine(info.PluginsClientPath, "_temp");
				if (!Directory.Exists(temppath))
				{
					Directory.CreateDirectory(temppath);
				}

				try
				{
					ExtractZipFile(buffer, temppath);

					string pluginCfg = Path.Combine(temppath, info.Directory);
					pluginCfg = Path.Combine(pluginCfg, "Plugin.xml");
					if (File.Exists(pluginCfg))
					{
						PluginInfo tempInfo = new PluginInfo(pluginCfg);
						if (String.IsNullOrEmpty(tempInfo.Version) || String.Compare(tempInfo.Version, PluignCheckVersionCommand.Version, true) < 0)
						{
							if (PluginAction.UPDATE == action || PluginAction.REMOTEUPDATE == action)
							{
								return new PluginJsonResult(false, "插件版本低于" + PluignCheckVersionCommand.Version + ",不能进行更新!").ToString();
							}
							else if (PluginAction.REMOTEINSTALL == action || PluginAction.INSTALL == action)
							{
								return new PluginJsonResult(false, "插件版本低于" + PluignCheckVersionCommand.Version + ",不能进行添加!").ToString();
							}
						}
						else
						{
							ExtractZipFile(buffer, info.PluginsClientPath);
							PluginInfo localInfo = PluginInfoCollection.CreateInstance(info.PluginType)[info.Directory];
							if (info != null)
							{
								localInfo.IsLocal = false;
								localInfo.Save();
							}
						}
					}
					else
					{
						ExtractZipFile(buffer, info.PluginsClientPath);
						PluginInfo localInfo = PluginInfoCollection.CreateInstance(info.PluginType)[info.Directory];
						if (localInfo != null)
						{
							localInfo.IsLocal = false;
							localInfo.Save();
						}
					}
				}
				catch (Exception ex)
				{
					return new PluginJsonResult(false, ex.Message + "堆栈：" + ex.StackTrace + "。Inner：" + ex.InnerException.Message).ToString();
				}
				finally
				{
					Directory.Delete(temppath, true);
				}
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message + "堆栈：" + ex.StackTrace + "。Inner：" + ex.InnerException.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}



		#endregion

		/// <summary>
		/// 解压ZIP文件
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="directory"></param>
		public void ExtractZipFile(byte[] buffer, string directory)
		{

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				ms.Position = 0;
				ZipUtils.ExtractZip(ms, directory);
			}
		}
	}

	/// <summary>
	/// 拷贝数据文件抽象类
	/// </summary>
	public abstract class PluginCopyCommand : IPluginCommand
	{

		#region IPluginCommand 成员

		public abstract string Run(PluginInfo info, PluginAction action);

		#endregion

		/// <summary>
		/// 拷贝安装文件到指定目录或删除指定目录下的文件
		/// </summary>
		/// <param name="info">控件信息</param>
		/// <param name="action">当前动作</param>
		/// <param name="source">源文件</param>
		/// <param name="target">目标文件</param>
		protected void CopyFile(PluginInfo info, PluginAction action, string source, string target)
		{
			string srcPath = Path.Combine(info.PluginClientPath, source);
			DirectoryInfo sourceDir = new DirectoryInfo(srcPath);
			if (!sourceDir.Exists)
				return;
			//throw new Exception("安装文件缺失,请检查安装文件");

			string targetPath = Path.Combine(info.BaseDircotry, target);
			DirectoryInfo targetDir = new DirectoryInfo(targetPath);

			if (!targetDir.Exists)
				targetDir.Create();

			switch (action)
			{
				case PluginAction.INSTALL:
				case PluginAction.UPDATE:
				case PluginAction.REMOTEINSTALL:
				case PluginAction.REMOTEUPDATE:
					foreach (FileInfo file in sourceDir.GetFiles())
					{
						file.CopyTo(Path.Combine(targetPath, Path.GetFileName(file.FullName)), true);
					}
					break;

				case PluginAction.UNSTALL:
					foreach (FileInfo file in sourceDir.GetFiles())
					{
						string filePath = Path.Combine(targetPath, Path.GetFileName(file.FullName));
						if (File.Exists(filePath))
							File.Delete(filePath);
					}
					foreach (DirectoryInfo di in sourceDir.GetDirectories())
					{
						if (di.Name.Contains(".svn"))
							continue;
						string dipath = Path.Combine(targetPath, di.Name);
						if (Directory.Exists(dipath))
							Directory.Delete(dipath);
					}
					if(0== targetDir.GetFileSystemInfos().Length)
						targetDir.Delete();;
					break;
			}
		}

		/// <summary>
		/// 从指定目录复制文件
		/// </summary>
		/// <param name="info">插件信息</param>
		/// <param name="action">当前动作</param>
		/// <param name="source">源目录</param>
		/// <param name="target">目录目录</param>
		protected void CopyDirectory(PluginInfo info, PluginAction action, string source, string target)
		{
			string srcPath = Path.Combine(info.BaseDircotry, source);
			DirectoryInfo sourceDir = new DirectoryInfo(srcPath);
			if (!sourceDir.Exists)
				return;

			string targetPath = Path.Combine(info.BaseDircotry, target);
			DirectoryInfo targetDir = new DirectoryInfo(targetPath);

			if (!targetDir.Exists)
				targetDir.Create();

			switch (action)
			{
				case PluginAction.INSTALL:
				case PluginAction.UPDATE:
				case PluginAction.REMOTEINSTALL:
				case PluginAction.REMOTEUPDATE:
					CopyDirectory(srcPath, targetPath);
					break;
				case PluginAction.UNSTALL:
					foreach (DirectoryInfo dir in sourceDir.GetDirectories())
					{
						string path = Path.Combine(targetDir.FullName, dir.Name);
						if (path.EndsWith(".svn", StringComparison.CurrentCultureIgnoreCase))
							continue;
						if (Directory.Exists(path))
						{
							Directory.Delete(path, true);
						}
					}
					break;
			}

		}

		/// <summary>
		///　拷贝目录
		/// </summary>
		/// <param name="source">源目录</param>
		/// <param name="target">目标目录</param>
		protected void CopyDirectory(string source, string target)
		{
			DirectoryInfo dirsrc = new DirectoryInfo(source);
			DirectoryInfo dirtarget = new DirectoryInfo(target);

			if (!dirsrc.Exists)
				throw new Exception("源目录不存在");

			if (!dirtarget.Exists)
				dirtarget.Create();

			foreach (FileInfo f in dirsrc.GetFiles())
			{
				f.CopyTo(Path.Combine(target, f.Name), true);
			}

			foreach (DirectoryInfo dir in dirsrc.GetDirectories())
			{
				if (dir.Name.Contains(".svn"))
					continue;
				CopyDirectory(dir.FullName, Path.Combine(target, dir.Name));
			}
		}
	}

	/// <summary>
	/// 安装数据映射文件
	/// </summary>
	public class PluginInstallDBFileCommand : PluginCopyCommand
	{

		public override string Run(PluginInfo info, PluginAction action)
		{
			if (info.PluginType == PluginType.RESOURCE)
				return new PluginJsonResult(true).ToString();

			try
			{
				CopyFile(info, action, "Data/Relation", "App_Data/XML");
				//CopyFile(info, action, "Data", "Install/SQL/plugin_" + info.Directory);
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
	}

	/// <summary>
	/// 删除插件
	/// </summary>
	public class PluginDeleteCommand : IPluginCommand
	{

		public string Run(PluginInfo info, PluginAction action)
		{
			try
			{
				DirectoryInfo dir = new DirectoryInfo(info.PluginClientPath);
				if (dir.Exists)
					dir.Delete(true);
				//删除sql目录
				dir = new DirectoryInfo(Path.Combine(info.BaseDircotry, "Install/SQL/plugin_" + info.Directory));
				if (dir.Exists)
					dir.Delete(true);
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
	}


	/// <summary>
	/// 安装前台部件和控件
	/// </summary>
	public class PluginInstallWidgetAndControlCommand : PluginCopyCommand
	{

		public override string Run(PluginInfo info, PluginAction action)
		{
			try
			{
				string widgetDownloadPath = Path.Combine(Constants.We7WidgetsFileFolder, "商城下载类");
				if (!Directory.Exists(widgetDownloadPath))
					Directory.CreateDirectory(widgetDownloadPath);

				BaseControlHelper baseControlHelper = new BaseControlHelper();
				string srcWidgetPath = Path.Combine(info.PluginClientPath, "Widget");
				if (Directory.Exists(srcWidgetPath))
				{
					CopyDirectory(info, action, srcWidgetPath, widgetDownloadPath);
					baseControlHelper.CreateWidegetsIndex();//创建部件索引
				}

				string srcPath = Path.Combine(info.PluginClientPath, "Control");
				if (Directory.Exists(srcPath))
				{
					CopyDirectory(info, action, srcPath, "We7Controls");
					baseControlHelper.CreateIntegrationIndexConfig();//创建控件索引               
				}
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
	}

	/// <summary>
	/// 安装前台模板
	/// </summary>
	public class PluginInstallSkinCommand : PluginCopyCommand
	{
		public override string Run(PluginInfo info, PluginAction action)
		{
			try
			{
				string srcPath = Path.Combine(info.PluginClientPath, "Skin");
				CopyDirectory(info, action, srcPath, "_Skins");
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
	}

	/// <summary>
	/// 模型安装
	/// </summary>
	public class PluginInstallModelsCommand : PluginCopyCommand
	{
		public override string Run(PluginInfo info, PluginAction action)
		{
			try
			{
				string srcPath = Path.Combine(info.PluginClientPath, "Model");
				CopyDirectory(info, action, srcPath, "Models");
				ModelHelper.ReCreateModelIndex();
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
	}



	/// <summary>
	/// 安装Dll文件
	/// </summary>
	public class PluginInstallDllCommand : PluginCopyCommand
	{
		public override string Run(PluginInfo info, PluginAction action)
		{
			if (info.PluginType == PluginType.RESOURCE)
				return new PluginJsonResult(true).ToString();

			try
			{
				CopyFile(info, action, "Bin", "Bin");
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
	}
	/// <summary>
	/// 安装或卸载插件中对数据库的更新
	/// </summary>
	public class PluginInstallDBCommand : IPluginCommand
	{
		#region IPluginCommand 成员

		public string Run(PluginInfo info, PluginAction action)
		{
			if (info.PluginType == PluginType.RESOURCE)
				return new PluginJsonResult(true).ToString();


			try
			{
				string dataDir = Path.Combine(info.PluginClientPath, "Data");

				switch (action)
				{
					case PluginAction.INSTALL:
					case PluginAction.REMOTEINSTALL:
						ExecuteDBFile(dataDir, info.Deployment.Install);
						ExecuteDBFile(dataDir, info.Deployment.Update);
						break;
					case PluginAction.UPDATE:
					case PluginAction.REMOTEUPDATE:
						ExecuteDBFile(dataDir, info.Deployment.Update);
						break;
					case PluginAction.UNSTALL:
						ExecuteDBFile(dataDir, info.Deployment.Unstall);
						MenuHelper menuHelper = HelperFactory.Instance.GetHelper<MenuHelper>();
						foreach (UrlItem ui in info.Pages)
						{
							menuHelper.DeleteMenuItem(ui.ID);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				return new PluginJsonResult(false, ex.Message).ToString();
			}
			return new PluginJsonResult(true).ToString();
		}
		/// <summary>
		/// 执行指定的数据文件
		/// </summary>
		/// <param name="dataDir"></param>
		/// <param name="DBFileList"></param>
		private void ExecuteDBFile(string dataDir, List<string> DBFileList)
		{
			foreach (string file in DBFileList)
			{
				string filePath = Path.Combine(dataDir, file);
				if (!File.Exists(filePath))
					throw new Exception(String.Format("数据文件{0}不存在!", file));
				ExcuteSQL(BaseConfigs.GetBaseConfig(), filePath);
			}
		}

		#endregion

		/// <summary>
		/// 执行SQL，进行数据库初始化
		/// </summary>
		public static void ExcuteSQL(BaseConfigInfo bci, string updateFile)
		{
			if (updateFile != "")
			{
				string connectionString = bci.DBConnectionString.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
				XmlDocument doc = new XmlDocument();
				doc.Load(updateFile);

				foreach (XmlNode node in doc.SelectNodes("/Update/Database"))
				{
					IDbDriver dbDriver = CreateDbDriver(bci.DBType);
					if (dbDriver == null) continue;

					//开始处理

					int success = 0;
					int errors = 0;
					using (IConnection conn = dbDriver.CreateConnection(connectionString))
					{
						foreach (XmlNode sub in node.SelectNodes("Sql"))
						{
							if (sub == null || String.IsNullOrEmpty(sub.InnerText) || String.IsNullOrEmpty(sub.InnerText.Trim()))
								continue;
							//不执行菜单操作
							string menuop = sub.InnerText.Trim().ToLower();
							if (menuop.StartsWith("insert") || menuop.StartsWith("delete") && menuop.Contains("menu")) continue;

							//读取SQL语句，逐一执行
							SqlStatement sql = new SqlStatement();
							sql.CommandType = System.Data.CommandType.Text;
							sql.SqlClause = sub.InnerText.Trim();
							dbDriver.FormatSQL(sql);
							try
							{
								conn.Update(sql);
								success++;
							}
							catch (Exception ex)
							{

								//出现了错误，我们继续执行
								We7.Framework.LogHelper.WriteFileLog(We7.Framework.LogHelper.sql_plugin_update,
																		"执行SQL：" + sql.SqlClause, ex.Message);
								errors++;
								continue;
							}
						}
					}


					We7.Framework.LogHelper.WriteFileLog(We7.Framework.LogHelper.sql_plugin_update.ToString(),
																	   "执行完毕：",
																	   string.Format("{3}执行完毕！共执行语句{0}条，成功{1}，失败{2} 。", success + errors, success, errors, updateFile));
				}
			}
		}

		/// <summary>
		/// 根据数据库类型创建驱动对象
		/// </summary>
		/// <param name="dbType">类型字符串</param>
		/// <returns></returns>
		public static IDbDriver CreateDbDriver(string dbType)
		{
			IDbDriver driver = null;
			switch (dbType.ToLower())
			{
				case "sqlite":
					driver = new SQLiteDriver();
					break;
				case "access":
					driver = new OleDbDriver();
					break;
				case "sqlserver":
					driver = new SqlDbDriver();
					break;
				case "mysql":
					driver = new MySqlDriver();
					break;
				case "oracle":
					driver = new OracleDriver();
					break;
			}
			return driver;

		}
	}

}
