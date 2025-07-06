// Assets/Scripts/Data/LevelConfig.cs
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelConfig",
    menuName = "MemoriaLudica/LevelConfig",
    order = 100)]
public class LevelConfig : ScriptableObject
{
    [Header("Escolha o Tema desta Fase")]
    public SpriteCategory spriteCategory;

    [Header("Quantos pares usar (X pares = 2X cartas)")]
    [Min(1)] public int pairsCount = 2;

    [Header("Quanto tempo mostrar no início (segundos)")]
    public float previewDuration = 2f;
}
