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


    private void Start()
    {
        StoreCreateShape();
    }

    // Start is called before the first frame update

    public int GetNumberCurrentShapeSelected()
    {
        return currentShape.transform.childCount;
    }

    public void StoreCreateShape()
    {
        foreach (var shape in createShapes)
        {
            if (shape.gameObject.transform.childCount == 0)
            {
                var shapeIndex = UnityEngine.Random.Range(0, LsPrefabs.Count);
                GameObject goClone = Instantiate(LsPrefabs[shapeIndex]);
                shape.Create_Shape(goClone);
            }
        }
    }

    public void RefeshPositionShape()
    {
        StartCoroutine(DestroyShape());
        
    }
    IEnumerator DestroyShape()
    {
        foreach (var shape in createShapes)
        {
            if (shape.gameObject.transform.GetChild(0) != null)
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
