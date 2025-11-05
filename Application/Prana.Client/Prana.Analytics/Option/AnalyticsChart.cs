using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Styles;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;


namespace Prana.Analytics
{
    public class AnalyticsChart : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.UltraWinChart.UltraChart chartStrikeVol;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;

        public AnalyticsChart()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Public Methods
        public override void Refresh()
        {
            try
            {
                foreach (XYSeries series in chartStrikeVol.Series)
                {
                    series.Points.Clear();
                }
                chartStrikeVol.Refresh();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void SetXYAxisNames(string xAxisName)
        {
            try
            {
                this.chartStrikeVol.TitleBottom.Text = xAxisName;
                this.chartStrikeVol.TitleBottom.FontColor = Color.White;
                this.chartStrikeVol.Legend.BorderColor = Color.White;
                this.chartStrikeVol.Legend.FontColor = Color.White;
                this.chartStrikeVol.Axis.X.LineColor = Color.White;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void SetData(DataTable dtSource, List<int> selectedIndexs, int offset, int xIndex)
        {
            try
            {
                foreach (XYSeries series in chartStrikeVol.Series)
                {
                    series.Points.Clear();
                }
                foreach (int index in selectedIndexs)
                {
                    SetSeries((XYSeries)chartStrikeVol.Series[index - offset], index, dtSource, xIndex);
                }

                // get max range for volatility values so as to find the ranges for axes
                double maxY1 = double.MinValue;
                double minY1 = double.MaxValue;
                foreach (XYSeries xy in chartStrikeVol.CompositeChart.ChartLayers[0].Series)
                {
                    if (maxY1 < xy.GetYMaximum())
                    {
                        maxY1 = xy.GetYMaximum();
                    }
                    if (minY1 > xy.GetYMinimum())
                    {
                        minY1 = xy.GetYMinimum();
                    }
                }

                double maxY2 = double.MinValue;
                double minY2 = double.MaxValue;
                foreach (XYSeries xy in chartStrikeVol.CompositeChart.ChartLayers[1].Series)
                {
                    if (maxY2 < xy.GetYMaximum())
                    {
                        maxY2 = xy.GetYMaximum();
                    }
                    if (minY2 > xy.GetYMinimum())
                    {
                        minY2 = xy.GetYMinimum();
                    }
                }

                //set strike price & volatility axis range as 1.1 times the max value 
                if (maxY1 != double.MinValue && minY1 != double.MaxValue)
                {
                    chartStrikeVol.CompositeChart.ChartLayers[0].AxisY.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                    chartStrikeVol.CompositeChart.ChartLayers[0].AxisY.RangeMax = (maxY1 > 0) ? (1.1 * maxY1) : (0.9 * maxY1);
                    chartStrikeVol.CompositeChart.ChartLayers[0].AxisY.RangeMin = (minY1 > 0) ? (0.9 * minY1) : (1.1 * minY1);
                }
                else
                {
                    chartStrikeVol.CompositeChart.ChartLayers[0].AxisY.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Automatic;
                }
                if (maxY2 != double.MinValue && minY2 != double.MaxValue)
                {
                    chartStrikeVol.CompositeChart.ChartLayers[1].AxisY.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                    chartStrikeVol.CompositeChart.ChartLayers[1].AxisY.RangeMax = (maxY2 > 0) ? (1.1 * maxY2) : (0.9 * maxY2);
                    chartStrikeVol.CompositeChart.ChartLayers[1].AxisY.RangeMin = (minY2 > 0) ? (0.9 * minY2) : (1.1 * minY2);
                }
                else
                {
                    chartStrikeVol.CompositeChart.ChartLayers[1].AxisY.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Automatic;
                }
                chartStrikeVol.Refresh();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        private void SetSeries(XYSeries series, int index, DataTable dtSource, int xIndex)
        {
            try
            {
                foreach (DataRow newdatarow in dtSource.Rows)
                {
                    double x;
                    double.TryParse(newdatarow.ItemArray[xIndex].ToString(), out x);
                    double y;
                    double.TryParse(newdatarow.ItemArray[index].ToString(), out y);
                    string column = dtSource.Columns[index].Caption;
                    //symbol information added to point so that on double click we know the symbol and don't need to search
                    series.Points.Add(new XYDataPoint(x, y, column, false));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chart3D_ChartDataDoubleClicked(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {
            try
            {
                Infragistics.UltraChart.Resources.Appearance.XYDataPoint point = (Infragistics.UltraChart.Resources.Appearance.XYDataPoint)e.Primitive.DataPoint;

                PointEventArgs pointInformation = new PointEventArgs(point.ValueX, point.ValueY, point.Label);
                if (PointDoubleClicked != null)
                    PointDoubleClicked(this, pointInformation);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chartStrikeVol_FillSceneGraph(object sender, Infragistics.UltraChart.Shared.Events.FillSceneGraphEventArgs e)
        {
            try
            {
                IAdvanceAxis xAxis = chartStrikeVol.CompositeChart.ChartLayers[1].ChartLayer.Grid["X"] as IAdvanceAxis;
                IAdvanceAxis yAxis = chartStrikeVol.CompositeChart.ChartLayers[1].ChartLayer.Grid["Y2"] as IAdvanceAxis;
                int yMapZero = Convert.ToInt32(yAxis.Map(0));
                int xAxis_MapMinimum = Convert.ToInt32(xAxis.MapMinimum);
                int xAxis_MapMaximum = Convert.ToInt32(xAxis.MapMaximum);
                Point xAxisStartPoint = new Point(xAxis_MapMinimum, yMapZero);
                Point xAxisEndPoint = new Point(xAxis_MapMaximum, yMapZero);
                Line myXAxisLine = new Line(xAxisStartPoint, xAxisEndPoint);
                LineStyle style = new LineStyle();
                style.DrawStyle = LineDrawStyle.Dash;
                myXAxisLine.PE.Stroke = Color.Red;
                myXAxisLine.PE.Fill = Color.Red;
                myXAxisLine.lineStyle = style;
                e.SceneGraph.Add(myXAxisLine);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public event EventHandler<PointEventArgs> PointDoubleClicked;
        public class PointEventArgs : EventArgs
        {
            private double pointVolatility = 0;
            private double pointStrike = 0;
            private string symbol = string.Empty;

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
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
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
            this.components = new System.ComponentModel.Container();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ChartArea chartArea1 = new Infragistics.UltraChart.Resources.Appearance.ChartArea();
            Infragistics.UltraChart.Resources.Appearance.AxisItem axisItem1 = new Infragistics.UltraChart.Resources.Appearance.AxisItem();
            Infragistics.UltraChart.Resources.Appearance.AxisItem axisItem2 = new Infragistics.UltraChart.Resources.Appearance.AxisItem();
            Infragistics.UltraChart.Resources.Appearance.AxisItem axisItem3 = new Infragistics.UltraChart.Resources.Appearance.AxisItem();
            Infragistics.UltraChart.Resources.Appearance.FontScalingAxisLabelLayoutBehavior fontScalingAxisLabelLayoutBehavior1 = new Infragistics.UltraChart.Resources.Appearance.FontScalingAxisLabelLayoutBehavior();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement2 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement3 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance chartLayerAppearance1 = new Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance();
            Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance scatterChartAppearance1 = new Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance chartLayerAppearance2 = new Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance();
            Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance scatterChartAppearance2 = new Infragistics.UltraChart.Resources.Appearance.ScatterChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.CompositeLegend compositeLegend1 = new Infragistics.UltraChart.Resources.Appearance.CompositeLegend();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries1 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement4 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint1 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement5 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint2 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement6 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries2 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement7 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint3 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement8 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint4 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement9 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries3 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement10 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint5 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement11 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint6 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement12 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries4 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement13 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint7 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement14 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint8 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement15 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries5 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement16 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint9 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement17 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint10 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement18 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries6 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement19 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint11 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement20 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint12 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement21 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries7 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement22 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint13 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement23 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint14 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement24 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries8 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement25 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint15 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement26 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint16 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement27 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries9 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement28 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint17 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement29 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint18 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement30 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries10 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement31 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint19 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement32 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint20 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement33 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries11 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement34 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint21 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement35 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint22 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement36 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries12 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement37 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint23 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement38 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint24 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement39 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries13 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement40 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint25 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement41 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint26 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement42 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYSeries xySeries14 = new Infragistics.UltraChart.Resources.Appearance.XYSeries();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement43 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint27 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement44 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.XYDataPoint xyDataPoint28 = new Infragistics.UltraChart.Resources.Appearance.XYDataPoint();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement45 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            this.chartStrikeVol = new Infragistics.Win.UltraWinChart.UltraChart();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chartStrikeVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            //			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
            //			'ChartType' must be persisted ahead of any Axes change made in design time.
            //		
            this.chartStrikeVol.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.Composite;
            // 
            // chartStrikeVol
            // 
            this.chartStrikeVol.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chartStrikeVol.Axis.PE = paintElement1;
            this.chartStrikeVol.Axis.X.Extent = 53;
            this.chartStrikeVol.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartStrikeVol.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X.LineColor = System.Drawing.Color.White;
            this.chartStrikeVol.Axis.X.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            this.chartStrikeVol.Axis.X.LineThickness = 1;
            this.chartStrikeVol.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.X.Margin.Near.Value = -1.4598540145985401D;
            this.chartStrikeVol.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.X.TickmarkPercentage = 0D;
            this.chartStrikeVol.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartStrikeVol.Axis.X.Visible = true;
            this.chartStrikeVol.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartStrikeVol.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartStrikeVol.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X2.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.X2.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.X2.Visible = false;
            this.chartStrikeVol.Axis.Y.Extent = 16;
            this.chartStrikeVol.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartStrikeVol.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            this.chartStrikeVol.Axis.Y.LineThickness = 1;
            this.chartStrikeVol.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Y.ScrollScale.Visible = true;
            this.chartStrikeVol.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartStrikeVol.Axis.Y.Visible = true;
            this.chartStrikeVol.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartStrikeVol.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y2.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Y2.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Y2.Visible = false;
            this.chartStrikeVol.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z.Labels.ItemFormatString = "";
            this.chartStrikeVol.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Z.Visible = false;
            this.chartStrikeVol.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z2.Labels.ItemFormatString = "";
            this.chartStrikeVol.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartStrikeVol.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z2.MajorGridLines.Visible = true;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartStrikeVol.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartStrikeVol.Axis.Z2.MinorGridLines.Visible = false;
            this.chartStrikeVol.Axis.Z2.Visible = false;
            this.chartStrikeVol.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartStrikeVol.Border.CornerRadius = 5;
            this.chartStrikeVol.ColorModel.AlphaLevel = ((byte)(150));
            axisItem1.DataType = Infragistics.UltraChart.Shared.Styles.AxisDataType.Numeric;
            axisItem1.Extent = 25;
            axisItem1.Key = "axisStrike";
            axisItem1.Labels.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            axisItem1.Labels.FontColor = System.Drawing.Color.White;
            axisItem1.Labels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            axisItem1.Labels.ItemFormatString = "<DATA_VALUE:0.##>";
            axisItem1.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            axisItem1.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
            axisItem1.Labels.SeriesLabels.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            axisItem1.Labels.SeriesLabels.FontColor = System.Drawing.Color.White;
            axisItem1.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            axisItem1.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem1.Labels.SeriesLabels.OrientationAngle = 45;
            axisItem1.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem1.Labels.VerticalAlign = System.Drawing.StringAlignment.Near;
            axisItem1.LineColor = System.Drawing.Color.White;
            axisItem1.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            axisItem1.LineThickness = 1;
            axisItem1.MajorGridLines.AlphaLevel = ((byte)(255));
            axisItem1.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            axisItem1.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem1.MajorGridLines.Visible = false;
            axisItem1.MinorGridLines.AlphaLevel = ((byte)(255));
            axisItem1.MinorGridLines.Color = System.Drawing.Color.LightGray;
            axisItem1.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem1.MinorGridLines.Visible = false;
            axisItem1.OrientationType = Infragistics.UltraChart.Shared.Styles.AxisNumber.X_Axis;
            axisItem1.SetLabelAxisType = Infragistics.UltraChart.Core.Layers.SetLabelAxisType.ContinuousData;
            axisItem1.TickmarkInterval = 1D;
            axisItem1.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            axisItem2.DataType = Infragistics.UltraChart.Shared.Styles.AxisDataType.Numeric;
            axisItem2.Extent = 52;
            axisItem2.Key = "axisVolatilityCall";
            axisItem2.Labels.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            axisItem2.Labels.FontColor = System.Drawing.Color.White;
            axisItem2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem2.Labels.ItemFormatString = "<DATA_VALUE:0.0000>";
            axisItem2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
            axisItem2.Labels.SeriesLabels.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            axisItem2.Labels.SeriesLabels.FontColor = System.Drawing.Color.White;
            axisItem2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem2.LineColor = System.Drawing.Color.White;
            axisItem2.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            axisItem2.LineThickness = 1;
            axisItem2.MajorGridLines.AlphaLevel = ((byte)(255));
            axisItem2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            axisItem2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem2.MajorGridLines.Visible = false;
            axisItem2.MinorGridLines.AlphaLevel = ((byte)(255));
            axisItem2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            axisItem2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem2.MinorGridLines.Visible = false;
            axisItem2.OrientationType = Infragistics.UltraChart.Shared.Styles.AxisNumber.Y_Axis;
            axisItem2.SetLabelAxisType = Infragistics.UltraChart.Core.Layers.SetLabelAxisType.ContinuousData;
            axisItem2.TickmarkInterval = 1D;
            axisItem2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            axisItem3.DataType = Infragistics.UltraChart.Shared.Styles.AxisDataType.Numeric;
            axisItem3.Extent = 67;
            axisItem3.Key = "axisVolatilityPut";
            axisItem3.Labels.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            axisItem3.Labels.FontColor = System.Drawing.Color.White;
            axisItem3.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem3.Labels.ItemFormatString = "  <DATA_VALUE:#,0.##>";
            axisItem3.Labels.Layout.BehaviorCollection.AddRange(new Infragistics.UltraChart.Resources.Appearance.AxisLabelLayoutBehavior[] {
            fontScalingAxisLabelLayoutBehavior1});
            axisItem3.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem3.Labels.SeriesLabels.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            axisItem3.Labels.SeriesLabels.FontColor = System.Drawing.Color.White;
            axisItem3.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            axisItem3.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            axisItem3.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem3.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            axisItem3.LineColor = System.Drawing.Color.White;
            axisItem3.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
            axisItem3.LineThickness = 1;
            axisItem3.MajorGridLines.AlphaLevel = ((byte)(255));
            axisItem3.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            axisItem3.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem3.MajorGridLines.Visible = false;
            axisItem3.MinorGridLines.AlphaLevel = ((byte)(255));
            axisItem3.MinorGridLines.Color = System.Drawing.Color.LightGray;
            axisItem3.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            axisItem3.MinorGridLines.Visible = false;
            axisItem3.OrientationType = Infragistics.UltraChart.Shared.Styles.AxisNumber.Y2_Axis;
            axisItem3.SetLabelAxisType = Infragistics.UltraChart.Core.Layers.SetLabelAxisType.ContinuousData;
            axisItem3.TickmarkInterval = 1D;
            axisItem3.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            chartArea1.Axes.Add(axisItem1);
            chartArea1.Axes.Add(axisItem2);
            chartArea1.Axes.Add(axisItem3);
            paintElement2.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            chartArea1.GridPE = paintElement2;
            chartArea1.Key = "area1";
            chartArea1.PE = paintElement3;
            this.chartStrikeVol.CompositeChart.ChartAreas.Add(chartArea1);
            chartLayerAppearance1.AxisXKey = "axisStrike";
            chartLayerAppearance1.AxisY2Key = "axisVolatilityCall";
            chartLayerAppearance1.AxisYKey = "axisVolatilityCall";
            chartLayerAppearance1.ChartAreaKey = "area1";
            chartLayerAppearance1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ScatterChart;
            scatterChartAppearance1.Character = '\0';
            scatterChartAppearance1.CharacterFont = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            scatterChartAppearance1.ConnectWithLines = true;
            scatterChartAppearance1.Icon = Infragistics.UltraChart.Shared.Styles.SymbolIcon.None;
            scatterChartAppearance1.IconSize = Infragistics.UltraChart.Shared.Styles.SymbolIconSize.Small;
            chartLayerAppearance1.ChartTypeAppearance = scatterChartAppearance1;
            chartLayerAppearance1.Key = "chartLayer1";
            chartLayerAppearance1.SeriesList = "series1|series2|series3|series4|series5";
            chartLayerAppearance2.AxisXKey = "axisStrike";
            chartLayerAppearance2.AxisY2Key = "axisVolatilityPut";
            chartLayerAppearance2.AxisYKey = "axisVolatilityPut";
            chartLayerAppearance2.ChartAreaKey = "area1";
            chartLayerAppearance2.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ScatterChart;
            scatterChartAppearance2.ConnectWithLines = true;
            scatterChartAppearance2.Icon = Infragistics.UltraChart.Shared.Styles.SymbolIcon.None;
            scatterChartAppearance2.LineAppearance.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            chartLayerAppearance2.ChartTypeAppearance = scatterChartAppearance2;
            chartLayerAppearance2.Key = "chartLayer2";
            chartLayerAppearance2.SeriesList = "series6|series7|series8|series9|series10|series11|series12|series13|series14";
            this.chartStrikeVol.CompositeChart.ChartLayers.AddRange(new Infragistics.UltraChart.Resources.Appearance.ChartLayerAppearance[] {
            chartLayerAppearance1,
            chartLayerAppearance2});
            compositeLegend1.Bounds = new System.Drawing.Rectangle(61, 1, 30, 47);
            compositeLegend1.BoundsMeasureType = Infragistics.UltraChart.Shared.Styles.MeasureType.Percentage;
            compositeLegend1.ChartLayerList = "chartLayer1|chartLayer2";
            compositeLegend1.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            compositeLegend1.LabelStyle.FontColor = System.Drawing.Color.White;
            this.chartStrikeVol.CompositeChart.Legends.Add(compositeLegend1);
            xySeries1.Data.LabelColumn = "";
            xySeries1.Data.ValueXColumn = "";
            xySeries1.Data.ValueYColumn = "";
            xySeries1.Key = "series1";
            xySeries1.Label = "Delta";
            paintElement4.Fill = System.Drawing.Color.Green;
            paintElement4.Stroke = System.Drawing.Color.Red;
            xySeries1.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement4});
            paintElement5.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint1.PE = paintElement5;
            xyDataPoint1.ValueX = -1D;
            xyDataPoint1.ValueY = -1D;
            paintElement6.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint2.PE = paintElement6;
            xyDataPoint2.ValueX = 1D;
            xyDataPoint2.ValueY = 1D;
            xySeries1.Points.Add(xyDataPoint1);
            xySeries1.Points.Add(xyDataPoint2);
            xySeries2.Data.LabelColumn = "";
            xySeries2.Data.ValueXColumn = "";
            xySeries2.Data.ValueYColumn = "";
            xySeries2.Key = "series2";
            xySeries2.Label = "Gamma";
            paintElement7.Fill = System.Drawing.Color.Red;
            xySeries2.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement7});
            paintElement8.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint3.PE = paintElement8;
            xyDataPoint3.ValueX = -1D;
            xyDataPoint3.ValueY = -1D;
            paintElement9.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint4.PE = paintElement9;
            xyDataPoint4.ValueX = 1D;
            xyDataPoint4.ValueY = 2D;
            xySeries2.Points.Add(xyDataPoint3);
            xySeries2.Points.Add(xyDataPoint4);
            xySeries3.Data.ValueXColumn = "";
            xySeries3.Data.ValueYColumn = "";
            xySeries3.Key = "series3";
            xySeries3.Label = "Theta";
            paintElement10.Fill = System.Drawing.Color.Yellow;
            xySeries3.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement10});
            paintElement11.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint5.PE = paintElement11;
            xyDataPoint5.ValueX = -1D;
            xyDataPoint5.ValueY = -1D;
            paintElement12.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint6.PE = paintElement12;
            xyDataPoint6.ValueX = 1D;
            xyDataPoint6.ValueY = 3D;
            xySeries3.Points.Add(xyDataPoint5);
            xySeries3.Points.Add(xyDataPoint6);
            xySeries4.Data.ValueXColumn = "";
            xySeries4.Data.ValueYColumn = "";
            xySeries4.Key = "series4";
            xySeries4.Label = "Vega";
            paintElement13.Fill = System.Drawing.Color.Cyan;
            xySeries4.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement13});
            paintElement14.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint7.PE = paintElement14;
            xyDataPoint7.ValueX = -1D;
            xyDataPoint7.ValueY = -1D;
            paintElement15.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint8.PE = paintElement15;
            xyDataPoint8.ValueX = 1D;
            xyDataPoint8.ValueY = 4D;
            xySeries4.Points.Add(xyDataPoint7);
            xySeries4.Points.Add(xyDataPoint8);
            xySeries5.Data.ValueXColumn = "";
            xySeries5.Data.ValueYColumn = "";
            xySeries5.Key = "series5";
            xySeries5.Label = "Rho";
            paintElement16.Fill = System.Drawing.Color.Blue;
            xySeries5.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement16});
            paintElement17.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint9.PE = paintElement17;
            xyDataPoint9.ValueX = -1D;
            xyDataPoint9.ValueY = -1D;
            paintElement18.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint10.PE = paintElement18;
            xyDataPoint10.ValueX = 1D;
            xyDataPoint10.ValueY = 5D;
            xySeries5.Points.Add(xyDataPoint9);
            xySeries5.Points.Add(xyDataPoint10);
            xySeries6.Data.ValueXColumn = "";
            xySeries6.Data.ValueYColumn = "";
            xySeries6.Key = "series6";
            xySeries6.Label = "Dollar Delta (Base)";
            paintElement19.Fill = System.Drawing.Color.Green;
            paintElement19.StrokeWidth = 2;
            xySeries6.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement19});
            paintElement20.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint11.PE = paintElement20;
            xyDataPoint11.ValueX = 8D;
            xyDataPoint11.ValueY = -1D;
            paintElement21.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint12.PE = paintElement21;
            xyDataPoint12.ValueX = 6D;
            xyDataPoint12.ValueY = 1D;
            xySeries6.Points.Add(xyDataPoint11);
            xySeries6.Points.Add(xyDataPoint12);
            xySeries7.Data.ValueXColumn = "";
            xySeries7.Data.ValueYColumn = "";
            xySeries7.Key = "series7";
            xySeries7.Label = "Dollar Gamma (Base)";
            paintElement22.Fill = System.Drawing.Color.Red;
            paintElement22.StrokeWidth = 2;
            xySeries7.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement22});
            paintElement23.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint13.PE = paintElement23;
            xyDataPoint13.ValueX = 8D;
            xyDataPoint13.ValueY = -1D;
            paintElement24.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint14.PE = paintElement24;
            xyDataPoint14.ValueX = 6D;
            xyDataPoint14.ValueY = 2D;
            xySeries7.Points.Add(xyDataPoint13);
            xySeries7.Points.Add(xyDataPoint14);
            xySeries8.Data.ValueXColumn = "";
            xySeries8.Data.ValueYColumn = "";
            xySeries8.Key = "series8";
            xySeries8.Label = "Dollar Theta (Base)";
            paintElement25.Fill = System.Drawing.Color.Yellow;
            paintElement25.StrokeWidth = 2;
            xySeries8.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement25});
            paintElement26.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint15.PE = paintElement26;
            xyDataPoint15.ValueX = 8D;
            xyDataPoint15.ValueY = -1D;
            paintElement27.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint16.PE = paintElement27;
            xyDataPoint16.ValueX = 6D;
            xyDataPoint16.ValueY = 3D;
            xySeries8.Points.Add(xyDataPoint15);
            xySeries8.Points.Add(xyDataPoint16);
            xySeries9.Data.ValueXColumn = "";
            xySeries9.Data.ValueYColumn = "";
            xySeries9.Key = "series9";
            xySeries9.Label = "Dollar Vega (Base)";
            paintElement28.Fill = System.Drawing.Color.Cyan;
            paintElement28.StrokeWidth = 2;
            xySeries9.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement28});
            paintElement29.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint17.PE = paintElement29;
            xyDataPoint17.ValueX = 8D;
            xyDataPoint17.ValueY = -1D;
            paintElement30.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint18.PE = paintElement30;
            xyDataPoint18.ValueX = 6D;
            xyDataPoint18.ValueY = 4D;
            xySeries9.Points.Add(xyDataPoint17);
            xySeries9.Points.Add(xyDataPoint18);
            xySeries10.Data.ValueXColumn = "";
            xySeries10.Data.ValueYColumn = "";
            xySeries10.Key = "series10";
            xySeries10.Label = "Dollar Rho (Base)";
            paintElement31.Fill = System.Drawing.Color.Blue;
            xySeries10.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement31});
            paintElement32.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint19.PE = paintElement32;
            xyDataPoint19.ValueX = 8D;
            xyDataPoint19.ValueY = -1D;
            paintElement33.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint20.PE = paintElement33;
            xyDataPoint20.ValueX = 6D;
            xyDataPoint20.ValueY = 1.5D;
            xySeries10.Points.Add(xyDataPoint19);
            xySeries10.Points.Add(xyDataPoint20);
            xySeries11.Data.ValueXColumn = "";
            xySeries11.Data.ValueYColumn = "";
            xySeries11.Key = "series11";
            xySeries11.Label = "Simulated Price (Base)";
            paintElement34.Fill = System.Drawing.Color.Pink;
            xySeries11.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement34});
            paintElement35.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint21.PE = paintElement35;
            xyDataPoint21.ValueX = 8D;
            xyDataPoint21.ValueY = -1D;
            paintElement36.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint22.PE = paintElement36;
            xyDataPoint22.ValueX = 6D;
            xyDataPoint22.ValueY = 2.5D;
            xySeries11.Points.Add(xyDataPoint21);
            xySeries11.Points.Add(xyDataPoint22);
            xySeries12.Data.ValueXColumn = "";
            xySeries12.Data.ValueYColumn = "";
            xySeries12.Key = "series12";
            xySeries12.Label = "Delta Exposure (Base)";
            paintElement37.Fill = System.Drawing.Color.LightYellow;
            xySeries12.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement37});
            paintElement38.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint23.PE = paintElement38;
            xyDataPoint23.ValueX = 8D;
            xyDataPoint23.ValueY = -1D;
            paintElement39.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint24.PE = paintElement39;
            xyDataPoint24.ValueX = 6D;
            xyDataPoint24.ValueY = 3.5D;
            xySeries12.Points.Add(xyDataPoint23);
            xySeries12.Points.Add(xyDataPoint24);
            xySeries13.Data.ValueXColumn = "";
            xySeries13.Data.ValueYColumn = "";
            xySeries13.Key = "series13";
            xySeries13.Label = "Cost Basis P&L (Base)";
            paintElement40.Fill = System.Drawing.Color.LightCyan;
            xySeries13.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement40});
            paintElement41.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint25.PE = paintElement41;
            xyDataPoint25.ValueX = 8D;
            xyDataPoint25.ValueY = -1D;
            paintElement42.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint26.PE = paintElement42;
            xyDataPoint26.ValueX = 6D;
            xyDataPoint26.ValueY = 4.5D;
            xySeries13.Points.Add(xyDataPoint25);
            xySeries13.Points.Add(xyDataPoint26);
            xySeries14.Data.ValueXColumn = "";
            xySeries14.Data.ValueYColumn = "";
            xySeries14.Key = "series14";
            xySeries14.Label = "Simulated P&L (Base)";
            paintElement43.Fill = System.Drawing.Color.LightBlue;
            xySeries14.PEs.AddRange(new Infragistics.UltraChart.Resources.Appearance.PaintElement[] {
            paintElement43});
            paintElement44.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint27.PE = paintElement44;
            xyDataPoint27.ValueX = 8D;
            xyDataPoint27.ValueY = -1D;
            paintElement45.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            xyDataPoint28.PE = paintElement45;
            xyDataPoint28.ValueX = 6D;
            xyDataPoint28.ValueY = 5.5D;
            xySeries14.Points.Add(xyDataPoint27);
            xySeries14.Points.Add(xyDataPoint28);
            this.chartStrikeVol.CompositeChart.Series.AddRange(new Infragistics.UltraChart.Data.Series.ISeries[] {
            xySeries1,
            xySeries2,
            xySeries3,
            xySeries4,
            xySeries5,
            xySeries6,
            xySeries7,
            xySeries8,
            xySeries9,
            xySeries10,
            xySeries11,
            xySeries12,
            xySeries13,
            xySeries14});
            this.chartStrikeVol.Data.ZeroAligned = true;
            this.chartStrikeVol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartStrikeVol.EmptyChartText = "Data Not Available. Plz Fill the values for strike price and volatility.";
            this.chartStrikeVol.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chartStrikeVol.Legend.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.chartStrikeVol.Legend.FontColor = System.Drawing.Color.White;
            this.chartStrikeVol.Location = new System.Drawing.Point(0, 0);
            this.chartStrikeVol.Name = "chartStrikeVol";
            this.chartStrikeVol.Size = new System.Drawing.Size(784, 474);
            this.chartStrikeVol.TabIndex = 6;
            this.chartStrikeVol.TitleBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.chartStrikeVol.TitleBottom.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.TitleBottom.Text = "Volatility";
            this.chartStrikeVol.TitleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.chartStrikeVol.TitleLeft.Visible = true;
            this.chartStrikeVol.TitleRight.Text = "Gamma";
            this.chartStrikeVol.TitleTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Pixel);
            this.chartStrikeVol.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartStrikeVol.TitleTop.VerticalAlign = System.Drawing.StringAlignment.Far;
            this.chartStrikeVol.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.chartStrikeVol.Tooltips.FormatString = "(<DATA_VALUE_X:0.#>, <DATA_VALUE:0.##>)";
            this.chartStrikeVol.Tooltips.HighlightDataPoint = false;
            this.chartStrikeVol.ChartDataDoubleClicked += new Infragistics.UltraChart.Shared.Events.ChartDataClickedEventHandler(this.chart3D_ChartDataDoubleClicked);
            this.chartStrikeVol.FillSceneGraph += new Infragistics.UltraChart.Shared.Events.FillSceneGraphEventHandler(this.chartStrikeVol_FillSceneGraph);
            // 
            // AnalyticsChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.chartStrikeVol);
            this.Name = "AnalyticsChart";
            this.Size = new System.Drawing.Size(784, 474);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.AnalyticsChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartStrikeVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.IContainer components = null;
        #endregion

        private void AnalyticsChart_Load(object sender, EventArgs e)
        {
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.chartStrikeVol.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.chartStrikeVol.ForeColor = System.Drawing.Color.WhiteSmoke;
                }
                else
                {
                    this.chartStrikeVol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                }
            }

        }
    }
}
