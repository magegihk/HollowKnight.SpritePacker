namespace HollowKnight.SpritePacker
{
    public class Sprite
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int xr { set; get; }
        public int yr { set; get; }
        public int width { set; get; }
        public int height { set; get; }

        public string collectionname { get; set; }
        public string path { get; set; }
        public bool flipped { get; set; }

        public Sprite(int _id, int _x, int _y, int _xr, int _yr, int _width, int _height, string _collectionname, string _path, bool _flipped)
        {
            id = _id;
            x = _x;
            y = _y;
            xr = _xr;
            yr = _yr;
            width = _width;
            height = _height;
            collectionname = _collectionname;
            path = _path;
            flipped = _flipped;
        }
    }
}