namespace WMS
{
    partial class NewProductForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblLastChecked = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.dtpLastChecked = new System.Windows.Forms.DateTimePicker();
            this.nudCount = new System.Windows.Forms.NumericUpDown();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.btnCreateNewProduct = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPosX = new System.Windows.Forms.Label();
            this.nudPosX = new System.Windows.Forms.NumericUpDown();
            this.nudPosY = new System.Windows.Forms.NumericUpDown();
            this.nudPosZ = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPosZ)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(13, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(30, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title:";
            // 
            // lblLastChecked
            // 
            this.lblLastChecked.AutoSize = true;
            this.lblLastChecked.Location = new System.Drawing.Point(13, 41);
            this.lblLastChecked.Name = "lblLastChecked";
            this.lblLastChecked.Size = new System.Drawing.Size(75, 13);
            this.lblLastChecked.TabIndex = 1;
            this.lblLastChecked.Text = "Last checked:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(13, 64);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(38, 13);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "Count:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(13, 117);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description:";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(106, 10);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(200, 20);
            this.tbTitle.TabIndex = 4;
            // 
            // dtpLastChecked
            // 
            this.dtpLastChecked.Checked = false;
            this.dtpLastChecked.Location = new System.Drawing.Point(106, 36);
            this.dtpLastChecked.Name = "dtpLastChecked";
            this.dtpLastChecked.Size = new System.Drawing.Size(200, 20);
            this.dtpLastChecked.TabIndex = 5;
            // 
            // nudCount
            // 
            this.nudCount.Location = new System.Drawing.Point(106, 62);
            this.nudCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudCount.Name = "nudCount";
            this.nudCount.Size = new System.Drawing.Size(200, 20);
            this.nudCount.TabIndex = 6;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(106, 114);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(200, 49);
            this.tbDescription.TabIndex = 7;
            // 
            // btnCreateNewProduct
            // 
            this.btnCreateNewProduct.AutoSize = true;
            this.btnCreateNewProduct.Location = new System.Drawing.Point(196, 169);
            this.btnCreateNewProduct.Name = "btnCreateNewProduct";
            this.btnCreateNewProduct.Size = new System.Drawing.Size(110, 23);
            this.btnCreateNewProduct.TabIndex = 8;
            this.btnCreateNewProduct.Text = "Create new product";
            this.btnCreateNewProduct.UseVisualStyleBackColor = true;
            this.btnCreateNewProduct.Click += new System.EventHandler(this.btnCreateNewProduct_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(115, 169);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPosX
            // 
            this.lblPosX.AutoSize = true;
            this.lblPosX.Location = new System.Drawing.Point(13, 90);
            this.lblPosX.Name = "lblPosX";
            this.lblPosX.Size = new System.Drawing.Size(77, 13);
            this.lblPosX.TabIndex = 10;
            this.lblPosX.Text = "Position (x,y,z):";
            // 
            // nudPosX
            // 
            this.nudPosX.Location = new System.Drawing.Point(106, 88);
            this.nudPosX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPosX.Name = "nudPosX";
            this.nudPosX.Size = new System.Drawing.Size(64, 20);
            this.nudPosX.TabIndex = 13;
            // 
            // nudPosY
            // 
            this.nudPosY.Location = new System.Drawing.Point(174, 88);
            this.nudPosY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPosY.Name = "nudPosY";
            this.nudPosY.Size = new System.Drawing.Size(64, 20);
            this.nudPosY.TabIndex = 14;
            // 
            // nudPosZ
            // 
            this.nudPosZ.Location = new System.Drawing.Point(242, 88);
            this.nudPosZ.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudPosZ.Name = "nudPosZ";
            this.nudPosZ.Size = new System.Drawing.Size(64, 20);
            this.nudPosZ.TabIndex = 15;
            // 
            // NewProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 202);
            this.Controls.Add(this.nudPosZ);
            this.Controls.Add(this.nudPosY);
            this.Controls.Add(this.nudPosX);
            this.Controls.Add(this.lblPosX);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateNewProduct);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.nudCount);
            this.Controls.Add(this.dtpLastChecked);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblLastChecked);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProductForm";
            this.Text = "New Product";
            ((System.ComponentModel.ISupportInitialize)(this.nudCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPosZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblLastChecked;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.DateTimePicker dtpLastChecked;
        private System.Windows.Forms.NumericUpDown nudCount;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Button btnCreateNewProduct;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPosX;
        private System.Windows.Forms.NumericUpDown nudPosX;
        private System.Windows.Forms.NumericUpDown nudPosY;
        private System.Windows.Forms.NumericUpDown nudPosZ;
    }
}