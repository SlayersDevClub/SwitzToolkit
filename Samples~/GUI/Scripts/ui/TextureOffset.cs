using UnityEngine;

public class TextureOffset : MonoBehaviour
{
    public RectTransform textureObject;
    public float scrollSpeed = .9f;
    Material textureObjectMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if(textureObjectMaterial == null)
        {
            textureObjectMaterial = textureObject.GetComponent<UnityEngine.UI.Image>().material;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        textureObjectMaterial.mainTextureOffset += new Vector2(scrollSpeed * 0.0001f, 0);
    }
}
