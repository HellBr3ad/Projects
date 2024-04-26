using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TodoListApp2
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = addButton;
            this.ActiveControl = taskTextBox;
            tasksListBox.ScrollAlwaysVisible = true;
            tasksListBox.IntegralHeight = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete && tasksListBox.Focused)
            {
                removeButton_Click(this, new EventArgs());
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // Check if the taskTextBox is not empty
            if (!string.IsNullOrWhiteSpace(taskTextBox.Text))
            {
                tasksListBox.Items.Add("• " + taskTextBox.Text);
                taskTextBox.Clear();
                // Reset the border color to the default (optional)
                taskTextBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                // Set the border color to red to indicate an error
                taskTextBox.BorderStyle = BorderStyle.Fixed3D;
                taskTextBox.BackColor = Color.LightCoral;

                // Optionally, you could set a Timer to clear the color after a few seconds
                var clearErrorTimer = new Timer();
                clearErrorTimer.Interval = 1500; // 2 seconds
                clearErrorTimer.Tick += (s, args) =>
                {
                    taskTextBox.BackColor = SystemColors.Window; // Reset back color to default
                    clearErrorTimer.Stop(); // Stop the timer
                };
                clearErrorTimer.Start();
            }
            taskTextBox.Focus(); // Puts focus back on the taskTextBox after adding or attempting to add an item
        }


        private void removeButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex != -1)
            {
                tasksListBox.Items.RemoveAt(tasksListBox.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Please select a task to remove.", "No Task Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            taskTextBox.Focus();
        }



    }
}
