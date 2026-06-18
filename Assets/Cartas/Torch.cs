using UnityEngine;

public class TorchObject : MonoBehaviour
{
    public Light torchLight;

    bool extinguished = false;

    public void Extinguish()
    {
        if (extinguished)
            return;

        extinguished = true;

        if (torchLight != null)
        {
            torchLight.enabled = false;
        }
    }
}