using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.Bot.Builder.Dialogs;

using GreatWall.Event;

namespace GreatWall.Model
{
    public class WMap
    {
        private Dictionary<string, Type> maps = new Dictionary<string, Type>();

        public WMap()
        {
            try
            {
                using (StreamReader sr = new StreamReader("Map.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string command, cl;
                        string[] strdata;

                        strdata = sr.ReadLine().Split(':');

                        command = strdata[0];
                        cl = strdata[1];

                        Type ty = Type.GetType(cl);
                        maps.Add(command, ty);

                        Console.WriteLine(maps.ToString());
                        
                    }
                }
            }
            catch
            { 

            }
        }

        public Type location(string loc)
        {
            Type ID;
            maps.TryGetValue("0,0",out ID);
            return ID;
        }
    }
}