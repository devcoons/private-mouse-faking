using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

namespace Mouse_Faking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas self;
        public static bool isActive = false;
        Thread x = new Thread(new ThreadStart(ThreadMouse));

        public MainWindow()
        {
            InitializeComponent();
            self = paintSurface;
            isActive = true;
            x.Start();
        }
        public static Rect GetAbsolutePlacement(FrameworkElement element, bool relativeToScreen = false)
        {
            var absolutePos = element.PointToScreen(new System.Windows.Point(0, 0));
            if (relativeToScreen)
            {
                return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
            }
            var posMW = Application.Current.MainWindow.PointToScreen(new System.Windows.Point(0, 0));
            absolutePos = new System.Windows.Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }

        public static void ThreadMouse()
        {
            Win32.POINT lpPoint = Win32.GetCursorPosition();
            Win32.POINT lpPointN = lpPoint;

            int x = 0;
            int y = 0;
            bool p = false;
            bool q = false;

            int k = 0;

            while (isActive)
            {

                System.Windows.WindowState currentState = WindowState.Normal;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    currentState = Application.Current.MainWindow.WindowState;
                });

                if (currentState == WindowState.Minimized)
                {
                    Thread.Sleep(1000);
                    if (isActive == false)
                        return;
                    continue;
                }

                lpPointN.x = lpPoint.x + (p == false ? 1 : -1);
                lpPointN.y = lpPoint.y + (q == false ? 1 : -1);
                Rect o = new Rect();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    o = GetAbsolutePlacement(self, true);
                });

                if (lpPointN.x < (o.Left + 5))
                {
                    lpPointN.x = (int)(o.Left + 5);
                    p = false;

                }
                if (lpPointN.y < (o.Top + 5))
                {
                    lpPointN.y = (int)(o.Top + 5);
                    q = false;

                }


                if (lpPointN.x > (o.Right - 5))
                {
                    lpPointN.x = (int)(o.Right - 5);
                    p = true;

                }
                if (lpPointN.y > (o.Bottom - 5))
                {
                    lpPointN.y = (int)(o.Bottom - 5);
                    q = true;

                }
                Win32.SetCursorPos(lpPointN.x, lpPointN.y);
                lpPoint = Win32.GetCursorPosition();

                Thread.Sleep(50);
                if (isActive == false)
                    return;
                Win32.POINT lpPointQ = Win32.GetCursorPosition();

                if (Math.Abs(lpPointQ.x - lpPointN.x) > 2 || Math.Abs(lpPointQ.y - lpPointN.y) > 2)
                {
                    do
                    {
                        Win32.POINT lpPointPre = Win32.GetCursorPosition();
                        for (int i = 0; i < 10; i++)
                        {
                            if (isActive == false)
                                return;
                            Thread.Sleep(500);
                            x = 0;
                            y = 0;
                            k = 0;
                            p = false;
                            q = false;
                        }
                        lpPoint = Win32.GetCursorPosition();

                        if (Math.Abs(lpPoint.x - lpPointPre.x) < 3 && Math.Abs(lpPoint.y - lpPointPre.y) < 3)
                            break;

                    } while (true);
                }

                byte[] qp = new byte[1];
                rngCsp.GetBytes(qp);

                bool mHor = lpPointN.x < (int)(o.Left + o.Right) / 2 ? false : true;
                bool mVer = lpPointN.y < (int)(o.Top + o.Bottom) / 2 ? false : true;


                if (qp[0] < 10)
                {
                    Thread.Sleep(10);
                    rngCsp.GetBytes(qp);
                    p = qp[0] > 200 ? !mHor : mHor;
                }
                rngCsp.GetBytes(qp);

                if (qp[0] < 10)
                {
                    Thread.Sleep(10);
                    rngCsp.GetBytes(qp);
                    q = qp[0] > 200 ? !mVer : mVer;
                }


                rngCsp.GetBytes(qp);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    currentState = Application.Current.MainWindow.WindowState;
                });

                if (currentState == WindowState.Minimized)
                {
                    Thread.Sleep(1000);
                    if (isActive == false)
                        return;
                    continue;
                }
                k = k < 100 ? k + 1 : 100;
                if (k > 10)
                {
                    if (qp[0] < 50)
                    {
                        Win32.sendMouseLeftclick(lpPointN);
                    }
                    if (qp[0] > 200)
                    {
                        Win32.sendMouseRightclick(lpPointN);
                    }
                }
                if (isActive == false)
                    return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isActive = false;
            x.Join();
        }
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private void PaintSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                var currentPoint = e.GetPosition(this);

                Line line = new Line();

                byte[] a = new byte[1];
                byte[] r = new byte[1];
                byte[] g = new byte[1];
                byte[] b = new byte[1];

                rngCsp.GetBytes(a);
                rngCsp.GetBytes(r);
                rngCsp.GetBytes(g);
                rngCsp.GetBytes(b);


                line.Stroke = new SolidColorBrush(Color.FromArgb(a[0], r[0], g[0], b[0]));
                line.X1 = currentPoint.X - 1;
                line.Y1 = currentPoint.Y - 1;
                line.X2 = e.GetPosition(this).X + 1;
                line.Y2 = e.GetPosition(this).Y + 1;

                currentPoint = e.GetPosition(this);

                paintSurface.Children.Add(line);
                Line line1 = new Line();
                line1.Stroke = new SolidColorBrush(Color.FromArgb(a[0], r[0], g[0], b[0]));
                line1.X1 = currentPoint.X - 1;
                line1.Y1 = currentPoint.Y + 1;
                line1.X2 = e.GetPosition(this).X + 1;
                line1.Y2 = e.GetPosition(this).Y - 1;
                paintSurface.Children.Add(line1);
            }
        }

    }
}
