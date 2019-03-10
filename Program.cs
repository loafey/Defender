using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defender {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Using OpenTK: 3.010");
            OpenTK.GameWindow mainWindow = new OpenTK.GameWindow(800, 600, new OpenTK.Graphics.GraphicsMode(32, 8, 0 , 0));
            mainWindow.Title = "Defender";

            Game game = new Game(mainWindow);

            mainWindow.Run(1.0 / 60.0);
        }
    }
}
