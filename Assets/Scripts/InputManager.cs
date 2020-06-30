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

        _initialOrder.onValueChanged.AddListener(delegate { SetValues(); });
        _rewritinRule.onValueChanged.AddListener(delegate { SetValues(); });
        _depth.onValueChanged.AddListener(delegate { SetValues(); });
        _angle.onValueChanged.AddListener(delegate { SetValues(); });
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
