using System.Linq;
using UnityEngine.Localization;

namespace Game.Managers
{
    public static class LocalizedStringHelpers
    {
        public static TextData GetTextData(this LocalizedString localizedString)
        {
            return new TextData(localizedString.GetLocalizedString());
        }

        public static TextData GetTextData(this LocalizedString localizedString, params TextData[] data)
        {
            string[] textData = data.Select(x => x.Text).ToArray();
            string[] shadowData = data.Select(x => x.Shadow).ToArray();

            string text = localizedString.GetLocalizedString(textData);
            string shadow = localizedString.GetLocalizedString(shadowData);

            return new TextData(text, shadow);
        }

        public static TextData GetTextData(this LocalizedString localizedString, object obj, TextData data)
        {
            string objText = obj.ToString();
            string text = localizedString.GetLocalizedString(objText, data.Text);
            string shadow = localizedString.GetLocalizedString(objText, data.Shadow);

            return new TextData(
                text,
                shadow);
        }

        public static TextData GetTextData(this LocalizedString localizedString, object data)
        {
            string text = localizedString.GetLocalizedString(data);
            string shadow = localizedString.GetLocalizedString(data);

            return new TextData(
                text,
                shadow);
        }
    }
}
