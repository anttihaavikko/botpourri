using UnityEditor.Tilemaps;
using UnityEngine;

public class PathLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    public int Count => line.positionCount;

    public void AddPoint(Vector3 pos)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, pos);
    }

    public Vector3 GetPoint(int index)
    {
        return line.GetPosition(index);
    }

    public bool Intersects(Vector3 start, Vector3 end)
    {
        for (var i = 0; i < line.positionCount - 1; i++)
        {
            if (LineIntersects(start, end, line.GetPosition(i), line.GetPosition(i + 1)))
            {
                return true;
            }
        }

        return false;
    }
    
    private bool LineIntersects(Vector2 line1point1, Vector2 line1point2, Vector2 line2point1, Vector2 line2point2) {
 
        var a = line1point2 - line1point1;
        var b = line2point1 - line2point2;
        var c = line1point1 - line2point1;
 
        var alphaNumerator = b.y * c.x - b.x * c.y;
        var betaNumerator = a.x * c.y - a.y * c.x;
        var denominator = a.y * b.x - a.x * b.y;
 
        if (denominator == 0) {
            return false;
        }
        
        if (denominator > 0) {
            if (alphaNumerator < 0.1f || alphaNumerator > denominator || betaNumerator < 0.1f || betaNumerator > denominator) {
                return false;
            }
        } else if (alphaNumerator > -0.1f || alphaNumerator < denominator || betaNumerator > -0.1f || betaNumerator < denominator) {
            return false;
        }
        
        return true;
    }
}