using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Defender {
    class Block {
        public float x, y;
        public float width, height;
        public int textureID;
        public string blockType;

        public Block(float width, float height, float x, float y, int TextureID, string BlockType) {
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
            this.textureID = TextureID;
            this.blockType = BlockType;
        }

        public float textureScale = 0.0313f;

        public void Draw() {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(PrimitiveType.Triangles);

            GL.Color4(1f, 1f, 1f, 1f);

            GL.TexCoord2(x * textureScale, y * textureScale); GL.Vertex2(x, y + height);
            GL.TexCoord2((x + width) * textureScale, (y + height) * textureScale); GL.Vertex2(x + width, y);
            GL.TexCoord2(x * textureScale, (y + height) * textureScale); GL.Vertex2(x, y);

            GL.TexCoord2(x * textureScale, y * textureScale); GL.Vertex2(x, y + height);
            GL.TexCoord2((x + width) * textureScale, y * textureScale); GL.Vertex2(x + width, y + height);
            GL.TexCoord2((x + width) * textureScale, (y + height) * textureScale); GL.Vertex2(x + width, y);

            /*
            GL.TexCoord2(x, y); GL.Vertex2(x, y+height);
            GL.TexCoord2(x+width, y + height); GL.Vertex2(x + width, y);
            GL.TexCoord2(x, y + height); GL.Vertex2(x, y);

            GL.TexCoord2(x, y); GL.Vertex2(x, y + height);
            GL.TexCoord2(x + width, y); GL.Vertex2(x + width, y + height);
            GL.TexCoord2(x + width, y + height); GL.Vertex2(x + width, y);
            */

            GL.End();
        }
    }
}
