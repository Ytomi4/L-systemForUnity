using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] TurtleInterpretation _turtle = default;

    public float CameraSize { get; private set; }

    private void Update() {
        var poslist = _turtle.VertexPositions;
        float minX = 0, minY = 0, maxX = 0, maxY = 0;
        foreach(Vector3 pos in poslist) {
            minX = Mathf.Min(pos.x, minX);
            minY = Mathf.Min(pos.y, minY);
            maxX = Mathf.Max(pos.x, maxX);
            maxY = Mathf.Max(pos.y, maxY);
        }
        var center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0f);
        var offset = new Vector3(0f, 0f, -1f);
        transform.position = center + offset;

        float w = maxX - center.x;
        float h = maxY - center.y;
        CameraSize = (h / Screen.height > w / Screen.width) ? h : w * Screen.height / Screen.width;
    }
}
