using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Circuits
{
    public abstract class Gate
    {
        // left is the left-hand edge of the main part of the gate.
        // So the input pins are further left than left.
        protected int left;

        // top is the top of the whole gate
        protected int top;

        // length of the connector legs sticking out left and right
        protected const int GAP = 10;

        protected bool selected = false;

        protected Brush selectedBrush = Brushes.Red;

        protected Brush normalBrush = Brushes.LightGray;
        protected List<Pin> pins = new List<Pin>();

        public Gate clone;

        public Gate(int x ,int y)
        {
            left = x;
            top = y;
            
        }
        /// <summary>
        /// </summary>
        /// checks if mouse is on the gate
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract bool IsMouseOn(int x, int y);
        /// <summary>
        /// Draws gate
        /// </summary>
        /// <param name="paper"></param>
        public abstract void Draw(Graphics paper);
        /// <summary>
        /// moves gate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public abstract void MoveTo(int x, int y);

        public abstract bool Evaluate();
        /// <summary>
        /// Clones gate
        /// </summary>
        public abstract Gate Clone();

        public virtual bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        public virtual int Left
        {
            get { return left; }
            set { left = value; }
        }

        public virtual int Top
        {
            get { return top; }
            set { top = value; }
        }
        public List<Pin> Pins
        {
            get { return pins; }
        }
        /// <summary>
        /// Gets the output pin 
        /// </summary>
        /// <returns></returns>
        public abstract Pin GetOutput();
    }
}