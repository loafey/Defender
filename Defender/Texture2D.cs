namespace Defender {
    //Using system.drawing, so it supports BMP, GIF, EXIF, JPG, PNG and TIFF.
    class Texture2D {
        private int id;
        private int width, height;

        public int ID { get { return id; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Texture2D(int id, int width, int height) {
            this.id = id;
            this.width = width;
            this.height = height;
        }
    }
}
