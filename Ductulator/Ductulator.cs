using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoosterTools;

namespace Ductulator
{
    public partial class Ductulator : Form
    {
        public Ductulator()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double ED = RoosterTools.SizingToolkit.DuctEquivalentDiameter(double.Parse(SideAText.Text), double.Parse(SideBText.Text));
            EDText.Text = ED.ToString();
        }

        private void SideBText_TextChanged(object sender, EventArgs e)
        {
            if (FixA.Checked)
            {
                SetEDText();
            }
            if (FixED.Checked)
            {
                SetSideText(SideBText, SideAText);
            }
        }

        private void SetSideText(TextBox senderBox, TextBox otherBox)
        {
            double secondSide = SizingToolkit.DuctSecondRectangularDimension(double.Parse(EDText.Text), 8);// double.Parse(EDText.Text), double.Parse(senderBox.Text));
            otherBox.Text = secondSide.ToString();
        }

        private void SetEDText()
        {
            double ED = RoosterTools.SizingToolkit.DuctEquivalentDiameter(double.Parse(SideAText.Text), double.Parse(SideBText.Text));
            EDText.Text = ED.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
