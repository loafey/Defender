﻿using System;
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

        public void Update(KeyboardState keyboardState, List<Block> blocks) {

            if (!onGround) {
                speedY += gravityForce;
            } else {
                speedY = 0;
            }

            if (keyboardState.IsKeyDown(Key.Left)) {
                speedX -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Key.Right)) {
                speedX += 0.5f;
            }

            speedX = MathHelper.Clamp(speedX, -5, 5);
            speedX = MathExtra.Lerp(speedX, 0, 0.2f);

            if(speedX < 0) {
                foreach(Block block in blocks) {
                    if(MathExtra.GetDistanceAxis(this.x, block.x) > 0 && MathExtra.GetDistanceAxis(this.x, block.x) < 16) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 1 && MathExtra.GetDistanceAxis(this.y, block.y) > -1) {
                            Console.WriteLine(MathExtra.GetDistanceAxis(this.x, block.x));
                            speedX = 0;
                            break;
                        }
                    }
                }
            }

            if (speedX > 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > -14 && MathExtra.GetDistanceAxis(this.x, block.x) < 14) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 1 && MathExtra.GetDistanceAxis(this.y, block.y) > -1) {
                            Console.WriteLine(MathExtra.GetDistanceAxis(this.x, block.x));
                            speedX = 0;
                            break;
                        }
                    }
                }
            }


            //Console.WriteLine(speedX);

            /*
            if (speedX > 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 4 && MathExtra.GetDistanceAxisAbs(this.y, block.y) > -4) {
                        //Console.WriteLine("y " + MathExtra.GetDistanceAxis(this.y, block.y));
                        Console.WriteLine("x " + MathExtra.GetDistanceAxisAbs(this.x, block.x));
                        if (MathExtra.GetDistanceAxisAbs(this.x, block.x) < 8) {
                            speedX = 0;
                            break;
                        }
                    }
                }
            }*/

            x += speedX;

            foreach(Block block in blocks) {
                //if(MathExtra.GetDistance(this.x, this.y, block.x, block.y) < 17) {
                if(MathExtra.GetDistanceAxisAbs(this.x, block.x) < 8){
                    if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16) {
                        //Console.WriteLine(MathExtra.GetDistanceAxisAbs(this.x, block.x));
                        speedY = 0;
                        this.y = block.y - this.height;
                        break;
                    }
                }
            }

            y += speedY;
        }
    }
}