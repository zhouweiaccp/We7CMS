<html>
<head>
    <script type="text/javascript" src="common.js"></script>    
</head>
<body>
    <div id="a">
        <table>
            <tr>
                <td>
                    姓名
                </td>
                <td>
                    <input type="text" id="keywords" />
                </td>
            </tr>
            <tr>
                <td>
                    密码
                </td>
                <td>
                    <button id="querybutton" value="搜索">
                    </button>
                </td>
            </tr>
        </table>       
    </div>
     <script type="text/javascript">
         $.installPlugins(["Search.js"]);
         $(function () {
             $("#a").SearchKeyWords();
         });
         
     </script>
</body>
</html>
