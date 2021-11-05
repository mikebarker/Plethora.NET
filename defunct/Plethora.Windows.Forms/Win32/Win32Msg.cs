namespace Plethora.Win32
{
    /// <summary>
    /// Class defining the windows messages constants.
    /// </summary>
    public static class Win32Msg
    {
        #region Fields

        /// <summary>
        /// WM_NULL win32 message.
        /// </summary>
        public readonly static int WM_NULL = 0x00;

        /// <summary>
        /// WM_CREATE win32 message.
        /// </summary>
        public readonly static int WM_CREATE = 0X01;

        /// <summary>
        /// WM_DESTROY win32 message.
        /// </summary>
        public readonly static int WM_DESTROY = 0X02;

        /// <summary>
        /// WM_MOVE win32 message.
        /// </summary>
        public readonly static int WM_MOVE = 0X03;

        /// <summary>
        /// WM_SIZE win32 message.
        /// </summary>
        public readonly static int WM_SIZE = 0X05;

        /// <summary>
        /// WM_ACTIVATE win32 message.
        /// </summary>
        public readonly static int WM_ACTIVATE = 0X06;

        /// <summary>
        /// WM_SETFOCUS win32 message.
        /// </summary>
        public readonly static int WM_SETFOCUS = 0X07;

        /// <summary>
        /// WM_KILLFOCUS win32 message.
        /// </summary>
        public readonly static int WM_KILLFOCUS = 0X08;

        /// <summary>
        /// WM_ENABLE win32 message.
        /// </summary>
        public readonly static int WM_ENABLE = 0X0A;

        /// <summary>
        /// WM_SETREDRAW win32 message.
        /// </summary>
        public readonly static int WM_SETREDRAW = 0X0B;

        /// <summary>
        /// WM_SETTEXT win32 message.
        /// </summary>
        public readonly static int WM_SETTEXT = 0X0C;

        /// <summary>
        /// WM_GETTEXT win32 message.
        /// </summary>
        public readonly static int WM_GETTEXT = 0X0D;

        /// <summary>
        /// WM_GETTEXTLENGTH win32 message.
        /// </summary>
        public readonly static int WM_GETTEXTLENGTH = 0X0E;

        /// <summary>
        /// WM_PAINT win32 message.
        /// </summary>
        public readonly static int WM_PAINT = 0X0F;

        /// <summary>
        /// WM_CLOSE win32 message.
        /// </summary>
        public readonly static int WM_CLOSE = 0X10;

        /// <summary>
        /// WM_QUERYENDSESSION win32 message.
        /// </summary>
        public readonly static int WM_QUERYENDSESSION = 0X11;

        /// <summary>
        /// WM_QUIT win32 message.
        /// </summary>
        public readonly static int WM_QUIT = 0X12;

        /// <summary>
        /// WM_QUERYOPEN win32 message.
        /// </summary>
        public readonly static int WM_QUERYOPEN = 0X13;

        /// <summary>
        /// WM_ERASEBKGND win32 message.
        /// </summary>
        public readonly static int WM_ERASEBKGND = 0X14;

        /// <summary>
        /// WM_SYSCOLORCHANGE win32 message.
        /// </summary>
        public readonly static int WM_SYSCOLORCHANGE = 0X15;

        /// <summary>
        /// WM_ENDSESSION win32 message.
        /// </summary>
        public readonly static int WM_ENDSESSION = 0X16;

        /// <summary>
        /// WM_SYSTEMERROR win32 message.
        /// </summary>
        public readonly static int WM_SYSTEMERROR = 0X17;

        /// <summary>
        /// WM_SHOWWINDOW win32 message.
        /// </summary>
        public readonly static int WM_SHOWWINDOW = 0X18;

        /// <summary>
        /// WM_CTLCOLOR win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLOR = 0X19;

        /// <summary>
        /// WM_WININICHANGE win32 message.
        /// </summary>
        public readonly static int WM_WININICHANGE = 0X1A;

        /// <summary>
        /// WM_SETTINGCHANGE win32 message.
        /// </summary>
        public readonly static int WM_SETTINGCHANGE = 0X1A;

        /// <summary>
        /// WM_DEVMODECHANGE win32 message.
        /// </summary>
        public readonly static int WM_DEVMODECHANGE = 0X1B;

        /// <summary>
        /// WM_ACTIVATEAPP win32 message.
        /// </summary>
        public readonly static int WM_ACTIVATEAPP = 0X1C;

        /// <summary>
        /// WM_FONTCHANGE win32 message.
        /// </summary>
        public readonly static int WM_FONTCHANGE = 0X1D;

        /// <summary>
        /// WM_TIMECHANGE win32 message.
        /// </summary>
        public readonly static int WM_TIMECHANGE = 0X1E;

        /// <summary>
        /// WM_CANCELMODE win32 message.
        /// </summary>
        public readonly static int WM_CANCELMODE = 0X1F;

        /// <summary>
        /// WM_SETCURSOR win32 message.
        /// </summary>
        public readonly static int WM_SETCURSOR = 0X20;

        /// <summary>
        /// WM_MOUSEACTIVATE win32 message.
        /// </summary>
        public readonly static int WM_MOUSEACTIVATE = 0X21;

        /// <summary>
        /// WM_CHILDACTIVATE win32 message.
        /// </summary>
        public readonly static int WM_CHILDACTIVATE = 0X22;

        /// <summary>
        /// WM_QUEUESYNC win32 message.
        /// </summary>
        public readonly static int WM_QUEUESYNC = 0X23;

        /// <summary>
        /// WM_GETMINMAXINFO win32 message.
        /// </summary>
        public readonly static int WM_GETMINMAXINFO = 0X24;

        /// <summary>
        /// WM_PAINTICON win32 message.
        /// </summary>
        public readonly static int WM_PAINTICON = 0X26;

        /// <summary>
        /// WM_ICONERASEBKGND win32 message.
        /// </summary>
        public readonly static int WM_ICONERASEBKGND = 0X27;

        /// <summary>
        /// WM_NEXTDLGCTL win32 message.
        /// </summary>
        public readonly static int WM_NEXTDLGCTL = 0X28;

        /// <summary>
        /// WM_SPOOLERSTATUS win32 message.
        /// </summary>
        public readonly static int WM_SPOOLERSTATUS = 0X2A;

        /// <summary>
        /// WM_DRAWITEM win32 message.
        /// </summary>
        public readonly static int WM_DRAWITEM = 0X2B;

        /// <summary>
        /// WM_MEASUREITEM win32 message.
        /// </summary>
        public readonly static int WM_MEASUREITEM = 0X2C;



        /// <summary>
        /// WM_DELETEITEM win32 message.
        /// </summary>
        public readonly static int WM_DELETEITEM = 0X2D;

        /// <summary>
        /// WM_VKEYTOITEM win32 message.
        /// </summary>
        public readonly static int WM_VKEYTOITEM = 0X2E;

        /// <summary>
        /// WM_CHARTOITEM win32 message.
        /// </summary>
        public readonly static int WM_CHARTOITEM = 0X2F;


        /// <summary>
        /// WM_SETFONT win32 message.
        /// </summary>
        public readonly static int WM_SETFONT = 0X30;

        /// <summary>
        /// WM_GETFONT win32 message.
        /// </summary>
        public readonly static int WM_GETFONT = 0X31;

        /// <summary>
        /// WM_SETHOTKEY win32 message.
        /// </summary>
        public readonly static int WM_SETHOTKEY = 0X32;

        /// <summary>
        /// WM_GETHOTKEY win32 message.
        /// </summary>
        public readonly static int WM_GETHOTKEY = 0X33;

        /// <summary>
        /// WM_QUERYDRAGICON win32 message.
        /// </summary>
        public readonly static int WM_QUERYDRAGICON = 0X37;

        /// <summary>
        /// WM_COMPAREITEM win32 message.
        /// </summary>
        public readonly static int WM_COMPAREITEM = 0X39;

        /// <summary>
        /// WM_COMPACTING win32 message.
        /// </summary>
        public readonly static int WM_COMPACTING = 0X41;

        /// <summary>
        /// WM_WINDOWPOSCHANGING win32 message.
        /// </summary>
        public readonly static int WM_WINDOWPOSCHANGING = 0X46;

        /// <summary>
        /// WM_WINDOWPOSCHANGED win32 message.
        /// </summary>
        public readonly static int WM_WINDOWPOSCHANGED = 0X47;

        /// <summary>
        /// WM_POWER win32 message.
        /// </summary>
        public readonly static int WM_POWER = 0X48;

        /// <summary>
        /// WM_COPYDATA win32 message.
        /// </summary>
        public readonly static int WM_COPYDATA = 0X4A;

        /// <summary>
        /// WM_CANCELJOURNAL win32 message.
        /// </summary>
        public readonly static int WM_CANCELJOURNAL = 0X4B;

        /// <summary>
        /// WM_NOTIFY win32 message.
        /// </summary>
        public readonly static int WM_NOTIFY = 0X4E;

        /// <summary>
        /// WM_INPUTLANGCHANGEREQUEST win32 message.
        /// </summary>
        public readonly static int WM_INPUTLANGCHANGEREQUEST = 0X50;

        /// <summary>
        /// WM_INPUTLANGCHANGE win32 message.
        /// </summary>
        public readonly static int WM_INPUTLANGCHANGE = 0X51;

        /// <summary>
        /// WM_TCARD win32 message.
        /// </summary>
        public readonly static int WM_TCARD = 0X52;

        /// <summary>
        /// WM_HELP win32 message.
        /// </summary>
        public readonly static int WM_HELP = 0X53;

        /// <summary>
        /// WM_USERCHANGED win32 message.
        /// </summary>
        public readonly static int WM_USERCHANGED = 0X54;

        /// <summary>
        /// WM_NOTIFYFORMAT win32 message.
        /// </summary>
        public readonly static int WM_NOTIFYFORMAT = 0X55;

        /// <summary>
        /// WM_CONTEXTMENU win32 message.
        /// </summary>
        public readonly static int WM_CONTEXTMENU = 0X7B;

        /// <summary>
        /// WM_STYLECHANGING win32 message.
        /// </summary>
        public readonly static int WM_STYLECHANGING = 0X7C;

        /// <summary>
        /// WM_STYLECHANGED win32 message.
        /// </summary>
        public readonly static int WM_STYLECHANGED = 0X7D;

        /// <summary>
        /// WM_DISPLAYCHANGE win32 message.
        /// </summary>
        public readonly static int WM_DISPLAYCHANGE = 0X7E;

        /// <summary>
        /// WM_GETICON win32 message.
        /// </summary>
        public readonly static int WM_GETICON = 0X7F;

        /// <summary>
        /// WM_SETICON win32 message.
        /// </summary>
        public readonly static int WM_SETICON = 0X80;


        /// <summary>
        /// WM_NCCREATE win32 message.
        /// </summary>
        public readonly static int WM_NCCREATE = 0X81;

        /// <summary>
        /// WM_NCDESTROY win32 message.
        /// </summary>
        public readonly static int WM_NCDESTROY = 0X82;

        /// <summary>
        /// WM_NCCALCSIZE win32 message.
        /// </summary>
        public readonly static int WM_NCCALCSIZE = 0X83;

        /// <summary>
        /// WM_NCHITTEST win32 message.
        /// </summary>
        public readonly static int WM_NCHITTEST = 0X84;

        /// <summary>
        /// WM_NCPAINT win32 message.
        /// </summary>
        public readonly static int WM_NCPAINT = 0X85;

        /// <summary>
        /// WM_NCACTIVATE win32 message.
        /// </summary>
        public readonly static int WM_NCACTIVATE = 0X86;

        /// <summary>
        /// WM_GETDLGCODE win32 message.
        /// </summary>
        public readonly static int WM_GETDLGCODE = 0X87;

        /// <summary>
        /// WM_NCMOUSEMOVE win32 message.
        /// </summary>
        public readonly static int WM_NCMOUSEMOVE = 0XA0;

        /// <summary>
        /// WM_NCLBUTTONDOWN win32 message.
        /// </summary>
        public readonly static int WM_NCLBUTTONDOWN = 0XA1;

        /// <summary>
        /// WM_NCLBUTTONUP win32 message.
        /// </summary>
        public readonly static int WM_NCLBUTTONUP = 0XA2;

        /// <summary>
        /// WM_NCLBUTTONDBLCLK win32 message.
        /// </summary>
        public readonly static int WM_NCLBUTTONDBLCLK = 0XA3;

        /// <summary>
        /// WM_NCRBUTTONDOWN win32 message.
        /// </summary>
        public readonly static int WM_NCRBUTTONDOWN = 0XA4;

        /// <summary>
        /// WM_NCRBUTTONUP win32 message.
        /// </summary>
        public readonly static int WM_NCRBUTTONUP = 0XA5;

        /// <summary>
        /// WM_NCRBUTTONDBLCLK win32 message.
        /// </summary>
        public readonly static int WM_NCRBUTTONDBLCLK = 0XA6;

        /// <summary>
        /// WM_NCMBUTTONDOWN win32 message.
        /// </summary>
        public readonly static int WM_NCMBUTTONDOWN = 0XA7;

        /// <summary>
        /// WM_NCMBUTTONUP win32 message.
        /// </summary>
        public readonly static int WM_NCMBUTTONUP = 0XA8;

        /// <summary>
        /// WM_NCMBUTTONDBLCLK win32 message.
        /// </summary>
        public readonly static int WM_NCMBUTTONDBLCLK = 0XA9;


        /// <summary>
        /// WM_KEYFIRST win32 message.
        /// </summary>
        public readonly static int WM_KEYFIRST = 0X100;

        /// <summary>
        /// WM_KEYDOWN win32 message.
        /// </summary>
        public readonly static int WM_KEYDOWN = 0X100;

        /// <summary>
        /// WM_KEYUP win32 message.
        /// </summary>
        public readonly static int WM_KEYUP = 0X101;

        /// <summary>
        /// WM_CHAR win32 message.
        /// </summary>
        public readonly static int WM_CHAR = 0X102;

        /// <summary>
        /// WM_DEADCHAR win32 message.
        /// </summary>
        public readonly static int WM_DEADCHAR = 0X103;

        /// <summary>
        /// WM_SYSKEYDOWN win32 message.
        /// </summary>
        public readonly static int WM_SYSKEYDOWN = 0X104;

        /// <summary>
        /// WM_SYSKEYUP win32 message.
        /// </summary>
        public readonly static int WM_SYSKEYUP = 0X105;

        /// <summary>
        /// WM_SYSCHAR win32 message.
        /// </summary>
        public readonly static int WM_SYSCHAR = 0X106;

        /// <summary>
        /// WM_SYSDEADCHAR win32 message.
        /// </summary>
        public readonly static int WM_SYSDEADCHAR = 0X107;

        /// <summary>
        /// WM_KEYLAST win32 message.
        /// </summary>
        public readonly static int WM_KEYLAST = 0X108;


        /// <summary>
        /// WM_IME_STARTCOMPOSITION win32 message.
        /// </summary>
        public readonly static int WM_IME_STARTCOMPOSITION = 0X10D;

        /// <summary>
        /// WM_IME_ENDCOMPOSITION win32 message.
        /// </summary>
        public readonly static int WM_IME_ENDCOMPOSITION = 0X10E;

        /// <summary>
        /// WM_IME_COMPOSITION win32 message.
        /// </summary>
        public readonly static int WM_IME_COMPOSITION = 0X10F;

        /// <summary>
        /// WM_IME_KEYLAST win32 message.
        /// </summary>
        public readonly static int WM_IME_KEYLAST = 0X10F;


        /// <summary>
        /// WM_INITDIALOG win32 message.
        /// </summary>
        public readonly static int WM_INITDIALOG = 0X110;

        /// <summary>
        /// WM_COMMAND win32 message.
        /// </summary>
        public readonly static int WM_COMMAND = 0X111;

        /// <summary>
        /// WM_SYSCOMMAND win32 message.
        /// </summary>
        public readonly static int WM_SYSCOMMAND = 0X112;

        /// <summary>
        /// WM_TIMER win32 message.
        /// </summary>
        public readonly static int WM_TIMER = 0X113;

        /// <summary>
        /// WM_HSCROLL win32 message.
        /// </summary>
        public readonly static int WM_HSCROLL = 0X114;

        /// <summary>
        /// WM_VSCROLL win32 message.
        /// </summary>
        public readonly static int WM_VSCROLL = 0X115;

        /// <summary>
        /// WM_INITMENU win32 message.
        /// </summary>
        public readonly static int WM_INITMENU = 0X116;

        /// <summary>
        /// WM_INITMENUPOPUP win32 message.
        /// </summary>
        public readonly static int WM_INITMENUPOPUP = 0X117;

        /// <summary>
        /// WM_MENUSELECT win32 message.
        /// </summary>
        public readonly static int WM_MENUSELECT = 0X11F;

        /// <summary>
        /// WM_MENUCHAR win32 message.
        /// </summary>
        public readonly static int WM_MENUCHAR = 0X120;

        /// <summary>
        /// WM_ENTERIDLE win32 message.
        /// </summary>
        public readonly static int WM_ENTERIDLE = 0X121;


        /// <summary>
        /// WM_CTLCOLORMSGBOX win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLORMSGBOX = 0X132;

        /// <summary>
        /// WM_CTLCOLOREDIT win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLOREDIT = 0X133;

        /// <summary>
        /// WM_CTLCOLORLISTBOX win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLORLISTBOX = 0X134;

        /// <summary>
        /// WM_CTLCOLORBTN win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLORBTN = 0X135;

        /// <summary>
        /// WM_CTLCOLORDLG win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLORDLG = 0X136;

        /// <summary>
        /// WM_CTLCOLORSCROLLBAR win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLORSCROLLBAR = 0X137;

        /// <summary>
        /// WM_CTLCOLORSTATIC win32 message.
        /// </summary>
        public readonly static int WM_CTLCOLORSTATIC = 0X138;


        /// <summary>
        /// WM_MOUSEFIRST win32 message.
        /// </summary>
        public readonly static int WM_MOUSEFIRST = 0X200;

        /// <summary>
        /// WM_MOUSEMOVE win32 message.
        /// </summary>
        public readonly static int WM_MOUSEMOVE = 0X200;

        /// <summary>
        /// WM_LBUTTONDOWN win32 message.
        /// </summary>
        public readonly static int WM_LBUTTONDOWN = 0X201;

        /// <summary>
        /// WM_LBUTTONUP win32 message.
        /// </summary>
        public readonly static int WM_LBUTTONUP = 0X202;

        /// <summary>
        /// WM_LBUTTONDBLCLK win32 message.
        /// </summary>
        public readonly static int WM_LBUTTONDBLCLK = 0X203;

        /// <summary>
        /// WM_RBUTTONDOWN win32 message.
        /// </summary>
        public readonly static int WM_RBUTTONDOWN = 0X204;

        /// <summary>
        /// WM_RBUTTONUP win32 message.
        /// </summary>
        public readonly static int WM_RBUTTONUP = 0X205;

        /// <summary>
        /// WM_RBUTTONDBLCLK win32 message.
        /// </summary>
        public readonly static int WM_RBUTTONDBLCLK = 0X206;

        /// <summary>
        /// WM_MBUTTONDOWN win32 message.
        /// </summary>
        public readonly static int WM_MBUTTONDOWN = 0X207;

        /// <summary>
        /// WM_MBUTTONUP win32 message.
        /// </summary>
        public readonly static int WM_MBUTTONUP = 0X208;

        /// <summary>
        /// WM_MBUTTONDBLCLK win32 message.
        /// </summary>
        public readonly static int WM_MBUTTONDBLCLK = 0X209;

        /// <summary>
        /// WM_MOUSELAST win32 message.
        /// </summary>
        public readonly static int WM_MOUSELAST = 0X20A;

        /// <summary>
        /// WM_MOUSEWHEEL win32 message.
        /// </summary>
        public readonly static int WM_MOUSEWHEEL = 0X20A;


        /// <summary>
        /// WM_PARENTNOTIFY win32 message.
        /// </summary>
        public readonly static int WM_PARENTNOTIFY = 0X210;

        /// <summary>
        /// WM_ENTERMENULOOP win32 message.
        /// </summary>
        public readonly static int WM_ENTERMENULOOP = 0X211;

        /// <summary>
        /// WM_EXITMENULOOP win32 message.
        /// </summary>
        public readonly static int WM_EXITMENULOOP = 0X212;

        /// <summary>
        /// WM_NEXTMENU win32 message.
        /// </summary>
        public readonly static int WM_NEXTMENU = 0X213;

        /// <summary>
        /// WM_SIZING win32 message.
        /// </summary>
        public readonly static int WM_SIZING = 0X214;

        /// <summary>
        /// WM_CAPTURECHANGED win32 message.
        /// </summary>
        public readonly static int WM_CAPTURECHANGED = 0X215;

        /// <summary>
        /// WM_MOVING win32 message.
        /// </summary>
        public readonly static int WM_MOVING = 0X216;

        /// <summary>
        /// WM_POWERBROADCAST win32 message.
        /// </summary>
        public readonly static int WM_POWERBROADCAST = 0X218;

        /// <summary>
        /// WM_DEVICECHANGE win32 message.
        /// </summary>
        public readonly static int WM_DEVICECHANGE = 0X219;


        /// <summary>
        /// WM_MDICREATE win32 message.
        /// </summary>
        public readonly static int WM_MDICREATE = 0X220;

        /// <summary>
        /// WM_MDIDESTROY win32 message.
        /// </summary>
        public readonly static int WM_MDIDESTROY = 0X221;

        /// <summary>
        /// WM_MDIACTIVATE win32 message.
        /// </summary>
        public readonly static int WM_MDIACTIVATE = 0X222;

        /// <summary>
        /// WM_MDIRESTORE win32 message.
        /// </summary>
        public readonly static int WM_MDIRESTORE = 0X223;

        /// <summary>
        /// WM_MDINEXT win32 message.
        /// </summary>
        public readonly static int WM_MDINEXT = 0X224;

        /// <summary>
        /// WM_MDIMAXIMIZE win32 message.
        /// </summary>
        public readonly static int WM_MDIMAXIMIZE = 0X225;

        /// <summary>
        /// WM_MDITILE win32 message.
        /// </summary>
        public readonly static int WM_MDITILE = 0X226;

        /// <summary>
        /// WM_MDICASCADE win32 message.
        /// </summary>
        public readonly static int WM_MDICASCADE = 0X227;

        /// <summary>
        /// WM_MDIICONARRANGE win32 message.
        /// </summary>
        public readonly static int WM_MDIICONARRANGE = 0X228;

        /// <summary>
        /// WM_MDIGETACTIVE win32 message.
        /// </summary>
        public readonly static int WM_MDIGETACTIVE = 0X229;

        /// <summary>
        /// WM_MDISETMENU win32 message.
        /// </summary>
        public readonly static int WM_MDISETMENU = 0X230;

        /// <summary>
        /// WM_ENTERSIZEMOVE win32 message.
        /// </summary>
        public readonly static int WM_ENTERSIZEMOVE = 0X231;

        /// <summary>
        /// WM_EXITSIZEMOVE win32 message.
        /// </summary>
        public readonly static int WM_EXITSIZEMOVE = 0X232;

        /// <summary>
        /// WM_DROPFILES win32 message.
        /// </summary>
        public readonly static int WM_DROPFILES = 0X233;

        /// <summary>
        /// WM_MDIREFRESHMENU win32 message.
        /// </summary>
        public readonly static int WM_MDIREFRESHMENU = 0X234;


        /// <summary>
        /// WM_IME_SETCONTEXT win32 message.
        /// </summary>
        public readonly static int WM_IME_SETCONTEXT = 0X281;

        /// <summary>
        /// WM_IME_NOTIFY win32 message.
        /// </summary>
        public readonly static int WM_IME_NOTIFY = 0X282;

        /// <summary>
        /// WM_IME_CONTROL win32 message.
        /// </summary>
        public readonly static int WM_IME_CONTROL = 0X283;

        /// <summary>
        /// WM_IME_COMPOSITIONFULL win32 message.
        /// </summary>
        public readonly static int WM_IME_COMPOSITIONFULL = 0X284;

        /// <summary>
        /// WM_IME_SELECT win32 message.
        /// </summary>
        public readonly static int WM_IME_SELECT = 0X285;

        /// <summary>
        /// WM_IME_CHAR win32 message.
        /// </summary>
        public readonly static int WM_IME_CHAR = 0X286;

        /// <summary>
        /// WM_IME_KEYDOWN win32 message.
        /// </summary>
        public readonly static int WM_IME_KEYDOWN = 0X290;

        /// <summary>
        /// WM_IME_KEYUP win32 message.
        /// </summary>
        public readonly static int WM_IME_KEYUP = 0X291;


        /// <summary>
        /// WM_MOUSEHOVER win32 message.
        /// </summary>
        public readonly static int WM_MOUSEHOVER = 0X2A1;

        /// <summary>
        /// WM_NCMOUSELEAVE win32 message.
        /// </summary>
        public readonly static int WM_NCMOUSELEAVE = 0X2A2;

        /// <summary>
        /// WM_MOUSELEAVE win32 message.
        /// </summary>
        public readonly static int WM_MOUSELEAVE = 0X2A3;


        /// <summary>
        /// WM_CUT win32 message.
        /// </summary>
        public readonly static int WM_CUT = 0X300;

        /// <summary>
        /// WM_COPY win32 message.
        /// </summary>
        public readonly static int WM_COPY = 0X301;

        /// <summary>
        /// WM_PASTE win32 message.
        /// </summary>
        public readonly static int WM_PASTE = 0X302;

        /// <summary>
        /// WM_CLEAR win32 message.
        /// </summary>
        public readonly static int WM_CLEAR = 0X303;

        /// <summary>
        /// WM_UNDO win32 message.
        /// </summary>
        public readonly static int WM_UNDO = 0X304;


        /// <summary>
        /// WM_RENDERFORMAT win32 message.
        /// </summary>
        public readonly static int WM_RENDERFORMAT = 0X305;

        /// <summary>
        /// WM_RENDERALLFORMATS win32 message.
        /// </summary>
        public readonly static int WM_RENDERALLFORMATS = 0X306;

        /// <summary>
        /// WM_DESTROYCLIPBOARD win32 message.
        /// </summary>
        public readonly static int WM_DESTROYCLIPBOARD = 0X307;

        /// <summary>
        /// WM_DRAWCLIPBOARD win32 message.
        /// </summary>
        public readonly static int WM_DRAWCLIPBOARD = 0X308;

        /// <summary>
        /// WM_PAINTCLIPBOARD win32 message.
        /// </summary>
        public readonly static int WM_PAINTCLIPBOARD = 0X309;


        /// <summary>
        /// WM_VSCROLLCLIPBOARD win32 message.
        /// </summary>
        public readonly static int WM_VSCROLLCLIPBOARD = 0X30A;

        /// <summary>
        /// WM_SIZECLIPBOARD win32 message.
        /// </summary>
        public readonly static int WM_SIZECLIPBOARD = 0X30B;

        /// <summary>
        /// WM_ASKCBFORMATNAME win32 message.
        /// </summary>
        public readonly static int WM_ASKCBFORMATNAME = 0X30C;

        /// <summary>
        /// WM_CHANGECBCHAIN win32 message.
        /// </summary>
        public readonly static int WM_CHANGECBCHAIN = 0X30D;

        /// <summary>
        /// WM_HSCROLLCLIPBOARD win32 message.
        /// </summary>
        public readonly static int WM_HSCROLLCLIPBOARD = 0X30E;

        /// <summary>
        /// WM_QUERYNEWPALETTE win32 message.
        /// </summary>
        public readonly static int WM_QUERYNEWPALETTE = 0X30F;
        #endregion
    }
}
