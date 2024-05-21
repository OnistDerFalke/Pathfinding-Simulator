using UnityEngine;

//Singleton game manager class
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public CameraDriver cameraDriver;
    public MapDriver mapDriver;
    public Broadcaster broadcaster;
    public Astar astar;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    _instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}