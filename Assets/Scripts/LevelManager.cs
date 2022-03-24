using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpecialCell
{
    public ECellType celltype;
    public Vector2 pos;
    
}


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [Header("基本设置")]
    public const float CELLSIZE = 1f;
    public int levelwidth, levellength;
    private List<List<Vector3>> cellPoses;       //访问对应（x，y）方块位置 -> cellPoses[x][y];
    private List<List<GameObject>> cellList;
    public Vector2 finalPos;
    [Header("特殊方块")] 
    public List<SpecialCell> SpecialCells;

    

    [Header("预制体")] 
    public GameObject playerPrefab;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject lockPrefab;

    [Header("关卡特殊物品")] 
    public GameObject Key;
    public GameObject Lock;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
            cellList = new List<List<GameObject>>();
        cellPoses = new List<List<Vector3>>();
        var position = transform.position;
        var ld = new Vector3(position.x - (levelwidth * CELLSIZE) / 2f, position.y, position.z - (levellength * CELLSIZE) / 2f);
        for (int i = 0; i < levelwidth; i++)
        {
            var tempPoses = new List<Vector3>();
            for (int j = 0; j < levellength; j++)
            {
                var tar = ld + new Vector3((i+1/2f) * CELLSIZE,0f,(j+1/2f) * CELLSIZE);
                tempPoses.Add(tar);
            }
            cellPoses.Add(tempPoses);
        }
        InitMap();
    }

    private void InitMap()
    {
        foreach (var list in cellPoses)
        {
            var objlist = new List<GameObject>();
            foreach (var pos in list)
            {
                var newPos = new Vector3(pos.x, -1.5f, pos.z);
                var obj = Instantiate(levelPrefab,newPos,Quaternion.identity,this.transform);
                obj.GetComponent<IslandCell>().CellType = ECellType.Null;
                objlist.Add(obj);
            }
            cellList.Add(objlist);
        }

        foreach (var sc in SpecialCells)
        {
            UpdateCell(sc.pos,sc.celltype);
        }
    }
    

    public Vector3 GetCellPos(Vector2 pos)
    {
        return cellPoses[(int)pos.x][(int)pos.y];
    }

    public ECellType GetCellType(Vector2 pos)
    {
        return cellList[(int) pos.x][(int) pos.y].GetComponent<IslandCell>().CellType;
    }

    public GameObject GetCellObject(Vector2 pos)
    {
        return cellList[(int) pos.x][(int) pos.y];
    }

    
    public void UpdateCell(GameObject obj, ECellType type)
    {
        obj.GetComponent<IslandCell>().CellType = type;
        
        switch (type)
        {
            case ECellType.Oneoff_placeble :
                obj.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            
            case ECellType.Perma_placeble :
                obj.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
                        
        }
    }
    public void UpdateCell(Vector2 pos, ECellType type)
    {
        cellList[(int) pos.x][(int) pos.y].GetComponent<IslandCell>().CellType = type;
        
        switch (type)
        {
            case ECellType.Oneoff_placeble :
                cellList[(int) pos.x][(int) pos.y].GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            
            case ECellType.Start:
                var player = Instantiate(playerPrefab, GetCellPos(pos) + new Vector3(0,0.3f,0), Quaternion.identity);
                player.GetComponent<PlayerMove>().playerPos = pos;
                SetCellColor(pos,Color.yellow);
                break;
                
            case ECellType.Perma:
                SetCellColor(pos,Color.yellow);
                break;
                      
            case ECellType.Null:
                SetCellColor(pos,Color.gray);
                break;
            
            case ECellType.Final:
                SetCellColor(pos,Color.red);
                finalPos = pos;
                break;
            
            case ECellType.Obstacle:
                SetCellColor(pos,Color.magenta);
                break;
            
            case ECellType.Key:
                Key = Instantiate(keyPrefab, GetCellPos(pos) + new Vector3(0,0.3f,0), Quaternion.identity,GetCellObject(pos).transform);
                SetCellColor(pos,Color.yellow);
                break;
            
            case ECellType.Lock:
                Lock = Instantiate(lockPrefab, GetCellPos(pos) + new Vector3(0,0.3f,0), Quaternion.identity,GetCellObject(pos).transform);
                SetCellColor(pos,Color.yellow);
                break;
        }
    }

    public void SetCellColor(Vector2 pos,Color color)
    {
        cellList[(int) pos.x][(int) pos.y].GetComponent<MeshRenderer>().material.color = color;
    }
    public void SetCellColor(Vector2Int pos,Color color)
    {
        cellList[pos.x][pos.y].GetComponent<MeshRenderer>().material.color = color;
    }
    

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireCube(position,new Vector3(levelwidth * CELLSIZE, 0f, levellength * CELLSIZE));
        var ld = new Vector3(position.x - (levelwidth * CELLSIZE) / 2, position.y, position.z - (levellength * CELLSIZE) / 2);
        for (int i = 1; i < levelwidth; i++)
        {
            
            Gizmos.DrawLine(ld + new Vector3(i * CELLSIZE,0,0),ld + new Vector3(i * CELLSIZE,0,levellength * CELLSIZE));
            
        }
        
        for (int i = 1; i < levellength; i++)
        {
            
            Gizmos.DrawLine(ld + new Vector3(0,0,i * CELLSIZE),ld + new Vector3(levelwidth * CELLSIZE,0,i * CELLSIZE));
            
        }

        
    }
}
