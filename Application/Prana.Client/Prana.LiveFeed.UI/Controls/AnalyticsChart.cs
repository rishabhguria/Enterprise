using System;


namespace Prana.LiveFeed.UI
{
    public class AnalyticsChart : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.UltraWinChart.UltraChart chartStrikeVol;
        //string _underlyingSymbol = string.Empty;
        //private DataTable _volatilityTable;

        public AnalyticsChart()
        {
            InitializeComponent();
        }

        #region Public Methods

        /// <summary>
        /// Sets new data on the layers and updates the Chart
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ismanual"></param>
        //internal void SetData(DataTable dt,bool ismanual)
        //{            
        //    DataTable dtSource = dt;

        //    XYSeries xy1 = this.chartStrikeVol.Series[0] as XYSeries;
        //    XYSeries xy2 = this.chartStrikeVol.Series[1] as XYSeries;
        //    if (xy1 != null)//check added as 'as'keyword does not throw exception on invalid cast
        //    {
        //        xy1.Points.Clear(); //remove old ponits
        //    }
        //    if (xy2 != null)//check added as 'as'keyword does not throw exception on invalid cast
        //    {
        //        xy2.Points.Clear();
        //    }
        //    if (xy1 != null && xy2 != null)
        //    {
        //        if (ismanual)
        //        {
        //            // add manual points
        //            foreach (DataRow newdatarow in dtSource.Rows)
        //            {
        //                double x;
        //                double.TryParse(newdatarow["Strike"].ToString(), out x);
        //                double yCall;
        //                double.TryParse(newdatarow["User Call Vol."].ToString(), out yCall);
        //                double yPut;
        //                double.TryParse(newdatarow["User Put Vol."].ToString(), out yPut);

        //                //symbol information added to point so that on double click we know the symbol and don't need to search
        //                xy1.Points.Add(new XYDataPoint(x, yCall, newdatarow["SymbolCall"].ToString(), false));
        //                xy2.Points.Add(new XYDataPoint(x, yPut, newdatarow["SymbolPut"].ToString(), false));
        //            }
        //        }
        //        else
        //        {
        //            // add Implied Vol. points
        //            foreach (DataRow newdatarow in dtSource.Rows)
        //            {
        //                double x;
        //                double.TryParse(newdatarow["Strike"].ToString(), out x);
        //                double yCall;
        //                double.TryParse(newdatarow["Implied Call Vol."].ToString(), out yCall);
        //                double yPut;
        //                double.TryParse(newdatarow["Implied Put Vol."].ToString(), out yPut);

        //                //symbol information added to point so that on double click we know the symbol and don't need to search
        //                xy1.Points.Add(new XYDataPoint(x, yCall, newdatarow["SymbolCall"].ToString(), false));
        //                xy2.Points.Add(new XYDataPoint(x, yPut, newdatarow["SymbolPut"].ToString(), false));
        //            }
        //        }

        //        // get maxm range for volatility values so as to find the ranges for axes
        //        double maxX = xy1.GetXMaximum();
        //        double maxY1Y2 = Math.Max(xy1.GetYMaximum(), xy2.GetYMaximum());

        //        //set strike price & volatility axis range as 1.1 times the maxm. value 
        //        if (maxX != double.MinValue && maxY1Y2 != double.MinValue)
        //        {
        //            chartStrikeVol.CompositeChart.ChartLayers[0].AxisX.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
        //            chartStrikeVol.CompositeChart.ChartLayers[0].AxisX.RangeMax = 1.1 * maxX;
        //            chartStrikeVol.CompositeChart.ChartLayers[0].AxisY.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
        //            chartStrikeVol.CompositeChart.ChartLayers[0].AxisY.RangeMax = 1.1 * maxY1Y2;
        //            chartStrikeVol.CompositeChart.ChartLayers[1].AxisY.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
        //            chartStrikeVol.CompositeChart.ChartLayers[1].AxisY.RangeMax = 1.1 * maxY1Y2;
        //        }
        //    }
        //    chartStrikeVol.Refresh();
        //}

