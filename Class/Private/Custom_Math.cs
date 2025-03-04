using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Align
{
    static public class Custom_Math
    {
        static public double DegreeToRadian(double angle) { return Math.PI * angle / 180; }
        static public double RadianToDegree(double angle) { return angle * (180 / Math.PI); }
    }
}
