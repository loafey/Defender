using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defender_Leveleditor {
    class Block {
        public int x;
        public int y;
        public string textureLocation;
        public Block(int X, int Y, string TextureLocation) {
            this.x = X;
            this.y = Y;
            this.textureLocation = TextureLocation;
        }
    }
}
