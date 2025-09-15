using UnityEngine;
using System.Collections.Generic;

public struct Hex
{
    public int q;
    public int r;
    public int s;

    public Hex(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = -q - r;
    }

    public static bool operator ==(Hex a, Hex b)
    {
        return (a.q == b.q && a.r == b.r);
    }
    
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
    
        Hex other = (Hex)obj;
        return q == other.q && r == other.r;
    }
    
    public static bool operator !=(Hex a, Hex b) { return !(a == b); }
    
    public static Hex operator +(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r);
    }

    public static Hex operator -(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r);
    }

    public static Hex operator *(Hex hex, int k)
    {
        return new Hex(hex.q * k, hex.r * k);
    }

    public static Hex[] Directions = {
        new Hex(1, 0), new Hex(1, -1), new Hex(0, -1),
        new Hex(-1, 0), new Hex(-1, 1), new Hex(0, 1)
    };

    public static Hex Direction(int dir)
    {
        dir = Mathf.Clamp(dir, 0, 5);
        return Directions[dir];
    }

    public Hex neighbor(int dir)
    {
        return this+Direction(dir);
    }
    
    public List<Hex> neighbors()
    {
        List<Hex> ret = new List<Hex>();
        for (int i = 0; i < 6; i++)
        {
            ret.Add(neighbor(i));
        }
        return ret;
    }
    
    public override string ToString()
    {
        return $"({q}, {r}, {s})";
    }
    
    public override int GetHashCode() {
        unchecked {
            int hash = 17;
            hash = hash * 23 + q.GetHashCode();
            hash = hash * 23 + r.GetHashCode();
            return hash;
        }
    }
}
