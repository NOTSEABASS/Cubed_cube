using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum ECellType
{
    Start,
    Final,
    Perma_placeble,
    Oneoff_placeble,
    Perma,
    Oneoff,
    Obstacle,
    Lock,
    Key,
    Null
}
public class Island : MonoBehaviour
{
    private Vector3 originPos;
    [SerializeField]private List<bool> hitCell;
    [SerializeField]private List<GameObject> hitObj;
    public bool canPlace;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnMouseEnter()
    {
        transform.DOScale(new Vector3(0.8f, 0.4f, 0.8f), 0.1f).SetEase(Ease.InOutBack);
    }

    private void OnMouseExit()
    {
        if (!canPlace)
        {
            transform.DOScale(new Vector3(0.6f, 0.3f, 0.6f), 0.1f).SetEase(Ease.InOutBack);
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha",1f);
        }
    }

    private void OnMouseDrag()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 m_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z);
        var finalPos = Camera.main.ScreenToWorldPoint(m_MousePos);
        transform.position = new Vector3(finalPos.x, -0.5f, finalPos.z);
        hitCell = new List<bool>();
        hitObj = new List<GameObject>();
        if (hitCell.Count < transform.childCount)
        {
            for (int i = 0; i <= transform.childCount; i++)
            {
                hitCell.Add(false);
                hitObj.Add(null);
            }
        }

        RaycastHit midhit;
        if (Physics.Raycast(finalPos, Vector3.down, out midhit))
        {
            var newPos = midhit.transform.position;
            transform.DOMove(new Vector3(newPos.x, transform.position.y, newPos.z),0.1f);
            hitCell[0] = midhit.transform.GetComponent<IslandCell>().CellType == ECellType.Null;
            hitObj[0] = midhit.transform.gameObject;
        }

        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (gameObject == null) return;
            RaycastHit hit;
            if (Physics.Raycast(transform.GetChild(i).position, Vector3.down, out hit))
            {
                hitCell[i+1] = hit.transform.GetComponent<IslandCell>().CellType == ECellType.Null;
                hitObj[i + 1] = hit.transform.gameObject;
            }
            else
            {
                hitCell[i+1] = false;
                hitObj[i + 1] = null;
            }
        }

        for (int i = 0; i < transform.childCount+1; i++)
        {
            if (gameObject == null) return;
            if (!hitCell[i])
            {
                canPlace = false;
                break;
            }
            if (i == transform.childCount)
            {
                canPlace = true;
            }
        }
        
        if (!canPlace)
        {
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha",0.5f);
        }
        else
        {
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Alpha",1f);
        }
        
        
    }

    private void OnMouseUp()
    {
        if(!canPlace)
            transform.DOMove(originPos, 0.2f);
        else
        {
            LevelManager.Instance.UpdateCell(hitObj[0],GetComponent<IslandCell>().CellType);
            for (int i = 1; i < hitObj.Count ; i ++)
            {
                LevelManager.Instance.UpdateCell(hitObj[i],transform.GetChild(i-1).GetComponent<IslandCell>().CellType);
            }
            
            //成功替换，播放音效和粒子
            ParticleManager.Instance.PlayPlaceParticle(gameObject.transform.position);
            DestroyImmediate(gameObject);
        }
    }



  
}
