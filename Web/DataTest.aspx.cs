using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class DataTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("a");
        dt.Columns.Add("b");
        DataRow dr = dt.NewRow();
        dr[0] = "2013/1";
        dr[1] = "50";
        dt.Rows.Add(dr);
        DataRow dr1 = dt.NewRow();
        dr1[0] = "2013/2";
        dr1[1] = "150";
        dt.Rows.Add(dr1);

        highcharts1.Type = HighchartsNET.ChartType.Line;
        highcharts1.DataKey = "a";
        highcharts1.DataValue = "b";
        highcharts1.YAxis = "降雨量(mm)";//Y轴的值;
        highcharts1.Tooltip = "valueSuffix: 'mm'";
        highcharts1.DataName = "武汉";
        highcharts1.DataSource = dt;
        highcharts1.DataBind();

        DataTable dt1 = new DataTable();
        dt1.Columns.Add("a");
        dt1.Columns.Add("b");
        DataRow dr2 = dt1.NewRow();
        dr2[0] = "2013/1";
        dr2[1] = "30";
        dt1.Rows.Add(dr2);
        DataRow dr3 = dt1.NewRow();
        dr3[0] = "2013/2";
        dr3[1] = "120";
        dt1.Rows.Add(dr3);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        ds.Tables.Add(dt1);

        highcharts2.Type = HighchartsNET.ChartType.Line;
        highcharts2.DataKey = "a";
        highcharts2.DataValue = "b";
        highcharts2.YAxis = "降雨量(mm)";//Y轴的值;
        highcharts2.Tooltip = "valueSuffix: 'mm'";
        highcharts2.DataName = new List<string>() { "武汉", "北京" };
        highcharts2.DataSource = ds;
        highcharts2.DataBind();        
    }
}