using Exa.Data.Json.Convert;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Exa.SyntaxExten;

namespace HoGiahLang.Map {
    public class MapContent {

        [JsonAttribute(Order=0)]
        public string Name {
            get;
            set;
        }

        [JsonAttribute(Order=1)]
        public MapBlock[] Blocks { get; set; }

        [JsonAttribute(Order=2)]
        public byte[][] BlockMap {get;set;}

        [JsonAttribute(Order=3)]
        public byte[][] DirectionMap { get; set; }

        
        public Uri[] Surfaces { get; set; }

        [JsonAttribute("Surface",Order = 4)]
        private string[] Surfaces_String {
            get {
                ArrayList result = new ArrayList();
                for(int i = 0; i < Surfaces.Length; i++) {
                    if(Surfaces[i] == null) {
                        result.Add(null);
                        continue;
                    }
                    result.Add(Surfaces[i].OriginalString);
                }
                return result.ToArray<string>();
            }
            set {
                ArrayList result = new ArrayList();
                foreach(string item in value) {
                    if(item == null) {
                        result.Add(null);
                        continue;
                    }
                    result.Add(new Uri(item));
                }
                Surfaces = result.ToArray<Uri>();
            }
        }

        [JsonAttribute(Order = 5)]
        public byte[][] SurfaceMap { get; set; }

        [JsonAttribute("Views", Order = 6)]
        private string[] Views_String {
            get {
                ArrayList result = new ArrayList();
                for(int i = 0; i < Views.Length; i++) {
                    if(Views[i] == null) {
                        result.Add(null);
                        continue;
                    }
                    result.Add(Views[i].OriginalString);
                }
                return result.ToArray<string>();
            }
            set {
                ArrayList result = new ArrayList();
                foreach(string item in value) {
                    if(item == null) {
                        result.Add(null);
                        continue;
                    }
                    result.Add(new Uri(item));
                }
                Views = result.ToArray<Uri>();
            }
        }

        public Uri[] Views {
            get;set;
        }

        [JsonAttribute]
        public byte[][] ViewMap { get; set; }

    }
}
