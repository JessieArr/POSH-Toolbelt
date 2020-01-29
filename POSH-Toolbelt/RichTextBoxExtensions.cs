using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace POSH_Toolbelt
{
    public static class RichTextBoxExtensions
    {
        public static void AddColoredText(this RichTextBox box, string text, SolidColorBrush color)
        {
            var para = new Paragraph();
            para.Margin = new Thickness(0);
            var textRange = new TextRange(para.ContentEnd, para.ContentEnd);
            textRange.Text = text;
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            box.Document.Blocks.Add(para);
        }
    }
}
