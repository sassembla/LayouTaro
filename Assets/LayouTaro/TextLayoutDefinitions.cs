
namespace UILayouTaro
{
    /*
        テキストレイアウトに必要なやつ
    */
    public enum TextLayoutStatus
    {
        HeadAndSingle,
        HeadAndMulti,
        NotHeadAndSingle,
        NotHeadAndMulti
    }

    public static class TextLayoutDefinitions
    {
        public static TextLayoutStatus GetTextLayoutStatus(bool isHeadOfLine, bool isMultiLined)
        {
            if (isHeadOfLine && isMultiLined)
            {
                return TextLayoutStatus.HeadAndMulti;
            }
            else if (isHeadOfLine && !isMultiLined)
            {
                return TextLayoutStatus.HeadAndSingle;
            }
            else if (!isHeadOfLine && !isMultiLined)
            {
                return TextLayoutStatus.NotHeadAndSingle;
            }
            return TextLayoutStatus.NotHeadAndMulti;
        }
    }
}