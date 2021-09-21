using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Circuits
{
    //1. Is it a better idea to fully document the Gate class or the AndGate
    //subclass
    //Can you inherit comments? Its better to fully document the Gate class as XML comments are inherited
    
    //

    //2.What is the advantage of making a method abstract in the superclass
    //rather than just writing a virtual method with no code in the body of
    //the method? Is there any disadvantage to an abstract method?

    //you dont have to write and extra method if that method is requied for each subclass and acts differently for each sub-class


    //3.If a class has an abstract method in it, does the class have to be
    //abstract?

    // Yes

    //4. What would happen in your program if one of the gates added to your
    //Compound Gate is another Compound Gate? Is your design robust  enough to cope with this situation?

    //Nothing because my design is not rbust enough to cope with this desin

    /// <summary>
    /// The main GUI for the COMP104 digital circuits editor.
    /// This has a toolbar, containing buttons called buttonAnd, buttonOr, etc.
    /// The contents of the circuit are drawn directly onto the form.
    /// 
    /// </summary>

    public partial class Form1 : Form
    {
        /// <summary>
        /// The (x,y) mouse position of the last MouseDown event.
        /// </summary>
        protected int startX, startY;

        /// <summary>
        /// If this is non-null, we are inserting a wire by
        /// dragging the mouse from startPin to some output Pin.
        /// </summary>
        protected Pin startPin = null;

        /// <summary>
        /// The (x,y) position of the current gate, just before we started dragging it.
        /// </summary>
        protected int currentX, currentY;

        /// <summary>
        /// The set of gates in the circuit
        /// </summary>
        /// 



        protected List<Gate> gates = new List<Gate>();

        /// <summary>
        /// The set of connector wires in the circuit
        /// </summary>
        protected List<Wire> wires = new List<Wire>();

        /// <summary>
        /// The currently selected gate, or null if no gate is selected.
        /// </summary>
        protected Gate current = null;

        /// <summary>
        /// The new gate that is about to be inserted into the circuit
        /// </summary>
        protected Gate newGate = null;
        /// <summary>
        /// The new compound cate that will be created
        /// </summary>
        protected CompoundGate newCompoundGate;
        //list of gate objects that are created
        protected List<Gate> selectedList = new List<Gate>();


        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Finds the pin that is close to (x,y), or returns
        /// null if there are no pins close to the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>

        public Pin findPin(int x, int y)
        {
            foreach (Gate g in gates)
            {

                foreach (Pin p in g.Pins)
                {
                    if (p.isMouseOn(x, y))
                        return p;
                }
            }
            return null;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Gate g in gates)
            {
                g.Draw(e.Graphics);
            }
            foreach (Wire w in wires)
            {
                w.Draw(e.Graphics);
            }
            if (startPin != null)
            {
                e.Graphics.DrawLine(Pens.White,
                    startPin.X, startPin.Y,
                    currentX, currentY);
            }
            if (newGate != null)
            {
                // show the gate that we are dragging into the circuit
                newGate.MoveTo(currentX, currentY);
                newGate.Draw(e.Graphics);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {    
            if (current != null)
            {
                current.Selected = false;
                current = null;
                this.Invalidate();
            }
            // See if we are inserting a new gate
            if (newGate != null)
            {
                // moves gate with mouse and adds gate to the list of gates
                newGate.MoveTo(e.X, e.Y);
                gates.Add(newGate);
                // resets newGate to null
                newGate = null;
                this.Invalidate();
            }
            else
            {
                // search for the first gate under the mouse position
                foreach (Gate g in gates)
                {
                    if (g.IsMouseOn(e.X, e.Y))
                    {
                        //sets gate to selected 
                        g.Selected = true;
                        // sets the current gate to the selected gate
                        current = g;
                     
                        this.Invalidate();
                        break;

                    }
                }
            }
              //checks if a new compound gate has been created 
            if (newCompoundGate != null)
            {
               foreach (Gate g in gates)
                    //adds gate to the new compound gate if it is selected.
                {
                    if (g.Selected == true)
                    {
                        
                        newCompoundGate.addGate(g);
                    }
                }
            }
            // Changes input status
            foreach (Gate g in gates)
            {
                // checks if gate is a input source 
                if (g is InputSource && g.IsMouseOn(e.X, e.Y))
                {
                    // changes status
                    ((InputSource)g).Voltage = !((InputSource)g).Voltage;
                }
                // checks if gate is an input source in a compound gate then changes status
                else if (g is CompoundGate)
                {
                    foreach (Gate cg in ((CompoundGate)g).CompoundGateList)
                    {
                        // checks id an inputsource is selected  that is part of a compound gate
                        if (cg is InputSource && cg.IsMouseOn(e.X, e.Y))
                        {
                            //changes status of input
                            ((InputSource)cg).Voltage = !((InputSource)cg).Voltage;

                        }
                    }
                }

            }

        }
           
    

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            if (current == null)
            {
                // try to start adding a wire
                startPin = findPin(e.X, e.Y);
            }
            else if (current.IsMouseOn(e.X, e.Y))
            {
                // start dragging the current object around
                startX = e.X;
                startY = e.Y;
                currentX = current.Left;
                currentY = current.Top;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (startPin != null)
            {
                Debug.WriteLine("wire from " + startPin + " to " + e.X + "," + e.Y);
                currentX = e.X;
                currentY = e.Y;
                this.Invalidate();  // this will draw the line
            }
            else if (startX >= 0 && startY >= 0 && current != null)
            {
                Debug.WriteLine("mouse move to " + e.X + "," + e.Y);
                current.MoveTo(currentX + (e.X - startX), currentY + (e.Y - startY));
                this.Invalidate();
            }
            else if (newGate != null)
            {
                currentX = e.X;
                currentY = e.Y;
                this.Invalidate();
            }
        }

        private void toolStripButtonOr_Click(object sender, EventArgs e)
        {
            //creates a new Or gate object
            newGate = new OrGate(0, 0);
        }

        private void toolStripButtonNot_Click(object sender, EventArgs e)
        {
            //creates a new Not gate object

            newGate = new NotGate(0, 0);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //creates a new Input Source gate object

            newGate = new InputSource(0, 0);

        }

        private void toolStripButtonOutput_Click(object sender, EventArgs e)
        {
            //creates a new Output Lamp gate object

            newGate = new OutputLamp(0, 0);
        }

        private void toolStripButtonEvaluate_Click(object sender, EventArgs e)
        {
            //checks for Output
            foreach (Gate g in gates)
            {
                //checks if the gate is a compound gate 
                if (g is CompoundGate)
                {
                    //searches for outup lamps in the compound gate list 
                    foreach(Gate cg in ((CompoundGate)g).CompoundGateList)
                    {
                        if (cg is OutputLamp)
                        {
                            // evlauates output lamps 
                            cg.Evaluate();
                            this.Invalidate();
                        }
                    }
                }
                // checks if gate is an output lamp
                if (g is OutputLamp)
                {
                    g.Evaluate();
                    this.Invalidate();
                }
            }
        }

        private void toolStripButtonClone_Click(object sender, EventArgs e)
        {
            //
            if (current != null)
            {
                newGate = current.Clone();
            }
            else
            {
                MessageBox.Show("Please select a gate to clone");
            }
 
           
        }

        private void toolStripButtonStartCompound_Click(object sender, EventArgs e)
        {

           // creates new compound gate 
            newCompoundGate = new CompoundGate(0, 0);

        }
    
    

        private void toolStripButtonEndCompound_Click(object sender, EventArgs e)
        {
            foreach (Gate g in newCompoundGate.CompoundList)
            {
                gates.Remove(g);
            }
            newGate = newCompoundGate;
            newCompoundGate = null;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (startPin != null)
            {
                // see if we can insert a wire
                Pin endPin = findPin(e.X, e.Y);
                if (endPin != null)
                {
                    Debug.WriteLine("Trying to connect " + startPin + " to " + endPin);
                    Pin input, output;
                    if (startPin.IsOutput)
                    {
                        input = endPin;
                        output = startPin;
                    }
                    else
                    {
                        input = startPin;
                        output = endPin;
                    }
                    if (input.IsInput && output.IsOutput)
                    {
                        if (input.InputWire == null)
                        {
                            Wire newWire = new Wire(output, input);
                            input.InputWire = newWire;
                            wires.Add(newWire);
                        }
                        else
                        {
                            MessageBox.Show("That input is already used.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: you must connect an output pin to an input pin.");
                    }
                }
                startPin = null;
                this.Invalidate();
            }
            // We have finished moving/dragging
            startX = -1;
            startY = -1;
            currentX = 0;
            currentY = 0;
   
        }

        private void buttonAnd_Click(object sender, EventArgs e)
        {
            //Creates new And Gate
            newGate = new AndGate(0, 0);
        }
    }
}