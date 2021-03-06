﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Bot.Json
{
    public static class JsonHelper
    {
        public static T1 Get<T1>(string Path)
        {
            try
            {
                var json = File.ReadAllText(Path);

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Substring(0, Path.LastIndexOf("/")));
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }


                return JsonConvert.DeserializeObject<T1>(json);

            }
            catch (Exception ex)
            {
                return default(T1);
            }
        }
        public static bool Set(string Path, object Obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(Obj);

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Substring(0,Path.LastIndexOf("/")));
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                File.WriteAllText(Path, json);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
