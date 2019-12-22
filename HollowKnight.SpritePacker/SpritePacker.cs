using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace HollowKnight.SpritePacker
{
    public partial class Form1 : Form
    {
        #region Fields

        private readonly string folderpath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\locallow\\Team Cherry\\Hollow Knight\\sprites";
        private bool refreshing;
        private bool goodtopack = false;
        private bool check = false;
        private bool modechosen = false;
        private bool fixedmode = false;
        private bool backup = true;

        private string _anim;
        private string _clip;
        private string _frame;
        private string _oriatlas;
        private string _genatlas;
        private string _msg;
        private List<string> _backups;
        private string _changed;
        private string Anim
        {
            get
            {
                if (listBox1.SelectedItem != null)
                {
                    _anim = listBox1.SelectedItem.ToString();
                }
                return _anim;
            }
            set
            {
                int index = listBox1.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox1.SelectedIndex = index;
                }
                _anim = value;
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
                return _clip;
            }
            set
            {
                int index = listBox2.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox2.SelectedIndex = index;
                }
                _clip = value;
            }
        }
        private string Freame
        {
            get
            {
                if (listBox3.SelectedItem != null)
                {
                    _frame = listBox3.SelectedItem.ToString();
                }
                return _frame;
            }
            set
            {
                int index = listBox3.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox3.SelectedIndex = index;
                }
                _frame = value;
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
                return _oriatlas;
            }
            set
            {
                int index = listBox4.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox4.SelectedIndex = index;
                }
                _oriatlas = value;
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
                return _genatlas;
            }
            set
            {
                int index = listBox5.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox5.SelectedIndex = index;
                }
                _genatlas = value;
            }
        }
        private string Msg
        {
            get
            {
                if (listBox6.SelectedItem != null)
                {
                    _msg = listBox6.SelectedItem.ToString();
                }
                return _msg;
            }
            set
            {
                int index = listBox6.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox6.SelectedIndex = index;
                }
                _msg = value;
            }
        }
        private List<string> Backups
        {
            get
            {
                if (listBox7.SelectedItem != null)
                {
                    _backups = new List<string>();
                    foreach (var item in listBox7.SelectedItems)
                    {
                        _backups.Add(item.ToString());
                    }
                }
                return _backups;
            }
            set
            {
                listBox7.SelectedIndices.Clear();
                foreach (var item in value)
                {
                    int index = listBox7.FindStringExact(item);
                    if (index != ListBox.NoMatches)
                    {
                        listBox7.SelectedIndices.Add(index);
                    }
                }
                _backups = value;
            }
        }
        private string Changed
        {
            get
            {
                if (listBox8.SelectedItem != null)
                {
                    _changed = listBox8.SelectedItem.ToString();
                }
                return _changed;
            }
            set
            {
                int index = listBox8.FindStringExact(value);
                if (index != ListBox.NoMatches)
                {
                    listBox8.SelectedIndex = index;
                }
                _changed = value;
            }
        }
        private int FrameRate = 12;
        private BackComparer backComparer = new BackComparer();

        private InspectMode _im = InspectMode.Animation;

        private enum InspectMode
        {
            Animation,
            Collection,
            AnimFrame,
            CollFrame
        }
        private enum ReplaceMode
        {
            Single,
            All,
            Restore
        }
        private SpritesFolder spritesFolder;
        private FileSystemWatcher watcher;

        public bool replaceall = false;


        #endregion Fields

        #region Event listeners

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

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            string changed = e.Name.Split('\\')[2];
            if (listBox8.FindStringExact(changed) == ListBox.NoMatches)
            {
                listBox8.Invoke(new MethodInvoker(delegate () { listBox8.Items.Add(changed); }));
                this.replaceall = false;

            }

        }





        #endregion

        #region Functions

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
            checkBox1.Text = GlobalData.GlobalLanguage.Main_CheckBox1;
        }

        public void Log(string s)
        {
            listBox6.Items.Add(s);
            listBox6.TopIndex = listBox6.Items.Count - 3;
        }

        private bool CheckSavePath()
        {
            return Directory.Exists(folderpath);
        }


        private void RefreshList1()
        {
            refreshing = true;
            listBox1.Items.Clear();
            if (CheckSavePath())
            {
                foreach (var anim in spritesFolder.animations)
                {
                    listBox1.Items.Add(anim.info.Name);
                }
                listBox1.SelectedIndex = listBox1.FindStringExact(_anim);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshList2()
        {
            refreshing = true;
            listBox2.Items.Clear();
            if (CheckSavePath())
            {
                foreach (var anim in spritesFolder.animations)
                {
                    if (anim.info.Name == _anim)
                    {
                        foreach (var clip in anim.clips)
                        {
                            listBox2.Items.Add(clip.info.Name);
                        }
                    }
                }
                listBox2.SelectedIndex = listBox2.FindStringExact(_clip);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshList3()
        {
            refreshing = true;
            listBox3.Items.Clear();
            if (CheckSavePath())
            {
                foreach (var anim in spritesFolder.animations)
                {
                    if (anim.info.Name == _anim)
                    {
                        foreach (var clip in anim.clips)
                        {
                            if (clip.info.Name == _clip && _im != InspectMode.Collection && _im != InspectMode.CollFrame)
                            {
                                foreach (var frame in clip.frames)
                                {
                                    listBox3.Items.Add(frame.info.Name);
                                }
                            }
                        }
                    }
                }
                foreach (var collection in Collection.collections)
                {
                    if (collection.info.FullName.Contains(_anim))
                    {
                        if (_im == InspectMode.Collection && collection.name == _oriatlas)
                        {
                            collection.SortFrame();
                            foreach (var frame in collection.frames)
                            {
                                listBox3.Items.Add(frame.info.Name);
                            }
                        }
                    }
                }
                listBox3.SelectedIndex = listBox3.FindStringExact(_frame);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshList4()
        {
            refreshing = true;
            listBox4.Items.Clear();
            if (CheckSavePath())
            {
                foreach (var collection in Collection.collections)
                {
                    if (collection.info.FullName.Contains(_anim))
                    {
                        listBox4.Items.Add(collection.name);
                    }
                }
                listBox4.SelectedIndex = listBox4.FindStringExact(_oriatlas);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshList5()
        {
            refreshing = true;
            listBox5.Items.Clear();
            if (CheckSavePath())
            {
                foreach (var collection in Collection.gencollections)
                {
                    if (collection.info.FullName.Contains(_anim))
                        listBox5.Items.Add(collection.name);
                }
                listBox5.SelectedIndex = listBox5.FindStringExact(_genatlas);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshList6()
        {
            refreshing = true;
            listBox6.Items.Clear();
            if (CheckSavePath())
            {
                Log("[FolderPath] " + new DirectoryInfo(folderpath).FullName);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshList7()
        {
            refreshing = true;
            listBox7.Items.Clear();
            _backups = new List<string>();
            if (CheckSavePath())
            {
                foreach (var backup in new DirectoryInfo(folderpath).GetFiles("???-??-???[backup]??????.png", SearchOption.AllDirectories))
                {
                    _backups.Add(backup.Name);
                }
                _backups.Sort(backComparer);
                foreach (var item in _backups)
                {
                    listBox7.Items.Add(item);
                }
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
        }
        private void RefreshForm()
        {
            refreshing = true;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            _backups = new List<string>();

            Collection.collections.Clear();
            Collection.gencollections.Clear();

            SetValues();
            if (CheckSavePath())
            {

                spritesFolder = new SpritesFolder(new DirectoryInfo(folderpath));
                //refresh 123
                foreach (var anim in spritesFolder.animations)
                {
                    listBox1.Items.Add(anim.info.Name);

                    if (anim.info.Name == _anim)
                    {
                        foreach (var clip in anim.clips)
                        {
                            listBox2.Items.Add(clip.info.Name);
                            if (clip.info.Name == _clip && _im != InspectMode.Collection && _im != InspectMode.CollFrame)
                            {
                                foreach (var frame in clip.frames)
                                {
                                    listBox3.Items.Add(frame.info.Name);
                                }
                            }
                        }
                    }
                }
                //refresh 3,4
                foreach (var collection in Collection.collections)
                {
                    if (collection.info.FullName.Contains(_anim))
                    {
                        listBox4.Items.Add(collection.name);
                        if (_im == InspectMode.Collection && collection.name == _oriatlas)
                        {
                            collection.SortFrame();
                            foreach (var frame in collection.frames)
                            {
                                listBox3.Items.Add(frame.info.Name);
                            }
                        }
                    }
                }
                //refresh 5
                foreach (var collection in Collection.gencollections)
                {
                    if (collection.info.FullName.Contains(_anim))
                        listBox5.Items.Add(collection.name);
                }
                //refresh 6
                Log("[FolderPath] " + new DirectoryInfo(folderpath).FullName);
                //refresh 7
                foreach (var backup in new DirectoryInfo(folderpath).GetFiles("???-??-???[backup]??????.png", SearchOption.AllDirectories))
                {
                    _backups.Add(backup.Name);
                }
                _backups.Sort(backComparer);
                foreach (var item in _backups)
                {
                    listBox7.Items.Add(item);
                }
                listBox1.SelectedIndex = listBox1.FindStringExact(_anim);
                listBox2.SelectedIndex = listBox2.FindStringExact(_clip);
                listBox3.SelectedIndex = listBox3.FindStringExact(_frame);
                listBox4.SelectedIndex = listBox4.FindStringExact(_oriatlas);
                listBox5.SelectedIndex = listBox5.FindStringExact(_genatlas);
            }
            else
            {
                Log("[Error01] " + GlobalData.GlobalLanguage.Message_Error01);
            }
            refreshing = false;
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
                            Log("\t" + item.ToString());
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
                                Log("\t" + collection.frames[i].info.Name);
                                Log("\t" + collection.frames[i + 1].info.Name);
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
                            items = listBox7.Items.OfType<string>().ToArray();
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
                                        if (fixedmode && !FramePixelEquals(frame, frameneeded) && mode != ReplaceMode.Restore)
                                        {
                                            Bitmap fix = Fix(cutted, frame);
                                            if (File.Exists(dst))
                                            {
                                                File.Delete(dst);
                                            }
                                            fix.Save(dst);
                                        }
                                        if (!fixedmode && !FrameMD5HashEquals(frame, frameneeded) || mode == ReplaceMode .Restore)
                                        {
                                            if (File.Exists(dst))
                                            {
                                                File.Delete(dst);
                                            }
                                            File.Copy(orig, dst);
                                            if (mode == ReplaceMode.Restore && File.Exists(orig))
                                            {
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
        #endregion Functions

        #region Graphics

        private void ShowPic(InspectMode mode)
        {
            string path;
            switch (mode)
            {
                case InspectMode.Animation:
                    path = folderpath + "\\" + _anim + "\\" + _clip + "\\" + _frame;
                    break;
                case InspectMode.Collection:
                    path = "";
                    break;
                case InspectMode.AnimFrame:
                    path = folderpath + "\\" + _anim + "\\" + _clip + "\\" + _frame;
                    break;
                case InspectMode.CollFrame:
                    path = "";
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
        private void ShowFrame()
        {
            if (_im == InspectMode.Animation || _im == InspectMode.AnimFrame)
            {
                string Fpath = folderpath + "\\" + _anim + "\\" + _clip + "\\" + _frame;
                if (File.Exists(Fpath))
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }
                    pictureBox1.Image = Image.FromFile(Fpath);
                }
            }
            if (_im == InspectMode.CollFrame)
            {
                Collection collection = Collection.GetCollectionByName(_oriatlas);
                foreach (var frame in collection.frames)
                {
                    if (frame.info.Name == _frame)
                    {
                        if (pictureBox1.Image != null)
                        {
                            pictureBox1.Image.Dispose();
                        }
                        pictureBox1.Image = Image.FromFile(frame.info.FullName);
                        Log("[FilePath] " + frame.info.FullName.Replace(new DirectoryInfo(folderpath).FullName + "\\", ""));

                        _anim = frame.info.DirectoryName.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[0];
                        _clip = frame.info.DirectoryName.Replace(new DirectoryInfo(folderpath).FullName + "\\", "").Split('\\')[1];
                        refreshing = true;
                        listBox1.SelectedIndex = listBox1.FindStringExact(_anim);
                        listBox2.SelectedIndex = listBox2.FindStringExact(_clip);
                        refreshing = false;
                    }
                }
            }
        }

        private void ShowCollection()
        {
            string Cpath = folderpath + "\\" + _anim + "\\0.Atlases\\" + _oriatlas + ".png";
            if (File.Exists(Cpath))
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = Image.FromFile(Cpath);
            }
        }

        private void ShowGenCollection()
        {
            string Cpath = folderpath + "\\" + _anim + "\\0.Atlases\\" + _genatlas + ".png";
            if (File.Exists(Cpath))
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = Image.FromFile(Cpath);
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
                Bitmap image = new Bitmap(frame.info.FullName);

                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        int xold = frame.sprite.flipped ? frame.sprite.x + j : frame.sprite.x + i;
                        int yold = frame.sprite.flipped ? genatlas.Height - (frame.sprite.y + i) - 1 : genatlas.Height - (frame.sprite.y + j) - 1;
                        int x = (frame.sprite.flipped ? frame.sprite.x + j - (fixedmode ? frame.sprite.yr : 0) : frame.sprite.x + i - (fixedmode ? frame.sprite.xr : 0));
                        int y = (frame.sprite.flipped ? genatlas.Height - (frame.sprite.y + i) - 1 + (fixedmode ? frame.sprite.xr : 0) : genatlas.Height - (frame.sprite.y + j) - 1 + (fixedmode ? frame.sprite.yr : 0));
                        if (!fixedmode && (0 <= x && x < genatlas.Width && 0 <= y && y < genatlas.Height) ||
                            fixedmode && (frame.sprite.xr <= i && i < frame.sprite.xr + frame.sprite.width && frame.sprite.yr <= j && j < frame.sprite.yr + frame.sprite.height) && (0 <= xold && xold < genatlas.Width && 0 <= yold && yold < genatlas.Height))
                        {
                            genatlas.SetPixel(x, y, image.GetPixel(i, image.Height - j - 1));
                        }
                    }
                }
                num++;
                progression = (int)(100 * num / collection.frames.Count);
                progressBar1.Value = progression;
                if (progression == 100)
                {
                    progressBar1.Visible = false;
                }
            }
            string savepath = collection.info.DirectoryName + "\\Gen-" + collection.info.Name;
            genatlas.Save(savepath);
            Log("[" + GlobalData.GlobalLanguage.Main_Button3 + "] " + "Pack Done");
            Log("[" + GlobalData.GlobalLanguage.Main_Button3 + "] " + savepath);
        }
        private Bitmap Cut(Frame frame)
        {
            if (frame.sprite == null)
            {
                return null;
            }
            using (Bitmap bitmap = new Bitmap(frame.info.FullName))
            {
                return bitmap.Clone(new Rectangle(frame.sprite.xr, bitmap.Height - frame.sprite.yr - frame.sprite.height, frame.sprite.width, frame.sprite.height), bitmap.PixelFormat);
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
        #endregion Graphics

        #region Class

        private class BackComparer : IComparer<string>
        {
            private int start;
            private int len;
            private SortBy sort;
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
            public BackComparer()
            {
                Sort = SortBy.time;
            }
            public int Compare(string a, string b)
            {
                return a.Substring(start, len).CompareTo(b.Substring(start, len));
            }
        }

        #endregion
        public Form1()
        {
            InitializeComponent();
            RefreshForm();
            if (listBox1.Items.Count > 0)
            {
                _anim = listBox1.Items[0].ToString();
                RefreshList1();
                RefreshList2();
                if (listBox2.Items.Count > 0)
                {
                    _clip = listBox2.Items[0].ToString();
                    RefreshList3();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!refreshing && listBox1.SelectedItem != null)
            {
                _anim = listBox1.SelectedItem.ToString();
                RefreshList1();
                RefreshList2();
                RefreshList3();
                RefreshList4();
                RefreshList5();
                RefreshList6();
            }
        }

        private void ListBox1_MouseClick(object sender, EventArgs eventArgs)
        {
        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!refreshing && listBox2.SelectedItem != null)
            {
                _clip = listBox2.SelectedItem.ToString();
                RefreshList2();
                RefreshList3();

            }
        }

        private void ListBox2_MouseClick(object sender, EventArgs eventArgs)
        {
            timer1.Interval = 1000 / FrameRate;
            _im = InspectMode.Animation;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!refreshing && listBox3.SelectedItem != null)
            {
                refreshing = true;
                int index = listBox8.FindStringExact(listBox3.SelectedItem.ToString());
                if (index != ListBox.NoMatches)
                {
                    listBox8.SelectedIndex = index;
                }
                refreshing = false;

                _frame = listBox3.SelectedItem.ToString();
                ShowFrame();
            }
        }

        private void ListBox3_MouseClick(object sender, EventArgs eventArgs)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            if (_im == InspectMode.Animation)
            {
                _im = InspectMode.AnimFrame;
            }
            if (_im == InspectMode.Collection)
            {
                _im = InspectMode.CollFrame;
            }
        }

        private void ListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!refreshing && listBox4.SelectedItem != null)
            {
                _oriatlas = listBox4.SelectedItem.ToString();
                RefreshList4();
                RefreshList3();
                ShowCollection();
            }
        }

        private void ListBox4_MouseClick(object sender, EventArgs eventArgs)
        {
            goodtopack = false;
            check = false;
            replaceall = false;
            button1.Visible = true;
            _im = InspectMode.Collection;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void ListBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!refreshing && listBox5.SelectedItem != null)
            {
                _genatlas = listBox5.SelectedItem.ToString();
                RefreshList5();
                ShowGenCollection();
            }
        }

        private void ListBox5_MouseClick(object sender, EventArgs eventArgs)
        {
            _im = InspectMode.Collection;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void ListBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ListBox6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox6.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                refreshing = true;
                listBox6.Items.RemoveAt(index);
                refreshing = false;
            }
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_im == InspectMode.Animation && listBox3.Items.Count > 0)
            {
                ShowFrame();
                if (listBox3.SelectedIndex < listBox3.Items.Count - 1)
                {
                    listBox3.SelectedIndex++;
                }
                else
                {
                    listBox3.SelectedIndex = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_im == InspectMode.Animation || _im == InspectMode.AnimFrame)
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
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (goodtopack && (_im == InspectMode.Collection || _im == InspectMode.CollFrame) && listBox4.SelectedIndex >= 0)
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            backup = checkBox1.Checked;
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
                refreshing = true;
                listBox8.Items.Clear();
                refreshing = false;
            }
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!refreshing && listBox3.SelectedItem != null)
            {
                refreshing = true;
                int index = listBox3.FindStringExact(listBox8.SelectedItem.ToString());
                if (index != ListBox.NoMatches)
                {
                    listBox3.SelectedIndex = index;
                }
                refreshing = false;
                _frame = listBox3.SelectedItem.ToString();
                ShowFrame();
            }
            listBox3.TopIndex = listBox3.SelectedIndex;
        }
        private void listBox8_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox8.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                refreshing = true;
                listBox8.Items.RemoveAt(index);
                refreshing = false;
            }
        }
        private void ListBox8_MouseClick(object sender, EventArgs eventArgs)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            if (_im == InspectMode.Animation)
            {
                _im = InspectMode.AnimFrame;
            }
            if (_im == InspectMode.Collection)
            {
                _im = InspectMode.CollFrame;
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

        private void button5_Click(object sender, EventArgs e)
        {
            Replace(ReplaceMode.Restore);
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}