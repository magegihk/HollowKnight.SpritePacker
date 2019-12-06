namespace HollowKnight.SpritePacker
{
    public class Sprite
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int atlaswidth { get; set; }
        public int atlasheight { get; set; }
        public string collectionname { get; set; }
        public string path { get; set; }
        public bool flipped { get; set; }

        public Sprite(int _id, int _x, int _y, int _width, int _height, int _atlaswidth, int _atlasheight, string _collectionname, string _path, bool _flipped)
        {
            id = _id;
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            atlaswidth = _atlaswidth;
            atlasheight = _atlasheight;
            collectionname = _collectionname;
            path = _path;
            flipped = _flipped;
        }
    }
}