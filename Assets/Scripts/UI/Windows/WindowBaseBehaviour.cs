using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBaseBehaviour : MonoBehaviour
{
    private WindowBase _wnd;
    private RectTransform rt;
    private GameObject _childObject;
    private RectTransform _rt;
    public void OnOpened()
    {
        _wnd.OnOpened();
    }

    private void Awake()
    {
        _childObject = transform.GetChild(0).gameObject;
        _rt = _childObject.GetComponent<RectTransform>();
        _rt.localScale = Vector3.zero;
    }

    private void Start()
    {
        _rt.LeanScale(new Vector3(5, 5, 5), 1.0f).setEaseInOutBack().setOnComplete(OnOpened);
    }
    public void Init(WindowBase wnd)
    {
        _wnd = wnd;
    }

    private void Update()
    {
      
    }

    public void Open()
    {
        _wnd.Open();
    }

    public void Close()
    {
        _wnd.Close();
    }

    public void Hide()
    {
        _wnd.Hide();
    }

    public void Show()
    {
        _wnd.Show();
    }
}
