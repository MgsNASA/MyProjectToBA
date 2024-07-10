using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public string shapeName;  // ������ ���������
    public Sprite sprite;     // ������ ���������
    public Color colorSprite; // ������ ���������
    public Vector2 size;      // ������ ���������

    // ����������� ����� ��� ���������, ������� ������ ���� ���������� � ����������� �������
    public abstract void Draw( );
}
