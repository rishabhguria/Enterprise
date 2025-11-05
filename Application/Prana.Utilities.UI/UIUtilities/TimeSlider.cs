using System;
using System.Drawing;
using System.Windows.Forms;

///////////////////////////////////////////////////////////////////////////////
// File  : TimeSlider.cs
// Picked from : http://www.codeproject.com/KB/miscctrl/TimeSlider.aspx
// Author: Tom Holt
// Desc  : A custom control for showing date values on a trackbar instead of
//         integer values. Also supports: 
//         1. Min and Max labels
//         2. Current value label
//         3. Ability to highlight a time segment within the given frame.
//         4. Ability to auto-increase the end date as time value increases.
///////////////////////////////////////////////////////////////////////////////

namespace Prana.Utilities.UI
{

    /// <summary>
    /// Available output date string formats
    /// </summary>
    public enum DateFormatEnum
    {
        /// <summary>Custom date format - uses CustomFormat string</summary>
        Custom,
        /// <summary>Long date string, no time string</summary>
        LongDateNoTime,
        /// <summary>Long date string, short time string</summary>
        LongDateShortTime,
        /// <summary>Long date string, long time string</summary>
        LongDateLongTime,
        /// <summary>Short date string, no time string</summary>
        ShortDateNoTime,
        /// <summary>Short date string, short time string</summary>
        ShortDateShortTime,
        /// <summary>Short date string, long time string</summary>
        ShortDateLongTime,
        /// <summary>No date string, long time string</summary>
        NoDateLongTime,
        /// <summary>No date string, short time string</summary>
        NoDateShortTime
    }

    /// <summary>
    /// The TimeSlider control acts like a trackbar, except that it uses dates 
    /// instead of integers. It also supports the showing of a highlighted time
    /// segment within the trackbar, to show a highlighted period of time.
    /// </summary>
    [ToolboxBitmap(typeof(TrackBar))]
    public class TimeSlider : TrackBar
    {

        #region Member variables

        private DateTime mdtMin;					// minimum date value
        private DateTime mdtMax;					// maximum date value
        private DateTime mdtCur;					// current date value
        private TimeSpan mtsSmall;					// small trackbar change
        private TimeSpan mtsLarge;					// large trackbar change
        private TimeSpan mtsTickFreq;				// tick frequency
        private Graphics mGraphics = null;			// the graphics object for the base trackbar

        private Label mMinLabel;					// label for min value
        private Label mMaxLabel;					// label for max value
        private Label mCurLabel;					// label for current value

        // these define where on the trackbar we place the segment. I never 
        // found a place that I found to be 'ideal', so I just made it easy
        // to change in the code.
        private const int SEGMENT_YPOS = 5;
        private const int SEGMENT_HEIGHT = 5;

        //private Panel mSegmentPanel;
        private Rectangle mSegmentRect;				// positioning for segment
        private Color mSegmentColor;				// color of segment
        private Brush mSegmentBrush;				// brush for drawing segment
        private DateTime mdtSegmentStart;			// start date for segment
        private DateTime mdtSegmentEnd;				// end date for segment
        private bool mbShowSegment = true;			// whether or not to show a segment

        private bool mbShowMinLabel = false;		// show min label flag
        private bool mbShowMaxLabel = false;		// show max label flag
        private bool mbShowCurLabel = true;			// show current label flag

        private bool mbSuspendLayout = false;		// ability to change min, max and current value within inter-dependant checks

        private DateFormatEnum meFormat = DateFormatEnum.Custom;	// how to show date values
        private string msCustomFormat = "MM/dd/yy HH:mm";			// custom date value format

