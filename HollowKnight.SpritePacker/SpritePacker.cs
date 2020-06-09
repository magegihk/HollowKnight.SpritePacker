using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace HollowKnight.SpritePacker
{
    public partial class Form1 : Form
    {
        #region Fields

        public bool replaceall = false;
        private readonly string folderpath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\locallow\\Team Cherry\\Hollow Knight\\sprites";
        private string _anim;
        private string _clip;
        private string _frame;
        private string _oriatlas;
        private string _genatlas;
        private string _backup;
        private bool backup = true;
        private bool check = false;
        private bool fixedmode = false;
        private bool goodtopack = false;
        private bool modechosen = false;
        private List<Frame> _backups;
        private SpritesFolder spritesFolder;
        private BackComparer backComparer = new BackComparer();
        private FileSystemWatcher watcher;
        private InspectMode _im = InspectMode.Animation;
        private enum InspectMode
        {
            Animation,
            Collection,
            GenCollection,
            AnimFrame,
            CollFrame,
            Backup
        }
        private enum ReplaceMode
        {
            Single,
            All,
            Restore
        }
        private string Anim
        {
            get
            {
                if (listBox1.SelectedItem != null)
                {
                    _anim = listBox1.SelectedItem.ToString();
                }
                else if (listBox1.Items.Count > 0)
                {
                    _anim = listBox1.Items[0].ToString();
                }
                else
                {
                    _anim = "";
                }
                return _anim;
            }
            set
            {
                int index = listBox1.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox1.SelectedIndex = index;
                    _anim = value;
                }
            }
        }
        private string Clip
        {
            get
            {
                if (listBox2.SelectedItem != null)
                {
                    _clip = listBox2.SelectedItem.ToString();
                }
                else if (listBox2.Items.Count > 0)
                {
                    _clip = listBox2.Items[0].ToString();
                }
                else
                {
                    _clip = "";
                }
                return _clip;
            }
            set
            {
                int index = listBox2.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox2.SelectedIndex = index;
                    _clip = value;
                }
            }
        }
        private string Frame
        {
            get
            {
                if (listBox3.SelectedItem != null)
                {
                    _frame = listBox3.SelectedItem.ToString();
                }
                else if (listBox3.Items.Count > 0)
                {
                    _frame = listBox3.Items[0].ToString();
                }
                else
                {
                    _frame = "";
                }
                return _frame;
            }
            set
            {
                int index = listBox3.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox3.SelectedIndex = index;
                    _frame = value;
                    if (IM != InspectMode.Backup)
                    {
                        ShowPic(IM);
                    }
                }
            }
        }
        private string Oriatlas
        {
            get
            {
                if (listBox4.SelectedItem != null)
                {
                    _oriatlas = listBox4.SelectedItem.ToString();
                }
                else if (listBox4.Items.Count > 0)
                {
                    _oriatlas = listBox4.Items[0].ToString();
                }
                else
                {
                    _oriatlas = "";
                }
                return _oriatlas;
            }
            set
            {
                int index = listBox4.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox4.SelectedIndex = index;
                    _oriatlas = value;
                    ShowPic(IM);
                }
            }
        }
        private string Genatlas
        {
            get
            {
                if (listBox5.SelectedItem != null)
                {
                    _genatlas = listBox5.SelectedItem.ToString();
                }
                else if (listBox5.Items.Count > 0)
                {
                    _genatlas = listBox5.Items[0].ToString();
                }
                else
                {
                    _genatlas = "";
                }
                return _genatlas;
            }
            set
            {
                int index = listBox5.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox5.SelectedIndex = index;
                    _genatlas = value;
                    ShowPic(IM);
                }
            }
        }
        private string Backup
        {
            get
            {
                if (listBox7.SelectedItem != null)
                {
                    _backup = listBox7.SelectedItem.ToString();
                }
                else if (listBox7.Items.Count > 0)
                {
                    _backup = listBox7.Items[0].ToString();
                }
                else
                {
                    _backup = "";
                }
                return _backup;
            }
            set
            {
                int index = listBox7.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox7.SelectedIndex = index;
                    _backup = value;
                    ShowPic(IM);
                }
            }
        }
        private InspectMode IM
        {
            get { return _im; }
            set
            {
                switch (value)
                {
                    case InspectMode.Animation:
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                        break;
                    case InspectMode.Collection:
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        button1.Visible = true;
                        goodtopack = false;
                        check = false;
                        replaceall = false;
                        break;
                    case InspectMode.GenCollection:
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        break;
                    case InspectMode.AnimFrame:
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                        if (_im == InspectMode.Collection || _im == InspectMode.CollFrame || _im == InspectMode.Backup)
                        {
                            value = InspectMode.CollFrame;
                        }
                        break;
                    case InspectMode.CollFrame:
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                        if (_im == InspectMode.Animation || _im == InspectMode.AnimFrame)
                        {
                            value = InspectMode.AnimFrame;
                        }
                        break;
                    case InspectMode.Backup:
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                        break;
                    default:
                        break;
                }
                _im = value;
            }
        }

        #endregion Fields

        #region Event listeners

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            string changed = e.Name.Split('\\')[2];
            if (listBox8.FindStringExact(changed) == ListBox.NoMatches)
            {
                listBox8.Invoke(new MethodInvoker(delegate () { listBox8.Items.Add(changed); }));
                this.replaceall = false;
            }
        }
        private FileSystemWatcher WatcherStart(string path, string filter)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = filter;
            watcher.Changed += new FileSystemEventHandler(OnProcess);
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.CreationTime;
            watcher.IncludeSubdirectories = true;
            return watcher;
        }

        #endregion Event listeners

        #region Refreshes

        private string RefreshList(string chosen, ListBox listBox, List<string> list)
        {
            listBox.Items.Clear();
            if (CheckSavePath())
            {
                foreach (var item in list)
                {
                    listBox.Items.Add(item);
                }
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            return chosen;
        }
        private void RefreshForm()
        {
            SetValues();
            if (CheckSavePath())
            {
                Collection.collections.Clear();
                Collection.gencollections.Clear();
                spritesFolder = new SpritesFolder(new DirectoryInfo(folderpath));

                RefreshList1();
                RefreshList2();
                RefreshList3();
                RefreshList4();
                RefreshList5();
                RefreshList6();
                RefreshList7();

                Anim = Anim;
                Clip = Clip;
                Frame = Frame;
                Oriatlas = Oriatlas;
                Genatlas = Genatlas;
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
        }
        private void RefreshList1()
        {
            Anim = RefreshList(Anim, listBox1, spritesFolder.animations.Select(a => a.info.Name).ToList());
        }
        private void RefreshList2()
        {
            Clip = RefreshList(Clip, listBox2, spritesFolder.animations.First(a => a.info.Name == Anim).clips.Select(c => c.info.Name).ToList());
        }
        private void RefreshList3()
        {
            if (IM == InspectMode.Animation || IM == InspectMode.AnimFrame)
            {
                Frame = RefreshList(Frame, listBox3, spritesFolder.animations.First(a => a.info.Name == Anim).clips.First(c => c.info.Name == Clip).frames.Select(f => f.info.Name).ToList());
            }
            if (IM == InspectMode.Collection || IM == InspectMode.CollFrame || IM == InspectMode.Backup)
            {
                Collection.collections.Where(c => c.info.FullName.Contains(Anim)).First(c => c.name == Oriatlas).SortFrame();
                Frame = RefreshList(Frame, listBox3, Collection.collections.Where(c => c.info.FullName.Contains(Anim)).First(c => c.name == Oriatlas).frames.Select(f => f.info.Name).ToList());
            }
        }
        private void RefreshList4()
        {
            Oriatlas = RefreshList(Oriatlas, listBox4, Collection.collections.Where(c => c.info.FullName.Contains(Anim)).Select(c => c.name).ToList());
        }
        private void RefreshList5()
        {
            Genatlas = RefreshList(Genatlas, listBox5, Collection.gencollections.Where(g => g.info.FullName.Contains(Anim)).Select(g => g.name).ToList());
        }
        private void RefreshList6()
        {
            listBox6.Items.Clear();
            if (CheckSavePath())
            {
                Log("[FolderPath] " + new DirectoryInfo(folderpath).FullName);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
        }
        private void RefreshList7()
        {
            listBox7.Items.Clear();
            if (CheckSavePath())
            {
                _backups = new DirectoryInfo(folderpath + "\\" + Anim).GetFiles("???-??-???[backup]??????.png", SearchOption.AllDirectories).Select(f => new Frame(f)).ToList();
                _backups.Sort(backComparer);
                foreach (var item in _backups)
                {
                    listBox7.Items.Add(item.info.Name);
                }
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
        }

        #endregion

        #region Functions

        public void Log(string s)
        {
            listBox6.Items.Add(s);
            listBox6.TopIndex = listBox6.Items.Count - 3;
        }
        private string CalculateMD5(Frame frame)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(frame.info.FullName))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        private void Check()
        {
            goodtopack = true;
            check = true;
            replaceall = true;
            foreach (var collection in Collection.collections)
            {
                if (collection.name == _oriatlas)
                {
                    collection.SortFrame();
                    Log("[Error05] " + GlobalData.GlobalLanguage.Message_Error05);
                    foreach (var item in listBox8.Items)
                    {
                        bool findone = false;
                        foreach (var frame in collection.frames)
                        {
                            if (item.ToString() == frame.info.Name)
                            {
                                findone = true;
                            }
                        }
                        if (!findone)
                        {
                            Log(item.ToString());
                            replaceall = false;
                        }
                    }
                    if (replaceall)
                    {
                        listBox6.Items.RemoveAt(listBox6.Items.Count - 1);
                    }
                    for (int i = 0; i < collection.frames.Count - 1; i++)
                    {
                        if ((collection.frames[i].sprite.id == collection.frames[i + 1].sprite.id))
                        {
                            if (!fixedmode && !FrameMD5HashEquals(collection.frames[i], collection.frames[i + 1]) || fixedmode && !FramePixelEquals(collection.frames[i], collection.frames[i + 1]))
                            {
                                goodtopack = false;
                                Log("[Error02] " + GlobalData.GlobalLanguage.Message_Error02);
                                Log(collection.frames[i].info.Name);
                                Log(collection.frames[i + 1].info.Name);
                                listBox3.SelectedIndex = listBox3.FindStringExact(collection.frames[i].info.Name);
                                listBox3.TopIndex = listBox3.SelectedIndex;
                                return;
                            }
                        }
                    }
                }
            }
            button1.Visible = false;
            Log("[" + GlobalData.GlobalLanguage.Main_Button1 + "] " + GlobalData.GlobalLanguage.Message_01);
        }
        private bool CheckSavePath()
        {
            return Directory.Exists(folderpath);
        }
        private bool FrameMD5HashEquals(Frame a, Frame b)
        {
            return CalculateMD5(a).Equals(CalculateMD5(b));
        }
        private bool FramePixelEquals(Frame a, Frame b)
        {
            using (Bitmap bitmapA = Cut(a))
            {
                using (Bitmap bitmapB = Cut(b))
                {
                    if (bitmapA == null || bitmapB == null)
                    {
                        return true;
                    }
                    bool equal = true;
                    for (int i = 0; i < bitmapA.Width; i++)
                    {
                        for (int j = 0; j < bitmapA.Height; j++)
                        {
                            if (i < bitmapB.Width && j < bitmapB.Height)
                            {
                                if (bitmapA.GetPixel(i, j) != bitmapB.GetPixel(i, j))
                                {
                                    equal = false;
                                }
                            }
                        }
                    }
                    return equal;
                }
            }
        }
        private void Replace(ReplaceMode mode)
        {
            foreach (var collection in Collection.collections)
            {
                if (collection.name == _oriatlas)
                {
                    string[] items;
                    List<Frame> frames;
                    switch (mode)
                    {
                        case ReplaceMode.Single:
                            items = new string[] { _frame };
                            frames = collection.frames;
                            break;

                        case ReplaceMode.All:
                            items = listBox8.Items.OfType<string>().ToArray();
                            frames = collection.frames;
                            break;

                        case ReplaceMode.Restore:
                            items = listBox7.SelectedItems.OfType<string>().ToArray();
                            frames = new DirectoryInfo(folderpath).GetFiles("???-??-???[backup]??????.png", SearchOption.AllDirectories).Select(f => new Frame(f)).ToList();
                            break;

                        default:
                            items = new string[] { };
                            frames = collection.frames;
                            break;
                    }
                    foreach (string item in items)
                    {
                        foreach (var frameneeded in frames)
                        {
                            if (frameneeded.info.Name == item)
                            {
                                Bitmap cutted = Cut(frameneeded);
                                foreach (var frame in collection.frames)
                                {
                                    if (frame.sprite.id == frameneeded.sprite?.id && frame.info.FullName != frameneeded.info.FullName || mode == ReplaceMode.Restore && frame.info.Name == frameneeded.info.Name.Remove(10, 14))
                                    {
                                        string orig = frameneeded.info.FullName;
                                        string _orig = frameneeded.info.Name;
                                        string dst = frame.info.FullName;
                                        string _dst = frame.info.Name;
                                        if (backup && mode != ReplaceMode.Restore)
                                        {
                                            string bak = frame.info.DirectoryName + "\\" + frame.info.Name.Substring(0, frame.info.Name.Length - 4) + "[backup]" + DateTime.Now.ToString("HHmmss") + ".png";
                                            string _bak = frame.info.Name.Substring(0, frame.info.Name.Length - 4) + "[backup]" + DateTime.Now.ToString("HHmmss") + ".png";
                                            if (File.Exists(bak))
                                            {
                                                File.Delete(bak);
                                            }
                                            File.Copy(dst, bak);
                                            Log("[" + GlobalData.GlobalLanguage.Main_CheckBox1 + "] " + _dst + " => " + _bak);
                                        }
                                        if (mode != ReplaceMode.Restore && fixedmode && !FramePixelEquals(frame, frameneeded))
                                        {
                                            Bitmap fix = Fix(cutted, frame);
                                            if (File.Exists(dst))
                                            {
                                                File.Delete(dst);
                                            }
                                            fix.Save(dst);
                                        }
                                        if (mode == ReplaceMode.Restore || !fixedmode && !FrameMD5HashEquals(frame, frameneeded))
                                        {
                                            if (File.Exists(dst))
                                            {
                                                File.Delete(dst);
                                            }
                                            File.Copy(orig, dst);
                                            if (mode == ReplaceMode.Restore && File.Exists(orig))
                                            {
                                                if (pictureBox1.Image != null)
                                                {
                                                    pictureBox1.Image.Dispose();
                                                }
                                                File.Delete(orig);
                                            }
                                        }
                                        Log("[" + GlobalData.GlobalLanguage.Main_Button2 + "] " + _orig + " => " + _dst);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void SetValues()
        {
            Text = GlobalData.GlobalLanguage.Main_Title;
            label1.Text = GlobalData.GlobalLanguage.Main_Label1;
            label2.Text = GlobalData.GlobalLanguage.Main_Label2;
            label3.Text = GlobalData.GlobalLanguage.Main_Label3;
            label4.Text = GlobalData.GlobalLanguage.Main_Label4;
            label5.Text = GlobalData.GlobalLanguage.Main_Label5;
            label6.Text = GlobalData.GlobalLanguage.Main_Label6;
            label7.Text = GlobalData.GlobalLanguage.Main_Label7;
            label8.Text = GlobalData.GlobalLanguage.Main_Label8;
            label9.Text = GlobalData.GlobalLanguage.Main_Label9;
            label10.Text = GlobalData.GlobalLanguage.Main_Label10;
            label11.Text = GlobalData.GlobalLanguage.Main_Label11;
            label12.Text = GlobalData.GlobalLanguage.Main_Label12;
            linkLabel1.Text = GlobalData.GlobalLanguage.Main_LinkLabel1;
            linkLabel2.Text = GlobalData.GlobalLanguage.Main_LinkLabel2;
            button1.Text = GlobalData.GlobalLanguage.Main_Button1;
            button2.Text = GlobalData.GlobalLanguage.Main_Button2;
            button3.Text = GlobalData.GlobalLanguage.Main_Button3;
            button4.Text = GlobalData.GlobalLanguage.Main_Button4;
            button5.Text = GlobalData.GlobalLanguage.Main_Button5;
            button6.Text = GlobalData.GlobalLanguage.Main_Button6;
            button7.Text = GlobalData.GlobalLanguage.Main_Button7;
            button8.Text = GlobalData.GlobalLanguage.Main_Button8;
            button9.Text = GlobalData.GlobalLanguage.Main_Button9;
            button10.Text = GlobalData.GlobalLanguage.Main_Button10;
            checkBox1.Text = GlobalData.GlobalLanguage.Main_CheckBox1;
        }

        #endregion Functions

        #region Graphics

        private Bitmap Cut(Frame frame)
        {
            if (frame.sprite == null)
            {
                return null;
            }
            try
            {
                using (Bitmap bitmap = new Bitmap(frame.info.FullName))
                {
                    return bitmap.Clone(new Rectangle(frame.sprite.xr, bitmap.Height - frame.sprite.yr - frame.sprite.height, frame.sprite.width, frame.sprite.height), bitmap.PixelFormat);
                }
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        private Bitmap Fix(Bitmap cut, Frame frame)
        {
            using (Bitmap temp = new Bitmap(frame.info.FullName))
            {
                Bitmap fix = new Bitmap(temp);
                for (int i = 0; i < cut.Width; i++)
                {
                    for (int j = 0; j < cut.Height; j++)
                    {
                        fix.SetPixel(i + frame.sprite.xr, fix.Height - (j + frame.sprite.yr) - 1, cut.GetPixel(i, cut.Height - j - 1));
                    }
                }
                return fix;
            }
        }
        private void Pack(Collection collection)
        {
            progressBar1.Visible = true;
            Bitmap genatlas = new Bitmap(collection.info.FullName);
            int num = 0;
            int progression = 0;
            foreach (var frame in collection.frames)
            {
                try
                {
                    num++;

                    Bitmap image = new Bitmap(frame.info.FullName);

                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            //int xold = frame.sprite.flipped ? frame.sprite.x + j : frame.sprite.x + i;
                            //int yold = frame.sprite.flipped ? genatlas.Height - (frame.sprite.y + i) - 1 : genatlas.Height - (frame.sprite.y + j) - 1;
                            int x = (frame.sprite.flipped ? frame.sprite.x + j - (fixedmode ? frame.sprite.yr : 0) : frame.sprite.x + i - (fixedmode ? frame.sprite.xr : 0));
                            int y = (frame.sprite.flipped ? genatlas.Height - (frame.sprite.y + i) - 1 + (fixedmode ? frame.sprite.xr : 0) : genatlas.Height - (frame.sprite.y + j) - 1 + (fixedmode ? frame.sprite.yr : 0));
                            if (!fixedmode && (0 <= x && x < genatlas.Width && 0 <= y && y < genatlas.Height) ||
                                fixedmode && (frame.sprite.xr <= i && i < frame.sprite.xr + frame.sprite.width && frame.sprite.yr <= j && j < frame.sprite.yr + frame.sprite.height) && (0 <= x && x < genatlas.Width && 0 <= y && y < genatlas.Height))
                            {
                                genatlas.SetPixel(x, y, image.GetPixel(i, image.Height - j - 1));
                            }
                        }
                    }
                    progression = (int)(100 * num / collection.frames.Count);
                    progressBar1.Value = progression;
                    if (progression == 100)
                    {
                        progressBar1.Visible = false;
                    }
                }
                catch (Exception)
                {
                }
                
            }
            string savepath = collection.info.DirectoryName + "\\Gen-" + collection.info.Name;
            genatlas.Save(savepath);
            Log("[" + GlobalData.GlobalLanguage.Main_Button3 + "] " + "Pack Done");
            Log("[" + GlobalData.GlobalLanguage.Main_Button3 + "] " + savepath);
        }
        private void ShowPic(InspectMode mode)
        {
            string path;
            switch (mode)
            {
                case InspectMode.Animation:
                    path = folderpath + "\\" + Anim + "\\" + Clip + "\\" + Frame;
                    break;

                case InspectMode.Collection:
                    path = folderpath + "\\" + Anim + "\\0.Atlases\\" + Oriatlas;
                    break;

                case InspectMode.GenCollection:
                    path = folderpath + "\\" + Anim + "\\0.Atlases\\" + Genatlas;
                    break;

                case InspectMode.AnimFrame:
                    path = folderpath + "\\" + Anim + "\\" + Clip + "\\" + Frame;
                    break;

                case InspectMode.CollFrame:
                    path = Collection.GetCollectionByName(Oriatlas).frames.First(f => f.info.Name == Frame).info.FullName;
                    Anim = path.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[0];
                    Clip = path.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[1];
                    break;
                case InspectMode.Backup:
                    path = _backups.First(b => b.info.Name == Backup).info.FullName;
                    Anim = path.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[0];
                    Clip = path.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[1];
                    Frame = path.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[2].Remove(10, 14);
                    break;
                default:
                    path = "";
                    break;
            }
            if (File.Exists(path))
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = Image.FromFile(path);
            }
        }

        #endregion Graphics

        #region Class

        private class BackComparer : IComparer<Frame>
        {
            private int len;
            private SortBy sort;
            private int start;

            public BackComparer()
            {
                Sort = SortBy.time;
            }

            public enum SortBy
            {
                clip,
                id,
                time
            }

            public SortBy Sort
            {
                get
                {
                    return sort;
                }
                set
                {
                    switch (value)
                    {
                        case SortBy.clip:
                            start = 0;
                            len = 3;
                            sort = SortBy.clip;
                            break;

                        case SortBy.id:
                            start = 7;
                            len = 3;
                            sort = SortBy.id;
                            break;

                        case SortBy.time:
                            start = 18;
                            len = 6;
                            sort = SortBy.time;
                            break;

                        default:
                            break;
                    }
                }
            }

            public int Compare(Frame a, Frame b)
            {
                return a.info.Name.Substring(start, len).CompareTo(b.info.Name.Substring(start, len));
            }
        }

        #endregion Class

        public Form1()
        {
            InitializeComponent();
            RefreshForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IM == InspectMode.Animation || IM == InspectMode.AnimFrame)
            {
                Log("[Error03] " + GlobalData.GlobalLanguage.Message_Error03);
                return;
            }
            if (!modechosen)
            {
                Log("[Error04] " + GlobalData.GlobalLanguage.Message_Error04);
                return;
            }
            Check();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!goodtopack && check)
            {
                Replace(ReplaceMode.Single);
                RefreshList7();
            }
            else
            {
                Log("[Error06] " + GlobalData.GlobalLanguage.Message_Error06);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (goodtopack && (IM == InspectMode.Collection || IM == InspectMode.CollFrame) && listBox4.SelectedIndex >= 0)
            {
                foreach (var collection in Collection.collections)
                {
                    if (collection.name == _oriatlas)
                    {
                        Pack(collection);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Replace(ReplaceMode.Restore);
            RefreshList7();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            watcher = WatcherStart(folderpath, "???-??-???.png");
            button6.Visible = false;
            Log("[" + GlobalData.GlobalLanguage.Main_Button6 + "] " + "Watcher On.");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            watcher.Dispose();
            button6.Visible = true;
            Log("[" + GlobalData.GlobalLanguage.Main_Button7 + "] " + "Watcher Off.");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!goodtopack && replaceall)
            {
                Replace(ReplaceMode.All);
                listBox8.Items.Clear();
            }
            else
            {
                Log("[Error06] " + GlobalData.GlobalLanguage.Message_Error06);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (backComparer.Sort < BackComparer.SortBy.time)
            {
                backComparer.Sort++;
            }
            else
                backComparer.Sort = BackComparer.SortBy.clip;
            Log("[" + GlobalData.GlobalLanguage.Main_Button9 + "] " + backComparer.Sort.ToString());
            RefreshList7();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                foreach (var item in listBox3.SelectedItems)
                {
                    listBox8.Items.Add(item);

                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            backup = checkBox1.Checked;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalData.SystemLanguage = comboBox1.SelectedItem.ToString();
            RefreshForm();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            goodtopack = false;
            check = false;
            replaceall = false;
            modechosen = true;
            if (comboBox2.SelectedIndex == 0)
            {
                fixedmode = false;
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                fixedmode = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            checkBox1.Checked = false;
            button6.PerformClick();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/magegihk/HollowKnight.GODump");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/magegihk/HollowKnight.SpritePacker");
        }

        private void ListBox1_MouseClick(object sender, EventArgs eventArgs)
        {
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                RefreshList2();
                RefreshList4();
                RefreshList5();
                RefreshList7();
            }
        }

        private void ListBox2_MouseClick(object sender, EventArgs eventArgs)
        {
            IM = InspectMode.Animation;
        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null && (IM == InspectMode.Animation || IM == InspectMode.AnimFrame || IM == InspectMode.Backup))
            {
                RefreshList3();
            }
        }

        private void ListBox3_MouseClick(object sender, EventArgs eventArgs)
        {
            IM = InspectMode.AnimFrame;
        }

        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                Frame = listBox3.SelectedItem.ToString();
            }
        }

        private void ListBox4_MouseClick(object sender, EventArgs eventArgs)
        {
            IM = InspectMode.Collection;
        }

        private void ListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null && (IM == InspectMode.Collection || IM == InspectMode.CollFrame))
            {
                RefreshList3();
            }
        }

        private void ListBox5_MouseClick(object sender, EventArgs eventArgs)
        {
            IM = InspectMode.GenCollection;
        }

        private void ListBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedItem != null)
            {
                
            }
        }
        private void ListBox6_MouseClick(object sender, EventArgs eventArgs)
        {
            if (listBox6.SelectedItem != null)
            {
                IM = InspectMode.AnimFrame;
            }
        }
        private void ListBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Frame = listBox6.SelectedItem.ToString();
        }
        private void ListBox7_MouseClick(object sender, EventArgs eventArgs)
        {
            IM = InspectMode.Backup;
        }
        private void listBox7_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox7.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                listBox7.Items.RemoveAt(index);
            }
        }
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox7.SelectedItem != null)
            {
                Backup = listBox7.SelectedItem.ToString();
            }
        }

        private void ListBox8_MouseClick(object sender, EventArgs eventArgs)
        {
            IM = InspectMode.AnimFrame;
        }

        private void listBox8_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox8.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                listBox8.Items.RemoveAt(index);
            }
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox8.SelectedItem != null)
            {
                Frame = listBox8.SelectedItem.ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IM == InspectMode.Animation && listBox3.Items.Count > 0)
            {
                if (listBox3.SelectedIndex < listBox3.Items.Count - 1)
                {
                    listBox3.SelectedIndex++;
                }
                else
                {
                    listBox3.SelectedIndex = 0;
                }
                Frame = Frame;
            }
        }

        
    }
}