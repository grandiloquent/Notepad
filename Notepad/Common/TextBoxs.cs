using System;
using System.Windows.Forms;
using System.Text;

namespace  Common
{
	
	public static class TextBoxs
	{
		    public static void Format(this TextBox textBox)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < textBox.Lines.Length; i++)
            {
            	if (string.IsNullOrWhiteSpace(textBox.Lines[i]))
                {
            		while (i + 1 < textBox.Lines.Length && string.IsNullOrWhiteSpace( textBox.Lines[i + 1]))
                    {
                        i++;
                    }
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(textBox.Lines[i]);
                }
            }
            textBox.Text = sb.ToString();

        }
		 public static void SelectLine(this TextBox textBox, bool trimEnd = false)
        {

            var start = textBox.SelectionStart;

            var length = textBox.Text.Length;
            var end = textBox.SelectionStart;
            var value = textBox.Text;
            while (start - 1 > -1 && value[start - 1] != '\n')
            {
                start--;
            }
            while (end + 1 < length && value[end + 1] != '\n')
            {
                end++;
            }
            if (trimEnd)
            {
            	if(start==value.Length){
            		start--;
            	}
                while (char.IsWhiteSpace(value[start]))
                {
                    start++;
                }
                if(end==value.Length)end--;
                while (char.IsWhiteSpace(value[end]))
                {
                    end--;
                }
            }

            textBox.SelectionStart = start;
            if (end > start)
                textBox.SelectionLength = end - start + 1;




        }
	}
}
