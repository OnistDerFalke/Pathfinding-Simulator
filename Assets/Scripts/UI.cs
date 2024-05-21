using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //Main ui control panel
    [SerializeField] private GameObject controlPanel;

    //Input fields for map size
    [SerializeField] private InputField xMapSizeInput;
    [SerializeField] private InputField yMapSizeInput;

    //Limit of the size (too many instances may throw memory exception)
    [SerializeField] private Vector2Int mapSizeLimit = new(100, 100);
   
    //Current size of the map, applied when apply button clicked
    private Vector2Int _currentSize;


    void Start()
    {
        xMapSizeInput.onValueChanged.AddListener(ApplyX);
        yMapSizeInput.onValueChanged.AddListener(ApplyY);
    }
    
    void Update()
    {
        //Changes UI visibility
        if(Input.GetKeyDown(KeyCode.H))
            controlPanel.SetActive(!controlPanel.activeSelf);
    }

    //Applies new size
    private void ApplyX(string input)
    {
        if (!int.TryParse(input, out var result))
        {
            GameManager.Instance.broadcaster.Broadcast("Width should be int.");
            return;
        }

        if (result > mapSizeLimit.x)
        {
            GameManager.Instance.broadcaster.Broadcast($"Map size width is limited to {mapSizeLimit.x}");
            return;
        }
        
        _currentSize.x = result;
    }
    
    private void ApplyY(string input)
    {
        if (!int.TryParse(input, out var result))
        {
            GameManager.Instance.broadcaster.Broadcast("Height should be int.");
            return;
        }

        if (result > mapSizeLimit.y)
        {
            GameManager.Instance.broadcaster.Broadcast($"Map size height is limited to {mapSizeLimit.y}");
            return;
        }
        
        _currentSize.y = result;
    }

    public void OnApplyButton()
    {
        if (_currentSize.x <= 0 || _currentSize.y <= 0)
            _currentSize = GameManager.Instance.mapDriver.defaultMapSize;
        GameManager.Instance.mapDriver.GenerateMap(_currentSize);
    }

    public void OnFindPathButton()
    {
        GameManager.Instance.mapDriver.ClearWayMarks();
        
        var path = GameManager.Instance.astar.GetAstarPath();
        
        if (path == null)
        {
            GameManager.Instance.broadcaster.Broadcast("Path does not exist");
            return;
        }
        
        foreach(var t in path)
            GameManager.Instance.mapDriver.SetWayTile(t);
    }

    public void OnClearButton()
    {
        GameManager.Instance.mapDriver.ClearWayMarks();
    }
}
