using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    public GameObject BottomMenu;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowBottomMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowBottomMenu()
    {
        BottomMenu.GetComponent<UI_Animation>().OpenUI();
    }
}
