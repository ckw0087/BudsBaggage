using UnityEngine;

public class Luggage : MonoBehaviour
{
    public bool Collected { get; private set; } = false;

    public void Collect()
    {
        Collected = true;
    }
}
