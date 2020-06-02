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
        private Dictionary<String, IDialog<string>> maps = new Dictionary<string, IDialog<string>>();

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
                        maps.Add(command, (IDialog<string>)Activator.CreateInstance(ty));

                        Console.WriteLine(maps.ToString());
                        
                    }
                }
            }
            catch
            { 

            }
        }

        public object location(string loc)
        {
            IDialog<string> ID;
            maps.TryGetValue("0,0",out ID);
            return (object)ID;
        }
    }
}