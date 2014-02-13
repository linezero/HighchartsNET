using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        highcharts1.Type = HighchartsNET.ChartType.Line;
        highcharts1.SubTitle = "二级菜单";
        highcharts1.Tooltip = "valueSuffix: '°C'";
        Dictionary<string, double> dic = new Dictionary<string, double>();
        Random r = new Random();
        for (int i = 0; i < 20; i++)
        {
            dic.Add(DateTime.Now.AddDays(i).ToShortDateString(), r.Next(20));
        }
        highcharts1.YAxis = "摄氏度℃";
        highcharts1.Series = new HighchartsNET.ChartsSeries { SeriesName="温度",SeriesData=dic};
    }
}