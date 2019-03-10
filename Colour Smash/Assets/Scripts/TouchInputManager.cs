using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    public static TouchInputManager instance;
    private Vector2 swipeStart, swipeDelta;
    private bool swipeRight, swipeLeft, swipeUp, swipeDown, isDragging;
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    // Start is called before the first frame update
    void Start()
    {
        if(!instance)
            instance = this;
        swipeRight = swipeLeft = swipeUp = swipeDown = isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        SwipeCheck(); 
    }


    void SwipeCheck()
    {
        swipeRight = swipeLeft = swipeUp = swipeDown = false;
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDragging = true;
                swipeStart = Input.touches[0].position;
            }
            else if ((Input.touches[0].phase == TouchPhase.Ended) || (Input.touches[0].phase == TouchPhase.Canceled))
            {
                Reset();
            }
        }

        if (isDragging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - swipeStart;
            }
        }

        if (swipeDelta.magnitude >= 50)
        {
            if (Mathf.Abs(swipeDelta.x) > (Mathf.Abs(swipeDelta.y)))
            {
                if (swipeDelta.x > 0)
                    swipeRight = true;
                else
                    swipeLeft = true;
            }
            else
            {
                if (swipeDelta.y > 0)
                    swipeUp = true;
                else
                    swipeDown = true;
            }
            Reset();
        }
    }
    void Reset()
    {
        swipeStart = swipeDelta = Vector2.zero;
        isDragging = false;
    }
}
