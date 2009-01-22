﻿namespace Huseyint.Windows7.WindowsForms.Demo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.setOverlayIconButton = new System.Windows.Forms.Button();
            this.overlayIconsGroup = new System.Windows.Forms.GroupBox();
            this.clearOverlayIconButton = new System.Windows.Forms.Button();
            this.overlayIconLabel = new System.Windows.Forms.Label();
            this.overlayIconsCombo = new System.Windows.Forms.ComboBox();
            this.progressStatesGroup = new System.Windows.Forms.GroupBox();
            this.simulateProgressStatesButton = new System.Windows.Forms.Button();
            this.setProgressStateButton = new System.Windows.Forms.Button();
            this.progressStatesCombo = new System.Windows.Forms.ComboBox();
            this.progressStateLabel = new System.Windows.Forms.Label();
            this.overlayIconsGroup.SuspendLayout();
            this.progressStatesGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // setOverlayIconButton
            // 
            this.setOverlayIconButton.Location = new System.Drawing.Point(175, 19);
            this.setOverlayIconButton.Name = "setOverlayIconButton";
            this.setOverlayIconButton.Size = new System.Drawing.Size(75, 23);
            this.setOverlayIconButton.TabIndex = 0;
            this.setOverlayIconButton.Text = "Set Icon";
            this.setOverlayIconButton.UseVisualStyleBackColor = true;
            this.setOverlayIconButton.Click += new System.EventHandler(this.SetOverlayIconButton_Click);
            // 
            // overlayIconsGroup
            // 
            this.overlayIconsGroup.Controls.Add(this.clearOverlayIconButton);
            this.overlayIconsGroup.Controls.Add(this.overlayIconLabel);
            this.overlayIconsGroup.Controls.Add(this.setOverlayIconButton);
            this.overlayIconsGroup.Controls.Add(this.overlayIconsCombo);
            this.overlayIconsGroup.Location = new System.Drawing.Point(12, 12);
            this.overlayIconsGroup.Name = "overlayIconsGroup";
            this.overlayIconsGroup.Size = new System.Drawing.Size(440, 56);
            this.overlayIconsGroup.TabIndex = 1;
            this.overlayIconsGroup.TabStop = false;
            this.overlayIconsGroup.Text = "Overlay Icons:";
            // 
            // clearOverlayIconButton
            // 
            this.clearOverlayIconButton.Location = new System.Drawing.Point(256, 19);
            this.clearOverlayIconButton.Name = "clearOverlayIconButton";
            this.clearOverlayIconButton.Size = new System.Drawing.Size(75, 23);
            this.clearOverlayIconButton.TabIndex = 2;
            this.clearOverlayIconButton.Text = "Clear Icon";
            this.clearOverlayIconButton.UseVisualStyleBackColor = true;
            this.clearOverlayIconButton.Click += new System.EventHandler(this.ClearOverlayIconButton_Click);
            // 
            // overlayIconLabel
            // 
            this.overlayIconLabel.AutoSize = true;
            this.overlayIconLabel.Location = new System.Drawing.Point(6, 24);
            this.overlayIconLabel.Name = "overlayIconLabel";
            this.overlayIconLabel.Size = new System.Drawing.Size(32, 13);
            this.overlayIconLabel.TabIndex = 1;
            this.overlayIconLabel.Text = "Icon:";
            // 
            // overlayIconsCombo
            // 
            this.overlayIconsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.overlayIconsCombo.FormattingEnabled = true;
            this.overlayIconsCombo.Items.AddRange(new object[] {
            "Error",
            "Info",
            "New",
            "Warning"});
            this.overlayIconsCombo.Location = new System.Drawing.Point(48, 21);
            this.overlayIconsCombo.Name = "overlayIconsCombo";
            this.overlayIconsCombo.Size = new System.Drawing.Size(121, 21);
            this.overlayIconsCombo.TabIndex = 0;
            // 
            // progressStatesGroup
            // 
            this.progressStatesGroup.Controls.Add(this.simulateProgressStatesButton);
            this.progressStatesGroup.Controls.Add(this.setProgressStateButton);
            this.progressStatesGroup.Controls.Add(this.progressStatesCombo);
            this.progressStatesGroup.Controls.Add(this.progressStateLabel);
            this.progressStatesGroup.Location = new System.Drawing.Point(12, 74);
            this.progressStatesGroup.Name = "progressStatesGroup";
            this.progressStatesGroup.Size = new System.Drawing.Size(440, 80);
            this.progressStatesGroup.TabIndex = 3;
            this.progressStatesGroup.TabStop = false;
            this.progressStatesGroup.Text = "Progress States:";
            // 
            // simulateProgressStatesButton
            // 
            this.simulateProgressStatesButton.Location = new System.Drawing.Point(175, 48);
            this.simulateProgressStatesButton.Name = "simulateProgressStatesButton";
            this.simulateProgressStatesButton.Size = new System.Drawing.Size(75, 23);
            this.simulateProgressStatesButton.TabIndex = 3;
            this.simulateProgressStatesButton.Text = "Simulate";
            this.simulateProgressStatesButton.UseVisualStyleBackColor = true;
            this.simulateProgressStatesButton.Click += new System.EventHandler(this.SimulateProgressStatesButton_Click);
            // 
            // setProgressStateButton
            // 
            this.setProgressStateButton.Location = new System.Drawing.Point(175, 19);
            this.setProgressStateButton.Name = "setProgressStateButton";
            this.setProgressStateButton.Size = new System.Drawing.Size(75, 23);
            this.setProgressStateButton.TabIndex = 2;
            this.setProgressStateButton.Text = "Set State";
            this.setProgressStateButton.UseVisualStyleBackColor = true;
            this.setProgressStateButton.Click += new System.EventHandler(this.SetProgressStateButton_Click);
            // 
            // progressStatesCombo
            // 
            this.progressStatesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.progressStatesCombo.FormattingEnabled = true;
            this.progressStatesCombo.Location = new System.Drawing.Point(48, 21);
            this.progressStatesCombo.Name = "progressStatesCombo";
            this.progressStatesCombo.Size = new System.Drawing.Size(121, 21);
            this.progressStatesCombo.TabIndex = 1;
            // 
            // progressStateLabel
            // 
            this.progressStateLabel.AutoSize = true;
            this.progressStateLabel.Location = new System.Drawing.Point(6, 24);
            this.progressStateLabel.Name = "progressStateLabel";
            this.progressStateLabel.Size = new System.Drawing.Size(36, 13);
            this.progressStateLabel.TabIndex = 0;
            this.progressStateLabel.Text = "State:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 282);
            this.Controls.Add(this.progressStatesGroup);
            this.Controls.Add(this.overlayIconsGroup);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows 7 Managed Taskbar Extensions Demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.overlayIconsGroup.ResumeLayout(false);
            this.overlayIconsGroup.PerformLayout();
            this.progressStatesGroup.ResumeLayout(false);
            this.progressStatesGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button setOverlayIconButton;
        private System.Windows.Forms.GroupBox overlayIconsGroup;
        private System.Windows.Forms.Label overlayIconLabel;
        private System.Windows.Forms.ComboBox overlayIconsCombo;
        private System.Windows.Forms.Button clearOverlayIconButton;
        private System.Windows.Forms.GroupBox progressStatesGroup;
        private System.Windows.Forms.Button setProgressStateButton;
        private System.Windows.Forms.ComboBox progressStatesCombo;
        private System.Windows.Forms.Label progressStateLabel;
        private System.Windows.Forms.Button simulateProgressStatesButton;
    }
}

