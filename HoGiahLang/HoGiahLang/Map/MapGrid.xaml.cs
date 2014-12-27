using System;
using System.Collections;
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
using Exa.SyntaxExten;
using System.Threading;
using HoGiahLang.Map.Object;

namespace HoGiahLang.Map {
    /// <summary>
    /// MpaGrid.xaml 的互動邏輯
    /// </summary>
    public partial class MapGrid : UserControl {
        #region 欄位
        public const float _nextX = 57.5f;//基礎常數
        public const float _nextY = 26.5f;
        const float _startX = 600f;
        const float _startY = 0f;

        public MapBlock[][] MapBlocks;
        ArrayList MapObjects;
        #endregion

        public MapGrid() {
            InitializeComponent();
            MapBlocks = new MapBlock[50][];
            MapObjects = new ArrayList();
            for(int i = 0; i < 50; i++) {
                MapBlocks[i] = new MapBlock[50];
            }
        }
        public void Load(MapContent Content) {
            for(int i = 0; i < Content.BlockMap.Length; i++) {
                for(int j = 0; j < Content.BlockMap[i].Length; j++) {
                    MapBlock Temp = Content.Blocks[Content.BlockMap[i][j]].Clone<MapBlock>();
                    if(Content.DirectionMap != null)
                        Temp.Direction = (Direction)Content.DirectionMap[i][j];
                    if(Content.SurfaceMap != null)
                        Temp.SurfaceTextureUri = Content.Surfaces[Content.SurfaceMap[i][j]];

                    
                    this[i ,j] = Temp;
                }
            }


            for(int i = 0; i < Content.ViewMap.Length; i++) {
                for(int j = 0; j < Content.ViewMap[i].Length; j++) {
                    GameObject V = new Map.Object.GameObject("Test" + i + j, new MapPosition() { X = i, Y = j });

                    if(Content.Views[Content.ViewMap[i][j]] == null)
                        continue;

                    V.Image = Content.Views[Content.ViewMap[i][j]];

                    this.AddGameObject(V);
                }
            }

            //for(int i = 0; i < Content.Blo)
        }
        public MapBlock this[int X ,int Y] {
            get {
                return GetBlock(new MapPosition() { X = X ,Y = Y });
            }
            set {
                SetBlock(new MapPosition() { X = X ,Y = Y } ,value);
            }
        }
        public void SetBlock(MapPosition Target ,MapBlock Block) {
            MapBlocks[Target.X][Target.Y] = Block;

            float Screen_X = Target.X * -_nextX + _startX;//取得X軸起點(X,0)座標
            float Screen_Y = Target.X * _nextY + _startY;

            
            Screen_X += Target.Y * _nextX;//加入Y軸偏移
            Screen_Y += Target.Y * _nextY;

            MapBlocks[Target.X][Target.Y].Margin = new Thickness(Screen_X ,Screen_Y ,0 ,0);
        }
        public MapBlock GetBlock(MapPosition Target) {
            return MapBlocks[Target.X][Target.Y];
        }
        public GameObject[] Objects{
            get{
                return this.MapObjects.ToArray<GameObject>();
            }
        }
        public void AddGameObject(GameObject NewObject) {
            MapObjects.Add(NewObject);
        }
        public void RemoveGameObject(GameObject Target) {
            MapObjects.Remove(Target);
        }
        public GameObject GetGameObject(string Id) {
            foreach(GameObject item in this.MapObjects) {
                if(item.Id == Id) {
                    return item;
                }
            }
            throw new KeyNotFoundException("找不到指定ID的遊戲物件");
        }
        public GameObject GetGameObject(MapPosition Position) {
            foreach(GameObject item in this.MapObjects) {
                if(item.Position.Equals(Position)) {
                    return item;
                }
            }
            return null;
        }
        public void ClearBlock() {
            this.MapBlocks = new MapBlock[50][];
            for(int i = 0; i < 50; i++) {
                MapBlocks[i] = new MapBlock[50];
            }
        }
        public void ClearObject() {
            this.MapObjects.Clear();
        }
        public void ClearScreen() {
            this.MainGrid.Children.Clear();
        }
        public async Task DisplayScreen() {
            int[][] displayOK = new int[MapBlocks.Length][];
            for(int i = 0 ; i < displayOK.Length ; i++){
                displayOK[i] = new int[MapBlocks[i].Length];
            }
            
            int now = 1;
            displayOK[0][0] = 1;//Start Position;
            await DisplayBlock(new MapPosition() { X = 0 ,Y = 0 });
            await Task.Run(async () => {
                while(true) {
                    int change = 0;
                    for(int i = 0; i < displayOK.Length; i++) {
                        for(int j = 0; j < displayOK[i].Length; j++) {
                            if(displayOK[i][j] != now)
                                continue;
                            MapPosition[] NextDisplay = GetNextDisplay(new MapPosition() { X = i ,Y = j } ,displayOK);
                            await DisplayBlocks(NextDisplay);
                            for(int k = 0; k < NextDisplay.Length; k++) {
                                Thread.Sleep(1);
                                displayOK[NextDisplay[k].X][NextDisplay[k].Y] = now + 1;
                                change++;
                            }
                        }
                    }
                    if(change == 0)
                        break;
                    now++;
                }
            });
        }
        #region DisplayAction
        private MapPosition[] GetNextDisplay(MapPosition Ref,int[][] DisplayRecode) {
            int[,] Next = {{1,0},{0,1},{-1,0},{0,-1}};//上下左右

            ArrayList result = new ArrayList();
            for(int i = 0; i < Next.GetLength(0); i++) {
                int new_X = Ref.X + Next[i ,0];
                int new_Y = Ref.Y + Next[i ,1];
                if(new_X < 0 || new_Y < 0)
                    continue;
                if(MapBlocks.Length <= new_X || MapBlocks[new_X].Length <= new_Y)
                    continue;
                if(DisplayRecode[new_X][new_Y] != 0)
                    continue;
                if(this.MapBlocks[new_X][new_Y] == null)
                    continue;
                result.Add(new MapPosition() { X = new_X ,Y = new_Y });
            }
            return result.ToArray<MapPosition>();
        }
        private async Task DisplayBlocks(MapPosition[] Target) {
            foreach(MapPosition item in Target) {
                Thread.Sleep(1);
                await DisplayBlock(item);
            }
        }
        public async Task DisplayBlock(MapPosition Target) {
            if(MapBlocks[Target.X][Target.Y] == null)
                return;
            await this.Dispatcher.InvokeAsync(() => {
                #region 側面顯示
                if(MapBlocks.Length - 1 == Target.X) {
                    MapBlocks[Target.X][Target.Y].LeftSideVisable = true;
                } else {
                    MapBlocks[Target.X][Target.Y].LeftSideVisable = false;
                }

                if(MapBlocks[Target.X].Length - 1 == Target.Y) {
                    MapBlocks[Target.X][Target.Y].RightSideVisable = true;
                } else {
                    MapBlocks[Target.X][Target.Y].RightSideVisable = false;
                }

                if(Target.X < MapBlocks.Length - 1) {
                    if(MapBlocks[Target.X + 1][Target.Y] == null) {
                        MapBlocks[Target.X][Target.Y].LeftSideVisable = true;
                    } else {
                        MapBlocks[Target.X][Target.Y].LeftSideVisable = false;
                    }
                }

                if(Target.Y < MapBlocks[Target.Y].Length - 1) {
                    if(MapBlocks[Target.X][Target.Y+1] == null) {
                        MapBlocks[Target.X][Target.Y].RightSideVisable = true;
                    } else {
                        MapBlocks[Target.X][Target.Y].RightSideVisable = false;
                    }
                }
                #endregion

                try {
                    try {
                        MainGrid.Children.Add(MapBlocks[Target.X][Target.Y]);
                    }catch {

                    }
                    GameObject OBJ = GetGameObject(new MapPosition() { X = Target.X, Y = Target.Y });
                    if(OBJ != null) {
                        Thickness blockposition = MapBlocks[Target.X][Target.Y].Margin;
                        OBJ.Margin = new Thickness(
                            blockposition.Left,
                            blockposition.Top - 68, 0, 0
                        );
                        MainGrid.Children.Add(OBJ);
                    }
                } catch { }
                MapBlocks[Target.X][Target.Y].Display();
            });
        }
        #endregion


        #region MoveAction
        public void MoveGameObject(ref GameObject obj, MapPosition position) {

            MapPosition old = obj.Position;
            
            obj.Position = position;

            DisplayBlock(old);
            DisplayBlock(position);
            
        }
        public void DeleteGameObject(GameObject obj) {

        }
        #endregion
    }
}