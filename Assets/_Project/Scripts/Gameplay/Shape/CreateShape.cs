using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateShape : MonoBehaviour
{
    private Vector3 scale = new Vector3(0.5f, 0.5f, 1);

    private bool moving;
    private float startPosX;
    private float startPosY;
    private Vector3 startPosition;

    public TurnManager turnManager;

    [HideInInspector] public bool IsDrag;

    private Sprite currentSprite;

    public Sprite DisableDrag;

    public ShapeStore shapeStore;


    private void OnDisable()
    {
        GameEvent.MoveShapeToStartPosition -= GameEvent_MoveShapeToStartPosition;
    }


    private void OnEnable()
    {
        GameEvent.MoveShapeToStartPosition += GameEvent_MoveShapeToStartPosition;
    }

    private void GameEvent_MoveShapeToStartPosition()
    {
        moving = false;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localPosition = startPosition;
    }

    public void AwakeCreateShape()
    {
        IsDrag = true;
    }

    public void StartCreateShape()
    {
        startPosition = this.transform.localPosition;
        
    }

    private void Update()
    {
        if (moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
        }
    }

    public bool IsOnStartPosition()
    {
        return this.transform.localPosition == startPosition;
    }

    public void Create_Shape(GameObject prefab)
    {
        prefab.transform.SetParent(this.transform);
        prefab.transform.localScale = scale;
        prefab.transform.localPosition = Vector3.zero;
        currentSprite = this.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void UpdateIsdrag(bool isDrag)
    {
        if (isDrag) UpdateSprite(currentSprite);
        else UpdateSprite(DisableDrag);
        IsDrag = isDrag;
    }

    private void UpdateSprite(Sprite sprite)
    {
        for (int i = 0; i < this.transform.GetChild(0).gameObject.transform.childCount; i++)
        {
            this.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }


    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;
            moving = true;

            this.transform.localScale = new Vector3(1.54f, 1.54f, 1);
        }
    }

    private void OnMouseUp()
    {
        shapeStore.currentShape = this.gameObject.transform.GetChild(0).gameObject;
        moving = false;
        GameEvent.CheckIfShapeCanbePlaced();
        this.transform.localScale = new Vector3(1,1,1);
    }

}
