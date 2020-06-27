using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private GameObject _target = default;
    [SerializeField, Range(1f, 2f)] private float _sizeMagnify = 1.3f;

    Camera _cam;

    private void OnEnable() {
        _cam = GetComponent<Camera>();
    }

    private void Update() {
        iTween.MoveUpdate(this.gameObject, iTween.Hash(
            "position", _target.transform.position,
            "time", 0.2f));
        _cam.orthographicSize = _target.GetComponent<CameraTarget>().CameraSize * _sizeMagnify;
    }
}
