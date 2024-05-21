using UnityEngine;

public class CameraDriver : MonoBehaviour
{
    //Camera angles depending on camera mode
    [SerializeField] private float perspectiveCameraAngle;
    [SerializeField] private float orthographicCameraAngle;
    
    void Awake()
    {
        //Setting camera driver
        GameManager.Instance.cameraDriver = this;
    }
    
    //Centers camera view on board
    public void CenterCameraOnBoard(Vector2Int boardSize, float totalOffset, float offset)
    {
        var x = (boardSize.x * totalOffset - offset) / 2f;
        var y = gameObject.transform.position.y;
        var z = (boardSize.y * totalOffset - offset) / 2f;
        
        //Camera is orthographic
        GetComponent<Camera>().orthographicSize = Mathf.Max(x, z)+1f;
        
        gameObject.transform.position = new Vector3(x, y, z);
    }

    //Changes camera mode and it's angle
    public void ChangeCameraMode()
    {
        var cam = GetComponent<Camera>();
        var orthographic = cam.orthographic;
        orthographic = !orthographic;
        cam.orthographic = orthographic;
        ChangeCameraAngle(cam, orthographic ? orthographicCameraAngle : perspectiveCameraAngle);
    }

    public bool IsOrthographic()
    {
        return GetComponent<Camera>().orthographic;
    }

    //Changes x rotation of the camera
    private void ChangeCameraAngle(Camera cam, float angle)
    {
        var rot = cam.gameObject.transform.rotation;
        var eulerAngles = rot.eulerAngles;
        eulerAngles.x = angle;
        var updated = Quaternion.Euler(eulerAngles);
        cam.gameObject.transform.rotation = updated;
    }
}
