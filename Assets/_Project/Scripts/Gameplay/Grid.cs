using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : SingletonMono<Grid>
{
    public int collumns = 0;
    public int rows = 0;
    public float squaresGap = 0.58f;
    public GameObject griddSquare;
    public Vector2 startPosition = new Vector2(0f, 0f);
    public float squareScale = 0.8f;
    public float everySquareOffset = 0f;
    public ShapeStore shapeStore;
    public LineIndicator lineIndicator;


    private Vector2 _offset = new Vector2(0f, 0f);
    private List<GameObject> _gripSquares = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        TurnManager.Instance.ChangeTurn();


    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanbePlaced -= GameEvent_CheckIfShapeCanbePlaced;
    }

    
    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanbePlaced += GameEvent_CheckIfShapeCanbePlaced;
    }

    private void GameEvent_CheckIfShapeCanbePlaced()
    {
        var squareIndexs = new List<GridSquare>();
        foreach(var square in _gripSquares)
        {
            var gridsquare = square.GetComponent<GridSquare>();
            if (gridsquare.CanWeUseThisSquare() && !gridsquare.SquareOccupied)
            {
                squareIndexs.Add(gridsquare);
                gridsquare.Selected = false;
            }
        }
        if(shapeStore.GetNumberCurrentShapeSelected() == squareIndexs.Count)
        {
            foreach(var square in squareIndexs)
            {
                square.ActivateSquare();
            }
            GameEvent.MoveShapeToStartPosition();
            StartCoroutine(DestroyAndProceed());
            
        }
        else
        {
            GameEvent.MoveShapeToStartPosition();
        }

        
    }

    IEnumerator DestroyAndProceed()
    {
        //Destroy(shapeStore.currentShape.gameObject);
        yield return new WaitForSeconds(0.1f); // Wait for one frame
        
        CheckIfAnylineIsCompleted();
        shapeStore.StoreCreateShape();

        foreach (var item in shapeStore.createShapes)
        {
            bool a = CheckInteractWithShape(item.transform.GetChild(0).gameObject);
            item.GetComponent<BoxCollider2D>().enabled = a;
            item.GetComponent<CreateShape>().UpdateIsdrag(a);
        }

        TurnManager.Instance.ChangeTurn();

        TurnManager.Instance.CheckResults();
    }


    private bool CheckInteractWithShape(GameObject shape)
    {
        List<(int, int)> pairs = new List<(int, int)>();
        int x = 0, y = 0;
        for (int i = 0; i < shape.transform.childCount; i++)
        {
            x = (int)Math.Round(shape.transform.GetChild(i).gameObject.transform.localPosition.x / 0.58);
            y = (int)Math.Round(shape.transform.GetChild(i).gameObject.transform.localPosition.y / 0.58);
            pairs.Add((x, y));
        }

        for (int i = 0; i < _gripSquares.Count; i++)
        {
            int index = -1;
            int count = 0;
            
            foreach (var item in pairs)
            {
                int col = item.Item1 + i;
                if ((item.Item1 + i%10) >= 10 || (item.Item1 + i % 10) < 0) break;
                int row = i - item.Item2 * 10;
                if (row < 0 || row >= 100) break;

                if (item.Item1 == 0 && item.Item2 == 0) index = i;
                else if (item.Item1 == 0) index = row;
                else if (item.Item2 == 0) index = col;
                else index = row + item.Item1;

                var grid = _gripSquares[index].GetComponent<GridSquare>();

                 if (grid.SquareOccupied)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }

            if (count == pairs.Count) return true;
        }
        

        return false;
    }

    void CheckIfAnylineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        //columns
        foreach (var column in lineIndicator.columnIndexes)
        {
            lines.Add(lineIndicator.GetVerticalLine(column));
        }

        //row
        for(int row = 0; row < 10; row++)
        {
            List<int> data = new List<int>(10);
            for(int i = 0; i < 10; i++)
            {
                data.Add(lineIndicator.line_data[row, i]);
            }
            lines.Add(data.ToArray());
        }

        CheckifSquareAreCompleted(lines);
    }

    private void CheckifSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        var linesCompleted = 0;
        foreach (var line in data)
        {
            var lineCompleted = true;
            foreach (var squareIndex in line)
            {
                var comp = _gripSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                }
            }
            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            StartCoroutine(DeactivateSquaresWithDelay(line));

            foreach (var squareIndex in line)
            {
                var comp = _gripSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }
        }
    }

    IEnumerator DeactivateSquaresWithDelay(IEnumerable<int> line)
    {
        foreach (var squareIndex in line)
        {
            var comp = _gripSquares[squareIndex].GetComponent<GridSquare>();
            comp.DeActivate();
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPosition();
    }

    private void SetGridSquaresPosition()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        Vector2 squareGapNumber = new Vector2(0f, 0f);
        bool rowMoved = false;

        var squareRect = _gripSquares[0].GetComponent<RectTransform>();

        _offset.x = squareRect.rect.width * squareRect.transform.localScale.x + everySquareOffset;
        _offset.y = squareRect.rect.height * squareRect.transform.localScale.y + everySquareOffset;

        foreach(GameObject square in _gripSquares)
        {
            if (columnNumber + 1 > collumns)
            {
                squareGapNumber.x = 0;

                columnNumber = 0;
                rowNumber++;
                rowMoved = false;
            }

            var posX_offset = _offset.x * columnNumber + (squareGapNumber.x * squaresGap);
            var posY_offset = _offset.y * rowNumber + (squareGapNumber.y * squaresGap);

            if(columnNumber>0 && columnNumber % 3 == 0)
            {
                squareGapNumber.x++;
                posX_offset += squaresGap;
            }
            if(rowNumber>0&& rowNumber%3==0 && rowMoved == false)
            {
                rowMoved = true;
                squareGapNumber.y++;
                posY_offset += squaresGap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + posX_offset, startPosition.y - posY_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + posX_offset, startPosition.y - posY_offset,0f);
            columnNumber++;
        }
    }

    private void SpawnGridSquares()
    {
        //int squareIndex = 0;
        for(int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {
                _gripSquares.Add(Instantiate(griddSquare) as GameObject);
                _gripSquares[_gripSquares.Count - 1].transform.SetParent(this.transform);
                _gripSquares[_gripSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, 1);
                //_gripSquares[_gripSquares.Count - 1].GetComponent<GridSquare>().SetImage(squareIndex % 2 == 0);
                //squareIndex++;
            }
        }
    }

    public void ResetMatch()
    {
        foreach (var square in _gripSquares)
        {
            square.GetComponent<GridSquare>().DeActivate();
            square.GetComponent<GridSquare>().ClearOccupied();
        }
        shapeStore.RefeshPositionShape();
        TurnManager.Instance.ReGame();
        TurnManager.Instance.ChangeTurn();
    }
}
