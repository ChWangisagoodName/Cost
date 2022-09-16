namespace CExportscope
{
    partial class FormRecordData
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonUpdateToInitiate = new System.Windows.Forms.RadioButton();
            this.radioButtonInitiate = new System.Windows.Forms.RadioButton();
            this.btnSure = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.btnDeleteAllData = new System.Windows.Forms.Button();
            this.radioButtonUpdateToComplete = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 146);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接数据库";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(75, 110);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(229, 21);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "root";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "密  码：";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(75, 83);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(229, 21);
            this.txtName.TabIndex = 1;
            this.txtName.Text = "root";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户名：";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(75, 56);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(229, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "3306";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "端  口：";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(75, 29);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(229, 21);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.1.5";
            // 
            // label1
            //  
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP地址：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonUpdateToComplete);
            this.groupBox2.Controls.Add(this.radioButtonUpdateToInitiate);
            this.groupBox2.Controls.Add(this.radioButtonInitiate);
            this.groupBox2.Location = new System.Drawing.Point(12, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态";
            // 
            // radioButtonUpdateToInitiate
            // 
            this.radioButtonUpdateToInitiate.AutoSize = true;
            this.radioButtonUpdateToInitiate.Location = new System.Drawing.Point(97, 20);
            this.radioButtonUpdateToInitiate.Name = "radioButtonUpdateToInitiate";
            this.radioButtonUpdateToInitiate.Size = new System.Drawing.Size(107, 16);
            this.radioButtonUpdateToInitiate.TabIndex = 0;
            this.radioButtonUpdateToInitiate.Text = "更新为初始状态";
            this.radioButtonUpdateToInitiate.UseVisualStyleBackColor = true;
            // 
            // radioButtonInitiate
            // 
            this.radioButtonInitiate.AutoSize = true;
            this.radioButtonInitiate.Checked = true;
            this.radioButtonInitiate.Location = new System.Drawing.Point(20, 20);
            this.radioButtonInitiate.Name = "radioButtonInitiate";
            this.radioButtonInitiate.Size = new System.Drawing.Size(71, 16);
            this.radioButtonInitiate.TabIndex = 0;
            this.radioButtonInitiate.TabStop = true;
            this.radioButtonInitiate.Text = "初始状态";
            this.radioButtonInitiate.UseVisualStyleBackColor = true;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(157, 272);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 2;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(241, 272);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtModelName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(13, 220);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 46);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "模型名称";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "名  称：";
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(74, 19);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(229, 21);
            this.txtModelName.TabIndex = 1;
            this.txtModelName.Text = "水发集团";
            // 
            // btnDeleteAllData
            // 
            this.btnDeleteAllData.Location = new System.Drawing.Point(13, 272);
            this.btnDeleteAllData.Name = "btnDeleteAllData";
            this.btnDeleteAllData.Size = new System.Drawing.Size(117, 23);
            this.btnDeleteAllData.TabIndex = 4;
            this.btnDeleteAllData.Text = "删除模型所有数据";
            this.btnDeleteAllData.UseVisualStyleBackColor = true;
            this.btnDeleteAllData.Click += new System.EventHandler(this.btnDeleteAllData_Click);
            // 
            // radioButtonUpdateToComplete
            // 
            this.radioButtonUpdateToComplete.AutoSize = true;
            this.radioButtonUpdateToComplete.Location = new System.Drawing.Point(209, 20);
            this.radioButtonUpdateToComplete.Name = "radioButtonUpdateToComplete";
            this.radioButtonUpdateToComplete.Size = new System.Drawing.Size(107, 16);
            this.radioButtonUpdateToComplete.TabIndex = 0;
            this.radioButtonUpdateToComplete.Text = "更新为完成状态";
            this.radioButtonUpdateToComplete.UseVisualStyleBackColor = true;
            // 
            // FormRecordData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 307);
            this.Controls.Add(this.btnDeleteAllData);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecordData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormRecordData";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonUpdateToInitiate;
        private System.Windows.Forms.RadioButton radioButtonInitiate;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnDeleteAllData;
        private System.Windows.Forms.RadioButton radioButtonUpdateToComplete;
    }
}