        /// <summary>
        /// method to update symbol on chart if symbol is modified on option chain
        /// </summary>
        /// <param name="newSymbol"></param>
        //internal void UpdateSymbolOnChart(string newSymbol)
        //{
        //    chartStrikeVol.TitleTop.Text = newSymbol;
        //}
        #endregion

        private void chart3D_ChartDataDoubleClicked(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            //string selectedSymbol = string.Empty;
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint point = (Infragistics.UltraChart.Resources.Appearance.XYDataPoint)e.Primitive.DataPoint;

            //following code commented as symbol information is stored in the point label now
            #region Commented COde
            //foreach (DataRow existingRow in _volatilityTable.Rows)
            //{
            //    if (existingRow["Strike"].ToString() == point.ValueX.ToString())
            //    {
            //        if (existingRow["Implied Call Vol."].ToString() == point.ValueY.ToString()
            //            || existingRow["User Call Vol."].ToString() == point.ValueY.ToString())
            //        {
            //            selectedSymbol = existingRow["SymbolCall"].ToString();
            //        }
            //        else if(existingRow["Implied Put Vol."].ToString() == point.ValueY.ToString()
            //            || existingRow["User Put Vol."].ToString() == point.ValueY.ToString())
            //        {
            //            selectedSymbol = existingRow["SymbolPut"].ToString();
            //        }
            //        break;
            //    }
            //}
            #endregion

            PointEventArgs pointInformation = new PointEventArgs(point.ValueX, point.ValueY, point.Label);
            PointDoubleClicked(this, pointInformation);
        }

        /// <summary>
        /// args for point specific data
        /// </summary>
        public event EventHandler<PointEventArgs> PointDoubleClicked;
        public class PointEventArgs : EventArgs
        {
            private double pointVolatility = 0;
            private double pointStrike = 0;
            private string symbol = string.Empty;

            //public PointEventArgs()
            //{ }
            public PointEventArgs(double Strike, double Volatility, string selectedSymbol)
            {
                pointStrike = Strike;
                pointVolatility = Volatility;
                symbol = selectedSymbol;
            }
            public double PointVolatility
            {
                get { return pointVolatility; }
                set { pointVolatility = value; }
            }
            public double PointStrike
            {
                get { return pointStrike; }
                set { pointStrike = value; }
            }
            public string SelectedSymbol
            {
                get { return symbol; }
                set { symbol = value; }
            }

        }

