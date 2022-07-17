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

    public static WindowManager instance = null; // Экземпляр объекта

    // Start is called before the first frame update
    void Start()
    {
        // Теперь, проверяем существование экземпляра
        if (instance == null)
        { // Экземпляр менеджера был найден
            instance = this; // Задаем ссылку на экземпляр объекта
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else 
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
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
