﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UV_DLP_3D_Printer.GUI.CustomGUI
{
    public partial class ctlHScroll : UserControl
    {
        protected int mBorderWidth;
        protected int mSliderWidth;
        protected int mMinVal;
        protected int mMaxVal;
        protected int mDelta;
        protected int mSliderPos;
        private int mSliderMax;
        private bool mDragging;
 
        protected enum MousePos
        {
            Left = 0,
            Slider,
            Right
        }

        // get/set functions
        [Description("Current slider value"), Category("Data")]
        public int Value
        {
            get { return (int)Math.Round(((float)(mMaxVal - mMinVal) / (float)mSliderMax) * mSliderPos) + mMinVal; }
            set { SetSliderPos((int)Math.Round(((float)mSliderMax / (float)(mMaxVal - mMinVal)) * (value - mMinVal))); }
        }

        [DefaultValue(100)]
        [Description("Maximum slider value"), Category("Data")]
        public int Maximum
        {
            get { return mMaxVal; }
            set { mMaxVal = value; NotifyClients(); }
        }

        [DefaultValue(0)]
        [Description("Minimum slider value"), Category("Data")]
        public int Minimum
        {
            get { return mMinVal; }
            set { mMinVal = value; NotifyClients(); }
        }

        [DefaultValue(32)]
        [Description("Width of slider in pixels"), Category("Data")]
        public int SliderWidth
        {
            get { return mSliderWidth; }
            set { mSliderWidth = value; Invalidate(); }
        }

        [DefaultValue(32)]
        [Description("Value added/subtraceted when clicking on right or left to the slider"), Category("Data")]
        public int Increment
        {
            get { return mDelta; }
            set { mDelta = value; }
        }

        // events
        [Description("Called when value is changed"), Category("CatAction")]
        public event EventHandler ValueChanged;
      
        
        public ctlHScroll()
        {
            InitializeComponent();
            mBorderWidth = 2;
            mSliderWidth = 32;
            mMaxVal = 0;
            mMaxVal = 100;
            mDelta = 10;
            mSliderPos = 0;
            mDragging = false;
            DoubleBuffered = true;
            CalcSliderMax();
        }

        private void CalcSliderMax()
        {
            mSliderMax = Width - mBorderWidth - 3;
        }

        private void NotifyClients()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            int bw = mBorderWidth;
            Rectangle barrc = new Rectangle(bw, bw, Width - 2 * bw, Height - 2 * bw);
            Rectangle sliderc = new Rectangle(bw + mSliderPos - mSliderWidth / 2, bw + 1, mSliderWidth, Height - 2 * bw - 2);
            Brush fbr = new SolidBrush(ForeColor);
            Pen fpen = new Pen(ForeColor, 1);
            Brush bbr = new SolidBrush(BackColor);
            gr.FillRectangle(fbr, barrc);
            gr.FillRectangle(bbr, sliderc);
            gr.DrawLine(fpen, bw + mSliderPos, bw + 2, bw + mSliderPos, Height - bw - 3);
            base.OnPaint(e);
        }

        protected MousePos GetMousePos(int xpos)
        {
            if (xpos < (mBorderWidth + mSliderPos - mSliderWidth / 2))
                return MousePos.Left;
            if (xpos > (mBorderWidth + mSliderPos + mSliderWidth / 2))
                return MousePos.Right;
            return MousePos.Slider;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (GetMousePos(e.X))
            {
                case MousePos.Left:
                    mSliderPos -= mDelta;
                    if (mSliderPos < 0)
                        mSliderPos = 0;
                    Invalidate();
                    NotifyClients();
                    break;

                case MousePos.Right:
                    mSliderPos += mDelta;
                    if (mSliderPos > mSliderMax)
                        mSliderPos = mSliderMax;
                    Invalidate();
                    NotifyClients();
                    break;

                case MousePos.Slider:
                    Capture = true;
                    mDragging = true;
                    break;
            }
        }

        protected void SetSliderPos(int newpos)
        {
            if (newpos < 0)
                newpos = 0;
            else if (newpos > mSliderMax)
                newpos = mSliderMax;
            if (newpos != mSliderPos)
            {
                mSliderPos = newpos;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!mDragging)
                return;
            SetSliderPos(e.X - mBorderWidth);
            NotifyClients();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Capture = false;
            mDragging = false;
            base.OnMouseUp(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalcSliderMax();
        }
    }
}
