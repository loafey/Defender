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

        private bool singleSpacePress = false;

        public bool onGround = false; 
        public float gravityForce = 0.0982f;
        public float speedX = 0;
        public float speedY = 0;

        Random r = new Random();

        //public float textureScale = -0.0313f * 2;

        public void Draw() {
            float textureScale = this.width / (-this.width * this.width);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            //GL.Begin(PrimitiveType.Triangles);
            GL.Begin(PrimitiveType.Triangles);

            GL.Color4(1f, 1f, 1f, 1f);

            GL.TexCoord2(0, 0);                       GL.Vertex2(x, y + height);
            GL.TexCoord2(width * textureScale, height * textureScale);      GL.Vertex2(x + width, y);
            GL.TexCoord2(0, height * textureScale);              GL.Vertex2(x, y);

            GL.TexCoord2(0, 0);                       GL.Vertex2(x, y + height);
            GL.TexCoord2(width * textureScale, 0);               GL.Vertex2(x + width, y + height);
            GL.TexCoord2(width * textureScale, height * textureScale);      GL.Vertex2(x + width, y);
            GL.End();
        }

        void Collision(List<Block> blocks) {
            //foreach(Block block in blocks) {
            //    if(MathExtra.PlayerBlockAABB(this, block)) {
            //        Console.WriteLine("Collision x:{0} y:{1}", MathExtra.GetDistanceAxis(this.x + this.width/2, block.x + block.width / 2), MathExtra.GetDistanceAxis(this.y, block.y));
            //        if (MathExtra.GetDistanceAxisAbs(this.x + this.width / 2, block.x + block.width / 2) < this.width) {
            //            if (MathExtra.GetDistanceAxisAbs(this.y + this.height / 2, block.y + block.height / 2) < this.height / 2) {
            //                Console.WriteLine("Collision stand on side {0}", MathExtra.GetDistanceAxis(this.x + this.width / 2, block.x + block.width / 2));
            //                bool direction; //false = left; true = right;
            //                if(MathExtra.GetDistanceAxis(this.x + this.width / 2, block.x + block.width / 2) < 0) {
            //                    direction = false;
            //                } else {
            //                    direction = true;
            //                }
            //                if (speedX > 0 && !direction) {
            //                    Console.WriteLine("Why can i move");
            //                    speedX = 0;
            //                }
            //                if (speedX < 0 && direction) {
            //                    speedX = 0;
            //                }
            //            }
            //        }

            //        Console.WriteLine("Collision!");

            //        if (MathExtra.GetDistanceAxis(this.y, block.y) > -this.height && MathExtra.GetDistanceAxis(this.y, block.y) < -this.height + 2) {
            //            this.y = block.y - this.height;
            //            onGround = true;
            //            break;
            //        }
            //    } else {
            //        onGround = false;
            //    }
            //}
            //x += speedX;
            if (speedX < 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > 0 && MathExtra.GetDistanceAxis(this.x, block.x) < 16) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 12 && MathExtra.GetDistanceAxis(this.y, block.y) > -12) {
                            speedX = 0;
                            break;
                        }
                    }
                }
            }

            if (speedX > 0) {
                foreach (Block block in blocks) {
                    if (MathExtra.GetDistanceAxis(this.x, block.x) > -15 && MathExtra.GetDistanceAxis(this.x, block.x) < 15) {
                        if (MathExtra.GetDistanceAxis(this.y, block.y) < 14 && MathExtra.GetDistanceAxis(this.y, block.y) > -14) {
                            speedX = 0;
                            break;
                        }
                    }
                }
            }

            foreach (Block block in blocks) {
                if (MathExtra.GetDistanceAxisAbs(this.x + this.width / 2, block.x + block.width / 2) < 14) {
                    if (MathExtra.GetDistanceAxisAbs(this.y, block.y) < 16) {
                        if (speedY > 0) {
                            onGround = true;
                            if (MathExtra.GetDistanceAxisAbs(this.y + this.height, block.y) < 14) {
                                this.y = block.y - this.height;
                            }
                            break;
                        } else {
                            speedY = 0f;
                            y += 0.4f;
                            break;
                        }
                    } else {
                        onGround = false;
                    }
                }
            }
            x += speedX;
        }

        public void Update(KeyboardState keyboardState, List<Block> blocks) {
            if (keyboardState.IsKeyDown(Key.A)) {
                speedX -= 0.5f;
            }
            if (keyboardState.IsKeyDown(Key.D)) {
                speedX += 0.5f;
            }

            speedX = MathHelper.Clamp(speedX, -5, 5);
            speedX = MathExtra.Lerp(speedX, 0, 0.2f);

            Collision(blocks);

            if (!onGround) {
                speedY += gravityForce;
            } else {
                speedY = 0;
            }

            if (keyboardState.IsKeyUp(Key.Space)) {
                singleSpacePress = false;
            }

            if (!singleSpacePress && keyboardState.IsKeyDown(Key.Space) && onGround) {
                singleSpacePress = true;
                speedY -= 2;
            }

            if (speedY != 0) {
                onGround = false;
            } else {
                onGround = true;
            }
            
            y += speedY;
        }
    }
}
