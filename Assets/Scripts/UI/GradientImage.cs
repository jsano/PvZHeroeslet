using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI Graphic that renders a linear two-color gradient.
/// Attach to a UI GameObject (RectTransform) to render. Set colors from inspector or via script.
/// </summary>
[AddComponentMenu("UI/Gradient Image")]
[RequireComponent(typeof(CanvasRenderer))]
[ExecuteAlways]
public class GradientImage : MaskableGraphic
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        Diagonal
    }

    [SerializeField] private Color m_Color1 = Color.white;
    [SerializeField] private Color m_Color2 = Color.black;
    [SerializeField] private Direction m_Direction = Direction.Horizontal;

    public Color Color1
    {
        get => m_Color1;
        set
        {
            if (m_Color1 == value) return;
            m_Color1 = value;
            SetVerticesDirty();
        }
    }

    public Color Color2
    {
        get => m_Color2;
        set
        {
            if (m_Color2 == value) return;
            m_Color2 = value;
            SetVerticesDirty();
        }
    }

    public Direction GradientDirection
    {
        get => m_Direction;
        set
        {
            if (m_Direction == value) return;
            m_Direction = value;
            SetVerticesDirty();
        }
    }

    /// <summary>
    /// Convenience method for scripts.
    /// </summary>
    public void SetColors(Color leftOrBottom, Color rightOrTop, Direction direction = Direction.Horizontal)
    {
        m_Color1 = leftOrBottom;
        m_Color2 = rightOrTop;
        m_Direction = direction;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect r = rectTransform.rect;
        Vector3 bl = new Vector3(r.xMin, r.yMin);
        Vector3 tl = new Vector3(r.xMin, r.yMax);
        Vector3 tr = new Vector3(r.xMax, r.yMax);
        Vector3 br = new Vector3(r.xMax, r.yMin);

        Color32 c1 = m_Color1;
        Color32 c2 = m_Color2;

        if (m_Direction == Direction.Horizontal)
        {
            // left = c1, right = c2
            vh.AddVert(bl, c1, new Vector2(0, 0));
            vh.AddVert(tl, c1, new Vector2(0, 1));
            vh.AddVert(tr, c2, new Vector2(1, 1));
            vh.AddVert(br, c2, new Vector2(1, 0));
        }
        else if (m_Direction == Direction.Vertical)
        {
            // bottom = c2, top = c1
            vh.AddVert(bl, c2, new Vector2(0, 0));
            vh.AddVert(tl, c1, new Vector2(0, 1));
            vh.AddVert(tr, c1, new Vector2(1, 1));
            vh.AddVert(br, c2, new Vector2(1, 0));
        }
        else // Diagonal: bottom-left (c1) -> top-right (c2)
        {
            // interpolation factor for a vertex at normalized coords (x,y) is (x + y) / 2
            // bl (0,0) -> t=0, tl (0,1) -> t=0.5, br (1,0) -> t=0.5, tr (1,1) -> t=1
            Color tlColor = Color.Lerp(c1, c2, 0.5f);
            Color brColor = Color.Lerp(c1, c2, 0.5f);

            vh.AddVert(bl, c1, new Vector2(0, 0));
            vh.AddVert(tl, tlColor, new Vector2(0, 1));
            vh.AddVert(tr, c2, new Vector2(1, 1));
            vh.AddVert(br, brColor, new Vector2(1, 0));
        }

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetVerticesDirty();
    }
#endif
}