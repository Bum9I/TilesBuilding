using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBuilder : MonoBehaviour
{
    private Vector2Int gridSize = new Vector2Int(10, 10);
    private Tile[,] tiles;
    private GameObject currentTileObject;

    public Vector2Int GridSize => gridSize;

    private void Awake()
    {
        tiles = new Tile[gridSize.x, gridSize.y];
    }

    public Vector2Int GetGridIndex(Vector3 worldPosition)
    {
        var tilePositionInMap = transform.InverseTransformPoint(worldPosition);
        
        var x = Mathf.FloorToInt(tilePositionInMap.x);
        var y = Mathf.FloorToInt(tilePositionInMap.z);
        
        var halfGridSize = gridSize / 2;
        var gridIndex = new Vector2Int(x, y)  + halfGridSize;

        return gridIndex;
    }

    public Vector3 GetTilePosition(Vector2Int gridIndex)
    {        
        var halfGridSize = gridSize / 2;
        var halfTileSize = Vector2.one / 2;

        var tilePositionXY = gridIndex - halfGridSize + halfTileSize;
        return new Vector3(tilePositionXY.x, 0, tilePositionXY.y);
    }

    public bool IsCellAvailable(Vector2Int gridIndex)
    {
        var isOutOfGrid = gridIndex.x < 0 || gridIndex.y < 0 ||
                          gridIndex.x >= tiles.GetLength(0) || gridIndex.y >= tiles.GetLength(1);
        if (isOutOfGrid)
        {
            return false;
        } 
        return (tiles[gridIndex.x, gridIndex.y] == null);
    }

    public void SetTile(Vector2Int gridIndex, Tile tile)
    {
        tiles[gridIndex.x, gridIndex.y] = tile;
    }

    public void StartPlacingTile(GameObject tilePrefab)
    {
        if (currentTileObject != null)
        {
            Destroy(currentTileObject);
        }
        
        currentTileObject = Instantiate(tilePrefab);
    }

    public void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hitInfo) && currentTileObject != null)
        {
            var gridIndex = GetGridIndex(hitInfo.point);
            var tilePosition = GetTilePosition(gridIndex);
            currentTileObject.transform.localPosition = tilePosition;

            var isAvailable = IsCellAvailable(gridIndex);
            var tileComponent = currentTileObject.GetComponent<Tile>();
            tileComponent.SetColor(isAvailable);
            
            if (Input.GetMouseButton(0) && isAvailable)
            {
                SetTile(gridIndex, tileComponent);
                tileComponent.ResetColor();
                currentTileObject = null;      
            } 
        }
    }
}
