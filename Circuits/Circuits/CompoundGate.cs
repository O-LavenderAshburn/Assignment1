using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Circuits
{
    public class CompoundGate : Gate
    {

        public List<Wire> Wirelist = new List<Wire>();
        public List<Gate> CompoundGateList = new List<Gate>();

        public CompoundGate(int x, int y) : base(x, y)
        {

        }
        public override Pin GetOutput()
        {
            throw new NotImplementedException();
        }
        public void addGate(Gate gate)
        {
            CompoundGateList.Add(gate);
            gate.Selected = true;
        }
        public override bool Selected
        {
            get { return selected; }
            set
            {
                foreach (Gate g in CompoundGateList)
                {
                    
                    g.Selected = value;

                }
            }
        }

        public override void Draw(Graphics paper)
        {
           
            foreach (Gate gate in CompoundGateList)
            {
               gate.Draw(paper);
            }
            foreach (Wire w in Wirelist)
            {
                w.Draw(paper);
            }
        }
        public override bool Evaluate()

        {
            return true;

        }
        public List<Gate> CompoundList
        {
            get { return CompoundGateList; }
            set { CompoundGateList = value; }
        }
        public override Gate Clone()
        {

       
            List<Gate> cloneList = new List<Gate>();

            foreach (Gate g in CompoundGateList)
            {

                Gate gateClone = g.Clone();
                cloneList.Add(gateClone);

              

            }
            // creates a new compound new compound gate object.
            CompoundGate CloneCompound = new CompoundGate(left, top);
            //copys the compound gate list
            CloneCompound.CompoundList = cloneList;

            foreach(Gate g in CompoundGateList)
            {
                foreach (Pin p in g.Pins)
                {
                      // checks if there is  a wire
                    if (p.InputWire != null)
                    {
                        //if the compound gate list contains the owner of the input wire.
                        if (CompoundGateList.Contains(p.InputWire.FromPin.Owner))
                        {
                            Pin input;
                            Pin output;
                            // gets the output pin from the index of the owner of owner of the input wire
                            output = cloneList[CompoundGateList.IndexOf(p.InputWire.FromPin.Owner)].GetOutput();
                            //input pin from from the list of pins in gate in the compound gate list of pins of the gate in gates list
                            input = cloneList[CompoundGateList.IndexOf(g)].Pins[g.Pins.IndexOf(p)];
                            // creates the new wire from the output pin to the input pin. 
                            Wire w = new Wire(output, input);

                            input.InputWire = w;
                            //adds wire to the list
                            CloneCompound.Wirelist.Add(w);

                        }

                    }

                }

            }

            return CloneCompound;

        }
        public override bool IsMouseOn(int x, int y)
        {
            foreach (Gate gate in CompoundGateList)
            {
                if (gate.IsMouseOn(x, y) == true)
                {
                    return true;
                }

            }
            return false;
        }
        public override void MoveTo(int x, int y)
        {
        
             foreach (Gate gate in CompoundGateList)
             {
                gate.MoveTo(gate.Left - left + x, gate.Top - top + y);
             }
            left = x;
            top = y;
           

        }
    }
}
