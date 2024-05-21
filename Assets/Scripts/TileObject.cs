using UnityEngine;

//Class of tile object to store info about it
public class TileObject : MonoBehaviour
{
    //Materials for tiles
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material obstacleMaterial;
    
    //Start/end/way marks over the fields
    [SerializeField] private GameObject startMark;
    [SerializeField] private GameObject endMark;
    [SerializeField] private GameObject wayMark;
    
    //On default there are no obstacles
    public bool isObstacle;

    //Tile placement on a grid
    public Vector2Int placement;

    //Inverts type of tile (obstacle->normal/normal->obstacle);
    public void InvertType()
    {
        var startPlacement = GameManager.Instance.mapDriver.startTilePlacement;
        var endPlacement = GameManager.Instance.mapDriver.endTilePlacement;
        
        //Start and end tile cannot be an obstacle
        if (placement.x == startPlacement.x && placement.y == startPlacement.y) 
            return;
        if (placement.x == endPlacement.x && placement.y == endPlacement.y) 
            return;
        
        //Invert tile type and change to proper material
        isObstacle = !isObstacle;
        GetComponent<MeshRenderer>().material = isObstacle ? obstacleMaterial : normalMaterial;
    }

    public void SetAsStartMark()
    {
        startMark.SetActive(true);
    }

    public void SetAsEndMark()
    {
        endMark.SetActive(true);
    }
    
    public void SetAsWayMark()
    {
        wayMark.SetActive(true);
    }

    public void ResetMark()
    {
        startMark.SetActive(false);
        endMark.SetActive(false);
    }

    public void ResetStartMark()
    {
        startMark.SetActive(false);
    }

    public void ResetEndMark()
    {
        endMark.SetActive(false);
    }

    public void ResetWayMark()
    {
        if(wayMark)
            wayMark.SetActive(false);
    }
}
