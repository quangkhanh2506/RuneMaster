using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : SingletonMono<TurnManager>
{
    private int Turn;

    private List<CreateShape> myShapes = new List<CreateShape>();

    private List<CreateShape> enemyShapes = new List<CreateShape>();

    [HideInInspector] public int isWin, isLose;

    private float timer;

    private void Awake()
    {
        
        ReGame();
    }

    private void Start()
    {
        
        for (int i = 0; i < Grid.Instance.shapeStore.createShapes.Count; i++)
        {
            if (i < 3)
            {
                myShapes.Add(Grid.Instance.shapeStore.createShapes[i]);
            }
            else
            {
                enemyShapes.Add(Grid.Instance.shapeStore.createShapes[i]);
            }
        }
    }
    public void ReGame()
    {
        Turn = -1;
        timer = 20;
        isWin = 0;
        isLose = 0;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            ChangeTurn();
            GameEvent.MoveShapeToStartPosition();
        }
        
    }

    public void CheckResults()
    {
        foreach (var item in myShapes)
        {
            if (!item.IsDrag && item.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite == item.DisableDrag) isLose++;
        }
        foreach (var item in enemyShapes)
        {
            if (!item.IsDrag && item.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite == item.DisableDrag) isWin++;
        }

        if (isWin==3 && isLose==3)
        {
            if (Turn % 2 == 0) Debug.Log("Lose");
            else Debug.Log("Win");
        }
        else if (isWin==3) Debug.Log("WIN");
        else if (isLose==3) Debug.Log("LOSE");
        isLose = 0;
        isWin = 0;
    }

    public void ChangeTurn()
    {
        Turn++;
        for (int i = 0; i < Grid.Instance.shapeStore.createShapes.Count; i++)
        {
            if (i < 3)
            {
                Grid.Instance.shapeStore.createShapes[i].gameObject.GetComponent<BoxCollider2D>().enabled = Turn % 2 == 0;

                if(!Grid.Instance.shapeStore.createShapes[i].IsDrag) Grid.Instance.shapeStore.createShapes[i].gameObject.GetComponent<BoxCollider2D>().enabled = Grid.Instance.shapeStore.createShapes[i].IsDrag;

            }
            else
            {
                Grid.Instance.shapeStore.createShapes[i].gameObject.GetComponent<BoxCollider2D>().enabled = Turn % 2 == 1;
                if (!Grid.Instance.shapeStore.createShapes[i].IsDrag) Grid.Instance.shapeStore.createShapes[i].gameObject.GetComponent<BoxCollider2D>().enabled = Grid.Instance.shapeStore.createShapes[i].IsDrag;
            }
        }
        timer = 20;
    }

    
}
