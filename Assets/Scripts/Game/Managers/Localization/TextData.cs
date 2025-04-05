using Framework.Definitions;
using Framework.Extensions;
using UnityEngine;

namespace Game.Managers
{
    public class TextData
    {
        public string Text { get; set; }

        public string Shadow { get; set; }

        public TextData()
        {
        }

        public TextData(params TextData[] textData)
        {
            int count = textData?.Length ?? 0;
            for (int i = 0; i < count; i++)
            {
                this.Text += textData[i].Text;
                this.Shadow += textData[i].Shadow;
            }
        }

        public TextData(string textAndShadow)
        {
            this.Text = textAndShadow;
            this.Shadow = textAndShadow;
        }

        public TextData(string text, string shadow)
        {
            this.Text = text;
            this.Shadow = shadow;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(this.Text);
        }

        public TextData Append(string str)
        {
            this.Text += str;
            this.Shadow += str;

            return this;
        }

        public TextData Append(TextData textData)
        {
            this.Text += textData.Text;
            this.Shadow += textData.Shadow;

            return this;
        }

        public TextData AppendInNewLineIfNotEmpty(string str)
        {
            return this.Append(new(str), prependStringIfNotEmpty: "\n");
        }

        public TextData AppendInNewLineIfNotEmpty(TextData textData)
        {
            return this.Append(textData, prependStringIfNotEmpty: "\n");
        }

        public TextData Append(TextData textData, string prependStringIfNotEmpty)
        {
            if (!this.IsEmpty())
            {
                this.Text += prependStringIfNotEmpty;
                this.Shadow += prependStringIfNotEmpty;
            }

            this.Text += textData.Text;
            this.Shadow += textData.Shadow;

            return this;
        }

        public void Clear()
        {
            this.Text = string.Empty;
            this.Shadow = string.Empty;
        }

        public TextData PadLeft(int width)
        {
            this.Text = this.Text.PadLeft(width);
            this.Shadow = this.Shadow.PadLeft(width);

            return this;
        }

        public TextData PadRight(int width)
        {
            this.Text = this.Text.PadRight(width);
            this.Shadow = this.Shadow.PadRight(width);

            return this;
        }

        public TextData Substring(int startIndex, int length)
        {
            this.Text = this.Text.Substring(startIndex, length);
            this.Shadow = this.Shadow.Substring(startIndex, length);

            return this;
        }

        public TextData Colorize(ColorDefinition colorDefinition)
        {
            return this.Colorize(colorDefinition.Color);
        }

        public TextData Colorize(Color color)
        {
            this.Text = this.Text.Colorize(color);
            this.Shadow = this.Shadow.Colorize(ColorHelpers.GetShadowColor(color));

            return this;
        }
    }
}
