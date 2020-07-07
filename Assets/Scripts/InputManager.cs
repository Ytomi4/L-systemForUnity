using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public string InitialOrder { get; private set; }
    public string[] RewritingRules { get; private set; } = new string[1];
    public int Depth { get; private set; }
    public float Angle { get; private set; }

    public Action UpdateInput;

    [SerializeField] protected InputField _initialOrder, _rewritinRule;
    [SerializeField] protected Dropdown _depth;
    [SerializeField] protected Slider _angle;
    [SerializeField] protected Text _angleText;

    private void OnEnable() {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif
        _initialOrder.text = "F";
        _rewritinRule.text = "F";
        _depth.value = 1;
        _angle.value = 90f;
        SetValues();

        _initialOrder.onValueChanged.AddListener(delegate { UpdateInput(); });
        _rewritinRule.onValueChanged.AddListener(delegate { UpdateInput(); });
        _depth.onValueChanged.AddListener(delegate { UpdateInput(); });
        _angle.onValueChanged.AddListener(delegate { UpdateInput(); });

        UpdateInput += SetValues;
    }

    private void SetValues() {
        InitialOrder = _initialOrder.text;
        RewritingRules[0] = _rewritinRule.text;
        Depth = _depth.value;
        Angle = _angle.value;

        _angleText.text = "angle = " + Angle;

        //Debug.Log("InitialOrder = " + InitialOrder + ", RewritingRule = " + RewritingRule + ", Depth = " + Depth + ", Angle = " + Angle);
    }
}
