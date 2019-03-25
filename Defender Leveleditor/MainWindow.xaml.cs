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
        public MainWindow() {
            InitializeComponent();
            LoadBlocks();
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
                }
            }
        }
    }
}
