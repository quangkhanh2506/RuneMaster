using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System.Reflection;
using UnityEngine.UIElements;

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
    private int resetGame = 0;


    private Vector2 _offset = new Vector2(0f, 0f);
    private List<GameObject> _gripSquares = new List<GameObject>();
    private List<List<int>> listMyShapePositionGrid = new List<List<int>>();
    private List<List<int>> listEnemyShapePositionGrid = new List<List<int>>();
    private List<int[]> _completedEnemyLines = new List<int[]>();



    // Start is called before the first frame update

    public void SetupGameplay()
    {
        shapeStore.StoreCreateShape();
        TurnManager.Instance.ReGame();
        TurnManager.Instance.StartTurnManager();
        foreach (var square in _gripSquares)
        {
            square.GetComponent<GridSquare>().DeActivate();
            square.GetComponent<GridSquare>().ClearOccupied();
        }
        if (_gripSquares.Count == 0)
            CreateGrid();
        CountNumberGridCanBeInteractAllMyShape();
        CountNumberGridCanBeInteractAllEnemyShape();
        TurnManager.Instance.ChangeTurn();
    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanbePlaced -= GameEvent_CheckIfShapeCanbePlaced;
        GameEvent.CheckIfShapeEnemyCanbePlaced -= GameEvent_CheckIfShapeEnemyCanbePlaced;
    }

    

    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanbePlaced += GameEvent_CheckIfShapeCanbePlaced;
        GameEvent.CheckIfShapeEnemyCanbePlaced += GameEvent_CheckIfShapeEnemyCanbePlaced;
    }

    private void GameEvent_CheckIfShapeEnemyCanbePlaced()
    {
        StartCoroutine(DestroyAndProceed());
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
        int index = -1;
        if (TurnManager.Instance.GetTurn() % 2 == 1)
        {
            for (int i = 3; i < shapeStore.createShapes.Count; i++)
            {
                if (shapeStore.createShapes[i].transform.childCount == 0) continue;
                if (shapeStore.createShapes[i].name == shapeStore.currentShape.transform.parent.gameObject.name)
                {
                    index++;
                    ClearMyShapePositionCanInteract(index);
                    break;
                }
                index++;
            }
            shapeStore.numberMyShapes--;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (shapeStore.createShapes[i].transform.childCount == 0) continue;
                if (shapeStore.createShapes[i].name == shapeStore.currentShape.transform.parent.gameObject.name)
                {
                    index++;
                    ClearEnemyShapePositionCanInteract(index);
                    break;
                }
                index++;
            }
            shapeStore.numberEnemyShapes--;
        }

        Destroy(shapeStore.currentShape.gameObject);
        yield return new WaitForSeconds(0.1f); // Wait for one frame

        
        CheckIfAnylineIsCompleted();

        if (shapeStore.numberMyShapes == 0)
        {
            ClearALLMyShapePositionCanInteract();
            shapeStore.RefeshThreeShape(true);
            CountNumberGridCanBeInteractAllMyShape();
        }
        else
        {
            int index_operator = -1;
            for (int i = 3; i < shapeStore.createShapes.Count; i++)
            {
                if (shapeStore.createShapes[i].transform.childCount == 0) continue;
                index_operator++;
                CheckListMyShapePositionGrid(shapeStore.createShapes[i].transform.GetChild(0).gameObject, index_operator);

            }
        }
        if (shapeStore.numberEnemyShapes == 0)
        {
            ClearALLEnemyShapePositionCanInteract();
            shapeStore.RefeshThreeShape(false);
            CountNumberGridCanBeInteractAllEnemyShape();
        }
        else
        {
            int index_operator = -1;
            for (int i = 0; i < 3; i++)
            {
                if (shapeStore.createShapes[i].transform.childCount == 0) continue;
                index_operator++;
                CheckListEnemyShapePositionGrid(shapeStore.createShapes[i].transform.GetChild(0).gameObject, index_operator);

            }
        }


        foreach (var item in shapeStore.createShapes)
        {
            if (item.transform.childCount == 0) continue;
            bool a = CheckInteractWithShape(item.transform.GetChild(0).gameObject);
            item.GetComponent<BoxCollider2D>().enabled = a;
            item.GetComponent<CreateShape>().UpdateIsdrag(a);
        }

        yield return new WaitForSeconds(0.1f);
        bool Play;

        Play = !Result();

        if (Play)
        {
            TurnManager.Instance.ChangeTurn();

            Play = !Result();

            yield return new WaitForSeconds(1f);
            if (Play)
            {
                PutShapeEnemy();
            }
        }

    }

    public void PutShapeEnemy()
    {
        if (TurnManager.Instance.GetTurn() % 2 == 0)
        {
            var a = CheckEnemyCanCompletedLine();
            PutSquareEnemy(shapeStore.createShapes[a.Item1].transform.GetChild(0).gameObject, a.Item2);
            shapeStore.currentShape = shapeStore.createShapes[a.Item1].transform.GetChild(0).gameObject;
            GameEvent.CheckIfShapeEnemyCanbePlaced();
            
        }
    }


    private void PutSquareEnemy(GameObject shape, int index)
    {
        List<(int, int)> pairs = new List<(int, int)>();
        int x = 0, y = 0;
        for (int i = 0; i < shape.transform.childCount; i++)
        {
            x = (int)Math.Round(shape.transform.GetChild(i).gameObject.transform.localPosition.x / 0.58);
            y = (int)Math.Round(shape.transform.GetChild(i).gameObject.transform.localPosition.y / 0.58);
            pairs.Add((x, y));
        }

        var sprite = shape.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;

        foreach (var item in pairs)
        {
            int i = 0;
            i = (item.Item1 + index) - (item.Item2 * 10);
            _gripSquares[i].GetComponent<GridSquare>().ActivateSquareEnemy(sprite);
        }

    }

    public bool Result()
    {
        int result = TurnManager.Instance.CheckResults();
        bool a = false;

        if (result == 1 || result == -1)
        {
            ClearALLEnemyShapePositionCanInteract();
            ClearALLMyShapePositionCanInteract();
            a = true;
        }

        if (result == 1) UIManager.Instance.ShowUI(UIIndex.UIVictory, new VictoryParam { coin = 200 });
        else if (result == -1) UIManager.Instance.ShowUI(UIIndex.UISecondChance, new SecondChanceParam { gem=10,countdown=10f,resetgame=resetGame});
        return a;
    }

    private void CheckListMyShapePositionGrid(GameObject shape, int index_operator)
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
                if ((item.Item1 + i % 10) >= 10 || (item.Item1 + i % 10) < 0) break;
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

            if (count == pairs.Count)
            {
                if (!listMyShapePositionGrid[index_operator].Contains(i))
                {
                    listMyShapePositionGrid[index_operator].Add(i);
                }
                
            }
            else
            {
                if (listMyShapePositionGrid[index_operator].Contains(i))
                {
                    listMyShapePositionGrid[index_operator].Remove(i);
                }
            }

        }
    }

    private void CheckListEnemyShapePositionGrid(GameObject shape, int index_operator)
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
                if ((item.Item1 + i % 10) >= 10 || (item.Item1 + i % 10) < 0) break;
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

            if (count == pairs.Count)
            {
                if (!listEnemyShapePositionGrid[index_operator].Contains(i))
                {
                    listEnemyShapePositionGrid[index_operator].Add(i);
                }
                
            }
            else
            {
                if (listEnemyShapePositionGrid[index_operator].Contains(i))
                {
                    listEnemyShapePositionGrid[index_operator].Remove(i);
                }
            }

        }
    }

    private void CountNumberGridCanBeInteractAllMyShape()
    {
        for (int i = 3; i < shapeStore.createShapes.Count; i++)
        {

            CountNumberGridInteractWithMyShape(shapeStore.createShapes[i].transform.GetChild(0).gameObject);
        }


    }


    private void CountNumberGridCanBeInteractAllEnemyShape()
    {

        for (int i = 0; i < 3; i++)
        {

            CountNumberGridInteractWithEnemyShape(shapeStore.createShapes[i].transform.GetChild(0).gameObject);
        }


    }

    private void CountNumberGridInteractWithMyShape(GameObject shape)
    {
        List<int> lsPosition = new List<int>();
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
                if ((item.Item1 + i % 10) >= 10 || (item.Item1 + i % 10) < 0) break;
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

            if (count == pairs.Count)
            {
                lsPosition.Add(i);
            }
                
        }

        listMyShapePositionGrid.Add(lsPosition);
        
    }

    private void CountNumberGridInteractWithEnemyShape(GameObject shape)
    {
        List<int> lsPosition = new List<int>();
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
                if ((item.Item1 + i % 10) >= 10 || (item.Item1 + i % 10) < 0) break;
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

            if (count == pairs.Count)
            {
                lsPosition.Add(i);
            }

        }

        listEnemyShapePositionGrid.Add(lsPosition);

    }



    private void ClearMyShapePositionCanInteract(int index)
    {
        if (listMyShapePositionGrid.Count > index)
        {
            listMyShapePositionGrid[index].Clear();
            listMyShapePositionGrid.RemoveAt(index);
        }
        
    }

    private void ClearEnemyShapePositionCanInteract(int index)
    {
        if (listEnemyShapePositionGrid.Count > index)
        {
            listEnemyShapePositionGrid[index].Clear();
            listEnemyShapePositionGrid.RemoveAt(index);
        }
        
    }

    private void ClearALLMyShapePositionCanInteract()
    {
        for (int i = 0; i < listMyShapePositionGrid.Count; i++)
        {
            listMyShapePositionGrid[i].Clear();
            listMyShapePositionGrid.RemoveAt(i);

        }
    }

    private void ClearALLEnemyShapePositionCanInteract()
    {
        for (int i = 0; i < listEnemyShapePositionGrid.Count; i++)
        {
            listEnemyShapePositionGrid[i].Clear();
            listEnemyShapePositionGrid.RemoveAt(i);

        }
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

    private (int,int) CheckEnemyCanCompletedLine()
    {
        int countExistShape = -1;
        int max = -1, indexShape = 0, position = 0;
        for (int i = 0; i < 3; i++)
        {
            if (shapeStore.createShapes[i].transform.childCount == 0) continue;
            countExistShape++;
            var a = findMostPosition(shapeStore.createShapes[i].transform.GetChild(0).gameObject, listEnemyShapePositionGrid[countExistShape]);
            if (max < a.Item2)
            {
                max = a.Item2;
                indexShape = i;
                position = a.Item1;
            }

        }

        return (indexShape, position);
    }

    private (int,int) findMostPosition(GameObject shape, List<int> positionShape)
    {
        List<(int, int)> pairs = new List<(int, int)>();
        int x = 0, y = 0;
        for (int i = 0; i < shape.transform.childCount; i++)
        {
            x = (int)Math.Round(shape.transform.GetChild(i).gameObject.transform.localPosition.x / 0.58);
            y = (int)Math.Round(shape.transform.GetChild(i).gameObject.transform.localPosition.y / 0.58);
            pairs.Add((x, y));
        }

        int position = 0, max = -1; 

        for (int i = 0; i < positionShape.Count; i++)
        {
            int index = positionShape[i];
            PutShapeEnemy(true, pairs, index);
            int mark = CheckifLineEnenyAreCompleted()*100 + CheckNumberBlockRune(pairs,index);
            
            if(max < mark)
            {
                position = index;
                max = mark;
            }

            PutShapeEnemy(false, pairs, index);
        }
        return (position, max);

    }


    private void PutShapeEnemy(bool Put, List<(int, int)> pairs, int index)
    {
        foreach (var item in pairs)
        {
            int i = 0;
            i = (item.Item1 + index) - (item.Item2 * 10);
            _gripSquares[i].GetComponent<GridSquare>().SquareOccupied = Put;
        }

    }

    private int CheckNumberBlockRune(List<(int, int)> pairs, int index)
    {
        int count = 0;
        foreach (var item in pairs)
        {
            int i = 0;
            i = (item.Item1 + index) - (item.Item2 * 10);
            for (int j = 0; j < listMyShapePositionGrid.Count; j++)  
            {
                if (listMyShapePositionGrid[j].Count > 0)
                {
                    if (listMyShapePositionGrid[j].Contains(i)) count++;
                }
                

            }
        }
        return count;
    }


    private int CheckifLineEnenyAreCompleted()
    {
        List<int[]> completedLines = new List<int[]>();
        foreach (var line in _completedEnemyLines)
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
        return completedLines.Count;
    }


    private void CheckIfAnylineIsCompleted()
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

        _completedEnemyLines = lines;
        CheckifSquareAreCompleted(lines);
    }

    private void CheckifSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
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
        resetGame = 0;
        shapeStore.RefeshPositionShape();
        TurnManager.Instance.ReGame();
        TurnManager.Instance.ChangeTurn();
    }
    public void ResetSecondChange()
    {
        foreach (var square in _gripSquares)
        {
            square.GetComponent<GridSquare>().DeActivate();
            square.GetComponent<GridSquare>().ClearOccupied();
        }
        resetGame++;
        shapeStore.RefeshPositionShape();
        TurnManager.Instance.ReGame();
        TurnManager.Instance.ChangeTurn();
        SetupGameplay();
    }
}
