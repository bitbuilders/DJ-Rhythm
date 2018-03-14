using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBackground : MonoBehaviour
{
    
    void Start()
    {
        ResizeSpriteToScreen();
    }
    
    void Update()
    {
        ResizeSpriteToScreen();
    }

    void ResizeSpriteToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        
        float newWidth = worldScreenWidth / width;
        float newHeight = worldScreenHeight / height;
        transform.localScale = new Vector3(newWidth + 0.1f, newHeight + 0.1f, 1.0f);
    }
}