        private bool mbShowLabelsAsDuration = false;	// abilty to show duration instead of absolute dates
        private bool mbAllowSlidingMaximum = false;		// ability to increase max as current value increases.

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for class. Initializes all our internal variables.
        /// </summary>
        public TimeSlider()
        {
            mdtMin = DateTime.Now.Date;
            mdtMax = DateTime.Now.Date.AddDays(1);
            mtsSmall = TimeSpan.FromMinutes(30);
            mtsLarge = TimeSpan.FromMinutes(30);
            mtsTickFreq = TimeSpan.FromMinutes(30);
            mdtCur = mdtMin;

            mSegmentColor = Color.FromArgb(127, 0, 128, 0);//.Green; // FromArgb(127, Color.GreenYellow); // half transparent green
            mSegmentBrush = new SolidBrush(mSegmentColor);

            mSegmentRect = new Rectangle();
            mSegmentRect.X = 0;
            mSegmentRect.Y = SEGMENT_YPOS;
            mSegmentRect.Width = 0;
            mSegmentRect.Height = SEGMENT_HEIGHT;

            mMinLabel = new System.Windows.Forms.Label();
            mMinLabel.AutoSize = true;
            mMinLabel.Location = new System.Drawing.Point(0, Height - 13);
            mMinLabel.Name = "lblMinValue";
            mMinLabel.Size = new System.Drawing.Size(0, 13);

            mMaxLabel = new System.Windows.Forms.Label();
            mMaxLabel.AutoSize = true;
            mMaxLabel.Location = new System.Drawing.Point(Width - 80, Height - 13);
            mMaxLabel.Name = "lblMaxValue";
            mMaxLabel.TextAlign = ContentAlignment.TopRight;
            mMaxLabel.Anchor = AnchorStyles.Right;
            mMaxLabel.Size = new System.Drawing.Size(0, 80);

            mCurLabel = new System.Windows.Forms.Label();
            mCurLabel.AutoSize = true;
            mCurLabel.Location = new System.Drawing.Point(0, Height - 13);
            mCurLabel.Name = "lblCurValue";
            mCurLabel.TextAlign = ContentAlignment.TopCenter;
            mCurLabel.Size = new System.Drawing.Size(0, 100);

            this.Controls.Add(mMinLabel);
            this.Controls.Add(mMaxLabel);
            this.Controls.Add(mCurLabel);

            // this call says "I'll draw it myself"
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

            InitializeComponent();
            SetIndexes();
        }

        #endregion

        #region Dispose function

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (mSegmentBrush != null)
                {
                    mSegmentBrush.Dispose();
                }
                if (mGraphics != null)
                {
                    mGraphics.Dispose();
                }

                if (mMinLabel != null)
                {
                    mMinLabel.Dispose();
                }

                if (mMaxLabel != null)
                {
                    mMaxLabel.Dispose();
                }

