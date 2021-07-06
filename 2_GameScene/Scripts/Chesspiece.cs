using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chesspiece : MonoBehaviour
{
    public int CurrentX{set;get;}
    public int CurrentY{set;get;}
    public bool isWhite;

    private Vector3 startMarker;
    private Vector3 endMarker;

    private float startTime;
    private float journeyLength;

    public float speed = 1f;

    public bool isAnimate = false;

    public void SetPosition(int x, int y){
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove(){
        return new bool[8,8]; //place holder
    }
    
    // // Start is called before the first frame update
    void Start()
    {
        startTime = 0f;
        journeyLength = 1f;
        startMarker = transform.position;
        endMarker = transform.position;
    }

    // // Update is called once per frame
    void Update()
    {
        if (isAnimate) {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
        }
    }

    public void SetDestination(Vector3 destination) {    

        startMarker = transform.position;
        endMarker = destination;

        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker, endMarker);
    }

    public void SetInvisible() {
        GameObject child = this.gameObject.transform.GetChild(0).gameObject;
        Destroy(child);
    }
}
