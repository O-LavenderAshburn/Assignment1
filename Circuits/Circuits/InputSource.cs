using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Circuits
{
    public class InputSource : Gate
    {
        private bool _Voltage;
        private const int WIDTH = 10;
        private const int HEIGHT = 10;
        private Brush OnBrush = Brushes.Green;
        private Pen SelectedPen = Pens.Red;
        private Pen normalPen = Pens.LightGray;


        public InputSource(int x, int y) : base(x, y)
        {

            _Voltage = false;
            Pins.Add(new Pin(this, false, 20));
        }

        public override Pin GetOutput()
        {
            Pin output = Pins[0];
            return output;
        }

        public override void Draw(Graphics paper)
        {
            Brush brush;
            Pen pen;

            
             if (selected && _Voltage ==true)
            {
                brush = OnBrush;
                pen = SelectedPen;

            }
            else if (Voltage == true && !selected)
            {
                brush = OnBrush;
                pen = normalPen;
            }
             else if (Voltage == false && !selected)
            {
                brush = normalBrush;
                pen = normalPen;
            }
             else
            {
                brush = normalBrush;
                pen = SelectedPen;
            }
            foreach (Pin p in pins)
                p.Draw(paper);
            Console.WriteLine(_Voltage.ToString());
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
        public override bool Evaluate()
        {
            return _Voltage;
        }
        public override void MoveTo(int x, int y)
        {
            left = x;
            top = y;
            Pins[0].X = x + (WIDTH*2);
            Pins[0].Y = y + HEIGHT/2;

        }
        public bool Voltage
        {
            get { return _Voltage; }
            set { _Voltage = value; }
        }
        public override Gate Clone()
        {
            clone = new InputSource(left, top);
            return clone;
        }
        

            
    }
}
