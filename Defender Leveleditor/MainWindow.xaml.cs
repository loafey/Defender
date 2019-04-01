using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Defender_Leveleditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        string selectedBlock = "None";
        int gridSize = 16;
        List<BlockInfo> blockInfoList = new List<BlockInfo>();
        public MainWindow() {
            InitializeComponent();
            CheckFolderExistance();
            LoadBlocks();
        }

        void CheckFolderExistance() {
            if (!Directory.Exists("Content/Levels")) {
                Directory.CreateDirectory("Content/Levels");
            }
            if (!Directory.Exists("Content/Blocks")) {
                Directory.CreateDirectory("Content/Blocks");
            }
        }

        void LoadBlocks() {
            DirectoryInfo dInfo = new DirectoryInfo("Content/Blocks");
            FileInfo[] files = dInfo.GetFiles("*.png");
            foreach (FileInfo file in files) {
                ListBoxItem fileItem = new ListBoxItem();
                fileItem.Content = file.Name;
                fileItem.Tag = file.FullName;
                fileItem.MouseDoubleClick += new MouseButtonEventHandler(SelectBlockEvent);
                blockList.Items.Add(fileItem);
            }
        }

        void SetSelectedBlock(string Block) {
            selectedBlock = Block;
            SelectedObject.Text = "Selected object: " + Block;
        }

        private void SelectBlockEvent(object sender, MouseButtonEventArgs e) {
            blockNameText.Text = sender.ToString().Split(':')[1].Trim(' ');
            foreach(ListBoxItem child in blockList.Items) {
                if(blockNameText.Text.Equals(child.Content.ToString())) {
                    blockLocationText.Text = child.Tag.ToString();
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(child.Tag.ToString());
                    image.EndInit();
                    imageBlock.Source = image;
                    SetSelectedBlock(blockNameText.Text.Split('.')[0]);
                }
            }
        }

        private void DetectKeyPressOnWindow(object sender, KeyEventArgs e) {
            if (Keyboard.IsKeyDown(Key.Escape)) {
                if(selectedBlock != "None") { 
                    SetSelectedBlock("None");
                    blockLocationText.Text = "None";
                    blockNameText.Text = "None";
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(System.IO.Path.GetFullPath("Content/none.png"));
                    image.EndInit();
                    imageBlock.Source = image;
                }
            }
        }

        private static float GridSnap(float n, float snap) {
            return (float)Math.Round((n / (float)snap), MidpointRounding.AwayFromZero) * snap; 
        }

        private void LevelCanvasLeftDown(object sender, MouseButtonEventArgs e) {
            if(selectedBlock != "None") {
                bool samePos = false;
                int mouseX = (int)GridSnap((float)Mouse.GetPosition(levelCanvas).X - gridSize / 2, gridSize);
                int mouseY = (int)GridSnap((float)Mouse.GetPosition(levelCanvas).Y - gridSize / 2, gridSize);

                foreach (BlockInfo b in blockInfoList) {
                    if(mouseX == b.x && mouseY == b.y) {
                        samePos = true;
                        break;
                    }
                }
                if (!samePos) {
                    Rectangle block = new Rectangle();
                    block.Width = 16;
                    block.Height = 16;

                    block.Fill = new ImageBrush(new BitmapImage(
                        new Uri(blockLocationText.Text)));

                    levelCanvas.Children.Add(block);

                    Canvas.SetLeft(block, mouseX);
                    Canvas.SetTop(block, mouseY);

                    BlockInfo blockInfo = new BlockInfo(
                        mouseX,
                        mouseY,
                        blockLocationText.Text,
                        blockInfoList.Count,
                        block
                    );

                    blockInfoList.Add(blockInfo);
                }
            }
        }
    }
}
