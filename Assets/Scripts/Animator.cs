using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for character movement from start to end animation
public class Animator : MonoBehaviour
{
    //How long in seconds move from tile to neighbor tile takes
    [SerializeField] private float singleAnimationDuration;
    
    //Animator for player character
    [SerializeField] private UnityEngine.Animator anim;
    
    //If animation is now running
    public bool AnimationInProgress { get; private set; }
    
    //If part of animation is running (from tile to it's neighbor)
    private bool _moveInProgress;
    
    //Value from 0-1 that shows which part of whole path player passed
    public float Progress { get; private set; }
    
    //Coroutine for whole animation
    private Coroutine _mainAnimationCoroutine;
    
    //Coroutine for single animation step
    private Coroutine _stepAnimationCoroutine;
    
    //Animation property that decide if character is idle or run
    private static readonly int IsRunning = UnityEngine.Animator.StringToHash("isRunning");
    
    void Start()
    {
        //Setting animator
        GameManager.Instance.animator = this;
    }

    void Update()
    {
        //If animation is not running - player stay on start
        var position = GameManager.Instance.mapDriver.PlacementToPosition(
            GameManager.Instance.mapDriver.startTilePlacement);
        if (position != gameObject.transform.position && !AnimationInProgress)
            ResetPlayerPosition();
    }
    
    //Run whole animation where player makes partial movement from step to step
    public void RunAnimation(List<Vector2Int> keyPositions)
    {
        //Character starts to run
        anim.SetBool(IsRunning, true);
        
        //Only one animation can run in the same time
        if (AnimationInProgress) return;
        
        //Before animating player must be on start
        ResetPlayerPosition();
        
        //Runs animation
        _mainAnimationCoroutine = StartCoroutine(AnimateWalk(keyPositions));
    }
    
    //Stops the animation
    public void StopAnimation()
    {
        //Reset progress and progressbar
        Progress = 0;
        
        //Reset flags
        AnimationInProgress = false;
        _moveInProgress = false;
        
        //Character is now idle
        anim.SetBool(IsRunning, false);
        
        //Stop all working coroutines
        if(_mainAnimationCoroutine != null)
            StopCoroutine(_mainAnimationCoroutine);
        if(_stepAnimationCoroutine != null)
            StopCoroutine(_stepAnimationCoroutine);
        
        //Set player on start
        ResetPlayerPosition();
    }

    //Sets player's position to spawn
    private void ResetPlayerPosition()
    {
        gameObject.transform.position = GameManager.Instance.mapDriver.PlacementToPosition(
            GameManager.Instance.mapDriver.startTilePlacement);
    }
    
    //Start animation in steps, each step waits for previous
    private IEnumerator AnimateWalk(IReadOnlyList<Vector2Int> keyPositions)
    {
        //Set animation flag
        AnimationInProgress = true;
        
        //Set player position to first keyPosition
        gameObject.transform.position = GameManager.Instance.mapDriver.PlacementToPosition(keyPositions[0]);
        
        //Iterates over steps and run single animations from tile to tile
        for (var i = 1; i < keyPositions.Count; i++)
        {
            //Update progress
            Progress = (float)i / keyPositions.Count;
            
            //Run single animation from current position to neighbor tile
            _stepAnimationCoroutine = StartCoroutine(
                AnimateSingleMove(keyPositions[i-1], keyPositions[i]));
            
            //Wait for end of the step animation
            while (_moveInProgress) 
                yield return new WaitForSeconds(0.05f);
        }

        //Disable animation flag
        AnimationInProgress = false;
        
        //Character is now idle
        anim.SetBool(IsRunning, false);
        
        //Reset progress
        Progress = 0;
    }

    //Animates single step from tile to it's neighbor
    private IEnumerator AnimateSingleMove(Vector2Int startPlacement, Vector2Int targetPlacement)
    {
        //Step is now animating
        _moveInProgress = true;
        
        //Get start and end world positions
        var start = gameObject.transform.position;
        var end = GameManager.Instance.mapDriver.PlacementToPosition(targetPlacement);
        
        //Time of the animation
        var elapsedTime = 0f;
        
        //Rotates player to the direction
        UpdateRotationForPlayer(startPlacement, targetPlacement);

        //Step animation works for time given
        while (elapsedTime < singleAnimationDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / singleAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Be sure that animation stopped in the end position
        gameObject.transform.position = end;
        
        //Step stopped animating
        _moveInProgress = false;
    }

    //Updates rotation for the player
    private void UpdateRotationForPlayer(Vector2Int startPlacement, Vector2Int targetPlacement)
    {
        //Decides the angle to rotate
        var rotAngle = 0;
        if (startPlacement.x != targetPlacement.x) // Left or right
            rotAngle = startPlacement.x - targetPlacement.x >= 0 ? -90 : 90;
        else if (startPlacement.y != targetPlacement.y) // Up or down
            rotAngle = startPlacement.y - targetPlacement.y >= 0 ? 180 : 0;
        
        //Change character rotation
        var currentRot = transform.rotation;
        transform.rotation = Quaternion.Euler(currentRot.eulerAngles.x, rotAngle, currentRot.eulerAngles.z);
    }
}
