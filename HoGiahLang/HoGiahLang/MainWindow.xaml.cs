using Exa.Data.Json;
using Exa.Data.Json.Convert;
using HoGiahLang.Map;
using HoGiahLang.Map.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HoGiahLang {
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private async void Grid_Loaded(object sender ,RoutedEventArgs e) {
            var obj = Assembly.GetExecutingAssembly().GetName().Version;
            VersionLabel.Content = string.Format("HoGiahLang Version {0}(Pre-alpha)" ,obj);
            this.Cursor = new Cursor(System.AppDomain.CurrentDomain.BaseDirectory + @"Cursor\Def.cur");


            #region 註解
            /*
            Uri Side = new Uri("pack://application:,,,/Texture/grass_side.png");
            
            Map.MapBlock temp1 = new Map.MapBlock();
            temp1.MainTextureUri = new Uri("pack://application:,,,/Texture/grass.png");
            temp1.LeftSideTextureUri = Side;
            temp1.RightSideTextureUri = Side;

            Map.MapBlock temp2 = temp1.Clone<MapBlock>();
            temp2.MainTextureUri = new Uri("pack://application:,,,/Texture/road.png");

            Map.MapBlock temp3 = temp1.Clone<MapBlock>();
            temp3.MainTextureUri = new Uri("pack://application:,,,/Texture/road_turn.png");

            Map.MapBlock temp4 = temp1.Clone<MapBlock>();
            temp4.MainTextureUri = new Uri("pack://application:,,,/Texture/road_There.png");


            MapContent map0 = new MapContent();
            map0.Name = "測試用地圖";
            map0.Blocks = new MapBlock[] {temp1,temp2,temp3,temp4};
            map0.BlockMap = new byte[][]{
                new byte[]{2,1,1,1,3,1,1,1,2},
                new byte[]{1,0,0,0,1,0,0,0,1},
                new byte[]{1,0,0,0,1,0,0,0,1},
                new byte[]{1,0,0,0,1,0,0,0,1},
                new byte[]{1,0,0,0,3,1,1,1,3},
                new byte[]{1,0,0,0,1,0,0,0,1},
                new byte[]{1,0,0,0,1,0,0,0,1},
                new byte[]{1,0,0,0,1,0,0,0,1},
                new byte[]{2,1,1,1,3,1,1,1,2}
            };
            map0.DirectionMap = new byte[][]{
                new byte[]{0,0,0,0,2,0,0,0,1},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{1,0,0,0,1,0,0,0,3},
                new byte[]{3,0,0,0,0,0,0,0,2}
            };
            map0.Surfaces = new Uri[]{null,
                new Uri("pack://application:,,,/Texture/CuevanaStorm-icon.png")
            };
            //System.AppDomain.CurrentDomain.BaseDirectory
            map0.SurfaceMap = new byte[][]{
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,1,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0},
                new byte[]{0,0,0,0,0,0,0,0,0}
            };


            string mapdd = JsonConvert.Serialize(map0).ToString();
            
            */
            #endregion

            LoadGrid.Visibility = Visibility.Visible;
            System.Windows.Forms.Application.DoEvents();
            //await Test.Focus();
        }
        bool loaded = false;
        private async void Window_Activated(object sender, EventArgs e) {
            if(loaded) return;

           

            JsonObject data = JsonObject.Load(new StreamReader(@"Maps\TestMap.json").ReadToEnd());

            this.Cmd.Focus();

            MapContent map = JsonConvert.Deserialize<MapContent>(data);
            MapView.Load(map);

            

            //GameObject Test = new Map.Object.GameObject("Test" ,new MapPosition() { X = 0 ,Y = 0 });

            //MapView.AddGameObject(Test);

            await MapView.DisplayScreen();

            await MapView[5, 5].Focus();

            System.Windows.Forms.Application.DoEvents();

            Thread.Sleep(2000);

            LoadGrid.Visibility = Visibility.Collapsed;
            Title = "逮丸の物語 - " + map.Name;

            loaded = true;
        }
        Point temp;
        private void Window_MouseMove(object sender ,MouseEventArgs e) {
            if(e.RightButton != MouseButtonState.Pressed) {
                return;
            }

            Point k = e.GetPosition(this);

            double X = k.X - temp.X;
            double Y = k.Y - temp.Y;

            MapView.Margin = new Thickness(MapView.Margin.Left + X*2 ,MapView.Margin.Top + Y*2 ,0 ,0);
            temp = k;
        }

        private void MainGrid_MouseRightButtonDown(object sender ,MouseButtonEventArgs e) {
            this.Cursor = new Cursor(System.AppDomain.CurrentDomain.BaseDirectory+ @"Cursor\AllMove.cur");
            temp = e.GetPosition(this);
        }

        private void Window_MouseRightButtonUp(object sender ,MouseButtonEventArgs e) {
            this.Cursor = new Cursor(System.AppDomain.CurrentDomain.BaseDirectory + @"Cursor\Def.cur");
        }

        public void MatrixAnimationUsingPathDoesRotateWithTangentExample()
        {

            // Create a NameScope for the page so that
            // we can use Storyboards.
            NameScope.SetNameScope(MainGrid ,new NameScope());

            // Create a button.
            Map.MapBlock aButton = new Map.MapBlock();
            aButton.LeftSideVisable = true;
            aButton.RightSideVisable = true;
            // Create a MatrixTransform. This transform
            // will be used to move the button.
            MatrixTransform buttonMatrixTransform = new MatrixTransform();
            aButton.RenderTransform = buttonMatrixTransform;

            // Register the transform's name with the page
            // so that it can be targeted by a Storyboard.
            MainGrid.RegisterName("ButtonMatrixTransform" ,buttonMatrixTransform);

            // Create a Canvas to contain the button
            // and add it to the page.
            // Although this example uses a Canvas,
            // any type of panel will work.
            Canvas mainPanel = new Canvas();
            mainPanel.Width = 400;
            mainPanel.Height = 400;
            mainPanel.Children.Add(aButton);
            MainGrid.Children.Add(mainPanel);

            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            pFigure.StartPoint = new Point(10, 100);
            PolyBezierSegment pBezierSegment = new PolyBezierSegment();
            pBezierSegment.Points.Add(new Point(35, 0));
            pBezierSegment.Points.Add(new Point(135, 0));
            pBezierSegment.Points.Add(new Point(160, 100));
            pBezierSegment.Points.Add(new Point(180, 190));
            pBezierSegment.Points.Add(new Point(285, 200));
            pBezierSegment.Points.Add(new Point(310, 100));
            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits.
            animationPath.Freeze();

            // Create a MatrixAnimationUsingPath to move the
            // button along the path by animating
            // its MatrixTransform.
            MatrixAnimationUsingPath matrixAnimation =
                new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = animationPath;
            matrixAnimation.Duration = TimeSpan.FromSeconds(5);
            matrixAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Set the animation's DoesRotateWithTangent property
            // to true so that rotates the rectangle in addition
            // to moving it.
            matrixAnimation.DoesRotateWithTangent = true;

            // Set the animation to target the Matrix property
            // of the MatrixTransform named "ButtonMatrixTransform".
            Storyboard.SetTargetName(matrixAnimation, "ButtonMatrixTransform");
            Storyboard.SetTargetProperty(matrixAnimation, 
                new PropertyPath(MatrixTransform.MatrixProperty));

            // Create a Storyboard to contain and apply the animation.
            Storyboard pathAnimationStoryboard = new Storyboard();
            pathAnimationStoryboard.Children.Add(matrixAnimation);

            // Start the storyboard when the button is loaded.
            aButton.Loaded += delegate(object sender, RoutedEventArgs e)
            {
                // Start the storyboard.
                pathAnimationStoryboard.Begin(MainGrid);
            };



        }


        GameObject Player;
        private async void TextBox_PreviewKeyDown(object sender ,KeyEventArgs e) {
            if(e.Key != Key.Enter)
                return;
                
            string[] data = Cmd.Text.Split(' ');
            if(data[0].ToLower() == "?focus") {
                try {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);
                    await MapView[x, y].Focus();
                } catch {
                    chat.Text += "\r\nSYSTEM : 指令錯誤";
                }
            } else if(data[0].ToLower() == "?move") {
                if(Player == null) return;
                try {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);
                    MapView.MoveGameObject(ref Player,new MapPosition() { X = x, Y = y });
                }catch {
                    chat.Text += "\r\nSYSTEM : 指令錯誤";
                }
            } else if(data[0].ToLower() == "?player") {
                try {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);

                    MapView.AddGameObject(Player = new GameObject("Shan", new MapPosition() { X = x, Y = y }) {
                        Image = new Uri("pack://application:,,,/Texture/Shan_R.png")
                    });

                    MapView.DisplayBlock(new MapPosition() { X = x, Y = y });


                    //this.MapView.ClearScreen();
                    //await this.MapView.DisplayScreen();
                    //await MapView[x, y].Focus();
                } catch {
                    chat.Text += "\r\nSYSTEM : 指令錯誤";
                }
            } else if(data[0].ToLower() == "?set") {
                try {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);
                    int x_ = int.Parse(data[3]);
                    int y_ = int.Parse(data[4]);

                    MapView[x_, y_] = MapView[x, y].Clone<MapBlock>();
                    this.MapView.ClearScreen();
                    await this.MapView.DisplayScreen();
                    //await MapView[x, y].Focus();
                } catch {
                    chat.Text += "\r\nSYSTEM : 指令錯誤";
                }
            } else if(data[0].ToLower() == "?clear") {
                chat.Text = "";
            } else if(data[0].ToLower() == "?redisplay") {
                MapView.ClearScreen();
                MapView.DisplayScreen();
            } else if(data[0].ToLower() == "?help") {
                chat.Text += "\r\n<<help>>\r\nfocus\r\nplayer\r\nmove\r\nset\r\nclear\r\nredisplay";
            } else { 
                chat.Text += "\r\nUser : " + Cmd.Text;
            }
            //chat.IsEnabled = false;
            chat.IsEnabled = true;

            chat.Focus();
            chat.ScrollToEnd();
            Cmd.Clear();
            Cmd.Focus();

            chat.IsEnabled = false;
        }

        private void Window_MouseLeave(object sender ,MouseEventArgs e) {
            this.Cursor = new Cursor(System.AppDomain.CurrentDomain.BaseDirectory + @"Cursor\Def.cur");
        }

        private void Cmd_TextChanged(object sender, TextChangedEventArgs e) {

        }

        
    }
}
