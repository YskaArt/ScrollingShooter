using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;
    private Renderer rend;
    private Vector2 offset;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        offset.y += scrollSpeed * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
    }
}
