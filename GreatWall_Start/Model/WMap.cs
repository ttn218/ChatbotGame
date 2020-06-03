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
    [Serializable]
    public class WMap
    {
        private Dictionary<string, IDialog<string>> maps = new Dictionary<string, IDialog<string>>();

        public WMap()
        {
            maps.Add("0,0", new BaseEvent());
        }

        public IDialog<string> location(string loc)
        {
            IDialog<string> ID;
            maps.TryGetValue(loc,out ID);
            return ID;
        }
    }
}