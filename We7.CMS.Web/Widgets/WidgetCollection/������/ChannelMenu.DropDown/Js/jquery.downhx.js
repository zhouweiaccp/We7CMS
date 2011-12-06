// JavaScript Document
function TabsA(num)
{
	var tab=$_$("Tab_"+num)
	var content=$_$("Content_"+num)
	for(var i=0;i<16;i++)
	{
		if((i+1)%4==0)
		{
			$_$("Tab_"+i).style.cssText="margin-right:0px;";
		}
		$_$("Tab_"+i).className="";
		$_$("Content_"+i).className="myul hidden";
	}
	content.className="myul block";
	if((num+1)%13==0)
	{
		tab.className="cli_5 lblue fbold";
		content.className="myul block";
	}
	else if((num+1)%14==0||(num+1)%15==0)
	{
		tab.className="cli_6 lblue fbold";
		content.className="myul block";
	}
	else if((num+1)%16==0)
	{
		tab.className="cli_7 lblue fbold";
		content.className="myul block";
	}
	else if((num+1)%4==1)
	{
		tab.className="cli_1 lblue fbold";
	}
	else if((num+1)%4==0)
	{
		tab.className="cli_3 lblue fbold";
	}
	else
	{
		tab.className="cli_2 lblue fbold";
	}
}


