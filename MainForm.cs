using GameOfLife.assets.classes;
using GameOfLife.assets.classes.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class MainForm : Form
    {
        private static Dictionary<string, Color> colors = new Dictionary<string, Color>() {
            { "gray 24", Color.FromArgb(24, 24, 24) },
            { "gray 32", Color.FromArgb(32, 32, 32) },
            { "gray 48", Color.FromArgb(48, 48, 48) },
            { "gray 64", Color.FromArgb(64, 64, 64) },
            { "white",   Color.White },
        };

        private static Thread thread_loop;
        private static Thread thread_mouse;

        private static Canvas canvas;
        private static Game game;

        private const int S = 6;    // Cell size (pixels)
        private const int W = 128;  // Grid width (cells)
        private const int H = 128;  // Grid height (cells)
        private const int D = 5;    // Grid live cell density when randomized (%)

        private const int targetUPS = 100;
        private const int targetFPS = 75;

        private static bool closing = false;

        public MainForm()
        {
            InitializeComponent();
            HandleCreated += Initialize;  // Wait for the Window Handle to be created before accessing components
            FormClosing += Close;
        }

        private void Initialize(object Sender, EventArgs e)
        {
            ClientSize = new Size((W + 2) * S + 8, (H + 2) * S + 8);
            this.CenterToScreen();

            game = new Game(W, H);
            canvas = new Canvas(W + 2, H + 2);
            canvas.DrawBorders(colors["gray 64"]);

            thread_loop = new Thread(new ThreadStart(RunLoop));
            thread_loop.Priority = ThreadPriority.Highest;
            thread_loop.Start();

            thread_mouse = new Thread(new ThreadStart(UpdateMouse));
            thread_mouse.Priority = ThreadPriority.Highest;
            thread_mouse.Start();
        }

        private void RunLoop()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            var elapsedUpdateTime = 0d;
            var elapsedRenderTime = 0d;
            var elapsedFormatTime = 0d;
            var updateCalls = 0;
            var renderCalls = 0;

            while (!closing)
            {
                var dt = (sw.Elapsed.TotalMilliseconds) / 1000d;
                sw.Restart();

                elapsedUpdateTime += dt;
                elapsedRenderTime += dt;
                elapsedFormatTime += dt;

                while (elapsedUpdateTime >= (1d / targetUPS))
                {
                    UpdateLoop();
                    updateCalls++;
                    elapsedUpdateTime -= (1d / targetUPS);
                }

                if (elapsedRenderTime >= (1d / targetFPS))
                {
                    //renderCalls += (int)(elapsedFormatTime / (1d / targetFPS));
                    renderCalls++;
                    RenderLoop();
                    elapsedRenderTime = elapsedRenderTime % (1d / targetFPS);
                }

                if (elapsedFormatTime >= 1d)
                {
                    var ups = (updateCalls / elapsedFormatTime).ToString("F2");
                    var fps = (renderCalls / elapsedFormatTime).ToString("F2");

                    Invoke((MethodInvoker)(() => { Text = ($"Game of Life - UPS: {ups} - FPS: {fps}"); }));

                    updateCalls = 0;
                    renderCalls = 0;
                    elapsedFormatTime = 0;
                }
            }

            Application.Exit();
        }

        private void UpdateLoop()
        {
            if(!game.paused)
            {
                game.Iterate();
            }

            var focused = false;
            Invoke((MethodInvoker)(() => { focused = Focused; }));
            if (focused)
            {
                if (Keyboard.IsKeyDown(Keyboard.Key.D))
                {
                    game.SetBorderType(Grid.BorderType.DeadBorders);
                    canvas.DrawBorders(colors["gray 24"]);
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.L))
                {
                    game.SetBorderType(Grid.BorderType.LiveBorders);
                    canvas.DrawBorders(colors["white"]);
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.W))
                {
                    game.SetBorderType(Grid.BorderType.WrapBorders);
                    canvas.DrawBorders(colors["gray 64"]);
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.R))
                {
                    game.Randomize(D);
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.C))
                {
                    game.Clear();
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.RIGHT))
                {
                    if (game.paused)
                        game.Iterate();
                    else
                        game.paused = true;
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.LEFT))
                {
                    if (game.paused)
                        game.Undo();
                    else
                        game.paused = true;
                }

                if (Keyboard.IsKeyDown(Keyboard.Key.SPACE))
                {
                    game.paused = !game.paused;
                }
            }
        }

        private void RenderLoop()
        {
            lock (canvas)
            {
                for (int i = 0; i < game.height; i++)
                {
                    for (int j = 0; j < game.width; j++)
                    {
                        if (game.GetCellValue(j, i) == 1)
                        {
                            canvas.SetPixel(1 + j, 1 + i, colors["white"]);
                        }
                        else
                        {
                            if (game.GetLastCellValue(j, i) == 1)
                            {
                                canvas.SetPixel(1 + j, 1 + i, colors["gray 48"]);
                            }
                            else
                            {
                                if ((i + j) % 2 == 0)
                                {
                                    canvas.SetPixel(1 + j, 1 + i, colors["gray 32"]);
                                }
                                else
                                {
                                    canvas.SetPixel(1 + j, 1 + i, colors["gray 24"]);
                                }
                            }
                        }
                    }
                }
            }

            Invoke((MethodInvoker)(() => { Refresh(); }));
        }

        private void UpdateMouse()
        {
            while (!(closing))
            {
                var focused = false;
                Invoke((MethodInvoker)(() => { focused = Focused; }));
                if (focused)
                {
                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        var position = MousePositionOnGrid();
                        if (!(position.X < 0 || position.Y < 0 || position.X >= W || position.Y >= H))
                        {
                            game.SetCellValue(position.X, position.Y, 1);
                        }
                    }

                    if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    {
                        var position = MousePositionOnGrid();
                        if (!(position.X < 0 || position.Y < 0 || position.X >= W || position.Y >= H))
                        {
                            game.SetCellValue(position.X, position.Y, 0);
                        }
                    }
                }
            }
        }

        private void Close(object Sender, FormClosingEventArgs args)
        {
            if (!thread_loop.Join(0) || !thread_mouse.Join(0))
            {
                closing = true;
                args.Cancel = true;
                var timer = new System.Timers.Timer();
                timer.AutoReset = false;
                timer.SynchronizingObject = this;
                timer.Elapsed += delegate {
                    if (!thread_loop.Join(0) || !thread_mouse.Join(0))
                    {
                        timer.Start();
                    }
                    else
                    {
                        Close();
                    }
                };
                timer.Start();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            lock (canvas)
            {
                var w = canvas.width * S;
                var h = canvas.height * S;
                var x = (ClientSize.Width - w) / 2;
                var y = (ClientSize.Height - h) / 2;

                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.DrawImage(canvas.ToBitmap(), x, y, w, h);
            }
        }

        private Point MousePositionOnGrid()
        {
            int mouseX = 0, mouseY = 0, clientW = 0, clientH = 0, canvasW, canvasH, x, y;

            Invoke((MethodInvoker)(() => {
                var pos = this.PointToClient(Mouse.GetPosition());
                mouseX = pos.X;
                mouseY = pos.Y;

                clientW = this.ClientSize.Width;
                clientH = this.ClientSize.Height;
            }));

            canvasW = canvas.width * S;
            canvasH = canvas.height * S;

            x = (mouseX - (clientW - canvasW) / 2) / S - 1;
            y = (mouseY - (clientH - canvasH) / 2) / S - 1;

            return new Point(x, y);
        }
    }
}

