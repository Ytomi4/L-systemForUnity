using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpretation : MonoBehaviour {
    [SerializeField] protected string _initialOrder = "F";
    [SerializeField] protected string[] _rewritingRules;
    [SerializeField, Range(0.1f, 90f)] protected float _angle = 90;
    [SerializeField, Range(0, 4)] protected int _depth = 1;
    [SerializeField] Color _color = Color.black;

    [SerializeField] bool _userInputEnable = true;
    [SerializeField] InputManager _input = default;

    //全体をカメラに移すため、全頂点の位置を用意しておく
    public List<Vector3> VertexPositions { get; private set; }

    protected float _length = 1f;
    protected List<char> _order = default;

    Material lineMat;

    private void OnEnable() {
        var shader = Shader.Find("Hidden/Internal-Colored");
        if(shader == null) {
            Debug.Log("Shader Hidden/Internal-Colored not found");
        }
        lineMat = new Material(shader);

        VertexPositions = new List<Vector3>();

        if (_userInputEnable) {
            _order = ComputeOrder(_input.InitialOrder, _input.RewritingRules, _input.Depth);
            _input.UpdateInput += OnInputeValueChanged;
        } else {
            _order = ComputeOrder(_initialOrder, _rewritingRules, _depth);
        }
    }

    private void OnRenderObject() {
        lineMat.SetColor("_Color", _color);
        lineMat.SetPass(0);

        VertexPositions.Clear();

        if (_userInputEnable) {
            DrawLine(_order, _length, _input.Angle);
        } else {
            DrawLine(_order, _length, _angle);
        }
    }

    private void OnValidate() {
        _order = ComputeOrder(_initialOrder, _rewritingRules, _depth);
    }

    private void OnInputeValueChanged() {
        _order = ComputeOrder(_input.InitialOrder, _input.RewritingRules, _input.Depth);
    }

    void DrawLine(List<char> orders, float length, float angle) {
        List<Matrix4x4> roots = new List<Matrix4x4>();
        roots.Add(transform.localToWorldMatrix);
        Matrix4x4 current = roots[roots.Count - 1];
        roots.RemoveAt(roots.Count - 1);
        VertexPositions.Add(new Vector3(current.m03, current.m13, current.m23));

        foreach(char c in orders) {
            switch (c) {
                case 'F':
                    GL.PushMatrix();
                    GL.MultMatrix(current);
                    GL.Begin(GL.LINES);
                    GL.Vertex(Vector3.zero);
                    GL.Vertex(new Vector3(0f, length, 0f));
                    GL.End();
                    GL.PopMatrix();
                    current *= Matrix4x4.Translate(new Vector3(0f, length, 0f));
                    VertexPositions.Add(new Vector3(current.m03, current.m13, current.m23));
                    break;
                case 'f':
                    current *= Matrix4x4.Translate(new Vector3(0f, length, 0f));
                    break;
                case '+':
                    current *= Matrix4x4.Rotate(Quaternion.AngleAxis(-angle, Vector3.forward));
                    break;
                case '-':
                    current *= Matrix4x4.Rotate(Quaternion.AngleAxis(angle, Vector3.forward));
                    break;
                case '[':
                    roots.Add(current);
                    break;
                case ']':
                    current = roots[roots.Count - 1];
                    roots.RemoveAt(roots.Count - 1);
                    break;
                    
            }
        }
    }

    List<char> ComputeOrder(string initialOrder, string[] rewritingRules, int depth) {
        List<char> orders = new List<char>();

        foreach (char c in initialOrder)
            orders.Add(c);

        for (int i = 0; i < depth; i++) {
            orders = RewritingOrder(rewritingRules, orders);
        }
        return orders;
    }

    List<char> RewritingOrder(string[] rewritingRules, List<char> previousOrders) {
        List<char> currentOrders = new List<char>();
        
        foreach(char c in previousOrders) {
            switch (c) {
                case 'F':
                    if(rewritingRules.Length > 0) {
                        foreach(char d in rewritingRules[0]) {
                            currentOrders.Add(d);
                        }
                    } else {
                        currentOrders.Add('F');
                    }
                    break;
                case 'f':
                    if (rewritingRules.Length > 1) {
                        foreach(char d in rewritingRules[1]) {
                            currentOrders.Add(d);
                        }
                    } else {
                        currentOrders.Add('f');
                    }
                    break;
                default:
                    currentOrders.Add(c);
                    break;
            }
        }
        return currentOrders;
    }
}
