using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class GridCellColorizer
        : IUIElementDrawFilter
    {
        #region Private Members

        private UltraGrid grid;
        private Timer timer;
        List<CellInfo> cellInfos;

        #endregion // Private Members

        #region Constructor
        public GridCellColorizer(UltraGrid grid)
        {
            this.grid = grid;

            if (this.grid.DrawFilter != null)
            {
                // The Colorizer uses the grid's DrawFilter to color the cell, so if there is already 
                // a DrawFilter on the grid, this won't work, since the grid can only have one DrawFilter. 
                throw new ArgumentException("The specific grid already has a DrawFilter.");
            }

            this.grid.DrawFilter = this;
        }
        #endregion // Constructor

        #region Properties

        #region Timer
        public Timer Timer
        {
            get
            {
                if (null == this.timer)
                {
                    this.timer = Infragistics.Win.Utilities.CreateTimer();
                    this.timer.Interval = 30;
                    this.timer.Tick += new EventHandler(timer_Tick);
                }

                return this.timer;
            }
        }
        #endregion // Timer

        #endregion // Properties

        #region Methods

        #region AddCell
        public void AddCell(UltraGridCell cell, Color backColor, int timeInMilliseconds)
        {
            if (null == this.cellInfos)
                this.cellInfos = new List<CellInfo>();

            cellInfos.Add(new CellInfo(cell, backColor, timeInMilliseconds));

            this.VerifyTimer();
        }
        #endregion // AddCell

        #region VerifyTimer
        private void VerifyTimer()
        {
            bool timerEnabled =
                null != this.cellInfos &&
                this.cellInfos.Count > 0;

            if (this.Timer.Enabled != timerEnabled)
                this.Timer.Enabled = timerEnabled;
        }
        #endregion // VerifyTimer

        #endregion // Methods

        #region Event Handlers

        #region timer_Tick
        void timer_Tick(object sender, EventArgs e)
        {
            this.VerifyTimer();

            List<CellInfo> finishedCellInfos = new List<CellInfo>();

            foreach (CellInfo cellInfo in this.cellInfos)
            {
                cellInfo.Process();

                if (cellInfo.IsFinished)
                    finishedCellInfos.Add(cellInfo);
            }

            foreach (CellInfo finishedCellInfo in finishedCellInfos)
                this.cellInfos.Remove(finishedCellInfo);

            this.VerifyTimer();
        }
        #endregion // timer_Tick

        #endregion // Event Handlers

        #region class CellInfo
        private class CellInfo
        {
            #region Private members
            private UltraGridCell cell;
            private Color backColor;
            private double timeInMilliseconds;

            private DateTime startTime;
            private bool isFinished = false;

            private List<CellUIElement> cellUIElements;
            #endregion // Private members

            #region Constructor
            internal CellInfo(UltraGridCell cell, Color backColor, double timeInMilliseconds)
            {
                this.cell = cell;
                this.backColor = backColor;
                this.timeInMilliseconds = timeInMilliseconds;

                this.startTime = DateTime.Now;
            }
            #endregion // Constructor

            #region Process
            internal void Process()
            {
                this.isFinished = this.TimeLeft <= 0;

                this.cellUIElements = this.GetCellUIElements();
                if (null != this.cellUIElements)
                {
                    foreach (CellUIElement cellElement in this.cellUIElements)
                        cellElement.Invalidate();
                }
            }
            #endregion // Process

            #region GetCellUIElements
            private List<CellUIElement> GetCellUIElements()
            {
                UltraGridLayout layout = this.cell.Band.Layout;
                UltraGridUIElement gridElement = layout.UIElement;
                if (null == gridElement)
                    return null;


                DataAreaUIElement dataAreaUIElement = gridElement.GetDescendant(typeof(DataAreaUIElement)) as DataAreaUIElement;
                if (null == dataAreaUIElement)
                    return null;

                List<CellUIElement> cellUIElements = new List<CellUIElement>();
                foreach (UIElement childElement in dataAreaUIElement.ChildElements)
                {
                    RowColRegionIntersectionUIElement rowColRegionIntersectionUIElement = childElement as RowColRegionIntersectionUIElement;
                    if (null == rowColRegionIntersectionUIElement)
                        continue;

                    CellUIElement cellElement = rowColRegionIntersectionUIElement.GetDescendant(typeof(CellUIElement), this.cell) as CellUIElement;
                    if (null != cellElement)
                        cellUIElements.Add(cellElement);
                }

                return cellUIElements;
            }
            #endregion // GetCellUIElements

            #region Properties

            #region BackColor
            public Color BackColor
            {
                get
                {
                    if (this.TimeLeft < 0)
                        return Color.Empty;

                    double percentage = (this.TimeLeft / this.timeInMilliseconds) * 0.75;
                    int alpha = (int)(255.0 * percentage);
                    return Color.FromArgb(alpha, this.backColor);
                }
            }
            #endregion // BackColor

            #region CellUIElements
            public List<CellUIElement> CellUIElements
            {
                get { return this.cellUIElements; }
            }
            #endregion // CellUIElements

            #region IsFinished
            public bool IsFinished
            {
                get { return this.isFinished; }
            }
            #endregion // IsFinished

            #region TimeLeft
            private double TimeLeft
            {
                get
                {
                    double elapsedTime = (DateTime.Now - this.startTime).TotalMilliseconds;
                    double timeLeft = this.timeInMilliseconds - elapsedTime;

                    return timeLeft;
                }
            }
            #endregion // TimeLeft

            #endregion // Properties

        }
        #endregion // class CellInfo

        #region IUIElementDrawFilter
        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            if (null == this.cellInfos)
                return false;



            CellUIElement element = drawParams.Element as CellUIElement;
            if (null == element)
                element = drawParams.Element.GetAncestor(typeof(CellUIElement)) as CellUIElement;

            if (null == element)
            {
                Debug.Fail("Failed to find a CellUIElement; unexpected.");
                return false;
            }

            foreach (CellInfo cellInfo in this.cellInfos)
            {
                if (null == cellInfo.CellUIElements)
                    continue;

                if (cellInfo.CellUIElements.Contains(element))
                {
                    using (SolidBrush brush = new SolidBrush(cellInfo.BackColor))
                    {
                        drawParams.Graphics.FillRectangle(brush, element.RectInsideBorders);
                    }
                }
            }

            return false;
        }

        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            if (drawParams.Element is CellUIElement ||
                drawParams.Element is EmbeddableUIElementBase)
            {
                return DrawPhase.AfterDrawBackColor;
            }


            return DrawPhase.None;
        }
        #endregion // IUIElementDrawFilter
    }
}
