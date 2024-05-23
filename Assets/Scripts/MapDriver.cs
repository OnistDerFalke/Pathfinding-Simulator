using UnityEngine;

public class MapDriver : MonoBehaviour
{
    //Tile mock
    [SerializeField] private GameObject mockTile;

    //Default map size before user changes it
    public Vector2Int defaultMapSize;

    //Stores generated tiles GameObjects
    public GameObject[,] mapTiles;

    //Stores placements of start and end tiles
    public Vector2Int startTilePlacement;
    public Vector2Int endTilePlacement;
    
    private bool marksSet;

    //Defines offset between tiles 
    private float offsetBetweenTiles = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        //Setting map driver
        GameManager.Instance.mapDriver = this;
        
        GenerateMap(defaultMapSize);
    }

    //Transforms coordinates to position
    public Vector3 PlacementToPosition(Vector2Int placement)
    {
        var offset = mockTile.transform.localScale.x + offsetBetweenTiles;
        return new Vector3(placement.x * offset, 0f, placement.y * offset);
    }

    //Generates map of given size
    public void GenerateMap(Vector2Int size)
    {
        //Destroying instances from last generation
        DestroyLatestInstances();

        if (size.x <= 0 || size.y <= 0)
            return;

        //Clears latest marks
        if(marksSet)
        {
            ClearMarkedTiles();
            ClearWayMarks();
        }
        
        //Allocating array for tiles
        mapTiles = new GameObject[size.x, size.y];

        //Tile is a square, x=y
        var tileSize = mockTile.transform.localScale.x;
        var totalOffset = tileSize + offsetBetweenTiles;

        //Generating tiles in proper positions
        for (var i = 0; i < size.x; i++)
        {
            for (var j = 0; j < size.y; j++)
            {
                //Rotation never changes, all works on one surface
                mapTiles[i, j] = Instantiate(
                    mockTile, PlacementToPosition(new Vector2Int(i, j)), Quaternion.identity);
                mapTiles[i, j].SetActive(true);
                mapTiles[i, j].GetComponent<TileObject>().placement = new Vector2Int(i, j);
            }
        }
        
        //Default start and end placements are in the opposite corners
        SetStartTile(new Vector2Int(0, 0));
        SetEndTile(new Vector2Int(size.x - 1, size.y - 1));
        marksSet = true;

        //Update camera position after regenerating map
        GameManager.Instance.cameraDriver.CenterCameraOnBoard(size, totalOffset, tileSize);
    }

    //Setting new start tile
    public void SetStartTile(Vector2Int placement)
    {
        //Cannot set obstacle as a start
        if (mapTiles[placement.x, placement.y].GetComponent<TileObject>().isObstacle)
            return;
        
        for (var i = 0; i < mapTiles.GetLength(0); i++)
            for (var j = 0; j < mapTiles.GetLength(1); j++)
                mapTiles[i, j].GetComponent<TileObject>().ResetStartMark();
        mapTiles[placement.x, placement.y].GetComponent<TileObject>().SetAsStartMark();
        startTilePlacement = placement;
    }
    
    //Setting new end tile
    public void SetEndTile(Vector2Int placement)
    {
        //Cannot set obstacle as an end
        if (mapTiles[placement.x, placement.y].GetComponent<TileObject>().isObstacle)
            return;
        
        for (var i = 0; i < mapTiles.GetLength(0); i++)
            for (var j = 0; j < mapTiles.GetLength(1); j++)
                mapTiles[i, j].GetComponent<TileObject>().ResetEndMark();
        mapTiles[placement.x, placement.y].GetComponent<TileObject>().SetAsEndMark();
        endTilePlacement = placement;
    }
    
    //Setting new end tile
    public void SetWayTile(Vector2Int placement)
    {
        if(marksSet)
            mapTiles[endTilePlacement.x, endTilePlacement.y].GetComponent<TileObject>().ResetWayMark();
        mapTiles[placement.x, placement.y].GetComponent<TileObject>().SetAsWayMark();
    }

    private void ClearMarkedTiles()
    {
        if (!marksSet) return;
        
        mapTiles[startTilePlacement.x, startTilePlacement.y].GetComponent<TileObject>().ResetMark();
        mapTiles[endTilePlacement.x, endTilePlacement.y].GetComponent<TileObject>().ResetMark();
    }

    public void ClearWayMarks()
    {
        for (var i = 0; i < mapTiles.GetLength(0); i++)
            for (var j = 0; j < mapTiles.GetLength(1); j++)
                if(mapTiles[i, j])
                    mapTiles[i, j].GetComponent<TileObject>().ResetWayMark();
    }

    //Destroys previous instances
    private void DestroyLatestInstances()
    {
        if (mapTiles == null) 
            return;
        
        for (var i = 0; i < mapTiles.GetLength(0); i++)
            for (var j = 0; j < mapTiles.GetLength(1); j++)
                Destroy(mapTiles[i, j]);
    }
}
