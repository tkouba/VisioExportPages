using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExportPages.Properties;
using Visio = Microsoft.Office.Interop.Visio;


namespace ExportPages
{
    public partial class FormExport : Form
    {
        Visio.Application application;

        public string FileNameTemplate => textBoxFileName.Text;

        public IEnumerable<string> SelectedPages
        {
            get
            {
                foreach (var item in listPages.CheckedItems)
                {
                    yield return item.ToString();
                }
            }
        }

        public FormExport(Visio.Application application)
        {
            this.application = application;
            InitializeComponent();
        }

        private void FormExport_Load(object sender, EventArgs e)
        {
            UpdateFileNameExample();
            //application.Settings.RasterExportOperation
            Visio.Document document = application.ActiveDocument;
            if (document != null)
            {
                foreach (Visio.Page page in document.Pages)
                {
                    listPages.Items.Add(page.Name, page.Background == 0);
                }
            }
        }

        private void UpdateFileNameExample()
        {
            try
            {
                Visio.IVPage page = application.ActivePage ?? application.ActiveDocument.Pages.Cast<Visio.IVPage>().FirstOrDefault();
                if (page == null)
                    throw new IndexOutOfRangeException(Resources.DocumentDoesNotContainAnyPageToExport);
                PageInfo pageInfo = new PageInfo(page);
                textBoxFileNameExample.Text = textBoxFileName.Text.FormatWith(pageInfo);
            }
            catch (Exception ex)
            {
                labelFileNameExample.Text = ex.Message;
            }

        }

        private void textBoxFileName_TextChanged(object sender, EventArgs e)
        {
            UpdateFileNameExample();
        }

        private void contextMenuStripPages_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolStripMenuItemSelectAll)
            {
                for (int i = 0; i < listPages.Items.Count; i++)
                {
                    listPages.SetItemChecked(i, true);
                }
            }
            if (e.ClickedItem == toolStripMenuItemDeselectAll)
            {
                for (int i = 0; i < listPages.Items.Count; i++)
                {
                    listPages.SetItemChecked(i, false);
                }
            }
        }
    }
}
