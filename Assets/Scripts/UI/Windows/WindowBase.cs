using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//управляет окном, анимационно открывает/закрывает окно, вызывает колбеки
//должен висеть на UI окне 
public class WindowBase
{
    public enum WindowState
    {
        Closed = 1,
        OpenPlaying,
        Opened,
        HidePlaying,
        Hidden,
        ClosePlaying,
        Destroying
    }
    public delegate void OnAction();

    public string openAnimationId = "ShowWindowAnim";
    public string closeAnimationId = "CloseWindowAnim";

    public string _prefab;
    private GameObject _gameObject;
    public AnimationClip _clip;

    public WindowState _windowState = WindowState.Closed;

    public OnAction _onBeforeOpen = null;
    public OnAction _onAfterOpen = null;
    public OnAction _onBeforeClose = null;
    public OnAction _onAfterClose = null;
    public OnAction _onBeforeHide = null;
    public OnAction _onAfterHide = null;
    public OnAction _onBeforeShow = null;
    public OnAction _onAfterShow = null;

    public WindowBase(string prefab)
    {
        _prefab = prefab;
    }

    void Deinit()
    {
        _gameObject = null;
        _onBeforeOpen = null;
        _onAfterOpen = null;
        _onBeforeClose = null;
        _onAfterClose = null;
    }

    public void Open()
    {
        _windowState = WindowState.OpenPlaying;
        if (_onBeforeOpen != null)
        {
            _onBeforeOpen();
        }
        
        CreateWindow();
    }

    public void OnOpened()
    {
        _windowState = WindowState.Opened;
        if (_onAfterOpen != null)
        {
            _onAfterOpen();
        }
    }

    public void OnClosed()
    {
        DestroyWindow();

        if (_onAfterClose != null)
        {
            _onAfterClose();
        }

        _windowState = WindowState.Closed;

        Deinit();
    }

    public void Close()
    {
        if (_onBeforeClose != null)
        {
            _onBeforeClose();
        }
        _windowState = WindowState.ClosePlaying;

        AnimationClip clip = Resources.Load<AnimationClip>("Animations/" + closeAnimationId);

        var animation = _gameObject.AddComponent<Animation>();
        animation.AddClip(clip, closeAnimationId);
        animation.Play(closeAnimationId);
    }

    public void Show()
    {
        if (_gameObject)
        {
            if (_onBeforeShow != null)
            {
                _onBeforeShow();
            }

            _gameObject.SetActive(true);

            if (_onAfterShow != null)
            {
                _onBeforeShow();
            }
        }
    }

    public void Hide()
    {
        if (_gameObject)
        {
            if (_onBeforeHide != null)
            {
                _onBeforeHide();
            }

            _gameObject.SetActive(false);

            if (_onAfterHide != null)
            {
                _onAfterHide();
            }
        }
    }

    public void CallBeforeOpen(OnAction callback)
    {
        _onBeforeOpen += callback;
    }

    public void CallAfterOpen(OnAction callback)
    {
        _onAfterOpen += callback;
    }

    public void CallBeforeClose(OnAction callback)
    {
        _onBeforeClose += callback;
    }

    public void CallAfterClose(OnAction callback)
    {
        _onAfterClose += callback;
    }

    void CreateWindow()
    {
        CreateGameObject();
    }

    void DestroyWindow()
    {
        GameObject.Destroy(_gameObject);
    }

    void CreateGameObject()
    {
        if (_gameObject)
        {
            GameObject.Destroy(_gameObject);
            _gameObject = null;
        }

        var path = "Prefabs/UI/Windows/" + _prefab;
        var go = Resources.Load<GameObject>(path);
        _gameObject = GameObject.Instantiate(go);

        var windowBeh = _gameObject.GetComponent<WindowBaseBehaviour>();
        windowBeh.Init(this);
    }
}
