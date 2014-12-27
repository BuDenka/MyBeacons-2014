using Exa.Data.Json.Convert;
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

namespace HoGiahLang.Map {
    /// <summary>
    /// MapBlock.xaml 的互動邏輯
    /// </summary>
    public partial class MapBlock : UserControl,ICloneable {
        #region 欄位
        Direction _direction = Direction.North;
        Uri _uMainTexture;
        Uri _uRightSideTexture;
        Uri _uLeftSideTexture;
        Uri _uSurfaceTexture;
        ImageSource _iMainTexture;
        ImageSource _iRightSideTexture;
        ImageSource _iLeftSideTexture;
        ImageSource _iSurfaceTexture;
        #endregion
        public MapBlock() {
            InitializeComponent();
            LeftSideVisable = false;
            RightSideVisable = false;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left ;
            VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        
        #region 資料屬性
        [JsonAttribute]
        public bool IsRoad {
            get; 
            set;
        }

        [JsonAttribute]
        public Direction Direction {//方塊方向性
            get { return _direction; }
            set {
                _direction = value;
            }
        }

        public int ZIndex {
            get {
                return Canvas.GetZIndex(this);
            }
            set {
                Canvas.SetZIndex(this ,value);
            }
        }
        #endregion

        #region 材質Uri
        public Uri MainTextureUri {
            get {
                return _uMainTexture;
            }
            set {
                if(value == null) {
                    _iMainTexture = null;
                    _uMainTexture = null;
                    return;
                }
                try {
                    _iMainTexture = new BitmapImage(value);
                } catch(System.IO.IOException e) {
                    _iMainTexture = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + value.AbsolutePath));
                }
                _uMainTexture = value;
            }
        }
        public Uri LeftSideTextureUri {
            get {
                return _uLeftSideTexture;
            }
            set {
                if(value == null) {
                    _iLeftSideTexture = null;
                    _uLeftSideTexture = null;
                    return;
                }
                try {
                    _iLeftSideTexture = new BitmapImage(value);
                } catch {
                    _iLeftSideTexture = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + value.AbsolutePath));
                }
                _uLeftSideTexture = value;
            }
        }
        public Uri RightSideTextureUri {
            get {
                return _uRightSideTexture;
            }
            set {
                if(value == null) {
                    _iRightSideTexture = null;
                    _uRightSideTexture = null;
                    return;
                }
                try {
                    _iRightSideTexture = new BitmapImage(value);
                } catch {
                    _iRightSideTexture = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + value.AbsolutePath));
                }
                _uRightSideTexture = value;
            }
        }
        public Uri SurfaceTextureUri {
            get {
                return _uSurfaceTexture;
            }
            set {
                if(value == null) {
                    _iSurfaceTexture = null;
                    _uMainTexture = null;
                    return;
                }
                try {
                    _iSurfaceTexture = new BitmapImage(value);
                } catch {
                    _iSurfaceTexture = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + value.AbsolutePath));
                }
                _uSurfaceTexture = value;
            }
        }
        #endregion

        #region 側邊顯示設定屬性
        public bool LeftSideVisable {
            get {
                return LeftSideTextureImage.Visibility == System.Windows.Visibility.Visible;
            }
            set {
                if(value) {
                    LeftSideShadow.Visibility = System.Windows.Visibility.Visible;
                    LeftSideTextureImage.Visibility = System.Windows.Visibility.Visible;
                } else {
                    LeftSideShadow.Visibility = System.Windows.Visibility.Collapsed;
                    LeftSideTextureImage.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
        public bool RightSideVisable {
            get {
                return RightSideTextureImage.Visibility == System.Windows.Visibility.Visible;
            }
            set {
                if(value) {
                    RightSideTextureImage.Visibility = System.Windows.Visibility.Visible;
                } else {
                    RightSideTextureImage.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
        #endregion
        public new async Task Focus() {
            MapGrid Grid = ((Grid)this.Parent).Parent as MapGrid;
            MainWindow Window = ((Grid)Grid.Parent).Parent as MainWindow;

            double x = Grid.Margin.Left + this.Margin.Left;
            double y = Grid.Margin.Top + this.Margin.Top;

            double Target_x = Window.ActualWidth / 2 - MapGrid._nextX;
            double Target_y = Window.ActualHeight / 2 - MapGrid._nextY * 4;

            await Task.Run(() => {
                this.Dispatcher.Invoke(() => {
                    Grid.Margin = new Thickness(Grid.Margin.Left + Target_x - x ,Grid.Margin.Top + Target_y - y ,0 ,0);
                });
            });
        }
        public void Display() {
            #region 方向性檢查
            int rotation = 0;
            if(_direction == Map.Direction.East){
                rotation = 90;
            }else if(_direction == Map.Direction.South){
                rotation = 180;
            }else if(_direction == Map.Direction.West){
                rotation = 270;
            }
            #endregion

            #region 貼圖主程序
            #region 主圖層
            if(_iMainTexture == null) {
                MainTextureImage.Source = null;
            } else {
                TransformedBitmap tran = new TransformedBitmap(
                    (BitmapSource)_iMainTexture.Clone(),
                    new RotateTransform(rotation)
                );
                MainTextureImage.Source = tran;
            }
            #endregion

            #region 表圖層
            if(_iSurfaceTexture == null) {
                SurfaceTextureImage.Source = null;
            } else {
                TransformedBitmap tran = new TransformedBitmap(
                    (BitmapSource)_iSurfaceTexture.Clone() ,
                    new RotateTransform(rotation)
                );
                SurfaceTextureImage.Source = tran;
            }
            #endregion

            #region 左側貼圖
            LeftSideTextureImage.Source = _iLeftSideTexture;
            #endregion

            #region 右側貼圖
            RightSideTextureImage.Source = _iRightSideTexture;
            #endregion
            #endregion
        }

        #region 複製物件
        public object Clone() {
            MapBlock result = new MapBlock();
            result.LeftSideTextureUri = _uLeftSideTexture;
            result.MainTextureUri = _uMainTexture;
            result.RightSideTextureUri = _uRightSideTexture;
            result.SurfaceTextureUri = _uSurfaceTexture;

            result._iLeftSideTexture = _iLeftSideTexture;
            result._iRightSideTexture = _iRightSideTexture;
            result._iMainTexture = _iMainTexture;
            result._iSurfaceTexture = _iSurfaceTexture;            

            result._direction = this.Direction;

            return result;
        }
        public T Clone<T>() {
            return (T)Clone();
        }
        #endregion

        #region Json輸出輸入
        [JsonAttribute("MainTexture")]
        private string MainTextureUriString {
            get {
                if(MainTextureUri == null)
                    return null;
                return MainTextureUri.OriginalString;
            }
            set {
                if(value == null) {
                    MainTextureUri = null;
                    return;
                }
                MainTextureUri = new Uri(value);
            }
        }

        [JsonAttribute("SurfaceTexture")]
        private string SurfaceTextureUriString {
            get {
                if(SurfaceTextureUri == null)
                    return null;
                return SurfaceTextureUri.OriginalString;
            }
            set {
                if(value == null) {
                    SurfaceTextureUri = null;
                    return;
                }
                SurfaceTextureUri = new Uri(value);
            }
        }

        [JsonAttribute("LeftSideTexture")]
        private string LeftSideTextureUriString {
            get {
                if(LeftSideTextureUri == null)
                    return null;
                return LeftSideTextureUri.OriginalString;
            }
            set {
                if(value == null) {
                    LeftSideTextureUri = null;
                    return;
                }
                LeftSideTextureUri = new Uri(value);
            }
        }

        [JsonAttribute("RightSideTexture")]
        private string RightSideTextureUriString {
            get {
                if(RightSideTextureUri == null)
                    return null;
                return RightSideTextureUri.OriginalString;
            }
            set {
                if(value == null) {
                    RightSideTextureUri = null;
                    return;
                }
                RightSideTextureUri = new Uri(value);
            }
        }
        #endregion
    }
}
