using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace Circuits
{
    public class OutputLamp : Gate
    {
        private const int WIDTH = 10;
        private const int HEIGHT = 10;
        private bool _status;
        private Brush OnBrush = Brushes.Yellow;
        private Pen SelectedPen = Pens.Red;
        private Pen normalPen = Pens.LightGray;


        public OutputLamp(int x ,int y):base(x,y)
        {
            _status = false;
            pins.Add(new Pin(this, true, 20));
        }
        public override Pin GetOutput()
        {
            Pin output = null;
            return output;
        }
        public override void Draw(Graphics paper)
        {
            Brush brush;
            Pen pen;

            if (_status)
            {
                brush = OnBrush;
            }
            else
            {
                brush = normalBrush;
            }
            if (selected)
            {
                pen = SelectedPen;
            }
            else
            {
                pen = normalPen;
                    
            }
        
            foreach (Pin p in pins)
                p.Draw(paper);
          
            paper.FillRectangle(brush, left, top, WIDTH, HEIGHT);
            paper.DrawRectangle(pen, left, top, WIDTH, HEIGHT);

        }

        public override bool IsMouseOn(int x, int y)
        {

            if (left <= x && x < left + WIDTH
            && top <= y && y < top + HEIGHT)
                return true;
            else
                return false;
        }

        public override void MoveTo(int x, int y)
        {
            left = x;
            top = y;
            Pins[0].X = x - WIDTH;
            Pins[0].Y = y + HEIGHT/2;

        }

        public override bool Evaluate()
        {
            if (pins[0].InputWire != null)
            {
                Gate gateA = pins[0].InputWire.FromPin.Owner;
                _status = gateA.Evaluate();
            }
            return false;
        }
        public override Gate Clone()
        {
            clone = new OutputLamp(left, top);
            return clone;
        }
    }
}
