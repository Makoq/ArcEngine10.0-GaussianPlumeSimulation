namespace 地信开发大作业
{
    partial class FormMeasureResult
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
            this.lblMeasureResult = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMeasureResult
            // 
            this.lblMeasureResult.AutoSize = true;
            this.lblMeasureResult.Location = new System.Drawing.Point(224, 119);
            this.lblMeasureResult.Name = "lblMeasureResult";
            this.lblMeasureResult.Size = new System.Drawing.Size(135, 15);
            this.lblMeasureResult.TabIndex = 3;
            this.lblMeasureResult.Text = "lblMeasureResult";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(91, 119);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(67, 15);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "量测结果";
            // 
            // FormMeasureResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 253);
            this.Controls.Add(this.lblMeasureResult);
            this.Controls.Add(this.lblResult);
            this.Name = "FormMeasureResult";
            this.Text = "FormMeasureResult";
            this.Load += new System.EventHandler(this.FormMeasureResult_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblMeasureResult;
        public System.Windows.Forms.Label lblResult;
    }
}