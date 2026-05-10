using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Type
{
    meat,
    vegetable,
    other,
}

[CreateAssetMenu()]
public class KitchenObjectSo : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public new string name;
    public E_Type type;

    public float height;
    public int priority;
}
