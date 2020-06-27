using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInterpretation : MonoBehaviour
{
    [SerializeField] protected string _initialOrder = "F";
    [SerializeField] protected string[] _rewritingRules;
    [SerializeField, Range(0.1f, 90f)] protected float _angle = 90;
    [SerializeField, Range(0, 4)] protected int _depth = 1;
    [SerializeField] Color color = Color.black;

    protected float _length = 1f;
    protected List<char> _order = default;

    Material lineMat;

    private void OnEnable() {
        var shader = Shader.Find("Hidden/Internal-Colored");
        if(shader == null) {
            Debug.Log("Shader Hidden/Internal-Colored not found");
        }
        lineMat = new Material(shader);

        _order = ComputeOrder(_initialOrder, _rewritingRules, _depth);
    }

    private void OnRenderObject() {
        lineMat.SetColor("_Color", color);
        lineMat.SetPass(0);

        DrawLine(_order, _length, _angle);
    }

    private void OnValidate() {
        _order = ComputeOrder(_initialOrder, _rewritingRules, _depth);
    }

    void DrawLine(List<char> orders, float length, float angle) {
        Matrix4x4 current = transform.localToWorldMatrix;

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
                    
            }
        }
    }

    List<char> ComputeOrder(string initialOrder, string[] rewritingRules, int depth) {
        List<char> orders = new List<char>();
        for (int i = 0; i < depth; i++) {
            orders = RewritingOrder(rewritingRules, initialOrder);
        }
        return orders;
    }

    List<char> RewritingOrder(string[] rewritingRules, string previousOrders) {
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
