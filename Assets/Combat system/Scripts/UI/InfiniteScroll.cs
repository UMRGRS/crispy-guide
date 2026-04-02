using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.05f;
    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    { 
        float offset = Time.time * scrollSpeed;
        rawImage.uvRect = new Rect(offset, 0f, 1f, 1f);
    }
}
