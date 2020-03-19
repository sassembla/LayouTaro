using UnityEngine;
using UnityEngine.UI;

public enum LayoutElementType
{
    Box,
    Image,
    Text,
    Button
}


public interface ILayoutElement
{
    LayoutElementType GetLayoutElementType();
}

public interface ILayoutableImage
{
    Vector2 RectSize();
}

public interface ILayoutableText
{
    string Text();
}