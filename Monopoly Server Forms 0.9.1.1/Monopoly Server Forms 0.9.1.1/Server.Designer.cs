namespace Monopoly_Server_Forms_0._9._1._1
{
    partial class Server
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this.UserList = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // UserList
            // 
            this.UserList.BackColor = System.Drawing.Color.White;
            this.UserList.FlatAppearance.BorderSize = 0;
            this.UserList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UserList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserList.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UserList.Location = new System.Drawing.Point(809, -1);
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(221, 48);
            this.UserList.TabIndex = 0;
            this.UserList.Text = "Connected Users";
            this.UserList.UseVisualStyleBackColor = false;
            this.UserList.Click += new System.EventHandler(this.UserList_Click);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "Monopoly Server";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 453);
            this.Controls.Add(this.UserList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Server";
            this.Text = "Server";
            this.SizeChanged += new System.EventHandler(this.Server_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button UserList;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
    }
}

