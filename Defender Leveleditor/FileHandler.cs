using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Defender_Leveleditor {
    class FileHandler {
        public static LevelInfo OpenFile(string FileName) {
            StreamReader streamReader = new StreamReader(FileName);

            string text = streamReader.ReadToEnd();
            string[] lines = text.Split(';');

            List<Block> blocks = new List<Block>();

            foreach(string line in lines) {
                string[] info = line.Split('|');
                if (info[0].Contains("block")) {
                    int x = int.Parse(info[1]);
                    int y = int.Parse(info[2]);
                    string textureLocation = info[3];
                    Block block = new Block(
                        x,
                        y,
                        textureLocation
                    );
                    blocks.Add(block);
                }
            }

            streamReader.Close();

            LevelInfo levelInfo = new LevelInfo(
                blocks    
            );
            return levelInfo;
        }
    }
}
