using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;



namespace Circuits
{
   

    public class OrGate:Gate 
    {

        private const int WIDTH = 70;
        private const int HEIGHT = 50;

        public OrGate(int x, int y) : base(x, y)
        {
            Pins.Add(new Pin(this, true, 20));
            Pins.Add(new Pin(this, true, 20));
            Pins.Add(new Pin(this, false, 20));
            MoveTo(x, y);
        }
        public override Pin GetOutput()
        {
            Pin output = Pins[2];
            return output;
        }
        public override void MoveTo(int x, int y)
        {
            left = x;
            top = y;
            Pins[0].X = x - GAP;
            Pins[0].Y = y + GAP;
            Pins[1].X = x - GAP;
            Pins[1].Y = y + HEIGHT - GAP;
            Pins[2].X = x + WIDTH + GAP;
            Pins[2].Y = y + HEIGHT / 2;
        }
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
            Image image = null;
            if (selected)
            {
                image = Properties.Resources.OrGateAllRed;
            }
            else
            {
              
                image = Properties.Resources.OrGate;

            }
            foreach (Pin p in Pins)
                p.Draw(paper);
            paper.DrawImageUnscaled(image, Left, Top);
        }
        public override bool Evaluate()
        {
            if (Pins[0].InputWire != null || Pins[1].InputWire != null)
            {
                Gate gateA = pins[0].InputWire.FromPin.Owner;

                Gate gateB = pins[1].InputWire.FromPin.Owner;
                
                return gateA.Evaluate() || gateB.Evaluate();
            }
            else
            {
                MessageBox.Show("one or more input pins is not connected to an output pin and will be assumed false");
                return false;

            }
        }
        public override Gate Clone()
        {
            clone = new OrGate(left, top);
            return clone;
        }

    }
}