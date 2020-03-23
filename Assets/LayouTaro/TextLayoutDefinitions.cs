
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
        NotHeadAndMulti,
        NotHeadAndOutOfView
    }

    public static class TextLayoutDefinitions
    {
        public static TextLayoutStatus GetTextLayoutStatus(bool isHeadOfLine, bool isMultiLined, bool isLayoutedOutOfView)
        {
            if (isHeadOfLine && isMultiLined)
            {
                return TextLayoutStatus.HeadAndMulti;
            }
            else if (isHeadOfLine && !isMultiLined)
            {
                return TextLayoutStatus.HeadAndSingle;
            }
            else if (!isHeadOfLine && isLayoutedOutOfView)
            {
                return TextLayoutStatus.NotHeadAndOutOfView;
            }
            else if (!isHeadOfLine && !isMultiLined)
            {
                return TextLayoutStatus.NotHeadAndSingle;
            }

            return TextLayoutStatus.NotHeadAndMulti;
        }
    }
}