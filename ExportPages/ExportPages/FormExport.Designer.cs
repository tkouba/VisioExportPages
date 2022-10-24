namespace ExportPages
{
    partial class FormExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExport));
            this.listPages = new System.Windows.Forms.CheckedListBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelPages = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelFileNameExample = new System.Windows.Forms.Label();
            this.textBoxFileNameExample = new System.Windows.Forms.TextBox();
            this.contextMenuStripPages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // listPages
            // 
            resources.ApplyResources(this.listPages, "listPages");
            this.listPages.CheckOnClick = true;
            this.listPages.ContextMenuStrip = this.contextMenuStripPages;
            this.listPages.FormattingEnabled = true;
            this.listPages.Name = "listPages";
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelPages
            // 
            resources.ApplyResources(this.labelPages, "labelPages");
            this.labelPages.Name = "labelPages";
            // 
            // textBoxFileName
            // 
            resources.ApplyResources(this.textBoxFileName, "textBoxFileName");
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.TextChanged += new System.EventHandler(this.textBoxFileName_TextChanged);
            // 
            // labelFileName
            // 
            resources.ApplyResources(this.labelFileName, "labelFileName");
            this.labelFileName.Name = "labelFileName";
            // 
            // labelFileNameExample
            // 
            resources.ApplyResources(this.labelFileNameExample, "labelFileNameExample");
            this.labelFileNameExample.Name = "labelFileNameExample";
            // 
            // textBoxFileNameExample
            // 
            resources.ApplyResources(this.textBoxFileNameExample, "textBoxFileNameExample");
            this.textBoxFileNameExample.Name = "textBoxFileNameExample";
            this.textBoxFileNameExample.ReadOnly = true;
            // 
            // contextMenuStripPages
            // 
            this.contextMenuStripPages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectAll,
            this.toolStripMenuItemDeselectAll});
            this.contextMenuStripPages.Name = "contextMenuStripPages";
            resources.ApplyResources(this.contextMenuStripPages, "contextMenuStripPages");
            this.contextMenuStripPages.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripPages_ItemClicked);
            // 
            // toolStripMenuItemSelectAll
            // 
            this.toolStripMenuItemSelectAll.Name = "toolStripMenuItemSelectAll";
            resources.ApplyResources(this.toolStripMenuItemSelectAll, "toolStripMenuItemSelectAll");
            // 
            // toolStripMenuItemDeselectAll
            // 
            this.toolStripMenuItemDeselectAll.Name = "toolStripMenuItemDeselectAll";
            resources.ApplyResources(this.toolStripMenuItemDeselectAll, "toolStripMenuItemDeselectAll");
            // 
            // FormExport
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.textBoxFileNameExample);
            this.Controls.Add(this.labelFileNameExample);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.labelPages);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.listPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExport";
            this.Load += new System.EventHandler(this.FormExport_Load);
            this.contextMenuStripPages.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listPages;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelPages;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelFileNameExample;
        private System.Windows.Forms.TextBox textBoxFileNameExample;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPages;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeselectAll;
    }
}