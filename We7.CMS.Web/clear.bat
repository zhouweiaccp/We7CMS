
@echo off
echo.正在清理...出现“文件无法找到到”等错误不影响清理！

RD  Models\System\Article /s/q
DEL Models\System\Article.xml
DEL Admin\Ajax\TagAdd.aspx
DEL Admin\Ajax\TagDelete.aspx
DEL Admin\ContentModel\Controls\LayoutBind.ascx
DEL Admin\ContentModel\Controls\AdviceLayoutBind.ascx

RD  Widgets\WidgetCollection\问卷类 /s/q

DEL admin\Addins\CommentDetail.aspx
DEL admin\Addins\Links.aspx
DEL admin\Addins\LinkDetail.aspx

DEL admin\manage\controls\Publish_account.ascx
DEL admin\manage\controls\Publish_chart.ascx
DEL admin\manage\controls\Publish_Profile.ascx
DEL admin\manage\controls\Statistics_chart.ascx
DEL admin\manage\controls\Statistics_Keyword.ascx
DEL admin\manage\controls\Statistics_Months.ascx
DEL admin\manage\controls\Statistics_Profile.ascx
DEL admin\manage\controls\StatisticsArticleControl.ascx
DEL admin\manage\controls\StatisticsUserControl.ascx
DEL admin\manage\controls\StatisticsUserDetailControl.ascx
DEL admin\manage\controls\VisiteCounterControl.ascx
DEL admin\manage\AddMenuUser.aspx
DEL admin\manage\CssDelete.aspx
DEL admin\manage\CssDetail.aspx
DEL admin\manage\CssList.aspx
DEL admin\manage\PublishStatisticses.aspx
DEL admin\manage\StatisticsDetail.aspx
DEL admin\manage\Statisticses.aspx
DEL admin\manage\StatisticsMonths.aspx
DEL admin\manage\StrategyDetail.aspx
DEL admin\manage\StrateList.aspx
DEL admin\manage\UserMenulist.aspx
DEL admin\manage\TemplateStaticize.aspx

RD  admin\Questionnaire /s/q
RD  admin\ScoreManager /s/q

RD  admin\tools\controls /s/q
DEL admin\tools\DBMaintenance.aspx
DEL admin\tools\NotifyMail.aspx

RD  admin\VisualTemplate\WidgetCssEditor /s/q

DEL admin\ModuleAdd.aspx
DEL admin\ModuleManager.aspx

echo.----------------------------------------

echo.清理完成！

echo.按任意键退出...
pause>nul
