using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UILineConnector : MonoBehaviour
{
    public UILineRenderer linePrefab;
    public RectTransform lineParent;
    public UnityEvent OnCorrectConnection;
    public UnityEvent OnWrongConnection;
    public UILineRenderer CreateTempLine()
    {
        UILineRenderer line = Instantiate(linePrefab, lineParent);

        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;   // 🔥 VERY IMPORTANT
        rt.localPosition = Vector3.zero;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        return line;
    }

    public void TryCompleteConnection(Dot orange, UILineRenderer line, PointerEventData eventData, Vector2 startPos)
    {
        GameObject hit = eventData.pointerEnter;

        if (hit == null)
        {
            Destroy(line.gameObject);
            return;
        }

        Dot blue = hit.GetComponent<Dot>();

        if (blue != null && !blue.isOrange && !blue.isConnected && blue.id == orange.id)
        {
            Vector2 endPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                lineParent,
                RectTransformUtility.WorldToScreenPoint(null, blue.transform.position),
                null,
                out endPos
            );
            OnCorrectConnection?.Invoke();
            Vector2 controlPoint = (startPos + endPos) / 2;

// Add curve offset (this creates the arc)
            controlPoint.y += 100f; // tweak this value for curve strength

            Vector2 dir = (endPos - startPos).normalized;
            float distance = Vector2.Distance(startPos, endPos);

// Control points (smooth curve)
            Vector2 control1 = startPos + new Vector2(distance * 0.5f, 0);
            Vector2 control2 = endPos - new Vector2(distance * 0.5f, 0);

            line.Points = new Vector2[]
            {
                startPos,
                control1,
                control2,
                endPos
            };

            orange.isConnected = true;
            blue.isConnected = true;
            orange.BlinkGreen();
            blue.BlinkGreen();
        }
        else
        {
            // 🔴 Always blink orange
            orange.BlinkRed();

             blue = eventData.pointerEnter != null 
                ? eventData.pointerEnter.GetComponent<Dot>() 
                : null;

            if (blue != null && !blue.isOrange)
            {
                blue.BlinkRed();
            }
            OnWrongConnection?.Invoke();
            Destroy(line.gameObject);
        }
    }
    
    
}