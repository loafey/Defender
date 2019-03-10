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
    }
}
