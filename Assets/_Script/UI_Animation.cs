using System.Collections;

using UnityEngine;
using UnityEngine.UI;


public class UI_Animation : MonoBehaviour
{
    public Vector2 originPos;
    public Vector2 targetPos;
    public float speed;

    RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originPos = rectTransform.anchoredPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        ObserverManager.instance.AddListener("Esc", this.CloseUI);
    }

    // Update is called once per frame
    public IEnumerator MoveTarget()
    {
        while (rectTransform.anchoredPosition != targetPos)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPos, speed * Time.deltaTime);
            yield return null;
        }
    }
    public void OpenUI()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTarget());
    }
    public void CloseUI()
    {
        StopAllCoroutines();
        StartCoroutine(MoveOrigin());
    }
    public IEnumerator MoveOrigin()
    {
        while (rectTransform.anchoredPosition != originPos)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, originPos, speed * Time.deltaTime);
            yield return null;
        }
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        ObserverManager.instance.ReMoveListener("Esc", this.CloseUI);
    }
}
