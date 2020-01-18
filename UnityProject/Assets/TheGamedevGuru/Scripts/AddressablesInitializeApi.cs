using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesInitializeApi : MonoBehaviour
{
    void Start()
    {
        Addressables.InitializeAsync();
    }
}
