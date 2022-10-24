using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using Visio = Microsoft.Office.Interop.Visio;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace ExportPages
{
    internal enum OutputFormat
    {
        PNG,
        JPG
    }

    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility
    {
        private ThisAddIn addIn;
        private Office.IRibbonUI ribbonUI;

        public Ribbon(ThisAddIn addIn)
        {
            this.addIn = addIn;
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID) => Properties.Resources.Ribbon;

        #endregion

        #region IRibbonUI Members

        public void Invalidate()
        {
            ribbonUI.Invalidate();
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226
        // See list of signatures https://docs.microsoft.com/en-us/previous-versions/office/developer/office-2007/aa722523(v=office.12)
        // see also https://osdn.net/projects/netoffice/scm/svn/tree/head/Source/Visio/
        // https://docs.microsoft.com/en-us/previous-versions/office/developer/office-2010/ee691833(v=office.14)
        // https://docs.microsoft.com/en-us/previous-versions/office/developer/office-2010/ee815851(v=office.14)
        // http://youpresent.co.uk/customising-powerpoint-2016-backstage-view/
        // http://unmanagedvisio.com/localizing-an-office-extension/
        public void OnAction(Office.IRibbonControl control)
        {
            switch (control.Id)
            {
                case "commandExport":
                    addIn.ExecuteExport();
                    break;
                case "commandAbout":
                    (new FormAbout()).ShowDialog();
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show(string.Format(@"Hello from the ribbon, the button ""{0}"" clicked!", control.Id));
                    break;
            }
        }

        public string GetLabel(Office.IRibbonControl control) 
            => Properties.Resources.ResourceManager.GetString($"{control.Id}Label", Properties.Resources.Culture);

        public string GetHelperText(Office.IRibbonControl control)
            => Properties.Resources.ResourceManager.GetString($"{control.Id}HelperText", Properties.Resources.Culture);

        public System.Drawing.Bitmap GetImage(Office.IRibbonControl control) 
            => (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject(control.Id, Properties.Resources.Culture);

        public System.Drawing.Bitmap GetImageBullet(Office.IRibbonControl control)
            => Properties.Resources.Bullet;

        //public Office.BackstageGroupStyle GetStyleFormat(Office.IRibbonControl control)
        //{
        //    if ((control.Id == "groupFormatPNG" && outputFormat == OutputFormat.PNG) ||
        //        (control.Id == "groupFormatJPG" && outputFormat == OutputFormat.JPG))
        //        return Office.BackstageGroupStyle.BackstageGroupStyleWarning;
        //    return Office.BackstageGroupStyle.BackstageGroupStyleNormal;
        //}

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbonUI = ribbonUI;
        }

        #endregion

    }
}
