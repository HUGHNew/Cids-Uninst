using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using static uninst.Item;
using Microsoft.Win32;

namespace uninst
{
    public class Item
    {
        private const EnvironmentVariableTarget Machine = EnvironmentVariableTarget.Machine;
        private const EnvironmentVariableTarget Process = EnvironmentVariableTarget.Process;
        public const string Cids = "Cids";
        public const string Id = "CidsUUID";
        public static readonly string ImagePath= // the parent of image
            Path.Combine(Environment.GetEnvironmentVariable("TMP",Machine),Cids);
        public static readonly string image = Path.Combine(ImagePath,"image","raw.jpg");
        public static readonly string ConfPath=Environment.GetEnvironmentVariable(Cids,Process);
    }
    class Clear
    {
        public static void Run()
        {
            RemoveFiles();
            RemoveEnvs();
        }
        public static void RemoveEnvs()
        {
            RemoveCids();
            RemoveUuId();
        }
        public static void RemoveCids() { 

            Environment.SetEnvironmentVariable(Cids,null, EnvironmentVariableTarget.Process);
        }
        public static void RemoveUuId() {
            Environment.SetEnvironmentVariable(Id, null, EnvironmentVariableTarget.Machine);
        }
        public static void RemoveFiles() {
            RemoveConfFile();
            RemoveImageFile();
        }
        public static void RemoveConfFile()
        {
            Directory.Delete(ConfPath);
        }
        public static void RemoveImageFile() {
            WallPaperMigrate();
            Directory.Delete(ImagePath);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni); // for Wallpaper Set
        public static void SetWallpaper(string strSavePath)
        {
            #region Set Wall
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true); // get the key of desk wallpaper

            key.SetValue(@"WallpaperStyle", 2.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());

            SystemParametersInfo(20, 0, strSavePath, 0x01|0x02);
            #endregion
        }
        //<summary>迁移壁纸 然后设置壁纸为原图</summary>
        public static void WallPaperMigrate() {
            string newfile=Path.GetTempFileName();
            File.Copy(image, newfile);
            SetWallpaper(newfile);
        }
    }
}
