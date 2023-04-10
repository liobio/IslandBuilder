using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashLoad : MonoBehaviour
{
    public string LoadSceneName;
    public Text loadingText;
    public Transform Parent;
    public Transform SliderDay;
    public Transform SliderNight;
    public Button clickAny;
    RectTransform currentSlider;
    private AsyncOperation asyncLoad;
    // Start is called before the first frame update

    void Start()
    {
        clickAny.gameObject.SetActive(false);
        currentSlider = Instantiate(SliderDay, Parent).GetComponent<RectTransform>();
        currentSlider.gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine(AsyncLoading());
    }

    IEnumerator AsyncLoading()
    {
        yield return new WaitForEndOfFrame();
        asyncLoad = SceneManager.LoadSceneAsync(LoadSceneName);
        //阻止当加载完成自动切换
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            targetValue = asyncLoad.progress;
            if (asyncLoad.progress >= 0.9f)
            {
                targetValue = 1.0f;
                currentSlider.anchoredPosition = Vector2.Lerp(currentSlider.anchoredPosition, new Vector3(targetValue * 1000, 0), Time.deltaTime * 1);
                //如果当前进度条value和目标值接近 设置进度条value为目标值 
                if (Mathf.Abs(currentSlider.anchoredPosition.x / 1000 - targetValue) < 0.01f)
                {
                    currentSlider.anchoredPosition = new Vector2(targetValue * 1000, 0);
                }
                if (currentSlider.anchoredPosition.x == 1000)
                {
                    loadingText.text = "继续";
                    clickAny.gameObject.SetActive(true);
                }
            }
            else
            {
                if (targetValue * 1000 != currentSlider.anchoredPosition.x)
                {
                    currentSlider.anchoredPosition = Vector2.Lerp(currentSlider.anchoredPosition, new Vector3(targetValue * 1000, 0), Time.deltaTime * 1);
                    //如果当前进度条value和目标值接近 设置进度条value为目标值 
                    if (Mathf.Abs(currentSlider.anchoredPosition.x / 1000 - targetValue) < 0.01f)
                    {
                        currentSlider.anchoredPosition = new Vector2(targetValue * 1000, 0);
                    }
                }
                loadingText.text = ((int)(currentSlider.anchoredPosition.x / 10)).ToString() + "%";
            }
            yield return null;

        }

    }

    float targetValue;
    // Update is called once per frame
    private void Update()
    {
        //if (asyncLoad.progress >= 0.9f)
        //{
        //    //progress的值最大为0.9 
        //    //targetValue = 1.0f;

        //    BeginMoveSlider();
        //}


    }
    //void BeginMoveSlider()
    //{
    //    if (targetValue >= 1.0f)
    //    { //允许异步加载完毕后自动切换场景 
    //        loadingText.text = "点击任意处继续";
    //        clickAny.gameObject.SetActive(true);
    //        return;
    //    }
    //    targetValue = targetValue + 0.001f; ;

    //    if (targetValue * 1000 != currentSlider.anchoredPosition.x)
    //    {
    //        currentSlider.anchoredPosition = Vector2.Lerp(currentSlider.anchoredPosition, new Vector3(targetValue * 1000, 0), Time.deltaTime * 1);
    //        //如果当前进度条value和目标值接近 设置进度条value为目标值 
    //        if (Mathf.Abs(currentSlider.anchoredPosition.x / 1000 - targetValue) < 0.01f)
    //        {
    //            currentSlider.anchoredPosition = new Vector2(targetValue * 1000, 0);
    //        }
    //    }
    //    loadingText.text = ((int)(currentSlider.anchoredPosition.x / 10)).ToString() + "%";
    //    //当进度条读取到百分之百时允许场景切换 

    //}
    public void ClickAny()
    {
        asyncLoad.allowSceneActivation = true;
    }
}
