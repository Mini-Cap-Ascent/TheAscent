using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    private Dictionary<char, string> rules;
   public string currentString;
    public string axiom = "F";
    public float angle;
    public int iterations = 1;

    void Start()
    {
        GenerateRules();
        currentString = axiom;

        for (int i = 0; i < iterations; i++)
        {
            currentString = ApplyRules(currentString);
        }

        Debug.Log(currentString); // For testing to see the generated string
    }

    void GenerateRules()
    {
        rules = new Dictionary<char, string>
        {
            {'F', "FF-[-F+F+F]+[+F-F-F]"} // Example rule for vine growth
        };
    }

    string ApplyRules(string input)
    {
        var output = new System.Text.StringBuilder();

        foreach (char c in input)
        {
            if (rules.TryGetValue(c, out string value))
            {
                output.Append(value);
            }
            else
            {
                output.Append(c.ToString());
            }
        }

        return output.ToString();
    }
}
