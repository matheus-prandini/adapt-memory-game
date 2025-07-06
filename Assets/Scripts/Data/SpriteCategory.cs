using UnityEngine;

[CreateAssetMenu(
    fileName = "SpriteCategory",
    menuName = "MemoriaLudica/SpriteCategory",
    order = 50)]
public class SpriteCategory : ScriptableObject
{
    [Header("Identificador do Tema")]
    public string    categoryName;
    [Header("Sprites deste Tema")]
    public Sprite[]  sprites;
}
