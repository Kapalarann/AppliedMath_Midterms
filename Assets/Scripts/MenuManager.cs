using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("To Activate")]
    public GameObject[] toActivate;

    [Header("References")]
    public CameraPan cameraPan; // Reference your CameraPan script here

    private bool Activated;

    void Update()
    {
        if (Activated) return;
        // Check for any mouse button click
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (cameraPan != null)
            {
                cameraPan.StartPan();
                Activate();
            }
            else
            {
                Debug.LogWarning("CameraPan reference is missing!");
            }
        }
    }

    void Activate()
    {
        foreach (GameObject item in toActivate)
        {
            if (item == null) continue;
            item.SetActive(true);
        }

        Activated = true;
    }
}