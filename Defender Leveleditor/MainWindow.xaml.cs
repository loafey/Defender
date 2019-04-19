using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Defender_Leveleditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        string selectedBlock = "None";
        int gridSize = 16;
        string loadedFile = "";
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
                ListBoxItem fileItem = new ListBoxItem {
                    Content = file.Name,
                    Tag = file.FullName//relativePath.MakeRelativeUri(referencePath).ToString()
                };
                fileItem.MouseDoubleClick += new MouseButtonEventHandler(SelectBlockEvent);
                blockList.Items.Add(fileItem);
            };
        }

        void SetSelectedBlock(string Block) {
            selectedBlock = Block;
            SelectedObject.Text = "Selected object: " + Block;
        }

        private void SelectBlockEvent(object sender, MouseButtonEventArgs e) {
            blockNameText.Text = sender.ToString().Split(':')[1].Trim(' ');
            foreach (ListBoxItem child in blockList.Items) {
                if (blockNameText.Text.Equals(child.Content.ToString())) {
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

        private void ClickBlockEvent(object sender, MouseButtonEventArgs e) {
            if (selectedBlock == "None") {
                Rectangle rect = (Rectangle)sender;
                if ((int)rect.Tag > blockInfoList.Count) {
                    blockInfoList.RemoveAt(blockInfoList.Count - 1);
                } else {
                    blockInfoList.RemoveAt((int)rect.Tag);
                }

                try {
                    levelCanvas.Children.Remove((Rectangle)sender);
                } catch (IndexOutOfRangeException error) {
                    MessageBox.Show(error.ToString());
                    return;
                }
            }
        }

        private void DetectKeyPressOnWindow(object sender, KeyEventArgs e) {
            if (Keyboard.IsKeyDown(Key.Escape)) {
                if (selectedBlock != "None") {
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

        }

        private void LevelCanvasMouseMoving(object sender, MouseEventArgs e) {
            if (Mouse.LeftButton == MouseButtonState.Pressed) {
                if (selectedBlock != "None") {
                    int mouseX = (int)GridSnap((float)Mouse.GetPosition(levelCanvas).X - gridSize / 2, gridSize);
                    int mouseY = (int)GridSnap((float)Mouse.GetPosition(levelCanvas).Y - gridSize / 2, gridSize);

                    PlaceBlock(mouseX, mouseY, blockLocationText.Text);
                }
            }
        }

        private void FileMenuSaveButton(object sender, MouseButtonEventArgs e) {
            if (loadedFile == "") {
                FileMenuSaveAsButton(sender, e);
            } else {
                string textFile = "";
                foreach (BlockInfo block in blockInfoList) {
                    Uri relativePath = new Uri(block.texture);
                    Uri referencePath = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);
                    textFile += "block|";
                    textFile += block.x + "|";
                    textFile += block.y + "|";
                    textFile += referencePath.MakeRelativeUri(relativePath).ToString();
                    textFile += ";\n";
                }
                File.WriteAllText(loadedFile, textFile);
                textFile = "";
            }
        }

        private void FileMenuSaveAsButton(object sender, MouseButtonEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true) {
                string textFile = "";
                foreach (BlockInfo block in blockInfoList) {
                    Uri relativePath = new Uri(block.texture);
                    Uri referencePath = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);
                    textFile += "block|";
                    textFile += block.x + "|";
                    textFile += block.y + "|";
                    textFile += referencePath.MakeRelativeUri(relativePath).ToString();
                    textFile += ";\n";
                }
                File.WriteAllText(saveFileDialog.FileName, textFile);
                textFile = "";
                loadedFile = saveFileDialog.FileName;
                this.Title = "Defender Editor - " + loadedFile;
            }
        }

        private void FileMenuLoadButton(object sender, MouseButtonEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                loadedFile = openFileDialog.FileName;
                this.Title = "Denfender Editor - " + loadedFile;
                levelCanvas.Children.RemoveRange(0, levelCanvas.Children.Count);
                blockInfoList.Clear();

                LevelInfo levelInfo = FileHandler.OpenFile(
                    System.IO.Path.GetFullPath(openFileDialog.FileName)
                );

                foreach(Block block in levelInfo.blocks) {
                    /*Console.WriteLine(block.x);
                    Console.WriteLine(block.y);
                    Console.WriteLine(block.textureLocation);
                    Console.WriteLine("");*/
                    PlaceBlock(block.x, block.y, System.IO.Path.GetFullPath(block.textureLocation));

                }
            }
        }

        private void PlaceBlock(int X, int Y, string BlockTexture) {
            bool samePos = false;

            foreach (BlockInfo b in blockInfoList) {
                if (X == b.x && Y == b.y) {
                    samePos = true;
                    break;
                }
            }
            if (!samePos) {
                Rectangle block = new Rectangle {
                    Width = 16,
                    Height = 16,

                    Fill = new ImageBrush(new BitmapImage(
                    new Uri(BlockTexture)))
                };

                levelCanvas.Children.Add(block);

                Canvas.SetLeft(block, X);
                Canvas.SetTop(block, Y);

                BlockInfo blockInfo = new BlockInfo(
                    X,
                    Y,
                    BlockTexture,
                    blockInfoList.Count,
                    block
                );

                block.MouseDown += ClickBlockEvent;
                block.Tag = blockInfo.arrayIndex;

                blockInfoList.Add(blockInfo);
            }
        }


        private void FileMenuExitButton(object sender, MouseButtonEventArgs e) {
            this.Close();
        }
    }
}
