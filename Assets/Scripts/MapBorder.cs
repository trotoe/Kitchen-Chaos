using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorder : MonoBehaviour
{
    public static MapBorder Instance { get; private set; }

    [SerializeField] private Transform topPoint;
    [SerializeField] private Transform bottomPoint;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Vector3 GetTopPoint()
    {
        return topPoint.position;
    }

    public Vector3 GetBottomPoint()
    {
        return bottomPoint.position;
    }

    public Vector3 GetLeftPoint()
    {
        return leftPoint.position;
    }

    public Vector3 GetRightPoint()
    {
        return rightPoint.position;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
