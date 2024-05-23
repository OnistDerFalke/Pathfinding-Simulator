using System.Collections;
using UnityEngine;
using UnityEngine.UI;


//Broadcast messages for player on screen if any event happens
public class Broadcaster : MonoBehaviour
{ 
    //Broadcast time if time not given
    [SerializeField] private float defaultBroadcastTime = 3f;
        
    //Text where broadcast message is shown
    [SerializeField] private Text broadcastText;
        
    //Lock to block two broadcasts in one time
    //If two messages are broadcasted, second is lost
    private bool broadcastLock;

    private void Start()
    {
        //Setting broadcaster to be globally available
        GameManager.Instance.broadcaster = this;
    }
        
    //Shows info, after the time given - it disappears
    private IEnumerator ShowOverTime(string message, float time)
    {
        broadcastText.gameObject.SetActive(true);
        broadcastLock = true;
            
        broadcastText.text = message;
        yield return new WaitForSeconds(time);
        broadcastText.text = "";
            
        broadcastLock = false;
        broadcastText.gameObject.SetActive(false);
    }

    //Broadcasts message for time given
    public void Broadcast(string message, float time = 0)
    {
        if (broadcastLock)
        {
            broadcastText.text = message;
            return;
        }
        
        if (time <= 0) 
            time = defaultBroadcastTime;
        
        StartCoroutine(ShowOverTime(message, time));
    }
}
