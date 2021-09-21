using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;


namespace Circuits
{
    /// <summary>
    /// This class implements an AND gate with two inputs
    /// and one output.
    /// </summary>
     
     
    public class AndGate : Gate
    {

        

        // width and height of the main part of the gate
        protected const int WIDTH = 70; 
        protected const int HEIGHT = 50;




        /// <summary>
        /// This is the list of all the pins of this gate.
        /// An AND gate always has two input pins (0 and 1)
        /// and one output pin (number 2).
        /// </summary>



        public AndGate(int x, int y): base (x,y)
        {
            pins.Add(new Pin(this, true, 20));
            pins.Add(new Pin(this, true, 20));
            pins.Add(new Pin(this, false, 20));
            MoveTo(x, y); // move the gate and position the pins
        }
        public override Pin GetOutput()
        {
            Pin output = Pins[2];
            return output;
        }







        /// <summary>
        /// True if the given (x,y) position is roughly
        /// on top of this gate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool IsMouseOn(int x, int y)
        {
            if (left <= x && x < left + WIDTH
                && top <= y && y < top + HEIGHT)
                return true;
            else
                return false;
        }

        public override void Draw(Graphics paper)
        {
            //Brush brush;
            //if (selected)
            //{
            //    brush = selectedBrush;
            //}
            //else
            //{
            //    brush = normalBrush;
            //}
            //foreach (Pin p in pins)
            //    p.Draw(paper);

            //// AND is simple, so we can use a circle plus a rectange.
            //// An alternative would be to use a bitmap.
            //paper.FillEllipse(brush, left, top, WIDTH, HEIGHT);
            //paper.FillRectangle(brush, left, top, WIDTH/2, HEIGHT);
            Image image = null;
            if (selected)
            {
                image = Properties.Resources.AndGateAllRed;
            }
            else
            {

                image = Properties.Resources.AndGate;

            }
            foreach (Pin p in Pins)
                p.Draw(paper);
            paper.DrawImageUnscaled(image, Left, Top);


        }
        

        public override void MoveTo(int x, int y)
        {
            Debug.WriteLine("pins = " + pins.Count);
            left = x;
            top = y;
            // must move the pins too
            pins[0].X = x - GAP;
            pins[0].Y = y + GAP;
            pins[1].X = x - GAP;
            pins[1].Y = y + HEIGHT - GAP;
            pins[2].X = x + WIDTH ;
            pins[2].Y = y + HEIGHT / 2;
        }
        public override bool Evaluate()
        {
            if (Pins[0].InputWire != null && Pins[1].InputWire != null)
            {
                Gate gateA = pins[0].InputWire.FromPin.Owner;
                Gate gateB = pins[1].InputWire.FromPin.Owner;
                return gateA.Evaluate() && gateB.Evaluate();
            }
            else
            {
                MessageBox.Show("one or more input pins is not connected to an output pin and will be assumed false");
                return false;

            }
        }
        public override Gate Clone()
        {
            clone = new AndGate(top,left);
      

            return clone;
        }
    }
}
