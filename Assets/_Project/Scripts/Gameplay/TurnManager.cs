using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class TurnManager : SingletonMono<TurnManager>
{
    private int Turn;

    private List<CreateShape> myShapes = new List<CreateShape>();

    private List<CreateShape> enemyShapes = new List<CreateShape>();

    [HideInInspector] public int isLose;

    private float timer;

    public int GetTurn()
    {
        return Turn;
    }
    public void StartTurnManager()
    {
        
        for (int i = 0; i < Grid.Instance.shapeStore.createShapes.Count; i++)
        {
            if (i >= 3)
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
        Turn = 0;
        timer = 20;
        isLose = 0;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            ChangeTurn();
            GameEvent.MoveShapeToStartPosition();
            if (Turn % 2 == 0)
            {
                if(CheckResults() != 1 && CheckResults() != -1)
                {
                    Grid.Instance.PutShapeEnemy();
                }
                
            }
        }
        
    }

    public int CheckResults()
    {
        var shapes = Turn % 2 == 1 ? myShapes : enemyShapes;
        int numberShapeNotNull = 0;
        foreach (var item in shapes)
        {
            if (item.transform.childCount == 0) continue;

            numberShapeNotNull++;
            if (!item.IsDrag && item.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite == item.DisableDrag)
                isLose++;
        }
        if ((isLose == numberShapeNotNull && numberShapeNotNull!=0) && Turn % 2 == 1) return -1;
        else if ((isLose == numberShapeNotNull && numberShapeNotNull != 0) && Turn % 2 == 0) return 1;
        
        isLose = 0;
        return 0;
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
