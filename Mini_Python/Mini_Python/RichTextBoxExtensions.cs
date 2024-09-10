public static class RichTextBoxExtensions
{
    private const int EM_SETCHARFORMAT = 0x0444;
    private const int SCF_SELECTION = 0x0001;
    private const int CFM_LINK = 0x00000020;
    private const int CFE_LINK = 0x00000020;

    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    public static void SetSelectionLink(this RichTextBox richTextBox, bool link)
    {
        CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
        cf.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(cf);
        cf.dwMask = CFM_LINK;
        cf.dwEffects = link ? (uint)CFE_LINK : 0;

        IntPtr wpar = new IntPtr(SCF_SELECTION);
        IntPtr lpar = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(cf));
        System.Runtime.InteropServices.Marshal.StructureToPtr(cf, lpar, false);

        SendMessage(richTextBox.Handle, EM_SETCHARFORMAT, wpar.ToInt32(), lpar);

        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(lpar);
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private struct CHARFORMAT2_STRUCT
    {
        public int cbSize;
        public uint dwMask;
        public uint dwEffects;
        public int yHeight;
        public int yOffset;
        public int crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szFaceName;
        public short wWeight;
        public short sSpacing;
        public int crBackColor;
        public int lcid;
        public int dwReserved;
        public short sStyle;
        public short wKerning;
        public byte bUnderlineType;
        public byte bAnimation;
        public byte bRevAuthor;
        public byte bReserved1;
    }
}
