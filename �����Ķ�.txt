欢迎进入We7世界，使用源码前您需要了解的几件事情，请务必明确：
1、本项目使用 Visual studio 2010 创建，其他版本未做测试，请尽量使用VS2010。
2、本项目的发布时不需要使用VS2010的“发布网站”功能，如想发布为干净的.aspx+dll的运行文件，请按下面步骤：
（1）安装7-zip的压缩工具，并尽量安装到 “c:\Program Files\7-zip\7z.exe”路径；
（2）确保VS2010的主程序在“C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe”路径下；
（3）如以上路径有变化，请打开根目录下的“build.bat”文件进行编辑，将对应的两个路径修改为与本机实际路径一致皆可。
（4）运行build.bat。
（5）结果生成在 与根目录同级的目录 Deploy 下，We7.CMS.Upgrade.zip，We7.CMS.Install.zip；其中，

We7.CMS.Upgrade.zip - 是用来升级旧版本的升级包；
We7.CMS.Install.zip - 是全新安装包。

3、如果本机未安装 SVN，请将 We7.Framework 项目属性中，"生成事件-预生成事件命令行(R)"中的内容清除。

4、因为windows操作系统版本不同，有的机器会出现“找不到System.Web.Extensions.dll的引用”的错误，处理办法：
在出错的项目下，删除原来System.Web.Extensions.dll的引用，重新对这个dll做一下引用即可（位置在 Solution Items\DLL下）。

5、更多关于源码方面的信息，请参看 http://help.we7.cn/library/7.html

希望We7代码能带给你愉悦！
We7开发小组 * 2010年12月