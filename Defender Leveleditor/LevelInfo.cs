using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defender_Leveleditor {
    class LevelInfo {
        public List<Block> blocks;
        public LevelInfo(List<Block> Blocks) {
            this.blocks = Blocks;
        }
    }
}
