using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTasks();
        }

        private void SaveTasks()
        {
            string filePath = "tasks.json";
            List<string> tasks = tasksListBox.Items.Cast<string>().ToList();
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void LoadTasks()
        {
            string filePath = "tasks.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<string> tasks = JsonConvert.DeserializeObject<List<string>>(json);
                tasksListBox.Items.Clear();
                foreach (var task in tasks)
                {
                    tasksListBox.Items.Add(task);
                }
            }
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
            if (!string.IsNullOrWhiteSpace(taskTextBox.Text))
            {
                tasksListBox.Items.Add("• " + taskTextBox.Text);
                taskTextBox.Clear();
                taskTextBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                taskTextBox.BorderStyle = BorderStyle.Fixed3D;
                taskTextBox.BackColor = Color.LightCoral;

                var clearErrorTimer = new Timer();
                clearErrorTimer.Interval = 1500;
                clearErrorTimer.Tick += (s, args) =>
                {
                    taskTextBox.BackColor = SystemColors.Window;
                    clearErrorTimer.Stop();
                };
                clearErrorTimer.Start();
            }
            taskTextBox.Focus();
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
