using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;

namespace HighchartsNET
{
    /// <summary>
    /// 图表自定义控件
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
        public List<object> XAxis { get; set; }
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

        private bool noscript = true;
        /// <summary>
        /// 是否自动引用脚本，默认为true 设为false即需要手动添加js引用
        /// </summary>
        public bool NoScript 
        {
            get { return noscript; }
            set { noscript = value; }
        }

        public string DataKey { get; set; }
        public string DataValue { get; set; }
        public object DataSource { get; set; }
        public object DataName { get; set; }

        public override void DataBind()
        {
            if (string.IsNullOrEmpty(DataKey) || string.IsNullOrEmpty(DataValue))
                return;
            if (DataSource != null)
            {
                System.Type type = DataSource.GetType();
                if (type.Name.Equals("DataTable"))
                {
                    DataTable dt = (DataTable)DataSource;
                    if (dt.Rows.Count <= 0) return;
                    if (dt.Columns.Contains(DataKey) && dt.Columns.Contains(DataValue)) 
                    {
                        var dic = new Dictionary<object, object>();
                        foreach (DataRow r in dt.Rows)
                        {
                            dic.Add(r[DataKey], r[DataValue]);
                        }
                        Series = new ChartsSeries { SeriesName = DataName, SeriesData = dic };
                    }
                }

                if (type.Name.Equals("DataSet"))
                {                    
                    DataSet ds = (DataSet)DataSource;
                    if (ds.Tables.Count <= 0) return;
                    var list=new List<ChartsSeries>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        if (dt.Rows.Count <= 0) continue;
                        if (dt.Columns.Contains(DataKey) && dt.Columns.Contains(DataValue))
                        {
                            var dic = new Dictionary<object, object>();
                            foreach (DataRow r in dt.Rows)
                            {
                                dic.Add(r[DataKey], r[DataValue]);
                            }
                            ChartsSeries cs = new ChartsSeries { SeriesName = GetDataName(i), SeriesData = dic };
                            list.Add(cs);
                        }
                    }
                    SeriesList = list;
                }
            }
        }

        private string GetDataName(int i) 
        {
            if (DataName == null) return "请填写数据名称";
            System.Type t = DataName.GetType();
            if (t.Name.Equals("String")) return DataName.ToString();
            if (!t.Name.Equals("List`1"))
                return "";
            var list = (List<string>)DataName;
            if (list.Count <= i) return "";
            return list[i];
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (NoScript)
            {
                this.Page.ClientScript.RegisterClientScriptResource(typeof(HighCharts), "HighchartsNET.js.jquery.min.js");
                this.Page.ClientScript.RegisterClientScriptResource(typeof(HighCharts), "HighchartsNET.js.highcharts.js");
            }
            base.OnPreRender(e);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write("<!--此代码由HighchartsNET自动生成-->");
            writer.WriteLine();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            StringBuilder innerhtml = new StringBuilder();
            if (string.IsNullOrEmpty(DivId))
                DivId = this.ID;
            writer.AddAttribute(HtmlTextWriterAttribute.Id, DivId);
            writer.AddAttribute(HtmlTextWriterAttribute.Style, Style.Value);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0px auto");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            writer.WriteLine();
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
            writer.WriteLine("<!--end by LineZero-->");
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
    }

    public struct ChartsSeries
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