        #region Designer Code

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (chartStrikeVol != null)
                {
                    chartStrikeVol.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.UltraChart.Resources.Appearance.ChartArea chartArea1 = new Infragistics.UltraChart.Resources.Appearance.ChartArea();
            Infragistics.UltraChart.Resources.Appearance.AxisItem axisItem1 = new Infragistics.UltraChart.Resources.Appearance.AxisItem();
            Infragistics.UltraChart.Resources.Appearance.AxisItem axisItem2 = new Infragistics.UltraChart.Resources.Appearance.AxisItem();
            Infragistics.UltraChart.Resources.Appearance.AxisItem axisItem3 = new Infragistics.UltraChart.Resources.Appearance.AxisItem();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance chartLayerAppearance1 = new Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance();
            Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance scatterChartAppearance1 = new Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance chartLayerAppearance2 = new Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance();
            Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance scatterChartAppearance2 = new Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.CompositeLegend compositeLegend1 = new Infragistics.UltraChart.Resources.Appearance.CompositeLegend();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement2 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries1 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement3 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint1 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement4 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint2 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement5 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint3 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement6 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint4 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement7 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries2 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement8 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint5 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement9 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint6 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement10 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint7 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement11 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint8 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement12 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint9 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement13 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint10 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement14 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            this.chartStrikeVol = new Infragistics.Win.UltraWinChart.UltraChart();
            ((System.ComponentModel.ISupportInitialize)(this.chartStrikeVol)).BeginInit();
            this.SuspendLayout();
            // 
            //'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
            //'ChartType' must be persisted ahead of any Axes change made in design time.
            //
            this.chartStrikeVol.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.Composite;
            // 
            // chartStrikeVol
            // 
            this.chartStrikeVol.Axis.X.Extent = 24;
            this.chartStrikeVol.Axis.X.Labels.Flip = false;
            this.chartStrikeVol.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartStrikeVol.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X.Labels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.Flip = false;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            this.chartStrikeVol.Axis.X.LineThickness = 1;
            this.chartStrikeVol.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X.MajorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.X.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X.MinorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.X.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.X.ScrollScale.Height = 10;
            this.chartStrikeVol.Axis.X.ScrollScale.Visible = false;
            this.chartStrikeVol.Axis.X.ScrollScale.Width = 15;
            this.chartStrikeVol.Axis.X.TickmarkPercentage = 0;
            this.chartStrikeVol.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartStrikeVol.Axis.X.Visible = true;
            this.chartStrikeVol.Axis.X2.Labels.Flip = false;
            this.chartStrikeVol.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartStrikeVol.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X2.Labels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.Flip = false;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X2.MajorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.X2.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X2.MinorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.X2.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.X2.ScrollScale.Height = 10;
            this.chartStrikeVol.Axis.X2.ScrollScale.Visible = false;
            this.chartStrikeVol.Axis.X2.ScrollScale.Width = 15;
            this.chartStrikeVol.Axis.X2.TickmarkInterval = 0;
            this.chartStrikeVol.Axis.X2.Visible = false;
            this.chartStrikeVol.Axis.Y.Labels.Flip = false;
            this.chartStrikeVol.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartStrikeVol.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y.Labels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.Flip = false;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            this.chartStrikeVol.Axis.Y.LineThickness = 1;
            this.chartStrikeVol.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y.MajorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Y.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y.MinorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Y.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Y.ScrollScale.Height = 10;
            this.chartStrikeVol.Axis.Y.ScrollScale.Visible = false;
            this.chartStrikeVol.Axis.Y.ScrollScale.Width = 15;
            this.chartStrikeVol.Axis.Y.TickmarkInterval = 0;
            this.chartStrikeVol.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartStrikeVol.Axis.Y.Visible = true;
            this.chartStrikeVol.Axis.Y2.Labels.Flip = false;
            this.chartStrikeVol.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartStrikeVol.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y2.Labels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.Flip = false;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Y2.ScrollScale.Height = 10;
            this.chartStrikeVol.Axis.Y2.ScrollScale.Visible = false;
            this.chartStrikeVol.Axis.Y2.ScrollScale.Width = 15;
            this.chartStrikeVol.Axis.Y2.TickmarkInterval = 0;
            this.chartStrikeVol.Axis.Y2.Visible = false;
            this.chartStrikeVol.Axis.Z.Labels.Flip = false;
            this.chartStrikeVol.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z.Labels.ItemFormatString = "";
            this.chartStrikeVol.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z.Labels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.Flip = false;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z.MajorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Z.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z.MinorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Z.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Z.ScrollScale.Height = 10;
            this.chartStrikeVol.Axis.Z.ScrollScale.Visible = false;
            this.chartStrikeVol.Axis.Z.ScrollScale.Width = 15;
            this.chartStrikeVol.Axis.Z.TickmarkInterval = 0;
            this.chartStrikeVol.Axis.Z.Visible = false;
            this.chartStrikeVol.Axis.Z2.Labels.Flip = false;
            this.chartStrikeVol.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z2.Labels.ItemFormatString = "";
            this.chartStrikeVol.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z2.Labels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.Flip = false;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.OrientationAngle = 0;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.Thickness = 1;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Z2.ScrollScale.Height = 10;
            this.chartStrikeVol.Axis.Z2.ScrollScale.Visible = false;
            this.chartStrikeVol.Axis.Z2.ScrollScale.Width = 15;
            this.chartStrikeVol.Axis.Z2.TickmarkInterval = 0;
            this.chartStrikeVol.Axis.Z2.Visible = false;
            this.chartStrikeVol.BackColor = System.Drawing.SystemColors.Control;
            this.chartStrikeVol.Border.CornerRadius = 5;
            this.chartStrikeVol.ColorModel.AlphaLevel = ((byte)(150));
            axisItem1.DataType = Infragistics.UltraChart.Shared.Styles.AxisDataType.Numeric;
            axisItem1.Extent = 25;
            axisItem1.Key = "axisStrike";
            axisItem1.Labels.Flip = false;
            axisItem1.Labels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            axisItem1.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            axisItem1.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem1.Labels.OrientationAngle = 45;
            axisItem1.Labels.SeriesLabels.Flip = false;
            axisItem1.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            axisItem1.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem1.Labels.SeriesLabels.OrientationAngle = 45;
            axisItem1.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem1.Labels.VerticalAlign = System.Drawing.StringAlignment.Near;
            axisItem1.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            axisItem1.MajorGridLines.AlphaLevel = ((byte)(255));
            axisItem1.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            axisItem1.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem1.MajorGridLines.Thickness = 1;
            axisItem1.MajorGridLines.Visible = true;
            axisItem1.MinorGridLines.AlphaLevel = ((byte)(255));
            axisItem1.MinorGridLines.Color = System.Drawing.Color.LightGray;
            axisItem1.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem1.MinorGridLines.Thickness = 1;
            axisItem1.MinorGridLines.Visible = false;
            axisItem1.OrientationType = Infragistics.UltraChart.Shared.Styles.AxisNumber.X_Axis;
            axisItem1.ScrollScale.Height = 10;
            axisItem1.ScrollScale.Visible = false;
            axisItem1.ScrollScale.Width = 15;
            axisItem1.SetLabelAxisType = Infragistics.UltraChart.Core.Layers.SetLabelAxisType.ContinuousData;
            axisItem1.TickmarkInterval = 2;
            axisItem1.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            axisItem2.DataType = Infragistics.UltraChart.Shared.Styles.AxisDataType.Numeric;
            axisItem2.Extent = 30;
            axisItem2.Key = "axisVolatilityCall";
            axisItem2.Labels.Flip = false;
            axisItem2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            axisItem2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem2.Labels.OrientationAngle = 0;
            axisItem2.Labels.SeriesLabels.Flip = false;
            axisItem2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Green;
            axisItem2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem2.Labels.SeriesLabels.OrientationAngle = 0;
            axisItem2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem2.LineColor = System.Drawing.Color.Green;
            axisItem2.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            axisItem2.MajorGridLines.AlphaLevel = ((byte)(255));
            axisItem2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            axisItem2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem2.MajorGridLines.Thickness = 1;
            axisItem2.MajorGridLines.Visible = true;
            axisItem2.MinorGridLines.AlphaLevel = ((byte)(255));
            axisItem2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            axisItem2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem2.MinorGridLines.Thickness = 1;
            axisItem2.MinorGridLines.Visible = false;
            axisItem2.OrientationType = Infragistics.UltraChart.Shared.Styles.AxisNumber.Y_Axis;
            axisItem2.ScrollScale.Height = 10;
            axisItem2.ScrollScale.Visible = false;
            axisItem2.ScrollScale.Width = 15;
            axisItem2.SetLabelAxisType = Infragistics.UltraChart.Core.Layers.SetLabelAxisType.ContinuousData;
            axisItem2.TickmarkInterval = 2;
            axisItem2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            axisItem3.DataType = Infragistics.UltraChart.Shared.Styles.AxisDataType.Numeric;
            axisItem3.Extent = 30;
            axisItem3.Key = "axisVolatilityPut";
            axisItem3.Labels.Flip = false;
            axisItem3.Labels.FontColor = System.Drawing.Color.Red;
            axisItem3.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem3.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            axisItem3.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem3.Labels.OrientationAngle = 0;
            axisItem3.Labels.SeriesLabels.Flip = false;
            axisItem3.Labels.SeriesLabels.FontColor = System.Drawing.Color.Red;
            axisItem3.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem3.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem3.Labels.SeriesLabels.OrientationAngle = 0;
            axisItem3.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem3.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem3.LineColor = System.Drawing.Color.Red;
            axisItem3.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            axisItem3.MajorGridLines.AlphaLevel = ((byte)(255));
            axisItem3.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            axisItem3.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem3.MajorGridLines.Thickness = 1;
            axisItem3.MajorGridLines.Visible = true;
            axisItem3.MinorGridLines.AlphaLevel = ((byte)(255));
            axisItem3.MinorGridLines.Color = System.Drawing.Color.LightGray;
            axisItem3.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem3.MinorGridLines.Thickness = 1;
            axisItem3.MinorGridLines.Visible = false;
            axisItem3.OrientationType = Infragistics.UltraChart.Shared.Styles.AxisNumber.Y2_Axis;
            axisItem3.ScrollScale.Height = 10;
            axisItem3.ScrollScale.Visible = false;
            axisItem3.ScrollScale.Width = 15;
            axisItem3.SetLabelAxisType = Infragistics.UltraChart.Core.Layers.SetLabelAxisType.ContinuousData;
            axisItem3.TickmarkInterval = 2;
            axisItem3.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            chartArea1.Axes.Add(axisItem1);
            chartArea1.Axes.Add(axisItem2);
            chartArea1.Axes.Add(axisItem3);
            chartArea1.Key = "area1";
            chartArea1.PE = paintElement1;
            this.chartStrikeVol.CompositeChart.ChartAreas.Add(chartArea1);
            chartLayerAppearance1.AxisXKey = "axisStrike";
            chartLayerAppearance1.AxisY2Key = "axisVolatilityPut";
            chartLayerAppearance1.AxisYKey = "axisVolatilityCall";
            chartLayerAppearance1.ChartAreaKey = "area1";
            chartLayerAppearance1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ScatterChart;
            scatterChartAppearance1.ConnectWithLines = true;
            scatterChartAppearance1.Icon = Infragistics.UltraChart.Shared.Styles.SymbolIcon.X;
            scatterChartAppearance1.IconSize = Infragistics.UltraChart.Shared.Styles.SymbolIconSize.Medium;
            scatterChartAppearance1.NullHandling = Infragistics.UltraChart.Shared.Styles.NullHandling.DontPlot;
            chartLayerAppearance1.ChartTypeAppearance = scatterChartAppearance1;
            chartLayerAppearance1.Key = "chartLayer1";
            chartLayerAppearance1.SeriesList = "series1";
            chartLayerAppearance2.AxisXKey = "axisStrike";
            chartLayerAppearance2.AxisY2Key = "axisVolatilityPut";
            chartLayerAppearance2.AxisYKey = "axisVolatilityPut";
            chartLayerAppearance2.ChartAreaKey = "area1";
            chartLayerAppearance2.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ScatterChart;
            scatterChartAppearance2.Character = '\0';
            scatterChartAppearance2.ConnectWithLines = true;
            scatterChartAppearance2.Icon = Infragistics.UltraChart.Shared.Styles.SymbolIcon.Plus;
            scatterChartAppearance2.IconSize = Infragistics.UltraChart.Shared.Styles.SymbolIconSize.Medium;
            scatterChartAppearance2.LineAppearance.Thickness = 2;
            scatterChartAppearance2.NullHandling = Infragistics.UltraChart.Shared.Styles.NullHandling.DontPlot;
            chartLayerAppearance2.ChartTypeAppearance = scatterChartAppearance2;
            chartLayerAppearance2.Key = "chartLayer2";
            chartLayerAppearance2.SeriesList = "series2";
            this.chartStrikeVol.CompositeChart.ChartLayers.AddRange(new Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance[] {
            chartLayerAppearance1,
            chartLayerAppearance2});
            compositeLegend1.Bounds = new System.Drawing.Rectangle(40, 0, 0, 0);
            compositeLegend1.ChartLayerList = "chartLayer1|chartLayer2";
            paintElement2.Fill = System.Drawing.Color.Empty;
            compositeLegend1.PE = paintElement2;
            this.chartStrikeVol.CompositeChart.Legends.Add(compositeLegend1);
            xySeries1.Data.LabelColumn = "";
            xySeries1.Data.ValueXColumn = "";
            xySeries1.Data.ValueYColumn = "";
            xySeries1.Key = "series1";
            xySeries1.Label = "call data";
            paintElement3.Fill = System.Drawing.Color.Green;
            xySeries1.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement3});
            paintElement4.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint1.PE = paintElement4;
            xyDataPoint1.ValueX = 1;
            xyDataPoint1.ValueY = 1;
            paintElement5.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint2.PE = paintElement5;
            xyDataPoint2.ValueX = 2;
            xyDataPoint2.ValueY = 4;
            paintElement6.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint3.PE = paintElement6;
            xyDataPoint3.ValueX = 3;
            xyDataPoint3.ValueY = 9;
            paintElement7.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint4.PE = paintElement7;
            xyDataPoint4.ValueX = 4;
            xyDataPoint4.ValueY = 16;
            xySeries1.Points.Add(xyDataPoint1);
            xySeries1.Points.Add(xyDataPoint2);
            xySeries1.Points.Add(xyDataPoint3);
            xySeries1.Points.Add(xyDataPoint4);
            xySeries2.Data.LabelColumn = "";
            xySeries2.Data.ValueXColumn = "";
            xySeries2.Data.ValueYColumn = "";
            xySeries2.Key = "series2";
            xySeries2.Label = "put data";
            paintElement8.Fill = System.Drawing.Color.Red;
            xySeries2.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement8});
            paintElement9.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint5.PE = paintElement9;
            xyDataPoint5.ValueX = 1;
            xyDataPoint5.ValueY = 1;
            paintElement10.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint6.PE = paintElement10;
            xyDataPoint6.ValueX = 2;
            xyDataPoint6.ValueY = 3;
            paintElement11.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint7.PE = paintElement11;
            xyDataPoint7.ValueX = 3;
            xyDataPoint7.ValueY = 4;
            paintElement12.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint8.PE = paintElement12;
            xyDataPoint8.ValueX = 4;
            xyDataPoint8.ValueY = 5;
            paintElement13.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint9.PE = paintElement13;
            xyDataPoint9.ValueX = 5;
            xyDataPoint9.ValueY = 6;
            paintElement14.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint10.PE = paintElement14;
            xySeries2.Points.Add(xyDataPoint5);
            xySeries2.Points.Add(xyDataPoint6);
            xySeries2.Points.Add(xyDataPoint7);
            xySeries2.Points.Add(xyDataPoint8);
            xySeries2.Points.Add(xyDataPoint9);
            xySeries2.Points.Add(xyDataPoint10);
            this.chartStrikeVol.CompositeChart.Series.AddRange(new Infragistics.UltraChart.Data.Series.ISeries[] {
            xySeries1,
            xySeries2});
            this.chartStrikeVol.Data.EmptyStyle.LineStyle.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dash;
            this.chartStrikeVol.Data.EmptyStyle.LineStyle.EndStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;
            this.chartStrikeVol.Data.EmptyStyle.LineStyle.MidPointAnchors = false;
            this.chartStrikeVol.Data.EmptyStyle.LineStyle.StartStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;
            this.chartStrikeVol.Data.ZeroAligned = true;
            this.chartStrikeVol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartStrikeVol.EmptyChartText = "Data Not Available. Plz Fill the values for strike price and volatility.";
            this.chartStrikeVol.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chartStrikeVol.Location = new System.Drawing.Point(0, 0);
            this.chartStrikeVol.Name = "chartStrikeVol";
            this.chartStrikeVol.Size = new System.Drawing.Size(622, 474);
            this.chartStrikeVol.TabIndex = 6;
            this.chartStrikeVol.TitleBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.chartStrikeVol.TitleBottom.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.TitleBottom.Text = "Strike Price";
            this.chartStrikeVol.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.chartStrikeVol.TitleLeft.Text = "Volatility";
            this.chartStrikeVol.TitleLeft.Visible = true;
            this.chartStrikeVol.TitleTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Pixel);
            this.chartStrikeVol.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.TitleTop.Text = "A";
            this.chartStrikeVol.TitleTop.VerticalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.chartStrikeVol.Tooltips.TooltipControl = null;
            this.chartStrikeVol.Tooltips.UseControl = false;
            this.chartStrikeVol.ChartDataDoubleClicked += new Infragistics.UltraChart.Shared.Events.ChartDataClickedEventHandler(this.chart3D_ChartDataDoubleClicked);
            // 
            // AnalyticsChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.chartStrikeVol);
            this.Name = "AnalyticsChart";
            this.Size = new System.Drawing.Size(622, 474);
            ((System.ComponentModel.ISupportInitialize)(this.chartStrikeVol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.IContainer components = null;

        #endregion

    }
}
