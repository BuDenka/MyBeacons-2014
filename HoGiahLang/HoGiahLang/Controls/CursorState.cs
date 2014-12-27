using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGiahLang.Controls {
    public enum CursorState {
        None,//隱藏游標
        Arrow,//預設
        ArrowWait,//等候
        ArrowUp,//特殊向上
        Hand,//首長
        Cross,//十字瞄準
        Help,//說明
        IBeam,//?
        No,//禁用
        Pen,//筆
        ScrollAll,//Move
        SizeE,
        SizeN,
        SizeNW,
        SizeNS,
        SizeNE,
        SizeS,
        SizeSE,
        SizeSW,
        SizeW,
        SizeWE 
    }
}