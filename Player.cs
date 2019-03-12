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

        public void Update(KeyboardState keyboardState, List<Block> blocks, Texture2D testTexture, Texture2D testTexture2) {
            if (keyboardState.IsKeyDown(Key.A)) {
                speedX -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Key.D)) {
                speedX += 0.5f;
            }

            speedX = MathHelper.Clamp(speedX, -5, 5);
            speedX = MathExtra.Lerp(speedX, 0, 0.2f);

            if(speedX < 0) {
                foreach(Block block in blocks) {
                    if(MathExtra.GetDistanceAxis(this.x, block.x) > 0 && MathExtra.GetDistanceAxis(this.x, block.x) < 16) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 8 && MathExtra.GetDistanceAxis(this.y, block.y) > -8) {
                            speedX = 0;
                            break;
                        }
                    }
                }
            }

            if (speedX > 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > -14 && MathExtra.GetDistanceAxis(this.x, block.x) < 14) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 8 && MathExtra.GetDistanceAxis(this.y, block.y) > -8) {
                            speedX = 0;
                            break;
                        }
                    }
                }
            }

            x += speedX;

            foreach(Block block in blocks) {
                if (MathExtra.GetDistanceAxisAbs(this.x + this.width / 2, block.x + block.width / 2) < 14) {
                    if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16) {
                        block.textureID = testTexture.ID;
                        //Console.WriteLine(MathExtra.GetDistanceAxisAbs(this.y, block.y));
                        speedY = 0;
                        onGround = true;
                        this.y = block.y - this.height - 0.001f;
                        break;
                    } else {
                        //set texture to grass;
                        block.textureID = testTexture2.ID;
                        onGround = false;
                    }
                }
            }

            if (!onGround) {
                speedY += gravityForce;
            } else {
                speedY = 0;
            }

            if (keyboardState.IsKeyDown(Key.Space)) {
                if (onGround) {
                    speedY -= 2;
                }
            }


            if (speedY != 0) {
                onGround = false;
            } else {
                onGround = true;
            }
            
            //Console.WriteLine(onGround);

            y += speedY;
        }
    }
}
