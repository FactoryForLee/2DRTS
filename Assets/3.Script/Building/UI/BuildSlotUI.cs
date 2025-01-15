using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Define;
using UnityEngine.EventSystems;

public class BuildSlotUI : MonoBehaviour, IOwnKey<int>, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    public Image Icon => icon;
    [SerializeField] private float clickedScale;
    [SerializeField] private int prefabKey;
    public UnityEvent OnBtnDown;
    public UnityEvent OnBtnUp;

    public int PrefabKey
    {
        get => prefabKey;
        set => prefabKey = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.selectedObject != gameObject) return;
        OnBtnDown?.Invoke();
        ChangeScale();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.selectedObject != gameObject) return;
        ResetToNormal();
        OnBtnUp?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.selectedObject != gameObject) return;
        ResetToNormal();
    }

    private void ChangeScale()
    {
        Vector3 newScale = icon.transform.localScale;
        newScale.x = clickedScale;
        newScale.y = clickedScale;
        icon.transform.localScale = newScale;
    }

    private void ResetToNormal()
    {
        Vector3 newScale = icon.transform.localScale;
        newScale.x = 1.0f;
        newScale.y = 1.0f;
        icon.transform.localScale = newScale;
    }
}