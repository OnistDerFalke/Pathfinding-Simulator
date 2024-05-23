using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UI : MonoBehaviour
{
    //Main ui control panel
    [SerializeField] private GameObject controlPanel;

    //Input fields for map size
    [SerializeField] private InputField xMapSizeInput;
    [SerializeField] private InputField yMapSizeInput;

    //Buttons that will be disabled in some cases
    [SerializeField] private Button applyButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;

    [SerializeField] private RectTransform progressBarValueRect;
    [SerializeField] private RectTransform progressBarRect;

    //Limit of the size (too many instances may throw memory exception)
    [SerializeField] private Vector2Int mapSizeLimit = new(100, 100);
   
    //Current size of the map, applied when apply button clicked
    private Vector2Int _currentSize;
    
    void Start()
    {
        //Setting UI
        GameManager.Instance.ui = this;
        
        xMapSizeInput.onValueChanged.AddListener(ApplyX);
        yMapSizeInput.onValueChanged.AddListener(ApplyY);
    }

    void Update()
    {
        //Disable buttons when animating, only reset button enabled
        applyButton.interactable = !GameManager.Instance.animator.AnimationInProgress;
        startButton.interactable = !GameManager.Instance.animator.AnimationInProgress;
        resetButton.interactable = GameManager.Instance.animator.AnimationInProgress;
        
        //Update value in progressbar
        var newWidth = GameManager.Instance.animator.Progress * 0.98f * progressBarRect.sizeDelta.x;
        progressBarValueRect.sizeDelta = new Vector2(newWidth, progressBarValueRect.sizeDelta.y);
    }

    public void Toggle()
    {
        controlPanel.SetActive(!controlPanel.activeSelf);
    }
    
    //Applies new width
    private void ApplyX(string input)
    {
        input = Regex.Replace(input, @"[^a-zA-Z0-9 ]", "");
        int.TryParse(input, out var result);

        if (result > mapSizeLimit.x)
        {
            GameManager.Instance.broadcaster.Broadcast($"Map size width is limited to {mapSizeLimit.x}");
            return;
        }
        
        _currentSize.x = result;
    }
    
    //Applies new height
    private void ApplyY(string input)
    {
        input = Regex.Replace(input, @"[^a-zA-Z0-9 ]", "");
        int.TryParse(input, out var result);
        
        if (result > mapSizeLimit.y)
        {
            GameManager.Instance.broadcaster.Broadcast($"Map size height is limited to {mapSizeLimit.y}");
            return;
        }
        
        _currentSize.y = result;
    }

    //Button for applying new map size
    public void OnApplyButton()
    {
        if (_currentSize.x <= 0 || _currentSize.y <= 0)
            _currentSize = GameManager.Instance.mapDriver.defaultMapSize;
        GameManager.Instance.mapDriver.GenerateMap(_currentSize);
    }

    //Finding the path on button click
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

    //Button for clearing the path
    public void OnClearButton()
    {
        GameManager.Instance.mapDriver.ClearWayMarks();
    }

    //Button for starting character animation
    public void OnStartButton()
    {
        GameManager.Instance.animator.RunAnimation(GameManager.Instance.astar.GetAstarPath());
    }

    //Button for reset character animation
    public void OnResetButton()
    {
        GameManager.Instance.animator.StopAnimation();
    }
}
