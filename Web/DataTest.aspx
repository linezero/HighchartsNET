<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTest.aspx.cs" Inherits="DataTest" %>

<%@ Register TagPrefix="Zero" Namespace="HighchartsNET" Assembly="HighchartsNET" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <Zero:HighCharts runat="server" ID="highcharts1" Title="折线图" />
    <Zero:HighCharts runat="server" ID="highcharts2" Title="折线图" />
    </div>
    </form>
</body>
</html>
