namespace ExpanseTracker
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            textBox3 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(268, 245);
            button1.Name = "button1";
            button1.Size = new Size(98, 34);
            button1.TabIndex = 0;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 22);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(180, 27);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 81);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(180, 27);
            textBox2.TabIndex = 2;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(397, 22);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(250, 27);
            dateTimePicker1.TabIndex = 3;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 145);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(180, 27);
            textBox3.TabIndex = 4;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 314);
            Controls.Add(textBox3);
            Controls.Add(dateTimePicker1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private DateTimePicker dateTimePicker1;
        private TextBox textBox3;
    }
}