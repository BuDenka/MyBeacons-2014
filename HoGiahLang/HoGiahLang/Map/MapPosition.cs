using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGiahLang.Map {
    public struct MapPosition {
        public int X {
            get;
            set;
        }
        public int Y {
            get;
            set;
        }
        public override bool Equals(object obj) {
            MapPosition? temp = obj as MapPosition?;
            if(!temp.HasValue)
                return false;

            return (temp.Value.X == X) && (temp.Value.Y == Y);
        }
    }
}
