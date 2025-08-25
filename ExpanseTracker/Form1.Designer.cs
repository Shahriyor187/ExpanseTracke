namespace ExpanseTracker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            listBox1 = new ListBox();
            label1 = new Label();
            button3 = new Button();
            dateTimePickerfrom = new DateTimePicker();
            dateTimePickerto = new DateTimePicker();
            button2 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            textBox3 = new TextBox();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(4, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(174, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(201, 12);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(174, 27);
            textBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(12, 78);
            button1.Name = "button1";
            button1.Size = new Size(87, 29);
            button1.TabIndex = 2;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(466, 58);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(378, 324);
            listBox1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(466, 401);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 4;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 210);
            button3.Name = "button3";
            button3.Size = new Size(107, 29);
            button3.TabIndex = 6;
            button3.Text = "Filter by date";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // dateTimePickerfrom
            // 
            dateTimePickerfrom.Location = new Point(4, 256);
            dateTimePickerfrom.Name = "dateTimePickerfrom";
            dateTimePickerfrom.Size = new Size(250, 27);
            dateTimePickerfrom.TabIndex = 7;
            // 
            // dateTimePickerto
            // 
            dateTimePickerto.Location = new Point(4, 326);
            dateTimePickerto.Name = "dateTimePickerto";
            dateTimePickerto.Size = new Size(250, 27);
            dateTimePickerto.TabIndex = 8;
            // 
            // button2
            // 
            button2.Location = new Point(281, 78);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 9;
            button2.Text = "Delete ";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button4
            // 
            button4.Location = new Point(147, 78);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 10;
            button4.Text = "Edit";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(147, 145);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 11;
            button5.Text = "Show Chart ";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(723, 12);
            button6.Name = "button6";
            button6.Size = new Size(133, 27);
            button6.TabIndex = 12;
            button6.Text = "Dark mode";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(12, 145);
            button7.Name = "button7";
            button7.Size = new Size(107, 29);
            button7.TabIndex = 13;
            button7.Text = "Excel export";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(410, 12);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(187, 27);
            textBox3.TabIndex = 14;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 447);
            Controls.Add(textBox3);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button2);
            Controls.Add(dateTimePickerto);
            Controls.Add(dateTimePickerfrom);
            Controls.Add(button3);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private ListBox listBox1;
        private Label label1;
        private Button button3;
        private DateTimePicker dateTimePickerfrom;
        private DateTimePicker dateTimePickerto;
        private Button button2;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private TextBox textBox3;
        private ErrorProvider errorProvider1;
    }
}
