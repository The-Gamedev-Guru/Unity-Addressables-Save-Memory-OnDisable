using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace TheGamedevGuru
{
public class Image_ManageMemoryLifecycle : MonoBehaviour
{
  [SerializeField] private Image image = null;
  [SerializeField] private AssetReferenceSprite spriteReference = null;

  void OnEnable()
  {
    spriteReference.LoadAssetAsync<Sprite>().Completed += handle => image.sprite = handle.Result;
  }
  void OnDisable()
  {
    var sprite = image.sprite;
    image.sprite = null;
    Addressables.Release(sprite);
  }
}
}
