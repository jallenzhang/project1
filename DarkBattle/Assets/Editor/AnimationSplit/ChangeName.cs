using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.AnimationSplit
{
    public class ChangeName
    {
        [MenuItem("Assets/修改资源名称")]
        public static void ChangeResName()
        {
            UnityEngine.Object activeObj = Selection.activeObject;
            string objPath = AssetDatabase.GetAssetPath(activeObj);
            objPath = Application.dataPath.Replace("Assets", objPath);
            Debug.Log(objPath);
            string old = "";
            string newStr = "";
            CopyDirectory(objPath, old, newStr);
            AssetDatabase.Refresh();
        }
        public static void GetOldAndNewStr(ref string newstr, ref string old, string path)
        {
            if (path.Contains("2006"))
            {
                old = "2006";
                newstr = "20003";
            }
            else if (path.Contains("2008"))
            {
                old = "2008";
                newstr = "20011";
            }
            else if (path.Contains("2002"))
            {
                old = "2002";
                newstr = "20004";
            }
            else if (path.Contains("2010"))
            {
                old = "2010";
                newstr = "20002";
            }
            else if (path.Contains("2009"))
            {
                old = "2009";
                newstr = "20007";
            }
            else if (path.Contains("2004"))
            {
                old = "2004";
                newstr = "20009";
            }
            else if (path.Contains("2005"))
            {
                old = "2005";
                newstr = "20008";
            }
            else if (path.Contains("2003"))
            {
                old = "2003";
                newstr = "20016";
            }
            else if (path.Contains("2003"))
            {
                old = "2003";
                newstr = "20016";
            }
            else if (path.Contains("2012"))
            {
                old = "2012";
                newstr = "20005";
            }
            else if (path.Contains("2012"))
            {
                old = "2012";
                newstr = "20005";
            }
            else if (path.Contains("2001") && !path.Contains("20010") &&
                !path.Contains("20016") 
                && !path.Contains("20011") 
                )
            {
                old = "2001";
                newstr = "20006";
            }
            else if (path.Contains("2011"))
            {
                old = "2011";
                newstr = "20001";
            }
            else if (path.Contains("2007"))
            {
                old = "2007";
                newstr = "20010";
            }
            else
            {
                old = "meiyouzhaodao";
                newstr = "20010";
            }
        }

        public static  string GetNewStr(string old)
        {
            if (old == "20003")
            {
                return "2006";
            }
            else if (old == "20011")
            {
                return "2008";
            }
            else if (old == "20004")
            {
                return "2002";
            }
            else if (old == "20002")
            {
                return "2010";
            }
            else if (old == "20007")
            {
                return "2009";
            }
            else if (old == "20009")
            {
                return "2004";
            }
            else if (old == "20008")
            {
                return "2005";
            }
            else if (old == "20016")
            {
                return "2003";
            }
            else if (old == "20005")
            {
                return "2012";
            }
            else if (old == "20005")
            {
                return "2012";
            }
            else if (old == "20006")
            {
                return "2001";
            }
            else if (old == "20001")
            {
                return "2011";
            }
            else if (old == "20010")
            {
                return "2007";
            }
            return "0";
        }
        public static void CopyDirectory(string sourcePath,string old,string newstr)
        {
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            if (!info.Exists)
            {
                return;
            }

            GetOldAndNewStr(ref newstr, ref old, sourcePath);
            
            if (info.FullName.Contains(old))
            {
                info.MoveTo(sourcePath.Replace(old, newstr));
            }
            info = new DirectoryInfo(sourcePath.Replace(old, newstr));
            Debug.Log("sourcePath.Replace(old, newstr) =" + sourcePath.Replace(old, newstr));
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                if (fsi is System.IO.FileInfo)
                {
                    if (fsi.FullName.Contains(old))
                    {
                        System.IO.File.Move(fsi.FullName, fsi.FullName.Replace(old, newstr));
                    }
                }

                else
                {
                    //Directory.Move(sourcePath, sourcePath.Replace(old, newstr));
                    CopyDirectory(fsi.FullName, old, newstr);
                }
            }
        }
    }

}
