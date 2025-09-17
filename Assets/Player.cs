using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager.ColorData playerColor;
    public MeshRenderer mesh;
    void Start()
    {
        SetColor();
    }


    public void SetColor()
    {
        try
        {
            mesh.material = GameManager.instance.GetColor(playerColor);
        }
        catch
        {

        }
    }
    private void OnValidate()
    {
        SetColor();
    }
}
