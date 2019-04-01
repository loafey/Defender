using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;


namespace Defender_Leveleditor {
    class BlockInfo {
        public int x;
        public int y;
        public string texture;
        public int arrayIndex;
        public Rectangle block;
        public BlockInfo(int X, int Y, string Texture, int ArrayIndex, Rectangle Block) {
            x = X;
            y = Y;
            block = Block;
            texture = Texture;
            arrayIndex = ArrayIndex;
        }
    }
}
