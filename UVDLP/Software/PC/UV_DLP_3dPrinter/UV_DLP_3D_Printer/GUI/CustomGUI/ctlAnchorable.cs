﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UV_DLP_3D_Printer.GUI.CustomGUI
{
    public class ctlAnchorable : UserControl
    {
        public enum AnchorTypes
        {
            None = -1,
            Top = 0,
            Left,
            Center,
            Right,
            Bottom
        }

        protected enum CtlState
        {
            Normal = 0,
            Hover = 1,
            Pressed = 2
        }

        protected CtlState mCtlState;
        int mGapx, mGapy;
        AnchorTypes mAnchorHoriz;
        AnchorTypes mAnchorVert;
        bool mChecked;
        //Control mCtlRefPos;

        // will apear in properties panel
        [Description("Horizontal space from anchored location"), Category("Anchoring")]
        public int Gapx
        {
            get { return mGapx; }
            set { mGapx = value; UpdatePosition(); }
        }
        [Description("Vertical space from anchored location"), Category("Data")]
        public int Gapy
        {
            get { return mGapy; }
            set { mGapy = value; UpdatePosition(); }
        }

        [Description("Horizontal anchor type"), Category("Data")]
        public AnchorTypes HorizontalAnchor
        {
            get { return mAnchorHoriz; }
            set { mAnchorHoriz = value; }
        }

        [Description("Vertical anchor type"), Category("Data")]
        public AnchorTypes VerticalAnchor
        {
            get { return mAnchorVert; }
            set { mAnchorVert = value; }
        }

        [Description("Check state"), Category("Data")]
        public bool Checked
        {
            get { return mChecked; }
            set { mChecked = value; Invalidate();  }
        }

        public ctlAnchorable()
        {
            mCtlState = CtlState.Normal;
            InitializeComponent();
            mGapx = 5;
            mGapy = 5;
            mAnchorHoriz = AnchorTypes.None;
            mAnchorVert = AnchorTypes.None;
        }

        public void SetPositioning(AnchorTypes horiz, AnchorTypes vert, int gapx, int gapy)
        {
            mAnchorHoriz = horiz;
            mAnchorVert = vert;
            mGapx = gapx;
            mGapy = gapy;
            UpdatePosition();
        }

        protected int GetPosition(int refpos, int refwidth, int width, int gap, AnchorTypes anchor)
        {
            int retval = 0;
            switch (anchor)
            {
                case AnchorTypes.Top:
                case AnchorTypes.Left:
                    retval = refpos + gap;
                    break;

                case AnchorTypes.Center:
                    retval = refpos + (refwidth - width) / 2 + gap;
                    break;

                case AnchorTypes.Right:
                case AnchorTypes.Bottom:
                    retval = refpos + refwidth - width - gap;
                    break;
            }
            return retval;
        }


        public void UpdatePosition()
        {
            if ((mAnchorHoriz == AnchorTypes.None) || (mAnchorVert == AnchorTypes.None))
                return;
            int x = GetPosition(0, Parent.Width, Width, mGapx, mAnchorHoriz);
            int y = GetPosition(0, Parent.Height, Height, mGapy, mAnchorVert);
            Location = new System.Drawing.Point(x,y);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            mCtlState = CtlState.Hover;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mCtlState = CtlState.Normal;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            mCtlState = CtlState.Pressed;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            mCtlState = CtlState.Hover;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnResize(EventArgs e)
        {
            UpdatePosition();
            base.OnResize(e);
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = !Checked;
            base.OnClick(e);
        }

        private void InitializeComponent()
        {
        }
   }
}
