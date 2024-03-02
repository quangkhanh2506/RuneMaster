using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image normalImage;
    public Image hoverImage;
    public Image ActiveImage;
    public List<Sprite> normalImages;

    private Sprite collisionImage;

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public bool CanWeUseThisSquare()
    {
        return hoverImage.gameObject.activeSelf;
    }

    public void ActivateSquare()
    {
        
        hoverImage.gameObject.SetActive(false);
        ActiveImage.gameObject.SetActive(true);
        ActiveImage.sprite = collisionImage;
        Selected = true;
        SquareOccupied = true;
    }

    public void DeActivate()
    {
        ActiveImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.sprite = setFirstImage ? normalImages[1] : normalImages[0];
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("PositionShape"))
        {
            hoverImage.gameObject.SetActive(true);
            collisionImage = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("PositionShape"))
        {
            hoverImage.gameObject.SetActive(false);
            collisionImage = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("PositionShape"))
        {
            hoverImage.gameObject.SetActive(true);
        }
    }


}