                if (mCurLabel != null)
                {
                    mCurLabel.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Whether or not to enable the segment feature of the time slider. This feature shows a timespan
        /// within the min and max to be highlighted within the slider.
        /// </summary>
        public bool ShowSegment
        {
            get { return mbShowSegment; }
            set
            {
                mbShowSegment = value;
                OnPaint(null);
                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// Whether the labels should show duration values or actual values.
        /// </summary>
        public bool ShowLabelsAsDuration
        {
            get { return mbShowLabelsAsDuration; }
            set
            {
                mbShowLabelsAsDuration = value;
                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// When set, you can increase the maximum date value by setting a higher
        /// value, which ups the max by 25%. If false, then setting a value > maximum 
        /// does nothing.
        /// </summary>
        public bool AllowSlidingMaximum
        {
            get { return mbAllowSlidingMaximum; }
            set
            {
                mbAllowSlidingMaximum = value;
            }
        }


        /// <summary>
        /// Whether or not to show the current minimum value on the slider control (far left side). The format for this
        /// value is defined by the Format property.
        /// </summary>
        public bool ShowMinimumLabel
        {
            get { return mbShowMinLabel; }
            set
            {
                mbShowMinLabel = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// Whether or not to show the current maximum value on the slider control (far right side). The format for this
        /// value is defined by the Format property.
        /// </summary>
        public bool ShowMaximumLabel
        {
            get { return mbShowMaxLabel; }
            set
            {
                mbShowMaxLabel = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// Whether or not to show the current value on the slider control (centered on the puck position). The format for this
        /// value is defined by the Format property.
        /// </summary>
        public bool ShowValueLabel
        {
            get { return mbShowCurLabel; }
            set
            {
                mbShowCurLabel = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// The format for any shown date values. If Custom is selected, then the CustomFormat string will 
        /// need to be set appropriately.
        /// </summary>
        public DateFormatEnum Format
        {
            get { return meFormat; }
            set
            {
                meFormat = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// If Custom is set for the Format property, then this is defines the string used to custom format
        /// shown date values.
        /// </summary>
        public string CustomFormat
        {
            get { return msCustomFormat; }
            set
            {
                msCustomFormat = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// If the segment feature is turned on, this is the start (or minimum) segment date value.
        /// </summary>
        public DateTime SegmentStart
        {
            get { return mdtSegmentStart; }
            set
            {
                if (!mbSuspendLayout)
                {
                    if ((value < mdtMin) || (value > mdtMax))
                    {
                        return;
                    }
                }

                mdtSegmentStart = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// If the segment feature is turned on, this is the end (or maximum) segment date value.
        /// </summary>
        public DateTime SegmentEnd
        {
            get { return mdtSegmentEnd; }
            set
            {
                if (!mbSuspendLayout)
                {
                    if ((value < mdtMin) || (value > mdtMax))
                    {
                        return;
                    }
                }

                mdtSegmentEnd = value;

                if (!mbSuspendLayout)
                {
                    DrawLabels();
                }
            }
        }

        /// <summary>
        /// The maximum allowable date value for this slider control.
        /// </summary>
        public new DateTime Minimum
        {
            get { return mdtMin; }
            set
            {
                // an interesting problem. in design mode you want this kind of
                // check, but at runtime you don't, as you will probably be 
                // resetting min and max at the same time. An alternative would 
                // be to have a SetMinMax function, but that isn't intuitive.
                // To get around this, call SuspendLayout and ResumeLayout.

                if (!mbSuspendLayout)
                {
                    //if ( value > Maximum ) {
                    //    return;
                    //}
                }

                mdtMin = value;

                if (!mbSuspendLayout)
                {
                    SetIndexes();
                }
            }
        }

        /// <summary>
        /// The minimum allowable date value for this slider control.
        /// </summary>
        public new DateTime Maximum
        {
            get { return mdtMax; }
            set
            {
                // see comments within Minimum
                if (!mbSuspendLayout)
                {
                    if (value < Minimum)
                    {
                        return;
                    }
                }

                mdtMax = value;

                if (!mbSuspendLayout)
                {
                    SetIndexes();
                }
            }
        }

        /// <summary>
        /// The current value for the slider, represented as a date.
        /// </summary>
        public new DateTime Value
        {
            get { return mdtCur; }
            set
            {

                if (!mbSuspendLayout)
                {
                    if (value.TimeOfDay < mdtMin.TimeOfDay)
                    {
                        return;
                    }

                    if (value > mdtMax)
                    {

                        //if ( mbAllowSlidingMaximum ) {
                        //    // ability to auto-increase the maximum based on the passed in value
                        //    TimeSpan ts = value - mdtMin;
                        //    TimeSpan ts2 = new TimeSpan((long)(ts.Ticks * 1.25));
                        //    mdtMax   = mdtMin + ts2;
                        //}
                        //else {
                        //    return;
                        //}
                        return;
                    }
                }

                mdtCur = value;
                if (!mbSuspendLayout)
                {
                    SetIndexes();
                }
            }
        }

        /// <summary>
        /// Override of the SmallChange feature within a trackbar, this defines how far the puck
        /// moves when it is drug along its path.
        /// </summary>
        public new TimeSpan SmallChange
        {
            get
            {
                return mtsSmall;
            }
            set
            {
                mtsSmall = value;
                SetIndexes();
            }
        }

        /// <summary>
        /// Override of the LargeChange feature within a trackbar, this defines how far the puck
        /// moves when it is clicked left or right of its current position.
        /// </summary>
        public new TimeSpan LargeChange
        {
            get
            {
                return mtsLarge;
            }
            set
            {
                mtsLarge = value;
                if (!mbSuspendLayout)
                {
                    SetIndexes();
                }
            }
        }

        /// <summary>
        /// Override of the TickFrequency feature within a trackbar, this defines how far apart 
        /// the ticks are for the control.
        /// </summary>        
        public new TimeSpan TickFrequency
        {
            get
            {
                return mtsTickFreq;
            }
            set
            {
                mtsTickFreq = value;
                if (!mbSuspendLayout)
                {
                    SetIndexes();
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Call this function if you want to set several values within the slider,
        /// and you don't want consistancy checks until updates are comlete. When
        /// done, call ResumeLayout.
        /// SuspendLayout basically gives you free reign to set any values into the 
        /// control you want. Note that it will blow up with bad values later, though!
        /// </summary>
        public new void SuspendLayout()
        {
            mbSuspendLayout = true;
        }

        /// <summary>
        /// Call this function if you called SuspendLayout during control value
        /// setting.
        /// </summary>
        public new void ResumeLayout()
        {
            mbSuspendLayout = false;
            SetIndexes();
        }

        #endregion

        #region Control overrides

        /// <summary>
        /// Resize event
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DrawLabels();
        }

        /// <summary>
        /// OnPaint event.
        /// We are kind of tricking the system, because I want to paint 
        /// on top of the trackbar. The system either wants to draw it all
        /// and not send OnPaint events or let me do everything. So, we say
        /// I'll-do-it-no-you-do-it-okay-I'll-do-it.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.SetStyle(ControlStyles.UserPaint, false);
            base.Refresh();


            if (mbShowSegment)
            {
                try
                {

                    //the line
                    //mGraphics = Graphics.FromHwnd(base.Handle);
                    //was originally Big Grincalled in the constructor, and that was probably to early !
                    mGraphics = Graphics.FromHwnd(base.Handle);
                    Rectangle colorRect = new Rectangle(mSegmentRect.X,
                            mSegmentRect.Y,
                            mSegmentRect.Width,
                            mSegmentRect.Height);

                    mGraphics.FillRectangle(mSegmentBrush,
                        colorRect);
                }
                catch
                {
                }
            }

            base.SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// When the slider value is changed, we need to update our current value to the correct
        /// date representing that position.
        /// </summary>
        protected override void OnScroll(EventArgs e)
        {

            SetCurrentFromIndexChange();
            base.OnScroll(e);
            //this.Parent.Controls[0
        }

        /// <summary>
        /// When the slider value is changed, we need to update our current value to the correct
        /// date representing that position.
        /// </summary>
        protected override void OnValueChanged(EventArgs e)
        {
            SetCurrentFromIndexChange();
            base.OnValueChanged(e);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Turn our date into a string based on the Format value.
        /// </summary>
        private string DateFormat(DateTime dt)
        {
            if (mbShowLabelsAsDuration)
            {
                return DateFormatAsDuration(dt);
            }
            else
            {
                return DateFormatAsDate(dt);
            }
        }

        /// <summary>
        /// Turn our date into a duration string, similar to what
        /// QuickTime shows.
        /// </summary>
        private string DateFormatAsDuration(DateTime dt)
        {
            TimeSpan ts = dt - Minimum;
            return string.Format("{0}:{1}:{2}",
                ts.TotalHours.ToString("00"),
                ts.Minutes.ToString("00"),
                ts.Seconds.ToString("00"));
        }

        /// <summary>
        /// Turn our date into a string based on the Format value.
        /// </summary>
        private string DateFormatAsDate(DateTime dt)
        {
            switch (meFormat)
            {
                case DateFormatEnum.LongDateNoTime:
                    return dt.ToLongDateString();
                case DateFormatEnum.LongDateShortTime:
                    return dt.ToLongDateString() + " " + dt.ToShortTimeString();
                case DateFormatEnum.LongDateLongTime:
                    return dt.ToLongDateString() + " " + dt.ToLongTimeString();
                case DateFormatEnum.ShortDateNoTime:
                    return dt.ToShortDateString();
                case DateFormatEnum.ShortDateShortTime:
                    return dt.ToShortDateString() + " " + dt.ToShortTimeString();
                case DateFormatEnum.ShortDateLongTime:
                    return dt.ToShortDateString() + " " + dt.ToLongTimeString();
                case DateFormatEnum.NoDateLongTime:
                    return dt.ToLongTimeString();
                case DateFormatEnum.NoDateShortTime:
                    return dt.ToShortTimeString();
                case DateFormatEnum.Custom:
                default:
                    return dt.ToString(msCustomFormat);
            }
        }

        /// <summary>
        /// Draw the labels and segment panel for our control, and put them in the 
        /// correct position based on current values.
        /// </summary>
        private void DrawLabels()
        {

            mMinLabel.Visible = mbShowMinLabel;
            mMaxLabel.Visible = mbShowMaxLabel;
            mCurLabel.Visible = mbShowCurLabel;

            mMinLabel.Text = DateFormat(mdtMin);
            mMaxLabel.Text = DateFormat(mdtMax);
            mCurLabel.Text = DateFormat(mdtCur);

            // figure out where the centered current label should go
            if (mbShowCurLabel)
            {
                double dPercent = ((double)base.Value) / 48;
                double x = ((double)Width * dPercent) - mCurLabel.Width / 2;

                // stay within the bounds of the control
                if (x < 0) x = 0;
                if ((x + mCurLabel.Width) > Width) x = Width - mCurLabel.Width;

                mCurLabel.Location = new Point((int)x, mCurLabel.Location.Y);
            }

            // set up the position for the max value label
            if (mbShowMaxLabel)
            {
                mMaxLabel.Location = new Point(Width - mMaxLabel.Width, mMaxLabel.Location.Y);
            }

            //SetSegment(SegmentStart,SegmentEnd);

        }

        public void SetSegment(DateTime startTime, DateTime endTime)
        {
            // figure out where the segment should go
            if (mbShowSegment)
            {
                TimeSpan startTime1 = startTime.TimeOfDay;
                TimeSpan endTime1 = endTime.TimeOfDay;
                if (endTime.TimeOfDay.CompareTo(startTime.TimeOfDay) <= 0)
                {
                    endTime1 = endTime1.Add(TimeSpan.FromDays(1));
                }
                double timeINMin = startTime1.Ticks / TimeSpan.TicksPerMinute;
                mSegmentRect.X = 9 + (int)((timeINMin / 30) * 8);
                mSegmentRect.Y = SEGMENT_YPOS;


                mSegmentRect.Width = (int)(((endTime1.Ticks - startTime1.Ticks) / TimeSpan.TicksPerMinute) / 30) * (8);

                mSegmentRect.Height = SEGMENT_HEIGHT;


            }
        }

        /// <summary>
        /// Set min, max and current values within the control. Also calls DrawLabels,
        /// so this function pretty much performs all layout within the control.
        /// </summary>
        private void SetIndexes()
        {

            if (mdtMin == mdtMax)
            {
                base.Enabled = false;
                base.Minimum = 0;
                base.Maximum = 0;
                base.Value = 0;

                mMinLabel.Visible = false;
                mMaxLabel.Visible = false;
                mCurLabel.Visible = false;

                return;
            }
            base.Enabled = true;

            double nTicksMin = mdtMin.Ticks;
            double nTicksMax = mdtMax.Ticks;
            double nTicksCur = mdtCur.Ticks;

            if (nTicksCur < nTicksMin)
            {
                nTicksCur = nTicksMin;
                mdtCur = new DateTime((long)nTicksCur);
            }
            if (nTicksCur > nTicksMax)
            {
                nTicksCur = nTicksMax;
                mdtCur = new DateTime((long)nTicksCur);
            }

            double dCurPercent = 48 * (nTicksCur - nTicksMin) / (nTicksMax - nTicksMin);

            // we always go 0 to 100 in the base control, and just adjust our values to fit
            // within that ranges.
            base.Minimum = 0;
            base.Maximum = 48;

            TimeSpan tsTotal = mdtMax - mdtMin;
            base.SmallChange = Convert.ToInt32((mtsSmall.TotalSeconds / tsTotal.TotalSeconds) * 48);
            base.LargeChange = Convert.ToInt32((mtsLarge.TotalSeconds / tsTotal.TotalSeconds) * 48);
            base.TickFrequency = Convert.ToInt32((mtsTickFreq.TotalSeconds / tsTotal.TotalSeconds) * 48);

            base.Value = (int)dCurPercent;

            mMinLabel.Text = mdtMin.ToShortDateString() + " " + mdtMin.ToShortTimeString();
            mMaxLabel.Text = mdtMax.ToShortDateString() + " " + mdtMax.ToShortTimeString();
            DrawLabels();
        }

        /// <summary>
        /// When the puck was changed through user interaction, update our current 
        /// value and labels based on the new value.
        /// </summary>
        private void SetCurrentFromIndexChange()
        {
            double nTicksMin = mdtMin.Ticks;
            double nTicksMax = mdtMax.Ticks;

            double d1 = ((double)base.Value) / 48.0;
            double nTicksCur = d1 * (nTicksMax - nTicksMin) + nTicksMin;
            mdtCur = new DateTime((long)nTicksCur);
            DrawLabels();
        }

        #endregion
    }
}