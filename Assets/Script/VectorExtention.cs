using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public static class VectorExtention
    {
        public static void SetX(this Vector3 v, float x)
        {
            v.Set(x, v.y, v.z);
        }

        public static void SetY(this Vector3 v, float y)
        {
            v.Set(v.x, y, v.z);
        }

        public static void SetZ(this Vector3 v, float z)
        {
            v.Set(v.x, v.y, z);
        }
    }
}
