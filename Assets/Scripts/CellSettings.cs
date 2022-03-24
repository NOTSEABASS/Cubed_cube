using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CellColor
{
    public ECellType type;
    public Color color;
}
[CreateAssetMenu(menuName="Cell/Cell Settings")]
public class CellSettings : ScriptableObject
{
    public Mesh CellMesh;
    public Material CellMat;
    [SerializeField]private List<CellColor> CellColors;

    public Dictionary<ECellType, Color> CellColorDic;

    private void Awake()
    {
        foreach (var cellColor in CellColors)
        {
            CellColorDic.Add(cellColor.type,cellColor.color);
        }
    }
}
