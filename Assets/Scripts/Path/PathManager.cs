using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager instance;

    public List<Path> paths = new List<Path>();

    private void Awake()
    {
        instance=this;

        foreach (Path p in GetComponentsInChildren<Path>())
        {
            paths.Add(p);
        }
    }
}
