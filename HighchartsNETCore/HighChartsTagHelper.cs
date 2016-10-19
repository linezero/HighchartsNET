using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HighchartsNETCore
{
    public class HighChartsTagHelper : TagHelper
    {
        /// <summary>
        /// 图表标题
        /// </summary>
        [HtmlAttributeName("title")]
        public string Title { get; set; }
        /// <summary>
        /// 图表类型
        /// </summary>
        [HtmlAttributeName("type")]
        public ChartType Type { get; set; }
        /// <summary>
        /// 图表2级标题
        /// </summary>
        [HtmlAttributeName("subtitle")]
        public string SubTitle { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        [HtmlAttributeName("series")]
        public ChartsSeries Series { get; set; }

        /// <summary>
        /// 一些附加选项
        /// </summary>
        [HtmlAttributeName("plotoptions")]
        public string PlotOptions { get; set; }
        /// <summary>
        /// X轴选项
        /// </summary>
        [HtmlAttributeName("xAxis")]
        public List<object> XAxis { get; set; }
        /// <summary>
        /// Y轴选项 默认可以只填名称
        /// </summary>
        [HtmlAttributeName("yAxis")]
        public string YAxis { get; set; }

        /// <summary>
        /// 提示格式
        /// </summary>
        [HtmlAttributeName("Tooltip")]
        public string Tooltip { get; set; }
        /// <summary>
        /// 图表层id（容器）
        /// </summary>
        [HtmlAttributeName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 图标下方标识是否显示 默认不显示
        /// </summary>
        [HtmlAttributeName("legend")]
        public bool Legend { get; set; }

        /// <summary>
        /// 高级功能，多个数据集，多条图表，饼图不需要。
        /// </summary>
        [HtmlAttributeName("serieslist")]
        public List<ChartsSeries> SeriesList { get; set; }

        [HtmlAttributeName("width")]
        public int Width { get; set; }

        [HtmlAttributeName("height")]
        public int Height { get; set; }

        private void HighChartsJs(StringBuilder jscode)
        {
            jscode.Append("$(function(){$('#" + Id + "').highcharts({ ");
            jscode.Append("credits: { enabled: false },");
            jscode.Append("chart:{ type: '" + Type.ToString().ToLower() + "'");
            if (Width>0)
                jscode.Append(",width:" + Width);
            if (Height>0)
                jscode.Append(",height:" + Height);
            jscode.Append("},");
            if (!string.IsNullOrEmpty(Title))
                jscode.Append("title: { text: '" + Title + "'},");
            if (!string.IsNullOrEmpty(SubTitle))
                jscode.Append("subtitle: { text: '" + SubTitle + "'},");
            //判断类型及数据显示
            if (XAxis != null && Type != ChartType.Pie)
            {                
                XAxisToString(jscode, XAxis);
            }
            else if (Series.SeriesData != null && Type != ChartType.Pie)
            {
                XAxisToString(jscode, Series.SeriesData.Keys.ToList());
            }
            else if (SeriesList != null && SeriesList.Count > 0)
            {
                XAxisToString(jscode, SeriesList[0].SeriesData.Keys.ToList());
            }
            if (!string.IsNullOrEmpty(YAxis))
            {
                if (YAxis.IndexOf("title") < 0)
                {
                    jscode.Append("yAxis: { title:{ text:'" + YAxis + "'}},");
                    if(string.IsNullOrEmpty(Tooltip))
                        jscode.Append("tooltip: { valueSuffix:'" + YAxis + "' },");
                }
                else
                {
                    jscode.Append("yAxis: {" + YAxis + "},");
                }
            }
            jscode.Append("legend: { enabled: "+Legend.ToString().ToLower()+" },");
            if (!string.IsNullOrEmpty(Tooltip))
                jscode.Append("tooltip: {" + Tooltip + "},");
            if (!string.IsNullOrEmpty(PlotOptions))
                jscode.Append("plotOptions:{" + PlotOptions + "},");
            //数据处理方法
            SeriesToString(jscode);
            jscode.Append(" }); });");
        }

        private void SeriesToString(StringBuilder sb)
        {
            sb.Append("series: [");
            string seriesdata = string.Empty;
            if (Series.SeriesData != null)
            {
                seriesdata = SeriesDataToString(Series);
            }
            if (SeriesList != null && SeriesList.Count != 0)
            {
                foreach (ChartsSeries ser in SeriesList)
                {
                    seriesdata += SeriesDataToString(ser) + ",";
                }
                seriesdata = seriesdata.TrimEnd(',');
            }
            sb.Append(seriesdata);
            sb.Append("]");
        }

        /// <summary>
        /// 数据部分转成js代码
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        private string SeriesDataToString(ChartsSeries series)
        {
            string seriesdata = "{ name: '" + series.SeriesName + "',data:[";
            foreach (var item in series.SeriesData)
            {
                seriesdata += "['" + item.Key + "'," + item.Value + "],";
            }
            seriesdata = seriesdata.TrimEnd(',');
            seriesdata += "] }";
            return seriesdata;
        }
        /// <summary>
        /// x轴上数据转换
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="xAxis"></param>
        private void XAxisToString(StringBuilder sb, List<object> xAxis)
        {            
            sb.Append("xAxis: { categories: [");
            string xaxis = string.Empty;
            foreach (var item in xAxis)
            {
                xaxis += "'" + item + "',";
            }
            xaxis = xaxis.TrimEnd(',');
            sb.Append(xaxis);
            sb.Append("]},");
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Series == null) return;
            output.Attributes.SetAttribute("title", "HighchartsNET自动生成 By:LineZero");
            output.Attributes.SetAttribute("id", Id);
            StringBuilder style = new StringBuilder("margin:0px auto;min-width:400px;");
            if (Width > 0)
                style.Append($"width:{Width}px;");
            if (Height > 0)
                style.Append($"heigth:{Height}px;");
            output.Attributes.SetAttribute("style",style.ToString());
            output.TagName = "div";
            StringBuilder innerhtml = new StringBuilder();
            innerhtml.Append("<script>");
            HighChartsJs(innerhtml);
            innerhtml.Append("</script>");
            output.PostElement.AppendHtml(innerhtml.ToString());
        }
    }

    public class ChartsSeries
    {
        public object SeriesName { get; set; }
        public Dictionary<object, object> SeriesData { get; set; }
    }
    public enum ChartType
    {
        Column,
        Pie,
        Line,
        Bar
    }
}
