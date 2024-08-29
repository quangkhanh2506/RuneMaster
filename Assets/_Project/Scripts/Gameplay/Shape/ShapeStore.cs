using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStore : MonoBehaviour
{
    public List<CreateShape> createShapes;
    public List<GameObject> LsPrefabs;

    private List<GameObject> _prefabs;

    [HideInInspector]
    public GameObject currentShape;

    [HideInInspector]
    public int numberMyShapes;


    [HideInInspector]
    public int numberEnemyShapes;

    // Start is called before the first frame update

    public int GetNumberCurrentShapeSelected()
    {
        return currentShape.transform.childCount;
    }

    public void StoreCreateShape()
    {
        numberMyShapes = numberEnemyShapes = 3;
        for (int i = 0; i < createShapes.Count; i++)
        {
            createShapes[i].AwakeCreateShape();
            createShapes[i].StartCreateShape();
            if (createShapes[i].gameObject.transform.childCount == 0)
            {
                var shapeIndex = UnityEngine.Random.Range(0, LsPrefabs.Count);
                GameObject goClone = Instantiate(LsPrefabs[shapeIndex]);
                createShapes[i].Create_Shape(goClone);
            }
        }
    }

    public void RefeshThreeShape(bool myShape)
    {
        int start = myShape ? 3 : 0;
        int end = myShape ? 6 : 3;
        for (int i = start; i < end; i++)
        {
            createShapes[i].AwakeCreateShape();
            createShapes[i].StartCreateShape();
            if (createShapes[i].gameObject.transform.childCount == 0)
            {
                var shapeIndex = UnityEngine.Random.Range(0, LsPrefabs.Count);
                GameObject goClone = Instantiate(LsPrefabs[shapeIndex]);
                createShapes[i].Create_Shape(goClone);
            }
        }
        if (myShape) numberMyShapes = 3;
        else numberEnemyShapes = 3;
    }

    public void RefeshPositionShape()
    {
        StartCoroutine(DestroyShape());
        
    }
    IEnumerator DestroyShape()
    {
        foreach (var shape in createShapes)
        {
            if (shape.gameObject.transform.childCount > 0)
            {
                Destroy(shape.gameObject.transform.GetChild(0).gameObject);
            }
            shape.IsDrag = true;
        }
        yield return new WaitForSeconds(0.1f);
        StoreCreateShape();
    }

    public void Cheat()
    {
        foreach (var shape in createShapes)
        {
            shape.UpdateIsdrag(false);
        }
    }
}
