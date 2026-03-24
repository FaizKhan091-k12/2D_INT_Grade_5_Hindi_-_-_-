using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // 👈 important
using UnityEngine.UI.Extensions;
using System.Collections;

public class Dot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int id;
    public bool isConnected = false;
    public bool isOrange;

    public UILineConnector connector;

    private UILineRenderer currentLine;
    private Vector2 fixedStartPos;

    // 👇 Works for Procedural Image (inherits from Graphic)
    private Graphic graphic;
    private Color originalColor;

    void Awake()
    {
        graphic = GetComponent<Graphic>(); // Procedural Image भी Graphic है
        originalColor = graphic.color;
    }

    // ---------------- DRAG LOGIC ----------------

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isOrange || isConnected) return;

        currentLine = connector.CreateTempLine();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            connector.lineParent,
            RectTransformUtility.WorldToScreenPoint(null, transform.position),
            null,
            out fixedStartPos
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentLine == null) return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            connector.lineParent,
            eventData.position,
            eventData.pressEventCamera,
            out mousePos
        );

        currentLine.Points = new Vector2[] { fixedStartPos, mousePos };
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentLine == null) return;

        connector.TryCompleteConnection(this, currentLine, eventData, fixedStartPos);
        currentLine = null;
    }

    // ---------------- FEEDBACK SYSTEM ----------------

    public void BlinkGreen()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkRoutine(Color.green, true));
    }

    public void BlinkRed()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkRoutine(Color.red, false));
    }

    IEnumerator BlinkRoutine(Color targetColor, bool stayGreen)
    {
        Color baseColor = graphic.color; // 👈 current color (important!)

        for (int i = 0; i < 3; i++)
        {
            graphic.color = targetColor;
            yield return new WaitForSeconds(0.15f);

            graphic.color = baseColor;
            yield return new WaitForSeconds(0.15f);
        }

        if (stayGreen)
        {
            graphic.color = Color.green;
        }
        else
        {
            graphic.color = originalColor; // always reset properly
        }
    }
}