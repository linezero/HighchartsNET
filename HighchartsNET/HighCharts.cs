using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace HighchartsNET
{
    /// <summary>
    /// 图表自定义控件，详细功能参看api文档 http://www.highcharts.me/api/index.html
    /// </summary>
    [ToolboxData("<{0}:HighCharts runat=server></{0}:HighCharts>")]
    public class HighCharts : WebControl
    {
        /// <summary>
        /// 图表标题
        /// </summary>
        [Description("图表标题")]
        public string Title { get; set; }
        /// <summary>
        /// 图表类型
        /// </summary>
        public ChartType Type { get; set; }
        /// <summary>
        /// 图表2级标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public ChartsSeries Series { get; set; }

        /// <summary>
        /// 一些附加选项
        /// </summary>
        public string PlotOptions { get; set; }
        /// <summary>
        /// X轴选项
        /// </summary>
        public List<string> XAxis { get; set; }
        /// <summary>
        /// Y轴选项 默认可以只填名称
        /// </summary>
        public string YAxis { get; set; }

        /// <summary>
        /// 提示格式
        /// </summary>
        public string Tooltip { get; set; }
        /// <summary>
        /// 图表层id（容器）
        /// </summary>
        public string DivId { get; set; }

        /// <summary>
        /// 高级功能，多个数据集，多条图表，饼图不需要。
        /// </summary>
        public List<ChartsSeries> SeriesList { get; set; }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //writer.WriteLine();
            writer.Write("<!--此代码由HighchartsNET自动生成-->");
            writer.WriteLine();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            StringBuilder innerhtml = new StringBuilder();
            if (string.IsNullOrEmpty(DivId))
                DivId = this.ID; //DivId = "zerochart";
            writer.AddAttribute(HtmlTextWriterAttribute.Id, DivId);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, Style.Value);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0px auto");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            HighChartsJs(innerhtml);
            writer.Write(innerhtml.ToString());
            writer.RenderEndTag();
        }

        private void HighChartsJs(StringBuilder jscode)
        {
            jscode.Append("$(function(){$('#" + DivId + "').highcharts({ ");
            jscode.Append("credits: { enabled: false },");
            jscode.Append("chart:{ type: '" + Type.ToString().ToLower() + "'},");
            if (!string.IsNullOrEmpty(Title))
                jscode.Append("title: { text: '" + Title + "'},");
            if (!string.IsNullOrEmpty(SubTitle))
                jscode.Append("subtitle: { text: '" + SubTitle + "'},");
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
                    jscode.Append("yAxis: { title:{ text:'" + YAxis + "'}},");
                else
                    jscode.Append("yAxis: {" + YAxis + "},");
            }
            if (!string.IsNullOrEmpty(Tooltip))
                jscode.Append("tooltip: {" + Tooltip + "},");
            if (!string.IsNullOrEmpty(PlotOptions))
                jscode.Append("plotOptions:{" + PlotOptions + "},");
            //数据处理方法
            SeriesToString(jscode);
            jscode.Append(" }); });");
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("<!--end by zero-->");
        }

        private void SeriesToString(StringBuilder sb)
        {
            sb.Append("series: [");
            string seriesdata = "";
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
                seriesdata = seriesdata.Substring(0, seriesdata.Length - 1);
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
            string seriesdata = "";
            seriesdata += "{ name: '" + series.SeriesName + "',data:[";
            foreach (var item in series.SeriesData)
            {
                seriesdata += "['" + item.Key + "'," + item.Value + "],";
            }
            seriesdata = seriesdata.Substring(0, seriesdata.Length - 1);
            seriesdata += "] }";
            return seriesdata;
        }

        private void XAxisToString(StringBuilder sb, List<string> xAxis)
        {
            sb.Append("xAxis: { categories: [");
            int i = 0;
            foreach (var item in xAxis)
            {
                i++;
                if (i == xAxis.Count)
                    sb.Append("'" + item + "'");
                else
                    sb.Append("'" + item + "',");
            }
            sb.Append("]},");
        }
    }

    public struct ChartsSeries
    {
        public string SeriesName { get; set; }
        public Dictionary<string, double> SeriesData { get; set; }
    }
    public enum ChartType
    {
        Column,
        Pie,
        Line,
        Bar
    }
}
