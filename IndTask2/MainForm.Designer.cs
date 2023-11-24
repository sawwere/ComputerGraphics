
namespace IndTask2
{
    partial class MainForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBoxLight = new System.Windows.Forms.CheckBox();
            this.numericUpDownZ = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.labelFPS = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cubeMirror = new System.Windows.Forms.CheckBox();
            this.sphereMirror = new System.Windows.Forms.CheckBox();
            this.cubeTransparent = new System.Windows.Forms.CheckBox();
            this.sphereTransparent = new System.Windows.Forms.CheckBox();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkBoxLight);
            this.groupBox6.Controls.Add(this.numericUpDownZ);
            this.groupBox6.Controls.Add(this.numericUpDownY);
            this.groupBox6.Controls.Add(this.numericUpDownX);
            this.groupBox6.Controls.Add(this.label40);
            this.groupBox6.Controls.Add(this.label41);
            this.groupBox6.Controls.Add(this.label42);
            this.groupBox6.Controls.Add(this.label43);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.groupBox6.ForeColor = System.Drawing.Color.SeaShell;
            this.groupBox6.Location = new System.Drawing.Point(5, 201);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(375, 101);
            this.groupBox6.TabIndex = 36;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Источник света";
            // 
            // checkBoxLight
            // 
            this.checkBoxLight.AutoSize = true;
            this.checkBoxLight.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.checkBoxLight.Location = new System.Drawing.Point(9, 56);
            this.checkBoxLight.Name = "checkBoxLight";
            this.checkBoxLight.Size = new System.Drawing.Size(85, 21);
            this.checkBoxLight.TabIndex = 25;
            this.checkBoxLight.Text = "Включен";
            this.checkBoxLight.UseVisualStyleBackColor = true;
            // 
            // numericUpDownZ
            // 
            this.numericUpDownZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.numericUpDownZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.numericUpDownZ.ForeColor = System.Drawing.Color.SeaShell;
            this.numericUpDownZ.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownZ.Location = new System.Drawing.Point(307, 21);
            this.numericUpDownZ.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownZ.Name = "numericUpDownZ";
            this.numericUpDownZ.Size = new System.Drawing.Size(60, 23);
            this.numericUpDownZ.TabIndex = 24;
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.numericUpDownY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.numericUpDownY.ForeColor = System.Drawing.Color.SeaShell;
            this.numericUpDownY.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownY.Location = new System.Drawing.Point(215, 22);
            this.numericUpDownY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(60, 23);
            this.numericUpDownY.TabIndex = 23;
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.numericUpDownX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.numericUpDownX.ForeColor = System.Drawing.Color.SeaShell;
            this.numericUpDownX.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownX.Location = new System.Drawing.Point(123, 22);
            this.numericUpDownX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(60, 23);
            this.numericUpDownX.TabIndex = 22;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label40.Location = new System.Drawing.Point(6, 25);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(83, 17);
            this.label40.TabIndex = 21;
            this.label40.Text = "Положение";
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.label41.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label41.Location = new System.Drawing.Point(282, 22);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(19, 20);
            this.label41.TabIndex = 7;
            this.label41.Text = "Z";
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.label42.ForeColor = System.Drawing.Color.PaleGreen;
            this.label42.Location = new System.Drawing.Point(189, 24);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(20, 20);
            this.label42.TabIndex = 6;
            this.label42.Text = "Y";
            // 
            // label43
            // 
            this.label43.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(95, 24);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(21, 20);
            this.label43.TabIndex = 5;
            this.label43.Text = "X";
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.BackColor = System.Drawing.Color.Transparent;
            this.labelFPS.ForeColor = System.Drawing.Color.Lime;
            this.labelFPS.Location = new System.Drawing.Point(394, 40);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(39, 13);
            this.labelFPS.TabIndex = 12;
            this.labelFPS.Text = "FPS: 0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Location = new System.Drawing.Point(396, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(700, 700);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(97)))), ((int)(((byte)(97)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.SeaShell;
            this.button1.Location = new System.Drawing.Point(47, 370);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 35);
            this.button1.TabIndex = 40;
            this.button1.Text = "Построить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.checkedListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkedListBox1.ForeColor = System.Drawing.Color.SeaShell;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Передняя",
            "Задняя",
            "Левая",
            "Правая",
            "Верхняя",
            "Нижняя"});
            this.checkedListBox1.Location = new System.Drawing.Point(5, 65);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(369, 130);
            this.checkedListBox1.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.label4.Location = new System.Drawing.Point(12, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 20);
            this.label4.TabIndex = 40;
            this.label4.Text = "Зеркальность стен";
            // 
            // cubeMirror
            // 
            this.cubeMirror.AutoSize = true;
            this.cubeMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cubeMirror.Location = new System.Drawing.Point(16, 318);
            this.cubeMirror.Name = "cubeMirror";
            this.cubeMirror.Size = new System.Drawing.Size(134, 21);
            this.cubeMirror.TabIndex = 45;
            this.cubeMirror.Text = "Зеркальный куб";
            this.cubeMirror.UseVisualStyleBackColor = true;
            // 
            // sphereMirror
            // 
            this.sphereMirror.AutoSize = true;
            this.sphereMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sphereMirror.Location = new System.Drawing.Point(173, 318);
            this.sphereMirror.Name = "sphereMirror";
            this.sphereMirror.Size = new System.Drawing.Size(139, 21);
            this.sphereMirror.TabIndex = 46;
            this.sphereMirror.Text = "Зеркальный шар";
            this.sphereMirror.UseVisualStyleBackColor = true;
            // 
            // cubeTransparent
            // 
            this.cubeTransparent.AutoSize = true;
            this.cubeTransparent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cubeTransparent.Location = new System.Drawing.Point(15, 345);
            this.cubeTransparent.Name = "cubeTransparent";
            this.cubeTransparent.Size = new System.Drawing.Size(136, 21);
            this.cubeTransparent.TabIndex = 47;
            this.cubeTransparent.Text = "Прозрачный куб";
            this.cubeTransparent.UseVisualStyleBackColor = true;
            // 
            // sphereTransparent
            // 
            this.sphereTransparent.AutoSize = true;
            this.sphereTransparent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sphereTransparent.Location = new System.Drawing.Point(173, 343);
            this.sphereTransparent.Name = "sphereTransparent";
            this.sphereTransparent.Size = new System.Drawing.Size(141, 21);
            this.sphereTransparent.TabIndex = 48;
            this.sphereTransparent.Text = "Прозрачный шар";
            this.sphereTransparent.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1117, 767);
            this.Controls.Add(this.sphereTransparent);
            this.Controls.Add(this.cubeTransparent);
            this.Controls.Add(this.sphereMirror);
            this.Controls.Add(this.cubeMirror);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.SeaShell;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cubeMirror;
        private System.Windows.Forms.CheckBox sphereMirror;
        private System.Windows.Forms.CheckBox cubeTransparent;
        private System.Windows.Forms.CheckBox sphereTransparent;
        private System.Windows.Forms.CheckBox checkBoxLight;
        private System.Windows.Forms.NumericUpDown numericUpDownZ;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
    }
}

