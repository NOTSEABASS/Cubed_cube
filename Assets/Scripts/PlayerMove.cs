using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

enum EInputDir
{
    Up,
    Down,
    Left,
    Right,
    Null
}
public class PlayerMove : MonoBehaviour
{
    public Vector2 playerPos;
    public bool hasKey;
    private EInputDir InputDir = EInputDir.Null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) InputDir = EInputDir.Right;
        if (Input.GetKeyDown(KeyCode.A)) InputDir = EInputDir.Left;
        if (Input.GetKeyDown(KeyCode.S)) InputDir = EInputDir.Down;
        if (Input.GetKeyDown(KeyCode.W)) InputDir = EInputDir.Up;

        switch (InputDir)
        {
            case EInputDir.Up:
                var newPos0 = playerPos + new Vector2(0, 1);
                if (newPos0.x < LevelManager.Instance.levelwidth && newPos0.x >= 0
                                                                 && newPos0.y < LevelManager.Instance.levellength &&
                                                                 newPos0.y >= 0)
                {
                    if (CanStepOn(newPos0))
                    {
                        UpdateOriginCell();
                        transform.DOMove(LevelManager.Instance.GetCellPos(newPos0), 0.2f);
                        playerPos = newPos0;
                    }
                }
                
                break;
            
            case EInputDir.Down:
                var newPos1 = playerPos + new Vector2(0, -1);
                if (newPos1.x < LevelManager.Instance.levelwidth && newPos1.x >= 0
                                                                 && newPos1.y < LevelManager.Instance.levellength &&
                                                                 newPos1.y >= 0)
                {
                    if (CanStepOn(newPos1))
                    {
                        UpdateOriginCell();
                        transform.DOMove(LevelManager.Instance.GetCellPos(newPos1), 0.2f);
                        playerPos = newPos1;
                    }
                }
               
                break;
            
            case EInputDir.Right:
                var newPos2 = playerPos + new Vector2(1, 0);
                if (newPos2.x < LevelManager.Instance.levelwidth && newPos2.x >= 0
                                                                 && newPos2.y < LevelManager.Instance.levellength &&
                                                                 newPos2.y >= 0)
                {
                    if (CanStepOn(newPos2))
                    {
                        UpdateOriginCell();
                        transform.DOMove(LevelManager.Instance.GetCellPos(newPos2), 0.2f);
                        playerPos = newPos2;
                    }
                }
               
                break;
            
            case EInputDir.Left:
                var newPos3 = playerPos + new Vector2(-1, 0);
                if (newPos3.x < LevelManager.Instance.levelwidth && newPos3.x >= 0
                && newPos3.y < LevelManager.Instance.levellength && newPos3.y >= 0)
                {
                    if (CanStepOn(newPos3))
                    {
                        UpdateOriginCell();
                        transform.DOMove(LevelManager.Instance.GetCellPos(newPos3), 0.2f);
                        playerPos = newPos3;
                    }
                }
                
                break;
        }

        InputDir = EInputDir.Null;
        
        if(playerPos == LevelManager.Instance.finalPos)
            GameManager.Instance.WinGame();
    }

    private void UpdateOriginCell()
    {
        if(LevelManager.Instance.GetCellType(playerPos) == ECellType.Oneoff || 
           LevelManager.Instance.GetCellType(playerPos) == ECellType.Oneoff_placeble)
        LevelManager.Instance.UpdateCell(playerPos,ECellType.Null);
    }

    private bool CanStepOn(Vector2 pos)
    {
        if (LevelManager.Instance.GetCellType(pos) == ECellType.Oneoff
            || LevelManager.Instance.GetCellType(pos) == ECellType.Oneoff_placeble
            || LevelManager.Instance.GetCellType(pos) == ECellType.Perma
            || LevelManager.Instance.GetCellType(pos) == ECellType.Perma_placeble
            || LevelManager.Instance.GetCellType(pos) == ECellType.Final)
            return true;
        else if (LevelManager.Instance.GetCellType(pos) == ECellType.Key)
        {
            hasKey = true;
            Destroy(LevelManager.Instance.Key);
            LevelManager.Instance.Lock.GetComponent<MeshRenderer>().material.color = Color.blue;
            return true;
        }
        else if (LevelManager.Instance.GetCellType(pos) == ECellType.Lock)
        {
            if (hasKey)
            {
                Destroy(LevelManager.Instance.Lock);
            }
            return hasKey;
        }
        return false;

    }
}
