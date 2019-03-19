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

        Texture2D grassBlockSprite;
        Texture2D dirtBlockSprite;
        Texture2D stoneBlockSprite;
        Texture2D testBlockSprite;
        Texture2D playerSprite;

        List<Block> blockList = new List<Block>();
        List<Player> playerList = new List<Player>();

        bool freeCam = false;

        float cameraX = 0,
              cameraY = 0;
        float cameraXSpeed = 0,
              cameraYSpeed = 0;
        private float zoom = 1f;
        private float zoomZ = 1f;

        int cameraOffsetX = 0; int cameraOffsetY = 0;

        public Game(GameWindow mainWindow) {
            this.mainWindow = mainWindow;

            mainWindow.Load += MainWindow_Load;
            mainWindow.UpdateFrame += MainWindow_UpdateFrame;
            mainWindow.RenderFrame += MainWindow_RenderFrame;
            mainWindow.KeyDown += MainWindow_KeyDown;
            mainWindow.KeyPress += MainWindow_KeyPress;
            mainWindow.KeyUp += MainWindow_KeyUp;
            mainWindow.Resize += MainWindow_Resize;
        }

        private void MainWindow_Resize(object sender, EventArgs e) {
            Console.WriteLine("Resize");
        }

        private void MainWindow_KeyPress(object sender, KeyPressEventArgs e) {}
        private void MainWindow_KeyDown(object sender, KeyboardKeyEventArgs e) {}
        private void MainWindow_KeyUp(object sender, KeyboardKeyEventArgs e) {}

        private void TextureInit() {
            grassBlockSprite = ContentPipe.LoadTexture("Content/Blocks/grass.png");
            dirtBlockSprite = ContentPipe.LoadTexture("Content/Blocks/dirt.png");
            stoneBlockSprite = ContentPipe.LoadTexture("Content/Blocks/stone.png");
            testBlockSprite = ContentPipe.LoadTexture("Content/Blocks/testTexture.png");
            playerSprite = ContentPipe.LoadTexture("Content/Player.png");
        }

        private void MainWindow_Load(object sender, EventArgs e) {
            zoom += 0; zoomZ += 0; //just here to trick vs so it doesn't tell me to make them readonly.

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            TextureInit();
            int gridSize = 16;
            int landLength = 128;
            int landStartingHeight = 256;
            int landHeight = 512;
            int dirtHeight = landHeight / 14;


            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(1, gridSize), MathExtra.GridLocation(0, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(2, gridSize), MathExtra.GridLocation(0, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(3, gridSize), MathExtra.GridLocation(0, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(4, gridSize), MathExtra.GridLocation(0, gridSize), grassBlockSprite.ID, "Grass"));
            //blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(2, gridSize), MathExtra.GridLocation(1, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(0, gridSize), MathExtra.GridLocation(2, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(1, gridSize), MathExtra.GridLocation(3, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(2, gridSize), MathExtra.GridLocation(3, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(3, gridSize), MathExtra.GridLocation(3, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(4, gridSize), MathExtra.GridLocation(3, gridSize), grassBlockSprite.ID, "Grass"));
            blockList.Add(new Block(gridSize, gridSize, MathExtra.GridLocation(5, gridSize), MathExtra.GridLocation(2, gridSize), grassBlockSprite.ID, "Grass"));

            playerList.Add(new Player(0, 0, 16, 16, playerSprite.ID));

            Random random = new Random();

            for (int i = 0; i < landLength; i += 1) {
                float sinY = landStartingHeight + (float)Math.Sin(i) * random.Next(0, 24);
                float yHeight = sinY - (sinY % gridSize);

                blockList.Add(new Block(gridSize, gridSize, i * gridSize, yHeight, grassBlockSprite.ID, "Grass" + i));

                for (int z = (int)yHeight + gridSize; z < landHeight; z += gridSize) {
                    if (z < (int)yHeight + dirtHeight) {
                        blockList.Add(new Block(gridSize, gridSize, i * gridSize, z, dirtBlockSprite.ID, "dirt"));
                    } else {
                        blockList.Add(new Block(gridSize, gridSize, i * gridSize, z, stoneBlockSprite.ID, "stone"));
                    }
                }
            }
        }

        KeyboardState lastKeyState;
        private void MainWindow_UpdateFrame(object sender, FrameEventArgs e) {
            KeyboardState keyState = Keyboard.GetState();

            //if (keyState.IsKeyDown(Key.Right)) {
            //    cameraXSpeed += 1;
            //    cameraOffsetX++;
            //}
            //if (keyState.IsKeyDown(Key.Left)) {
            //    cameraXSpeed -= 1;
            //    cameraOffsetX--;
            //}
            //if (keyState.IsKeyDown(Key.Up)) {
            //    cameraYSpeed += 1;
            //    cameraOffsetY++;
            //}
            //if (keyState.IsKeyDown(Key.Down)) {
            //    cameraYSpeed -= 1;
            //    cameraOffsetY--;
            //}
            cameraXSpeed = MathHelper.Clamp(cameraXSpeed, -10, 10);
            cameraYSpeed = MathHelper.Clamp(cameraYSpeed, -10, 10);
            cameraXSpeed = MathExtra.Lerp(cameraXSpeed, 0, 0.1f);
            cameraYSpeed = MathExtra.Lerp(cameraYSpeed, 0, 0.1f);

            if (freeCam) {
                cameraX += cameraXSpeed; cameraY += cameraYSpeed;
            } else {
                cameraX = -1 * playerList[0].x + mainWindow.Width / 2 + cameraOffsetX + (mainWindow.Width / 4 - playerList[0].width / 2);// + playerList[0].width / 2;
                cameraY = -1 * playerList[0].y + mainWindow.Height / 2 + cameraOffsetY + (mainWindow.Height / 4 - playerList[0].height / 2);// + playerList[0].height / 2;
            }


            if (keyState.IsKeyDown(Key.Escape) && lastKeyState.IsKeyUp(Key.Escape)) {
                Console.WriteLine("Escape!");
                mainWindow.Exit();
            }

            if (keyState.IsKeyDown(Key.Enter) && lastKeyState.IsKeyUp(Key.Enter)) {
                Console.WriteLine("Enter down!");}
            if (keyState.IsKeyUp(Key.Enter) && lastKeyState.IsKeyDown(Key.Enter)) {
                Console.WriteLine("Enter up!");
            }

            foreach (Player player in playerList) {
                player.Update(keyState, blockList);
            }

            lastKeyState = keyState;
        }

        private void MainWindow_RenderFrame(object sender, FrameEventArgs e) {
            GL.ClearColor(Color.CornflowerBlue);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projMatrix = Matrix4.CreateOrthographicOffCenter(
                mainWindow.Width / 2,
                mainWindow.Width,
                mainWindow.Height,
                mainWindow.Height / 2,
                0,
                1
            );
            //Matrix4 projMatrix = Matrix4.CreateOrthographic(mainWindow.Width, mainWindow.Height, 0, 1);
            //Console.WriteLine("{0} : {1}", mainWindow.Width, mainWindow.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMatrix);

            Matrix4 modelViewMatrix =
                Matrix4.CreateScale(zoom, zoom, zoomZ) *
                Matrix4.CreateRotationZ(0f) *
                Matrix4.CreateTranslation(cameraX, cameraY, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            foreach(Block block in blockList) {
                block.Draw();
            }

            foreach(Player player in playerList) {
                player.Draw();
            }

            mainWindow.SwapBuffers();
        }
    }
}
