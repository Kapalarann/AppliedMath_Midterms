using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public static SpaceManager instance;

    public List<Space> spaces = new List<Space>();

    private void Awake()
    {
        instance = this;

        foreach (Space s in GetComponentsInChildren<Space>())
        {
            spaces.Add(s);
        }
    }
}
