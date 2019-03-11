using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Defender {
    class Player {
        public float x, y;
        public float width, height;
        public int textureID;
        public Player(float X, float Y, float Width, float Height, int TextureID) {
            this.x = X;
            this.y = Y;
            this.width = Width;
            this.height = Height;
            this.textureID = TextureID;
        }

        public bool onGround = false;
        public float gravityForce = 0.0982f;
        public float speedX = 0;
        public float speedY = 0;

        public float textureScale = -0.0313f * 2;

        public void Draw() {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(PrimitiveType.Triangles);

            GL.Color4(1f, 1f, 1f, 1f);

            GL.TexCoord2(1 * textureScale,              y * textureScale);                       GL.Vertex2(x, y + height);
            GL.TexCoord2((1 + width) * textureScale,    (y + height) * textureScale);            GL.Vertex2(x + width, y);
            GL.TexCoord2(1 * textureScale,              (y + height) * textureScale);            GL.Vertex2(x, y);

            GL.TexCoord2(1 * textureScale,              y * textureScale);                       GL.Vertex2(x, y + height);
            GL.TexCoord2((1 + width) * textureScale,    y * textureScale);                       GL.Vertex2(x + width, y + height);
            GL.TexCoord2((1 + width) * textureScale,    (y + height) * textureScale);            GL.Vertex2(x + width, y);

            GL.End();
        }

        public void Update(KeyboardState keyboardState) {
            if (!onGround) {
                speedY += gravityForce;
            } else {
                speedY = 0;
            }

            if (keyboardState.IsKeyDown(Key.Left)) {
                speedX -= 1;
            }
            if (keyboardState.IsKeyDown(Key.Right)) {
                speedX += 1;
            }

            speedX = MathHelper.Clamp(speedX, -10, 10);
            speedX = MathExtra.Lerp(speedX, 0, 0.2f);

            x += speedX;

            //y += speedY;
        }
    }
}
