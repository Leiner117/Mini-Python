using System;
using System.Drawing;
using System.Windows.Forms;

public class LineNumberedRichTextBox : UserControl
{
    private RichTextBox richTextBox;
    private Panel lineNumberPanel;

    public LineNumberedRichTextBox()
    {
        richTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(255, 30, 30, 30),
            ForeColor = Color.White,
            Font = new Font("Consolas", 16, FontStyle.Bold)
        };

        lineNumberPanel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 50,
            BackColor = Color.FromArgb(255, 30, 30, 30)
        };

        richTextBox.VScroll += (sender, e) => lineNumberPanel.Invalidate();
        richTextBox.TextChanged += (sender, e) => lineNumberPanel.Invalidate();
        richTextBox.FontChanged += (sender, e) => lineNumberPanel.Invalidate();

        lineNumberPanel.Paint += LineNumberPanel_Paint;

        Controls.Add(richTextBox);
        Controls.Add(lineNumberPanel);
    }

    private void LineNumberPanel_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.Clear(lineNumberPanel.BackColor);

        int firstIndex = richTextBox.GetCharIndexFromPosition(new Point(0, 0));
        int firstLine = richTextBox.GetLineFromCharIndex(firstIndex);
        int lastIndex = richTextBox.GetCharIndexFromPosition(new Point(0, richTextBox.ClientSize.Height));
        int lastLine = richTextBox.GetLineFromCharIndex(lastIndex);

        for (int i = firstLine; i <= lastLine + 1; i++)
        {
            Point point = richTextBox.GetPositionFromCharIndex(richTextBox.GetFirstCharIndexFromLine(i));
            e.Graphics.DrawString((i + 1).ToString(), richTextBox.Font, Brushes.White, new PointF(0, point.Y));
        }
    }

    public RichTextBox RichTextBox => richTextBox;
}
