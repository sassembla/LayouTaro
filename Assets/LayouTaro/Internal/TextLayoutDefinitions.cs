
using TMPro;

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

        public static (bool exist, uint codePoint) TMPro_ChechIfEmojiOrMarkExist(string emojiOrMarkStr)
        {
            uint codePoint = 0;
            for (var i = 0; i < emojiOrMarkStr.Length; i++)
            {
                codePoint = (uint)char.ConvertToUtf32(emojiOrMarkStr, i);

                // indexで切り分けられるようであれば、この時点で判断を行う。
                // 現状では2文字ずつしかsurrogateで追わないため、このブロックに処理がくることはない。
                if (char.IsSurrogatePair(emojiOrMarkStr, i))
                {
                    i++;
                }
            }

            var spriteAsset = TMPro.TMP_Settings.GetSpriteAsset();
            if (-1 < spriteAsset.GetSpriteIndexFromUnicode(codePoint))
            {
                // 絵文字か記号が既存のSpriteAssetに存在する
                return (true, codePoint);
            }

            // fallbackに登録されているSpriteAssetsも見る
            foreach (var sAsset in spriteAsset.fallbackSpriteAssets)
            {
                if (-1 < sAsset.GetSpriteIndexFromUnicode(codePoint))
                {
                    return (true, codePoint);
                }
            }

            // 存在しないのでfalseを返す
            return (false, codePoint);
        }

        public static bool TMPro_ChechIfTextCharacterExist(TMP_FontAsset font, char text)
        {
            if (font.HasCharacter(text))
            {
                // 文字がFont内に存在する
                return true;
            }

            // fallbackに登録されているAssetsも見る
            foreach (var sAsset in font.fallbackFontAssetTable)
            {
                if (sAsset.HasCharacter(text))
                {
                    return true;
                }
            }

            // 存在しないのでfalseを返す
            return false;
        }
    }
}