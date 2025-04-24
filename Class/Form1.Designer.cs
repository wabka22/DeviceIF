namespace DeviceIF
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.start_button = new System.Windows.Forms.Button();
            this.value_label = new System.Windows.Forms.Label();
            this.port_comboBox = new System.Windows.Forms.ComboBox();
            this.Baud_Rate_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.state_label = new System.Windows.Forms.Label();
            this.connection = new System.Windows.Forms.Button();
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(21, 29);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(154, 82);
            this.start_button.TabIndex = 0;
            this.start_button.Text = "START";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // value_label
            // 
            this.value_label.AutoSize = true;
            this.value_label.Location = new System.Drawing.Point(1196, 193);
            this.value_label.Name = "value_label";
            this.value_label.Size = new System.Drawing.Size(132, 16);
            this.value_label.TabIndex = 2;
            this.value_label.Text = "Значение датчика:";
            // 
            // port_comboBox
            // 
            this.port_comboBox.FormattingEnabled = true;
            this.port_comboBox.Location = new System.Drawing.Point(1207, 69);
            this.port_comboBox.Name = "port_comboBox";
            this.port_comboBox.Size = new System.Drawing.Size(121, 24);
            this.port_comboBox.TabIndex = 4;
            // 
            // Baud_Rate_comboBox
            // 
            this.Baud_Rate_comboBox.FormattingEnabled = true;
            this.Baud_Rate_comboBox.Location = new System.Drawing.Point(1207, 117);
            this.Baud_Rate_comboBox.Name = "Baud_Rate_comboBox";
            this.Baud_Rate_comboBox.Size = new System.Drawing.Size(121, 24);
            this.Baud_Rate_comboBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(1082, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(1082, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Baud Rate:";
            // 
            // state_label
            // 
            this.state_label.AutoSize = true;
            this.state_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.state_label.Location = new System.Drawing.Point(368, 29);
            this.state_label.Name = "state_label";
            this.state_label.Size = new System.Drawing.Size(151, 25);
            this.state_label.TabIndex = 8;
            this.state_label.Text = "Неподключено";
            // 
            // connection
            // 
            this.connection.Location = new System.Drawing.Point(208, 29);
            this.connection.Name = "connection";
            this.connection.Size = new System.Drawing.Size(154, 82);
            this.connection.TabIndex = 9;
            this.connection.TabStop = false;
            this.connection.Text = "Disable Connection Check";
            this.connection.UseVisualStyleBackColor = true;
            this.connection.Click += new System.EventHandler(this.button1_Click);
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.FormattingEnabled = true;
            this.deviceComboBox.Location = new System.Drawing.Point(1207, 21);
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(121, 24);
            this.deviceComboBox.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(1082, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Device:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 745);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.deviceComboBox);
            this.Controls.Add(this.connection);
            this.Controls.Add(this.state_label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Baud_Rate_comboBox);
            this.Controls.Add(this.port_comboBox);
            this.Controls.Add(this.value_label);
            this.Controls.Add(this.start_button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Label value_label;
        private System.Windows.Forms.ComboBox port_comboBox;
        private System.Windows.Forms.ComboBox Baud_Rate_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label state_label;
        private System.Windows.Forms.Button connection;
        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.Label label3;
    }
}

