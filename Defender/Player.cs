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

        private bool singleSpacePress = false;

        public bool onGround = false; 
        public float gravityForce = 0.0982f;
        public float xSpeed = 0;
        public float ySpeed = 0;

        Random r = new Random();

        //public float textureScale = -0.0313f * 2;

        public void Draw() {
            float textureScale = this.width / (-this.width * this.width);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            //GL.Begin(PrimitiveType.Triangles);
            GL.Begin(PrimitiveType.Triangles);

            GL.Color4(1f, 1f, 1f, 1f);

            GL.TexCoord2(0, 0);                                             GL.Vertex2(x, y + height);
            GL.TexCoord2(width * textureScale, height * textureScale);      GL.Vertex2(x + width, y);
            GL.TexCoord2(0, height * textureScale);                         GL.Vertex2(x, y);

            GL.TexCoord2(0, 0);                                             GL.Vertex2(x, y + height);
            GL.TexCoord2(width * textureScale, 0);                          GL.Vertex2(x + width, y + height);
            GL.TexCoord2(width * textureScale, height * textureScale);      GL.Vertex2(x + width, y);
            GL.End();
        }

        void Collision(List<Block> blocks) {
            /*
            if (xSpeed < 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > 0 && MathExtra.GetDistanceAxis(this.x, block.x) < 16) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 12 && MathExtra.GetDistanceAxis(this.y, block.y) > -12) {
                            xSpeed = 0;
                            break;
                        }
                    }
                }
            }

            if (xSpeed > 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > -16 && MathExtra.GetDistanceAxis(this.x, block.x) < 0) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 14 && MathExtra.GetDistanceAxis(this.y, block.y) > -14) {
                            xSpeed = 0;
                            break;
                        }
                    }
                }
            }

            foreach (Block block in blocks) {
                if (MathExtra.GetDistanceAxisAbs(this.x + this.width / 2, block.x + block.width / 2) < 14) {
                    //if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16 && ySpeed > 0 ) {
                    if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16) {
                        if (ySpeed > 0) {
                            onGround = true;
                            if (MathExtra.GetDistanceAxisAbs(this.y + this.height, block.y) < 14) {
                                y = MathExtra.Lerp(y, block.y - this.height, 0.5f);
                            }
                        }
                        if(MathExtra.GetDistanceAxisAbs(this.y + this.height, block.y) > block.height) {
                            y = MathExtra.Lerp(y, block.y + block.height, 0.5f);
                            ySpeed = Math.Abs(ySpeed) / 5;
                        }
                        break;
                    } else {
                        onGround = false;
                    }
                }
            }
            x += xSpeed;
            */
            foreach (Block block in blocks) {
                if (xSpeed < 0) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > 0 && MathExtra.GetDistanceAxis(this.x, block.x) < 16) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 12 && MathExtra.GetDistanceAxis(this.y, block.y) > -12) {
                            xSpeed = 0;
                            break;
                        }
                    }
                }

                if (xSpeed > 0) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > -16 && MathExtra.GetDistanceAxis(this.x, block.x) < 0) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 14 && MathExtra.GetDistanceAxis(this.y, block.y) > -14) {
                            xSpeed = 0;
                            break;
                        }
                    }
                }

                if (MathExtra.GetDistanceAxisAbs(this.x + this.width / 2, block.x + block.width / 2) < 14) {
                    //if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16 && ySpeed > 0 ) {
                    if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16) {
                        if (ySpeed > 0) {
                            onGround = true;
                            if (MathExtra.GetDistanceAxisAbs(this.y + this.height, block.y) < 14) {
                                y = MathExtra.Lerp(y, block.y - this.height, 0.5f);
                            }
                        }
                        if (MathExtra.GetDistanceAxisAbs(this.y + this.height, block.y) > block.height) {
                            y = MathExtra.Lerp(y, block.y + block.height, 0.5f);
                            ySpeed = Math.Abs(ySpeed) / 5;
                        }
                        break;
                    } else {
                        onGround = false;
                    }
                }
            }
            x += xSpeed;
        }

        public void Update(KeyboardState keyboardState, List<Block> blocks) {
            if (keyboardState.IsKeyDown(Key.A)) {
                xSpeed -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Key.D)) {
                xSpeed += 0.5f;
            }

            xSpeed = MathHelper.Clamp(xSpeed, -5, 5);
            xSpeed = MathExtra.Lerp(xSpeed, 0, 0.2f);

            Collision(blocks);

            if (!onGround) {
                ySpeed += gravityForce;
            } else {
                ySpeed = 0;
            }

            if (keyboardState.IsKeyUp(Key.Space)) {
                singleSpacePress = false;
            }

            if (!singleSpacePress && keyboardState.IsKeyDown(Key.Space) && onGround) {
                singleSpacePress = true;
                ySpeed -= 2;
            }

            if (ySpeed != 0) {
                onGround = false;
            } else {
                onGround = true;
            }
            
            y += ySpeed;
        }
    }
}
