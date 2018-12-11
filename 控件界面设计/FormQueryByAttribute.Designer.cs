namespace 地信开发大作业
{
    partial class FormQueryByAttribute
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
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxWhere = new System.Windows.Forms.TextBox();
            this.labelwhere = new System.Windows.Forms.Label();
            this.labelSelect = new System.Windows.Forms.Label();
            this.buttonGetUniqeValue = new System.Windows.Forms.Button();
            this.listBoxValues = new System.Windows.Forms.ListBox();
            this.buttonPercent = new System.Windows.Forms.Button();
            this.buttonUnderLine = new System.Windows.Forms.Button();
            this.buttonIs = new System.Windows.Forms.Button();
            this.buttonNot = new System.Windows.Forms.Button();
            this.buttonBrackets = new System.Windows.Forms.Button();
            this.buttonOr = new System.Windows.Forms.Button();
            this.buttonLessEqual = new System.Windows.Forms.Button();
            this.buttonLess = new System.Windows.Forms.Button();
            this.buttonAnd = new System.Windows.Forms.Button();
            this.buttonGeaterEqual = new System.Windows.Forms.Button();
            this.buttonGreater = new System.Windows.Forms.Button();
            this.buttonLike = new System.Windows.Forms.Button();
            this.buttonNotEqual = new System.Windows.Forms.Button();
            this.buttonEqual = new System.Windows.Forms.Button();
            this.listBoxFields = new System.Windows.Forms.ListBox();
            this.comboBoxSelectMethod = new System.Windows.Forms.ComboBox();
            this.labelSelectionMethod = new System.Windows.Forms.Label();
            this.checkBoxSelectableLayers = new System.Windows.Forms.CheckBox();
            this.comboBoxLayerName = new System.Windows.Forms.ComboBox();
            this.labelLayerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(426, 539);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 25);
            this.buttonApply.TabIndex = 95;
            this.buttonApply.Text = "应用";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(529, 539);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 25);
            this.buttonClose.TabIndex = 94;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(321, 539);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 25);
            this.buttonOK.TabIndex = 93;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(35, 520);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(581, 4);
            this.groupBox1.TabIndex = 92;
            this.groupBox1.TabStop = false;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(57, 481);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 25);
            this.buttonClear.TabIndex = 91;
            this.buttonClear.Text = "清除";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBoxWhere
            // 
            this.textBoxWhere.Location = new System.Drawing.Point(35, 363);
            this.textBoxWhere.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxWhere.Multiline = true;
            this.textBoxWhere.Name = "textBoxWhere";
            this.textBoxWhere.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxWhere.Size = new System.Drawing.Size(580, 110);
            this.textBoxWhere.TabIndex = 90;
            // 
            // labelwhere
            // 
            this.labelwhere.AutoSize = true;
            this.labelwhere.Location = new System.Drawing.Point(149, 331);
            this.labelwhere.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelwhere.Name = "labelwhere";
            this.labelwhere.Size = new System.Drawing.Size(15, 15);
            this.labelwhere.TabIndex = 89;
            this.labelwhere.Text = " ";
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.Location = new System.Drawing.Point(32, 331);
            this.labelSelect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(119, 15);
            this.labelSelect.TabIndex = 88;
            this.labelSelect.Text = "SELECT * FORM ";
            // 
            // buttonGetUniqeValue
            // 
            this.buttonGetUniqeValue.Location = new System.Drawing.Point(444, 301);
            this.buttonGetUniqeValue.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGetUniqeValue.Name = "buttonGetUniqeValue";
            this.buttonGetUniqeValue.Size = new System.Drawing.Size(160, 25);
            this.buttonGetUniqeValue.TabIndex = 87;
            this.buttonGetUniqeValue.Text = "获取唯一属性值";
            this.buttonGetUniqeValue.UseVisualStyleBackColor = true;
            this.buttonGetUniqeValue.Click += new System.EventHandler(this.buttonGetUniqeValue_Click);
            // 
            // listBoxValues
            // 
            this.listBoxValues.FormattingEnabled = true;
            this.listBoxValues.ItemHeight = 15;
            this.listBoxValues.Location = new System.Drawing.Point(441, 109);
            this.listBoxValues.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxValues.Name = "listBoxValues";
            this.listBoxValues.Size = new System.Drawing.Size(173, 184);
            this.listBoxValues.TabIndex = 86;
            this.listBoxValues.DoubleClick += new System.EventHandler(this.listBoxValues_DoubleClick);
            // 
            // buttonPercent
            // 
            this.buttonPercent.Location = new System.Drawing.Point(261, 225);
            this.buttonPercent.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPercent.Name = "buttonPercent";
            this.buttonPercent.Size = new System.Drawing.Size(27, 39);
            this.buttonPercent.TabIndex = 85;
            this.buttonPercent.Text = "%";
            this.buttonPercent.UseVisualStyleBackColor = true;
            this.buttonPercent.Click += new System.EventHandler(this.buttonPercent_Click);
            // 
            // buttonUnderLine
            // 
            this.buttonUnderLine.Location = new System.Drawing.Point(232, 225);
            this.buttonUnderLine.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUnderLine.Name = "buttonUnderLine";
            this.buttonUnderLine.Size = new System.Drawing.Size(27, 39);
            this.buttonUnderLine.TabIndex = 84;
            this.buttonUnderLine.Text = "_";
            this.buttonUnderLine.UseVisualStyleBackColor = true;
            this.buttonUnderLine.Click += new System.EventHandler(this.buttonUnderLine_Click);
            // 
            // buttonIs
            // 
            this.buttonIs.Location = new System.Drawing.Point(232, 271);
            this.buttonIs.Margin = new System.Windows.Forms.Padding(4);
            this.buttonIs.Name = "buttonIs";
            this.buttonIs.Size = new System.Drawing.Size(56, 22);
            this.buttonIs.TabIndex = 83;
            this.buttonIs.Text = "Is";
            this.buttonIs.UseVisualStyleBackColor = true;
            this.buttonIs.Click += new System.EventHandler(this.buttonIs_Click);
            // 
            // buttonNot
            // 
            this.buttonNot.Location = new System.Drawing.Point(360, 225);
            this.buttonNot.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNot.Name = "buttonNot";
            this.buttonNot.Size = new System.Drawing.Size(56, 39);
            this.buttonNot.TabIndex = 82;
            this.buttonNot.Text = "Not";
            this.buttonNot.UseVisualStyleBackColor = true;
            this.buttonNot.Click += new System.EventHandler(this.buttonNot_Click);
            // 
            // buttonBrackets
            // 
            this.buttonBrackets.Location = new System.Drawing.Point(296, 225);
            this.buttonBrackets.Margin = new System.Windows.Forms.Padding(4);
            this.buttonBrackets.Name = "buttonBrackets";
            this.buttonBrackets.Size = new System.Drawing.Size(56, 39);
            this.buttonBrackets.TabIndex = 81;
            this.buttonBrackets.Text = "( )";
            this.buttonBrackets.UseVisualStyleBackColor = true;
            this.buttonBrackets.Click += new System.EventHandler(this.buttonBrackets_Click);
            // 
            // buttonOr
            // 
            this.buttonOr.Location = new System.Drawing.Point(360, 186);
            this.buttonOr.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOr.Name = "buttonOr";
            this.buttonOr.Size = new System.Drawing.Size(56, 22);
            this.buttonOr.TabIndex = 80;
            this.buttonOr.Text = "Or";
            this.buttonOr.UseVisualStyleBackColor = true;
            this.buttonOr.Click += new System.EventHandler(this.buttonOr_Click);
            // 
            // buttonLessEqual
            // 
            this.buttonLessEqual.Location = new System.Drawing.Point(296, 186);
            this.buttonLessEqual.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLessEqual.Name = "buttonLessEqual";
            this.buttonLessEqual.Size = new System.Drawing.Size(56, 22);
            this.buttonLessEqual.TabIndex = 79;
            this.buttonLessEqual.Text = "<=";
            this.buttonLessEqual.UseVisualStyleBackColor = true;
            this.buttonLessEqual.Click += new System.EventHandler(this.buttonLessEqual_Click);
            // 
            // buttonLess
            // 
            this.buttonLess.Location = new System.Drawing.Point(232, 186);
            this.buttonLess.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLess.Name = "buttonLess";
            this.buttonLess.Size = new System.Drawing.Size(56, 22);
            this.buttonLess.TabIndex = 78;
            this.buttonLess.Text = "<";
            this.buttonLess.UseVisualStyleBackColor = true;
            this.buttonLess.Click += new System.EventHandler(this.buttonLess_Click);
            // 
            // buttonAnd
            // 
            this.buttonAnd.Location = new System.Drawing.Point(360, 147);
            this.buttonAnd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAnd.Name = "buttonAnd";
            this.buttonAnd.Size = new System.Drawing.Size(56, 22);
            this.buttonAnd.TabIndex = 77;
            this.buttonAnd.Text = "And";
            this.buttonAnd.UseVisualStyleBackColor = true;
            this.buttonAnd.Click += new System.EventHandler(this.buttonAnd_Click);
            // 
            // buttonGeaterEqual
            // 
            this.buttonGeaterEqual.Location = new System.Drawing.Point(296, 147);
            this.buttonGeaterEqual.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGeaterEqual.Name = "buttonGeaterEqual";
            this.buttonGeaterEqual.Size = new System.Drawing.Size(56, 22);
            this.buttonGeaterEqual.TabIndex = 76;
            this.buttonGeaterEqual.Text = ">=";
            this.buttonGeaterEqual.UseVisualStyleBackColor = true;
            this.buttonGeaterEqual.Click += new System.EventHandler(this.buttonGeaterEqual_Click);
            // 
            // buttonGreater
            // 
            this.buttonGreater.Location = new System.Drawing.Point(232, 147);
            this.buttonGreater.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGreater.Name = "buttonGreater";
            this.buttonGreater.Size = new System.Drawing.Size(56, 22);
            this.buttonGreater.TabIndex = 75;
            this.buttonGreater.Text = ">";
            this.buttonGreater.UseVisualStyleBackColor = true;
            this.buttonGreater.Click += new System.EventHandler(this.buttonGreater_Click);
            // 
            // buttonLike
            // 
            this.buttonLike.Location = new System.Drawing.Point(360, 109);
            this.buttonLike.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLike.Name = "buttonLike";
            this.buttonLike.Size = new System.Drawing.Size(56, 22);
            this.buttonLike.TabIndex = 74;
            this.buttonLike.Text = "Like";
            this.buttonLike.UseVisualStyleBackColor = true;
            this.buttonLike.Click += new System.EventHandler(this.buttonLike_Click);
            // 
            // buttonNotEqual
            // 
            this.buttonNotEqual.Location = new System.Drawing.Point(296, 109);
            this.buttonNotEqual.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNotEqual.Name = "buttonNotEqual";
            this.buttonNotEqual.Size = new System.Drawing.Size(56, 22);
            this.buttonNotEqual.TabIndex = 73;
            this.buttonNotEqual.Text = "<>";
            this.buttonNotEqual.UseVisualStyleBackColor = true;
            this.buttonNotEqual.Click += new System.EventHandler(this.buttonNotEqual_Click);
            // 
            // buttonEqual
            // 
            this.buttonEqual.Location = new System.Drawing.Point(232, 109);
            this.buttonEqual.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEqual.Name = "buttonEqual";
            this.buttonEqual.Size = new System.Drawing.Size(56, 22);
            this.buttonEqual.TabIndex = 72;
            this.buttonEqual.Text = "=";
            this.buttonEqual.UseVisualStyleBackColor = true;
            this.buttonEqual.Click += new System.EventHandler(this.buttonEqual_Click);
            // 
            // listBoxFields
            // 
            this.listBoxFields.FormattingEnabled = true;
            this.listBoxFields.ItemHeight = 15;
            this.listBoxFields.Location = new System.Drawing.Point(35, 109);
            this.listBoxFields.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxFields.Name = "listBoxFields";
            this.listBoxFields.Size = new System.Drawing.Size(173, 184);
            this.listBoxFields.TabIndex = 71;
            this.listBoxFields.SelectedIndexChanged += new System.EventHandler(this.listBoxFields_SelectedIndexChanged);
            this.listBoxFields.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxFields_MouseDoubleClick);
            // 
            // comboBoxSelectMethod
            // 
            this.comboBoxSelectMethod.FormattingEnabled = true;
            this.comboBoxSelectMethod.Items.AddRange(new object[] {
            "创建新选择集",
            "添加到当前选择集",
            "从当前选择集中删除",
            "从当前选择集中选择"});
            this.comboBoxSelectMethod.Location = new System.Drawing.Point(127, 64);
            this.comboBoxSelectMethod.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxSelectMethod.Name = "comboBoxSelectMethod";
            this.comboBoxSelectMethod.Size = new System.Drawing.Size(488, 23);
            this.comboBoxSelectMethod.TabIndex = 70;
            // 
            // labelSelectionMethod
            // 
            this.labelSelectionMethod.AutoSize = true;
            this.labelSelectionMethod.Location = new System.Drawing.Point(32, 67);
            this.labelSelectionMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelectionMethod.Name = "labelSelectionMethod";
            this.labelSelectionMethod.Size = new System.Drawing.Size(82, 15);
            this.labelSelectionMethod.TabIndex = 69;
            this.labelSelectionMethod.Text = "选择方式：";
            // 
            // checkBoxSelectableLayers
            // 
            this.checkBoxSelectableLayers.AutoSize = true;
            this.checkBoxSelectableLayers.Location = new System.Drawing.Point(127, 44);
            this.checkBoxSelectableLayers.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSelectableLayers.Name = "checkBoxSelectableLayers";
            this.checkBoxSelectableLayers.Size = new System.Drawing.Size(134, 19);
            this.checkBoxSelectableLayers.TabIndex = 68;
            this.checkBoxSelectableLayers.Text = "只显示可选图层";
            this.checkBoxSelectableLayers.UseVisualStyleBackColor = true;
            // 
            // comboBoxLayerName
            // 
            this.comboBoxLayerName.FormattingEnabled = true;
            this.comboBoxLayerName.Location = new System.Drawing.Point(126, 13);
            this.comboBoxLayerName.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxLayerName.Name = "comboBoxLayerName";
            this.comboBoxLayerName.Size = new System.Drawing.Size(488, 23);
            this.comboBoxLayerName.TabIndex = 67;
            this.comboBoxLayerName.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayerName_SelectedIndexChanged);
            // 
            // labelLayerName
            // 
            this.labelLayerName.AutoSize = true;
            this.labelLayerName.Location = new System.Drawing.Point(35, 16);
            this.labelLayerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLayerName.Name = "labelLayerName";
            this.labelLayerName.Size = new System.Drawing.Size(82, 15);
            this.labelLayerName.TabIndex = 66;
            this.labelLayerName.Text = "图层名称：";
            // 
            // FormQueryByAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 638);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.textBoxWhere);
            this.Controls.Add(this.labelwhere);
            this.Controls.Add(this.labelSelect);
            this.Controls.Add(this.buttonGetUniqeValue);
            this.Controls.Add(this.listBoxValues);
            this.Controls.Add(this.buttonPercent);
            this.Controls.Add(this.buttonUnderLine);
            this.Controls.Add(this.buttonIs);
            this.Controls.Add(this.buttonNot);
            this.Controls.Add(this.buttonBrackets);
            this.Controls.Add(this.buttonOr);
            this.Controls.Add(this.buttonLessEqual);
            this.Controls.Add(this.buttonLess);
            this.Controls.Add(this.buttonAnd);
            this.Controls.Add(this.buttonGeaterEqual);
            this.Controls.Add(this.buttonGreater);
            this.Controls.Add(this.buttonLike);
            this.Controls.Add(this.buttonNotEqual);
            this.Controls.Add(this.buttonEqual);
            this.Controls.Add(this.listBoxFields);
            this.Controls.Add(this.comboBoxSelectMethod);
            this.Controls.Add(this.labelSelectionMethod);
            this.Controls.Add(this.checkBoxSelectableLayers);
            this.Controls.Add(this.comboBoxLayerName);
            this.Controls.Add(this.labelLayerName);
            this.Name = "FormQueryByAttribute";
            this.Text = "FormQueryByAttribute";
            this.Load += new System.EventHandler(this.FormQueryByAttribute_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBoxWhere;
        private System.Windows.Forms.Label labelwhere;
        private System.Windows.Forms.Label labelSelect;
        private System.Windows.Forms.Button buttonGetUniqeValue;
        private System.Windows.Forms.ListBox listBoxValues;
        private System.Windows.Forms.Button buttonPercent;
        private System.Windows.Forms.Button buttonUnderLine;
        private System.Windows.Forms.Button buttonIs;
        private System.Windows.Forms.Button buttonNot;
        private System.Windows.Forms.Button buttonBrackets;
        private System.Windows.Forms.Button buttonOr;
        private System.Windows.Forms.Button buttonLessEqual;
        private System.Windows.Forms.Button buttonLess;
        private System.Windows.Forms.Button buttonAnd;
        private System.Windows.Forms.Button buttonGeaterEqual;
        private System.Windows.Forms.Button buttonGreater;
        private System.Windows.Forms.Button buttonLike;
        private System.Windows.Forms.Button buttonNotEqual;
        private System.Windows.Forms.Button buttonEqual;
        private System.Windows.Forms.ListBox listBoxFields;
        private System.Windows.Forms.ComboBox comboBoxSelectMethod;
        private System.Windows.Forms.Label labelSelectionMethod;
        private System.Windows.Forms.CheckBox checkBoxSelectableLayers;
        private System.Windows.Forms.ComboBox comboBoxLayerName;
        private System.Windows.Forms.Label labelLayerName;
    }
}