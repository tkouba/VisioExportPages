using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExportPages
{
    class LabelSeparator : Label
    {
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoSize { get => false; set { } }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ContentAlignment TextAlign { get => ContentAlignment.BottomLeft; set { } }

        public LabelSeparator()
        {
            base.AutoSize = false;
            base.TextAlign = ContentAlignment.BottomLeft;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            SizeF textSize = e.Graphics.MeasureString(Text, Font);
            //using (Brush brush = new SolidBrush(ForeColor))
            //{
            //    e.Graphics.DrawString(Text, Font, brush, 0, (ClientSize.Height - textSize.Height) / 2);
            //}

            Color back = this.BackColor;
            Color dark = Color.FromArgb(back.R >> 1, back.G >> 1, back.B >> 1);
            int y = ClientSize.Height - Convert.ToInt32(textSize.Height / 2);
            using (var pen = new Pen(dark))
            {
                e.Graphics.DrawLine(pen, textSize.Width + 3, y, ClientSize.Width, y);
            }
            e.Graphics.DrawLine(Pens.White, textSize.Width + 3, y + 1, ClientSize.Width, y + 1);
        }
    }
}
