using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Visio = Microsoft.Office.Interop.Visio;

namespace ExportPages
{
    public partial class ThisAddIn
    {
        Ribbon ribbon = null;

        /// <summary>
        /// A simple command
        /// </summary>
        public void ExecuteExport()
        {
            try
            {
                //Application.Settings.RasterExportDataFormat
                //Visio.Document document = Globals.ThisAddIn.Application.ActiveDocument;
                //if (document != null)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    foreach (Visio.Page page in document.Pages)
                //    {
                //        sb.AppendLine(page.Name);
                //    }
                //    MessageBox.Show(sb.ToString(), document.FullName);
                //}
                //Application.Settings.SetRasterExportResolution(Visio.VisRasterExportResolution)
                using (FormExport formExport = new FormExport(Application))
                {
                    if (formExport.ShowDialog() == DialogResult.OK)
                    {
                        int exported = 0;
                        Visio.Document document = Globals.ThisAddIn.Application.ActiveDocument;
                        if (document != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (string pageName in formExport.SelectedPages)
                            {
                                Visio.Page page = document.Pages[pageName];
                                PageInfo pageInfo = new PageInfo(page);
                                string fileName = formExport.FileNameTemplate.FormatWith(pageInfo);
                                string dir = System.IO.Path.GetDirectoryName(fileName);
                                if (!String.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                                    System.IO.Directory.CreateDirectory(dir);
                                page.Export(fileName);
                                exported++;
                            }
                        }
                        MessageBox.Show(String.Format(Properties.Resources.ExportDone, exported),
                            Properties.Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.Trim(' ', '\n', 'r'),
                    Properties.Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            if (ribbon == null)
                ribbon = new Ribbon(this);
            return ribbon;
        }

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            // Get user interface language
            int lcid = Application.LanguageSettings.LanguageID[Office.MsoAppLanguageID.msoLanguageIDUI];
            if (Properties.Resources.Culture == null || Properties.Resources.Culture.LCID != lcid)
            {
                Properties.Resources.Culture = new CultureInfo(lcid);
                System.Diagnostics.Debug.WriteLine(Properties.Resources.Culture.Name, "User interface language");
                ribbon.Invalidate();
            }
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }

    }
}
