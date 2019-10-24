using System;
using System.Text;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using System.Threading;

namespace WPFHangman
{
    public partial class MainWindow : Window
    {
        private const int BTN_WIDTH = 60;
        private const int BTN_HEIGHT = 40;
        private const int BTN_FSIZE = 20;
        private const int ROW_SIZE = 9;
        private static string ANSWER = "RACHMANINOFF";
        private static string GUESS = "";
        private int count = 0;
        private List<Button> btnList = new List<Button>();

        public void addStartBtn()
        {
            int btnWidth = 200;
            Button btnStart = new Button();
            btnStart.Content = "START GAME";
            btnStart.FontSize = 20;
            btnStart.FontFamily = new FontFamily("Consolas");
            btnStart.Background = Brushes.White;
            btnStart.Foreground = Brushes.Black;
            btnStart.Width = btnWidth;
            btnStart.Click += StartGame;

            Canvas.SetLeft(btnStart, (canvasStart.Width - btnWidth) / 2);
            Canvas.SetTop(btnStart, 10);
            canvasStart.Children.Add(btnStart);
        }

        private void addLine(int X1, int Y1, int X2, int Y2)
        {
            Line line = new Line();
            line.StrokeThickness = 4;
            line.Stroke = Brushes.White;
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;

            canvasMain.Children.Add(line);
        }

        private void drawHangMan()
        {
            switch (count)
            {
               case 1:
                    Ellipse e = new Ellipse();
                    e.Stroke = Brushes.White;
                    e.StrokeThickness = 4;
                    e.Height = 50;
                    e.Width = 50;
                    Canvas.SetLeft(e, 500 - 25);
                    Canvas.SetTop(e, 100);
                    canvasMain.Children.Add(e);
                    break;
                case 2:
                    addLine(500, 150, 500, 200);break;
                case 3:
                    addLine(500, 160, 480, 180);break;
                case 4:
                    addLine(500, 160, 520, 180); break;
                case 5:
                    addLine(500, 200, 480, 220); break;
                case 6:
                    addLine(500, 200, 520, 220);
                    break;
            }
        }

        public void addChallenge()
        {
            foreach (char c in ANSWER)
            {
                if (c == ' ')
                    GUESS += " ";
                else
                    GUESS += "*";
            }

            addLine(200, 280, 800, 280);
            addLine(350, 280, 350, 50);
            addLine(350, 50, 600, 50); 
            addLine(500, 50, 500, 100);
        }

        private void solveChallenge(object sender, RoutedEventArgs e)
        {
            char attempt = (char)((Button)sender).Content;
            StringBuilder sb = new StringBuilder(GUESS);

            for (int i = 0; i < ANSWER.Length; i++)
                if (ANSWER[i] == attempt)
                    sb[i] = attempt;

            if (GUESS == sb.ToString())
            {
                count++;
                Console.WriteLine(count);
                ((Button)sender).Visibility = Visibility.Hidden;
                drawHangMan();
            }

            GUESS = sb.ToString();
            txtTitle.Text = GUESS;

            if (GUESS == ANSWER)
            {
                Console.WriteLine("WIN!");
            }
            else if(count == 6)
            {
                canvasMain.Children.Clear();
                TextBlock tb = new TextBlock();
                tb.Foreground = Brushes.Red;
                tb.Background = Brushes.Black;
                tb.Text = "Nice try...\n\nAnswer:\n\"" + ANSWER + "\"";
                tb.FontFamily = new FontFamily("Consolas");
                tb.FontSize = 30;
                tb.Height = 200;
                tb.Width = 800;
                tb.TextAlignment = TextAlignment.Center;
                Canvas.SetTop(tb, 50);
                Canvas.SetLeft(tb, 100);


                Thread.Sleep(1000);
                canvasMain.Children.Add(tb);
            }

            ((Button)sender).IsEnabled = false;
        }

        public MainWindow()
        {
            InitializeComponent();
            addStartBtn();
            addChallenge();
        }

        private Button getGameBtn(char txt)
        {
            Button btn = new Button();
            btn.Content = txt;

            // Styling
            btn.Foreground = Brushes.Black;
            btn.Background = Brushes.White;
            btn.FontSize = BTN_FSIZE;
            btn.Width = BTN_WIDTH;
            btn.Height = BTN_HEIGHT;
            btn.FontFamily = new FontFamily("Consolas");
            btn.Click += solveChallenge;

            return btn;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 26; i++)
            {
                Button btn = getGameBtn((char)('A' + i));
                Canvas.SetLeft(btn, i % ROW_SIZE * BTN_WIDTH);

                if (i >= ROW_SIZE)
                {
                    if (i < ROW_SIZE * 2)
                        Canvas.SetTop(btn, BTN_HEIGHT);
                    else
                        Canvas.SetTop(btn, BTN_HEIGHT * 2);
                }
                btnList.Add(btn);
                canvasBtn.Children.Add(btn);
            }

            txtTitle.Text = GUESS;
        }
    }
}
