namespace 地信开发大作业
{
    partial class frmSymbol
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSymbol));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SymbologyCtrl = new ESRI.ArcGIS.Controls.AxSymbologyControl();
            ((System.ComponentModel.ISupportInitialize)(this.SymbologyCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(346, 470);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(201, 470);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 29);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SymbologyCtrl
            // 
            this.SymbologyCtrl.Location = new System.Drawing.Point(75, 16);
            this.SymbologyCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.SymbologyCtrl.Name = "SymbologyCtrl";
            this.SymbologyCtrl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SymbologyCtrl.OcxState")));
            this.SymbologyCtrl.Size = new System.Drawing.Size(493, 444);
            this.SymbologyCtrl.TabIndex = 6;
            this.SymbologyCtrl.OnMouseDown += new ESRI.ArcGIS.Controls.ISymbologyControlEvents_Ax_OnMouseDownEventHandler(this.SymbologyCtrl_OnMouseDown);
            // 
            // frmSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 514);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.SymbologyCtrl);
            this.Name = "frmSymbol";
            this.Text = "frmSymbol";
            this.Load += new System.EventHandler(this.frmSymbol_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SymbologyCtrl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private ESRI.ArcGIS.Controls.AxSymbologyControl SymbologyCtrl;

    }
}