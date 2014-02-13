<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register TagPrefix="Zero" Namespace="HighchartsNET" Assembly="HighchartsNET" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery.1.9.1.min.js" type="text/javascript"></script>
    <script src="js/highcharts.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:500px;">   
    </div>
    <Zero:HighCharts runat="server" ID="highcharts1" Title="图表插件" />
    </form>
</body>
</html>
