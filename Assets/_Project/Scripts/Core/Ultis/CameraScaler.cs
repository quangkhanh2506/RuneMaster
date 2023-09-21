using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float minOrthoSize = 7.8f;
    public float maxOrthoSize = 8.5f;

    [SerializeField] private float defaultOrthoSize = 7.8f;
    private float curWidth;


    void Start()
    {
        curWidth = 0;
    }

    private void Update()
    {
        var width = Screen.width;
        if (curWidth != width)
        {
            float masterRatio = 16f / 9f;
            float ratio = (float)Screen.height / Screen.width;
            float orthoSizeValue = Mathf.Clamp(defaultOrthoSize * ratio / masterRatio, minOrthoSize, maxOrthoSize);
            Camera.main.orthographicSize = orthoSizeValue;
            curWidth = width;
        }
    }
}