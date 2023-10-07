
namespace Lab4
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
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.radioButtonMove = new System.Windows.Forms.RadioButton();
            this.radioButtonRotate = new System.Windows.Forms.RadioButton();
            this.radioButtonScale = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAction = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCustomPosition = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxCenterRotate = new System.Windows.Forms.CheckBox();
            this.checkBoxCenterScale = new System.Windows.Forms.CheckBox();
            this.labelCustomPoint = new System.Windows.Forms.Label();
            this.buttonClassifyCustomPointPos = new System.Windows.Forms.Button();
            this.labelClassifyCustomPointPos = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonFindIntersections = new System.Windows.Forms.Button();
            this.numericUpDownRotate = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownScaleX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownScaleY = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleY)).BeginInit();
            this.SuspendLayout();
            // 
            // modeComboBox
            // 
            this.modeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F);
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Items.AddRange(new object[] {
            "Point",
            "Edge",
            "Polygon"});
            this.modeComboBox.Location = new System.Drawing.Point(12, 12);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(128, 37);
            this.modeComboBox.TabIndex = 1;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(150, 22);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 11;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // radioButtonMove
            // 
            this.radioButtonMove.AutoSize = true;
            this.radioButtonMove.Checked = true;
            this.radioButtonMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.radioButtonMove.Location = new System.Drawing.Point(14, 83);
            this.radioButtonMove.Name = "radioButtonMove";
            this.radioButtonMove.Size = new System.Drawing.Size(67, 24);
            this.radioButtonMove.TabIndex = 13;
            this.radioButtonMove.TabStop = true;
            this.radioButtonMove.Text = "Move";
            this.radioButtonMove.UseVisualStyleBackColor = true;
            // 
            // radioButtonRotate
            // 
            this.radioButtonRotate.AutoSize = true;
            this.radioButtonRotate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.radioButtonRotate.Location = new System.Drawing.Point(14, 113);
            this.radioButtonRotate.Name = "radioButtonRotate";
            this.radioButtonRotate.Size = new System.Drawing.Size(76, 24);
            this.radioButtonRotate.TabIndex = 14;
            this.radioButtonRotate.Text = "Rotate";
            this.radioButtonRotate.UseVisualStyleBackColor = true;
            // 
            // radioButtonScale
            // 
            this.radioButtonScale.AutoSize = true;
            this.radioButtonScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.radioButtonScale.Location = new System.Drawing.Point(14, 143);
            this.radioButtonScale.Name = "radioButtonScale";
            this.radioButtonScale.Size = new System.Drawing.Size(69, 24);
            this.radioButtonScale.TabIndex = 15;
            this.radioButtonScale.Text = "Scale";
            this.radioButtonScale.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "---------------------------------------------------------------------------------" +
    "------------------";
            // 
            // buttonAction
            // 
            this.buttonAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F);
            this.buttonAction.Location = new System.Drawing.Point(14, 382);
            this.buttonAction.Name = "buttonAction";
            this.buttonAction.Size = new System.Drawing.Size(299, 55);
            this.buttonAction.TabIndex = 17;
            this.buttonAction.Text = "Perform Action";
            this.buttonAction.UseVisualStyleBackColor = true;
            this.buttonAction.Click += new System.EventHandler(this.buttonAction_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 366);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "---------------------------------------------------------------------------------" +
    "------------------";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(304, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "---------------------------------------------------------------------------------" +
    "------------------";
            // 
            // labelCustomPosition
            // 
            this.labelCustomPosition.AutoSize = true;
            this.labelCustomPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.labelCustomPosition.Location = new System.Drawing.Point(196, 183);
            this.labelCustomPosition.Name = "labelCustomPosition";
            this.labelCustomPosition.Size = new System.Drawing.Size(37, 20);
            this.labelCustomPosition.TabIndex = 21;
            this.labelCustomPosition.Text = "0; 0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Lab4.Properties.Resources.grid;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(319, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(550, 425);
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // checkBoxCenterRotate
            // 
            this.checkBoxCenterRotate.AutoSize = true;
            this.checkBoxCenterRotate.Checked = true;
            this.checkBoxCenterRotate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCenterRotate.Location = new System.Drawing.Point(96, 119);
            this.checkBoxCenterRotate.Name = "checkBoxCenterRotate";
            this.checkBoxCenterRotate.Size = new System.Drawing.Size(93, 17);
            this.checkBoxCenterRotate.TabIndex = 24;
            this.checkBoxCenterRotate.Text = "Around center";
            this.checkBoxCenterRotate.UseVisualStyleBackColor = true;
            // 
            // checkBoxCenterScale
            // 
            this.checkBoxCenterScale.AutoSize = true;
            this.checkBoxCenterScale.Checked = true;
            this.checkBoxCenterScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCenterScale.Location = new System.Drawing.Point(96, 142);
            this.checkBoxCenterScale.Name = "checkBoxCenterScale";
            this.checkBoxCenterScale.Size = new System.Drawing.Size(93, 17);
            this.checkBoxCenterScale.TabIndex = 25;
            this.checkBoxCenterScale.Text = "Around center";
            this.checkBoxCenterScale.UseVisualStyleBackColor = true;
            // 
            // labelCustomPoint
            // 
            this.labelCustomPoint.AutoSize = true;
            this.labelCustomPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.labelCustomPoint.Location = new System.Drawing.Point(14, 183);
            this.labelCustomPoint.Name = "labelCustomPoint";
            this.labelCustomPoint.Size = new System.Drawing.Size(176, 20);
            this.labelCustomPoint.TabIndex = 26;
            this.labelCustomPoint.Text = "Custom point position:";
            // 
            // buttonClassifyCustomPointPos
            // 
            this.buttonClassifyCustomPointPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.buttonClassifyCustomPointPos.Location = new System.Drawing.Point(12, 206);
            this.buttonClassifyCustomPointPos.Name = "buttonClassifyCustomPointPos";
            this.buttonClassifyCustomPointPos.Size = new System.Drawing.Size(95, 27);
            this.buttonClassifyCustomPointPos.TabIndex = 27;
            this.buttonClassifyCustomPointPos.Text = "Classify";
            this.buttonClassifyCustomPointPos.UseVisualStyleBackColor = true;
            this.buttonClassifyCustomPointPos.Click += new System.EventHandler(this.buttonClassifyCustomPointPos_Click);
            // 
            // labelClassifyCustomPointPos
            // 
            this.labelClassifyCustomPointPos.AutoSize = true;
            this.labelClassifyCustomPointPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.labelClassifyCustomPointPos.Location = new System.Drawing.Point(113, 209);
            this.labelClassifyCustomPointPos.Name = "labelClassifyCustomPointPos";
            this.labelClassifyCustomPointPos.Size = new System.Drawing.Size(48, 20);
            this.labelClassifyCustomPointPos.TabIndex = 28;
            this.labelClassifyCustomPointPos.Text = "None";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(304, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "---------------------------------------------------------------------------------" +
    "------------------";
            // 
            // buttonFindIntersections
            // 
            this.buttonFindIntersections.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.buttonFindIntersections.Location = new System.Drawing.Point(12, 328);
            this.buttonFindIntersections.Name = "buttonFindIntersections";
            this.buttonFindIntersections.Size = new System.Drawing.Size(177, 35);
            this.buttonFindIntersections.TabIndex = 30;
            this.buttonFindIntersections.Text = "Find intersections";
            this.buttonFindIntersections.UseVisualStyleBackColor = true;
            this.buttonFindIntersections.Click += new System.EventHandler(this.buttonFindIntersections_Click);
            // 
            // numericUpDownRotate
            // 
            this.numericUpDownRotate.Location = new System.Drawing.Point(196, 119);
            this.numericUpDownRotate.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.numericUpDownRotate.Name = "numericUpDownRotate";
            this.numericUpDownRotate.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownRotate.TabIndex = 31;
            this.numericUpDownRotate.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // numericUpDownScaleX
            // 
            this.numericUpDownScaleX.Location = new System.Drawing.Point(196, 142);
            this.numericUpDownScaleX.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownScaleX.Name = "numericUpDownScaleX";
            this.numericUpDownScaleX.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownScaleX.TabIndex = 32;
            this.numericUpDownScaleX.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDownScaleY
            // 
            this.numericUpDownScaleY.Location = new System.Drawing.Point(263, 142);
            this.numericUpDownScaleY.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownScaleY.Name = "numericUpDownScaleY";
            this.numericUpDownScaleY.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownScaleY.TabIndex = 33;
            this.numericUpDownScaleY.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 446);
            this.Controls.Add(this.numericUpDownScaleY);
            this.Controls.Add(this.numericUpDownScaleX);
            this.Controls.Add(this.numericUpDownRotate);
            this.Controls.Add(this.buttonFindIntersections);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelClassifyCustomPointPos);
            this.Controls.Add(this.buttonClassifyCustomPointPos);
            this.Controls.Add(this.labelCustomPoint);
            this.Controls.Add(this.checkBoxCenterScale);
            this.Controls.Add(this.checkBoxCenterRotate);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelCustomPosition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonScale);
            this.Controls.Add(this.radioButtonRotate);
            this.Controls.Add(this.radioButtonMove);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.modeComboBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRotate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox modeComboBox;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.RadioButton radioButtonMove;
        private System.Windows.Forms.RadioButton radioButtonRotate;
        private System.Windows.Forms.RadioButton radioButtonScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCustomPosition;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBoxCenterRotate;
        private System.Windows.Forms.CheckBox checkBoxCenterScale;
        private System.Windows.Forms.Label labelCustomPoint;
        private System.Windows.Forms.Button buttonClassifyCustomPointPos;
        private System.Windows.Forms.Label labelClassifyCustomPointPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonFindIntersections;
        private System.Windows.Forms.NumericUpDown numericUpDownRotate;
        private System.Windows.Forms.NumericUpDown numericUpDownScaleX;
        private System.Windows.Forms.NumericUpDown numericUpDownScaleY;
    }
}

