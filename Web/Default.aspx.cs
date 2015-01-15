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
        Dictionary<object, object> dic = new Dictionary<object, object>();
        Random r = new Random();
        for (int i = 0; i < 12; i++)
        {
            dic.Add(DateTime.Now.AddDays(i).ToShortDateString(), r.Next(20));
        }

        Dictionary<object, object> dic1 = new Dictionary<object, object>();
        Random r1 = new Random();
        for (int i = 0; i < 12; i++)
        {
            dic1.Add(DateTime.Now.AddDays(i).ToShortDateString(), r.Next(20,100));
        }

        highcharts1.Type = HighchartsNET.ChartType.Line;
        highcharts1.SubTitle = "二级标题";
        highcharts1.Tooltip = "valueSuffix: '°C'";
        highcharts1.YAxis = "摄氏度℃";//Y轴的值;
        highcharts1.Series = new HighchartsNET.ChartsSeries { SeriesName = "温度", SeriesData = dic };

        highcharts2.Type = HighchartsNET.ChartType.Column;
        highcharts2.Tooltip = "pointFormat: '<span style=\"color:{series.color};padding:0\">{series.name}: <b>{point.y:.1f} ℃</b></span>'";
        highcharts2.YAxis = "摄氏度℃";//Y轴的值;
        highcharts2.Series = new HighchartsNET.ChartsSeries { SeriesName = "温度", SeriesData = dic };

        highcharts3.DivId = "chart3";
        highcharts3.Type = HighchartsNET.ChartType.Pie;
//        highcharts3.PlotOptions = @"pie: {
//                allowPointSelect: true,
//                cursor: 'pointer',
//                dataLabels: {
//                    enabled: true,
//                    color: '#000000',
//                    connectorColor: '#000000',
//                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
//                }
//            }";
        highcharts3.Tooltip = "pointFormat: '{series.name}: <b>{point.percentage:.1f} %</b>'";
        highcharts3.Series = new HighchartsNET.ChartsSeries { SeriesName = "温度", SeriesData = dic };

        mychart.Type = HighchartsNET.ChartType.Line;
        mychart.SeriesList = new List<HighchartsNET.ChartsSeries> { 
            new HighchartsNET.ChartsSeries { SeriesName = "温度", SeriesData = dic },
            new HighchartsNET.ChartsSeries { SeriesName = "湿度", SeriesData = dic1 }
        };

        highcharts5.DivId = "chart5";
        highcharts5.Type = HighchartsNET.ChartType.Column;
        highcharts5.SeriesList = new List<HighchartsNET.ChartsSeries> { 
            new HighchartsNET.ChartsSeries { SeriesName = "温度", SeriesData = dic },
            new HighchartsNET.ChartsSeries { SeriesName = "湿度", SeriesData = dic1 }
        };

        highcharts6.DivId = "chart6";
        highcharts6.Type = HighchartsNET.ChartType.Bar;
        highcharts6.Tooltip = "pointFormat: '<span style=\"color:{series.color};padding:0\">{series.name}: <b>{point.y:.1f} ℃</b></span>'";
        highcharts6.YAxis = "摄氏度℃";//Y轴的值;
        highcharts6.Series = new HighchartsNET.ChartsSeries { SeriesName = "温度", SeriesData = dic };
    }
}