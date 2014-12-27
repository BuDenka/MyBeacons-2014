using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HoGiahLang.Map.Object {
    /// <summary>
    /// GameObject.xaml 的互動邏輯
    /// </summary>
    public partial class GameObject : UserControl {
        public GameObject(string Id ,MapPosition InitPosition) {
            InitializeComponent();
            this.Id = Id;
            Position = InitPosition;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        #region 屬性
        public string Id {
            get; private set;
        }
        public MapPosition Position {
            get;  set;
        }


        private Uri _image;
        public Uri Image {
            get {
                return _image;
            }
            set {
                this.View.Source = new BitmapImage(value);
                _image = value;
            }
        }
        #endregion

        #region 方法
        public new async Task Focus() {
            MapGrid Grid = ((Grid)this.Parent).Parent as MapGrid;
            await Grid[this.Position.X ,this.Position.Y].Focus();
        }
        #endregion
    }
}
