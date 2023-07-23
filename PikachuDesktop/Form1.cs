using System.Runtime.InteropServices;
using System.Timers;

namespace PikachuDesktop
{
    public partial class Form1 : Form
    {
        private bool isDragging;
        private Point lastCursorPosition;
        private PictureBox pikachuBox;
        private System.Timers.Timer timer;
        public Form1()
        {
            InitializeComponent();

            this.Text = "Pikachu";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(120, 100);
            this.TransparencyKey = this.BackColor;

            this.TopMost = true;

            pikachuBox = new PictureBox();
            pikachuBox.Image = Properties.Resources.pikachu;
            pikachuBox.Size = new Size(100, 100);
            pikachuBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pikachuBox.Location = new Point(this.ClientSize.Width / 2 - pikachuBox.Width / 2, this.ClientSize.Height / 2 - pikachuBox.Height / 2);

            pikachuBox.MouseDown += PictureBox_MouseDown;
            pikachuBox.MouseMove += PictureBox_MouseMove;
            pikachuBox.MouseUp += PictureBox_MouseUp;

   

            this.Controls.Add(pikachuBox);
        }

   

        private void GlobalMouseHook_DoubleClick(object sender, Point e)
        {
            // Center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalMouseHook.Start();
            GlobalMouseHook.DoubleClick += GlobalMouseHook_DoubleClick;
        }

        public static class GlobalMouseHook
        {
            private static IntPtr hookId = IntPtr.Zero;
            private static LowLevelMouseProc mouseProc;

            private const int WH_MOUSE_LL = 14;
            private const int WM_LBUTTONDBLCLK = 0x0203;

            public static event EventHandler<Point> DoubleClick;

            public static void Start()
            {
                mouseProc = HookCallback;
                hookId = SetHook(mouseProc);
            }

            public static void Stop()
            {
                UnhookWindowsHookEx(hookId);
            }

            private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

            private static IntPtr SetHook(LowLevelMouseProc proc)
            {
                using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
                using (var curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDBLCLK)
                {
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    Point clickPoint = new Point(hookStruct.pt.x, hookStruct.pt.y);
                    DoubleClick?.Invoke(null, clickPoint);
                }
                return CallNextHookEx(hookId, nCode, wParam, lParam);
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct POINT
            {
                public int x;
                public int y;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct MSLLHOOKSTRUCT
            {
                public POINT pt;
                public int mouseData;
                public int flags;
                public int time;
                public IntPtr dwExtraInfo;
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalMouseHook.Stop();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true; 
            lastCursorPosition= e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDragging)
            {
                int dx = e.X - lastCursorPosition.X;
                int dy = e.Y - lastCursorPosition.Y;
                this.Location = new Point(this.Left + dx, this.Top + dy);
                
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
           
            isDragging = true;
            lastCursorPosition = e.Location;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDragging)
            {
                int dx = e.X - lastCursorPosition.X;
                int dy = e.Y - lastCursorPosition.Y;
                this.Location = new Point(this.Left + dx, this.Top + dy);
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            
            isDragging = false;
        }
    }

}