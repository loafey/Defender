using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defender {
    class MathExtra {
        public MathExtra() { }
        public static float Lerp(float n, float goal, float multiplier) {
            return n * (1 - multiplier) + goal * multiplier;
        }

        public static float GridLocation(int n, int size) {
            return n * size;
        }

        public static double GetDistance(double x1, double y1, double x2, double y2) {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
