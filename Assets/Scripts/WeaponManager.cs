using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Serializable]
    public struct Weapon
    {
        public string name;
        public GameObject prefab;
    }
    public Weapon[] weapons;

    public GameObject FindWeaponByName(string name)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.name == name)
            {
                return weapon.prefab;
            }
        }
        return null;
    }
}
