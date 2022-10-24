using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExportPages
{
    class PageInfo
    {
        //{Path}\{FileName}\{PageName}.png
        public string Path { get; set; }
        public string FileName { get; set; }
        public int PageNumber { get; set; }
        public string PageName { get; set; }

        public PageInfo(IVPage page)
        {
            Path = page.Document.Path.TrimEnd('\\', '/', ' ');
            FileName = System.IO.Path.GetFileNameWithoutExtension(page.Document.Name).Trim();
            PageNumber = page.Index;
            PageName = page.Name;
        }
    }
}
