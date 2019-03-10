using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace Defender {
    class Game {
        GameWindow mainWindow;
        Texture2D grassBlock;
        Texture2D dirtBlock;
        Texture2D stoneBlock;
        List<Block> BlockList = new List<Block>();

        float cameraX = 0,
              cameraY = 0;
        float cameraXSpeed = 0,
              cameraYSpeed = 0;
        float zoom = 1f,
              zoomZ = 1f;

        public Game(GameWindow mainWindow) {
            this.mainWindow = mainWindow;

            mainWindow.Load += MainWindow_Load;
            mainWindow.UpdateFrame += MainWindow_UpdateFrame;
            mainWindow.RenderFrame += MainWindow_RenderFrame;
            mainWindow.KeyDown += MainWindow_KeyDown;
            mainWindow.KeyPress += MainWindow_KeyPress;
            mainWindow.KeyUp += MainWindow_KeyUp;
        }

        private void MainWindow_KeyPress(object sender, KeyPressEventArgs e) {}
        private void MainWindow_KeyDown(object sender, KeyboardKeyEventArgs e) {}
        private void MainWindow_KeyUp(object sender, KeyboardKeyEventArgs e) {}

        private void MainWindow_Load(object sender, EventArgs e) {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            grassBlock = ContentPipe.LoadTexture("Content/grass.png");
            dirtBlock = ContentPipe.LoadTexture("Content/dirt.png");
            stoneBlock = ContentPipe.LoadTexture("Content/stone.png");

            BlockList.Add(new Block(32, 32, 0.0f, 0.0f, grassBlock.ID, "Grass"));

            BlockList.Add(new Block(32, 32, 32.0f, 0.0f, grassBlock.ID, "Grass"));
            BlockList.Add(new Block(32, 32, 128.0f, 0.0f, grassBlock.ID, "Grass"));

            BlockList.Add(new Block(32, 32, 0.0f, 64.0f, grassBlock.ID, "Grass"));
            BlockList.Add(new Block(32, 32, 32.0f, 96.0f, grassBlock.ID, "Grass"));
            BlockList.Add(new Block(32, 32, 64.0f, 96.0f, grassBlock.ID, "Grass"));
            BlockList.Add(new Block(32, 32, 96.0f, 96.0f, grassBlock.ID, "Grass"));
            BlockList.Add(new Block(32, 32, 128.0f, 96.0f, grassBlock.ID, "Grass"));
            BlockList.Add(new Block(32, 32, 160.0f, 64.0f, grassBlock.ID, "Grass"));

            Random r = new Random();

            for (int i = 0; i < 2048; i += 32) {
                float gridSnap = 32;
                float sinY = 256 + (float)Math.Sin(i) * r.Next(0,96);
                float yHeight = sinY - (sinY % gridSnap);

                BlockList.Add(new Block(32, 32, i, yHeight, grassBlock.ID, "Grass"));

                for (int z = (int)yHeight + 32; z < 2048; z += 32) {
                    if(z < (int)yHeight + 256) { 
                        BlockList.Add(new Block(32, 32, i, z, dirtBlock.ID, "Dirt"));
                    } else {
                        BlockList.Add(new Block(32, 32, i, z, stoneBlock.ID, "Stone"));
                    }
                }
            }
        }

        KeyboardState lastKeyState;
        private void MainWindow_UpdateFrame(object sender, FrameEventArgs e) {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.A)) {
                cameraXSpeed += 1;
            }
            if (keyState.IsKeyDown(Key.D)) {
                cameraXSpeed -= 1;
            }
            if (keyState.IsKeyDown(Key.W)) {
                cameraYSpeed += 1;
            }
            if (keyState.IsKeyDown(Key.S)) {
                cameraYSpeed -= 1;
            }
            cameraXSpeed = MathHelper.Clamp(cameraXSpeed, -10, 10);
            cameraYSpeed = MathHelper.Clamp(cameraYSpeed, -10, 10);
            cameraXSpeed = MathExtra.Lerp(cameraXSpeed, 0, 0.1f);
            cameraYSpeed = MathExtra.Lerp(cameraYSpeed, 0, 0.1f);

            if (keyState.IsKeyDown(Key.Up)) {
                zoom += 0.005f;
            }
            if (keyState.IsKeyDown(Key.Down)) {
                zoom -= 0.005f;
            }

            if (keyState.IsKeyDown(Key.Left)) {
                zoomZ += 0.005f;
            }
            if (keyState.IsKeyDown(Key.Right)) {
                zoomZ -= 0.005f;
            }

            cameraX += cameraXSpeed; cameraY += cameraYSpeed;

            if(keyState.IsKeyDown(Key.Escape) && lastKeyState.IsKeyUp(Key.Escape)) {
                Console.WriteLine("Escape!");
                mainWindow.Exit();
            }

            if (keyState.IsKeyDown(Key.Enter) && lastKeyState.IsKeyUp(Key.Enter)) {
                Console.WriteLine("Enter down!");}
            if (keyState.IsKeyUp(Key.Enter) && lastKeyState.IsKeyDown(Key.Enter)) {
                Console.WriteLine("Enter up!");
            }

            lastKeyState = keyState;
        }

        private void MainWindow_RenderFrame(object sender, FrameEventArgs e) {
            GL.ClearColor(Color.CornflowerBlue);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projMatrix = Matrix4.CreateOrthographicOffCenter(0, mainWindow.Width, mainWindow.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMatrix);

            Matrix4 modelViewMatrix =
                Matrix4.CreateScale(zoom, zoom, zoomZ) *
                Matrix4.CreateRotationZ(0f) *
                Matrix4.CreateTranslation(cameraX, cameraY, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            foreach(Block block in BlockList) {
                block.Draw();
            }

            mainWindow.SwapBuffers();
        }
    }
}
