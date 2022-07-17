using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    //[SerializeField]
    //public struct WindowEntry
    //{
    //    public string name;
    //    public GameObject window;
    //}

    public GameObject[] _windows;
    private Dictionary<string, WindowBase> _windowsControllers = new Dictionary<string, WindowBase>();

    public static WindowManager instance = null; // ��������� �������

    // Start is called before the first frame update
    void Start()
    {
        // ������, ��������� ������������� ����������
        if (instance == null)
        { // ��������� ��������� ��� ������
            instance = this; // ������ ������ �� ��������� �������
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else 
        { // ��������� ������� ��� ���������� �� �����
            Destroy(gameObject); // ������� ������
        }
    }

    void Init()
    {
        foreach (var item in _windows)
        {
            _windowsControllers[item.name] = new WindowBase(item.name);
        }
    }

    public void OpenWindow(string name)
    {
        if (_windowsControllers.ContainsKey(name))
        {
            _windowsControllers[name].Open();
        }
    }

    public void CloseWindow(string name)
    {
        if (_windowsControllers.ContainsKey(name))
        {
            _windowsControllers[name].Close();
        }
    }

    public WindowBase GetWindowById(string id)
    {
        if (_windowsControllers.ContainsKey(id))
        {
            return _windowsControllers[id];
        }
        else
        {
            return null;
        }
    }
}
