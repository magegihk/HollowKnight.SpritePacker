namespace HollowKnight.SpritePacker
{
    public class Sprite
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string collectionname { get; set; }
        public string path { get; set; }
        public bool flipped { get; set; }

        public Sprite(int _id, int _x, int _y, string _collectionname, string _path, bool _flipped)
        {
            id = _id;
            x = _x;
            y = _y;
            collectionname = _collectionname;
            path = _path;
            flipped = _flipped;
        }
    }
}