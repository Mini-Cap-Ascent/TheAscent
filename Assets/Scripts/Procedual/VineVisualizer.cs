using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineVisualizer : MonoBehaviour
{
    public GameObject vineSegmentPrefab; // Assign a prefab with a MeshRenderer
    public LSystem lSystemGenerator; // Reference to the L-System generator
    public float segmentLength = 1.0f;
    public float segmentWidth = 0.1f;

    private Stack<TransformInfo> transformStack;
    private Vector3 initialPosition;

    void Start()
    {
        lSystemGenerator = GetComponent<LSystem>();
        StartCoroutine(GenerateVine());
    }

    IEnumerator GenerateVine()
    {
        transformStack = new Stack<TransformInfo>();
        initialPosition = transform.position;

        Vector3 position = initialPosition;
        Vector3 direction = Vector3.up;
        Quaternion rotation = Quaternion.identity;

        foreach (char c in lSystemGenerator.currentString)
        {
            yield return new WaitForSeconds(0.1f); // Slows down the generation for visualization

            switch (c)
            {
                case 'F':
                    // Create a vine segment
                    GameObject segment = Instantiate(vineSegmentPrefab, position, rotation);
                    segment.transform.localScale = new Vector3(segmentWidth, segmentLength, segmentWidth);
                    position += direction * segmentLength;
                    break;

                case '+': // Turn right
                    rotation *= Quaternion.Euler(Vector3.forward * lSystemGenerator.angle);
                    break;

                case '-': // Turn left
                    rotation *= Quaternion.Euler(Vector3.forward * -lSystemGenerator.angle);
                    break;

                case '[': // Save state
                    transformStack.Push(new TransformInfo()
                    {
                        position = position,
                        rotation = rotation
                    });
                    break;

                case ']': // Load state
                    if (transformStack.Count > 0)
                    {
                        TransformInfo ti = transformStack.Pop();
                        position = ti.position;
                        rotation = ti.rotation;
                    }
                    else
                    {
                        Debug.LogWarning("Stack is empty");
                    }
                    break;
            }
        }
    }

    private struct TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
    }
}
