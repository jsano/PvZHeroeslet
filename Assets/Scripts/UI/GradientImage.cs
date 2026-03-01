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
    [Header("Optional Sprite")]
    [SerializeField] private Sprite m_Sprite;

    [SerializeField] private Color m_Color1 = Color.white;
    [SerializeField] private Color m_Color2 = Color.black;
    [SerializeField] private Direction m_Direction = Direction.Horizontal;

    public Sprite Sprite
    {
        get => m_Sprite;
        set
        {
            if (m_Sprite == value) return;
            m_Sprite = value;
            SetAllDirty();
        }
    }

    public override Texture mainTexture
    {
        get
        {
            if (m_Sprite != null && m_Sprite.texture != null) return m_Sprite.texture;
            return base.mainTexture;
        }
    }

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

        // Optionally preserve aspect of the sprite (similar to UnityEngine.UI.Image.preserveAspect)
        Rect drawRect = r;
        if (m_Sprite != null)
        {
            // Use sprite rect (in pixels) to compute aspect ratio
            float spriteW = m_Sprite.rect.width;
            float spriteH = m_Sprite.rect.height;
            if (spriteH > 0f && r.height > 0f)
            {
                float spriteAspect = spriteW / spriteH;
                float rectAspect = r.width / r.height;

                if (spriteAspect > rectAspect)
                {
                    // Fit width, adjust height
                    float fitHeight = r.width / spriteAspect;
                    float yOffset = (r.height - fitHeight) * 0.5f;
                    drawRect = new Rect(r.xMin, r.yMin + yOffset, r.width, fitHeight);
                }
                else
                {
                    // Fit height, adjust width
                    float fitWidth = r.height * spriteAspect;
                    float xOffset = (r.width - fitWidth) * 0.5f;
                    drawRect = new Rect(r.xMin + xOffset, r.yMin, fitWidth, r.height);
                }
            }
        }

        Vector3 bl = new Vector3(drawRect.xMin, drawRect.yMin);
        Vector3 tl = new Vector3(drawRect.xMin, drawRect.yMax);
        Vector3 tr = new Vector3(drawRect.xMax, drawRect.yMax);
        Vector3 br = new Vector3(drawRect.xMax, drawRect.yMin);

        // Compute gradient colors per corner
        Color c1 = m_Color1;
        Color c2 = m_Color2;

        Color blColor;
        Color tlColor;
        Color trColor;
        Color brColor;

        if (m_Direction == Direction.Horizontal)
        {
            blColor = c1;
            tlColor = c1;
            trColor = c2;
            brColor = c2;
        }
        else if (m_Direction == Direction.Vertical)
        {
            blColor = c1;
            tlColor = c2;
            trColor = c2;
            brColor = c1;
        }
        else // Diagonal: bottom-left (c1) -> top-right (c2)
        {
            blColor = c1;
            trColor = c2;
            // top-left and bottom-right are midpoints for a smooth diagonal
            var mid = 0.5f;
            tlColor = Color.Lerp(c1, c2, mid);
            brColor = Color.Lerp(c1, c2, mid);
        }

        // Determine UVs. If a sprite is assigned, map to the sprite.textureRect region.
        Vector2 uvBL;
        Vector2 uvTL;
        Vector2 uvTR;
        Vector2 uvBR;

        if (m_Sprite != null && m_Sprite.texture != null)
        {
            Rect texRect = m_Sprite.textureRect; // pixel rect within the texture
            float texW = m_Sprite.texture.width;
            float texH = m_Sprite.texture.height;

            float uMin = texRect.x / texW;
            float vMin = texRect.y / texH;
            float uMax = (texRect.x + texRect.width) / texW;
            float vMax = (texRect.y + texRect.height) / texH;

            // Map corners BL, TL, TR, BR to rect UVs
            uvBL = new Vector2(uMin, vMin);
            uvTL = new Vector2(uMin, vMax);
            uvTR = new Vector2(uMax, vMax);
            uvBR = new Vector2(uMax, vMin);
        }
        else
        {
            // fallback to normalized quad UVs so shader still works with default white texture
            uvBL = new Vector2(0f, 0f);
            uvTL = new Vector2(0f, 1f);
            uvTR = new Vector2(1f, 1f);
            uvBR = new Vector2(1f, 0f);
        }

        // Add vertices (position, color, uv)
        vh.AddVert(bl, blColor, uvBL);
        vh.AddVert(tl, tlColor, uvTL);
        vh.AddVert(tr, trColor, uvTR);
        vh.AddVert(br, brColor, uvBR);

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