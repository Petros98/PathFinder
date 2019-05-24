using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PathFinder
{

    public partial class MainWindow : Window
    {
        public struct Vector
        {
            public int I { get; }
            public int J { get; }

            public Vector(int i, int j)
            {
                I = i;
                J = j;
            }
        }

        List<Vector> Walls = new List<Vector>();
        List<Vector> Vectors = new List<Vector>();

        DoubleAnimation buttonAnimation = new DoubleAnimation();
        DoubleAnimation sizeAnimation = new DoubleAnimation();
        DoubleAnimation heightAnimation = new DoubleAnimation();
        DispatcherTimer timer = new DispatcherTimer();


        int[,] Map, cMap;
        int[] path_X, path_Y;
        int[] dx = { 1, 0, -1, 0 }, dy = { 0, 1, 0, -1 };
        int p = -50, wall = -200, empty = -100, path_lengt, x, y, x2, y2, list_items = 0;
        int k = 1;
        int stp1i, stp1j, stp2i, stp2j;
        int a1, a2, b1, b2;
        int X, Y, step = 1;
        public int size;

        bool list_added, add_clicked = false, start_clicked = false;


        bool matrixGenerated = false, ashxati = true, elqChka = false;

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            double cursorX, cursorY;
            if (matrixGenerated)
            {
                cursorX = (int)(e.GetPosition(this).X - 77);
                cursorY = (int)(e.GetPosition(this).Y - grid.Margin.Top);
                Y = (int)(Math.Floor(cursorX / (rect[0, 0].Height)));
                X = (int)(Math.Floor(cursorY / (rect[0, 0].Width)));
                // listBox.Items.Add(X.ToString() + "  " + Y.ToString());
                if (cMap[X, Y] == empty)
                {
                    rect[X, Y].Background = Brushes.Black;
                }
            }

        }

        private void Grid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (matrixGenerated && cMap[X, Y] == empty)
            {
                switch (step)
                {
                    case 1:
                        {
                            a1 = X;
                            a2 = Y;
                            break;
                        }
                    case 2:
                        {
                            b1 = X;
                            b2 = Y;
                            Add_coordinate(a1, a2, b1, b2);
                            step = 0;
                            break;
                        }
                }

                step++;

            }

        }

        private void Grid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            double cursorX, cursorY;
            int X, Y;
            if (matrixGenerated)
            {
                cursorX = (int)(e.GetPosition(this).X - 77);
                cursorY = (int)(e.GetPosition(this).Y - grid.Margin.Top);

                if ((int)(Math.Floor(cursorY / (rect[0, 0].Width))) == size)
                {
                    Y = size - 1;
                }
                else
                    Y = (int)(Math.Floor(cursorY / (rect[0, 0].Width)));

                if ((int)(Math.Floor(cursorX / (rect[0, 0].Width))) == size)
                {
                    X = size - 1;
                }
                else
                    X = (int)(Math.Floor(cursorX / (rect[0, 0].Width)));

                if (cMap[Y, X] == wall)
                {
                    info.Foreground = Brushes.Red;
                    info.Text = "wall";
                }
                else
                {
                    info.Foreground = Brushes.Green;
                    info.Text = "empty";
                }
                textBlock.Text = Y + "  " + X;
            }
        }

        bool waves = false, add = true, finded = true, point_finded = false;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        Random rd1 = new Random();
        SolidColorBrush color1;
        TextBox[,] rect;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            k = 1;
            size = (int)slider1.Value;
            rect = new TextBox[size, size];
            cMap = new int[size, size];
            Map = new int[size, size];
            path_X = new int[size * size];
            path_Y = new int[size * size];
            listBox.Items.Clear();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    rect[i, j] = new TextBox();
                    Map[i, j] = new int();
                    cMap[i, j] = new int();
                    cMap[i, j] = empty;
                    Map[i, j] = empty;
                    rect[i, j].VerticalAlignment = VerticalAlignment.Top;
                    rect[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    rect[i, j].Height = (grid.Height / size);
                    rect[i, j].Width = (grid.Width / size);
                    rect[i, j].Focusable = false;
                    rect[i, j].FontSize = 250 / size;
                    rect[i, j].Margin = new Thickness(j * rect[i, j].Width, (rect[i, j].Height * i), 0, 0);
                    rect[i, j].Background = Brushes.White;
                    rect[i, j].BorderBrush = Brushes.Gray;
                    //rect[i, j].IsEnabled = false;
                    heightAnimation.From = rect[i, j].ActualWidth;
                    heightAnimation.To = rect[i, j].Width;
                    heightAnimation.Duration = TimeSpan.FromSeconds(1);
                    rect[i, j].BeginAnimation(Button.WidthProperty, heightAnimation);
                    grid.Children.Add(rect[i, j]);
                    matrixGenerated = true;
                    hraman.Text = "";
                }
            }


        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!add_clicked)
                {
                    if (start_clicked)
                    {
                        elqChka = false;
                    }
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                        {
                            if (cMap[i, j] != wall)
                            {
                                cMap[i, j] = empty;
                            }
                        }
                    if (elqChka)
                    {
                        cMap[stp1i, stp1j] = empty;
                        cMap[stp2i, stp2j] = empty;
                        rect[stp1i, stp1j].Background = Brushes.White;
                        rect[stp2i, stp2j].Background = Brushes.White;
                    }
                    GetPoints();
                    while (shmays(stp1i, stp1j, stp2i, stp2j))
                    {
                        GetPoints();
                    }
                    finded = true;


                    rect[stp1i, stp1j].Background = Brushes.Blue;
                    rect[stp2i, stp2j].Background = Brushes.Red;
                    cMap[stp1i, stp1j] = wall;
                    cMap[stp2i, stp2j] = wall;
                    Clear_count();
                    hraman.Text = "Click Find";
                }
                else
                    hraman.Text = "click Start";
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message);
            }
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            Find_Wave(stp1i, stp1j, stp2i, stp2j);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            Show_Wave(stp1i, stp1j, stp2i, stp2j);
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (list_added && Vectors.Count > 1)
            {
                for (int i = 0; i < Vectors.Count; i += 2)
                {
                    Find_Wave(Vectors[i].I, Vectors[i].J, Vectors[i + 1].I, Vectors[i + 1].J);

                    if (point_finded)
                    {
                        Show_Wave(Vectors[i].I, Vectors[i].J, Vectors[i + 1].I, Vectors[i + 1].J);
                    }
                    else
                    {
                        cMap[Vectors[i].I, Vectors[i].J] = empty;
                        cMap[Vectors[i + 1].I, Vectors[i + 1].J] = empty;
                        Walls.Remove(new Vector(Vectors[i + 1].I, Vectors[i + 1].J));
                        Walls.Remove(new Vector(Vectors[i].I, Vectors[i].J));
                        rect[Vectors[i].I, Vectors[i].J].Background = Brushes.White;
                        rect[Vectors[i + 1].I, Vectors[i + 1].J].Background = Brushes.White;
                        //listBox.Items[i] += "kkk";
                        Clear_count();
                    }
                    //listBox.Items[i] += "kkk";
                    point_finded = false;
                    add = true;
                    finded = true;
                    for (int k = 0; k < size; k++)
                        for (int j = 0; j < size; j++)
                        {
                            if (cMap[k, j] != wall)
                            {
                                cMap[k, j] = empty;
                            }
                        }
                }
                add_clicked = false;
                list_added = false;
                if (Vectors.Count > 0)
                {
                    Vectors.Clear();
                }
                start_clicked = true;
                hraman.Text = "Choose coordinates";
                step = 1;
            }
            else
                hraman.Text = "Choose coordinates";
        }


        private void Delete()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)

                    grid.Children.Remove(rect[i, j]);
            }

            Walls.Clear();
            Vectors.Clear();
            listBox.Items.Clear();


        }


        public bool shmays(int stp1i, int stp1j, int stp2i, int stp2j)
        {
            if ((stp1i == stp2i && Math.Abs(stp1j - stp2j) <= 1) || (stp1j == stp2j && Math.Abs(stp1i - stp2i) <= 1))
            {
                return true;
            }
            else
                return false;
        }

        private void GetPoints()
        {

            do
            {
                stp1i = rd1.Next(0, size);
                stp1j = rd1.Next(0, size);
                stp2i = rd1.Next(0, size);
                stp2j = rd1.Next(0, size);

            }
            while (stp1i == stp2i && stp1j == stp2j || cMap[stp1i, stp1j] == wall || cMap[stp2i, stp2j] == wall);

        }

        public void Clear_count()
        {
            for (int i = 0; i < size; i++)

                for (int j = 0; j < size; j++)

                    rect[i, j].Text = null;

        }

        public void LastPart(List<Vector> list1, List<Vector> list2)
        {
            if (ashxati)
            {
                waves = true;

                path_lengt = k;

                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        Map[i, j] = empty;

                list1.RemoveRange(0, list1.Count);
                list2.RemoveRange(0, list2.Count);

                if (add)
                {
                    hraman.Text = "Click Show";
                }

                finded = false;
                add = false;
            }
            ashxati = false;
        }

        public void Add_coordinate(int a1, int a2, int b1, int b2)
        {
            if (!shmays(a1, a2, b1, b2))
            {
                Vector a = new Vector(a1, a2);
                Vector b = new Vector(b1, b2);


                if (Walls.Contains(a) || Walls.Contains(b))
                {
                    hraman.Text = "Not valid Coordinate";
                    list_added = false;
                }
                else
                {
                    Vectors.Add(a);
                    Vectors.Add(b);
                    cMap[a1, a2] = wall;
                    cMap[b1, b2] = wall;
                    Walls.Add(a);
                    Walls.Add(b);

                    color1 = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rd1.Next(0, 255)),
                                                        Convert.ToByte(rd1.Next(0, 255)),
                                                        Convert.ToByte(rd1.Next(0, 255))));
                    rect[a.I, a.J].Background = color1;
                    rect[b.I, b.J].Background = color1;

                    list_added = true;
                    hraman.Text = "Click Start";
                    list_items++;
                }

                if (list_added)
                {
                    listBox.Items.Add("X1 = " + a.I.ToString() + "   Y1 = " +
                                        a.J.ToString() + "   X2 = " +
                                        b.I.ToString() + "  Y2 =   " +
                                        b.J.ToString());
                }

                add_clicked = true;
                start_clicked = false;
            }
            else
            {
                rect[a1, a2].Background = Brushes.White;
                rect[b1, b2].Background = Brushes.White;
                hraman.Text = "Not valid coordinates";
            }


        }
        public void Find_Wave(int stp1i, int stp1j, int stp2i, int stp2j)
        {
            k = 1;
            if (finded)
            {
                add = true;

                cMap[stp1i, stp1j] = 0;
                cMap[stp2i, stp2j] = 0;

                List<Vector> list1 = new List<Vector>();
                List<Vector> list2 = new List<Vector>();

                list1.Add(new Vector(stp1i, stp1j));
                list2.Add(new Vector(stp2i, stp2j));

                int oldcount1 = 1;
                int oldcount2 = 1;

                while (add)
                {
                    int newcount1 = list1.Count;
                    int newcount2 = list2.Count;

                    for (int i = 0; i < newcount1; i++)
                    {
                        if (list1[i].I - 1 >= 0 && list1[i].J - 1 >= 0)
                        {
                            if (cMap[list1[i].I - 1, list1[i].J] == empty)
                            {
                                cMap[list1[i].I - 1, list1[i].J] = -k;
                                Map[list1[i].I - 1, list1[i].J] = p;
                                list1.Add(new Vector(list1[i].I - 1, list1[i].J));
                                rect[list1[i].I - 1, list1[i].J].Text = k.ToString();
                            }
                        }

                        if (list1[i].I + 1 < size)
                        {
                            if (cMap[list1[i].I + 1, list1[i].J] == empty)
                            {
                                cMap[list1[i].I + 1, list1[i].J] = -k;
                                Map[list1[i].I + 1, list1[i].J] = p;
                                list1.Add(new Vector(list1[i].I + 1, list1[i].J));
                                rect[list1[i].I + 1, list1[i].J].Text = k.ToString();
                            }

                        }

                        if (list1[i].J - 1 >= 0)
                        {
                            if (cMap[list1[i].I, list1[i].J - 1] == empty)
                            {
                                cMap[list1[i].I, list1[i].J - 1] = -k;
                                Map[list1[i].I, list1[i].J - 1] = p;
                                list1.Add(new Vector(list1[i].I, list1[i].J - 1));
                                rect[list1[i].I, list1[i].J - 1].Text = k.ToString();
                            }
                        }

                        if (list1[i].J + 1 < size)
                        {
                            if (cMap[list1[i].I, list1[i].J + 1] == empty)
                            {
                                cMap[list1[i].I, list1[i].J + 1] = -k;
                                Map[list1[i].I, list1[i].J + 1] = p;
                                list1.Add(new Vector(list1[i].I, list1[i].J + 1));
                                rect[list1[i].I, list1[i].J + 1].Text = k.ToString();
                            }
                        }
                    }

                    for (int i = 0; i < newcount2; i++)
                    {
                        if (list2[i].I - 1 >= 0 && !(k > size * size))
                        {
                            if (Map[list2[i].I - 1, list2[i].J] == p)
                            {
                                rect[list2[i].I - 1, list2[i].J].Background = Brushes.Yellow;
                                x = list2[i].I - 1;
                                y = list2[i].J;
                                point_finded = true;
                                LastPart(list1, list2);
                                break;

                            }
                            else if (cMap[list2[i].I - 1, list2[i].J] == empty)
                            {
                                cMap[list2[i].I - 1, list2[i].J] = k;
                                list2.Add(new Vector(list2[i].I - 1, list2[i].J));
                                rect[list2[i].I - 1, list2[i].J].Text = k.ToString();
                            }

                        }


                        if (list2[i].I + 1 < size && !(k > size * size))
                        {
                            if (Map[list2[i].I + 1, list2[i].J] == p)
                            {
                                rect[list2[i].I + 1, list2[i].J].Background = Brushes.Yellow;
                                x = list2[i].I + 1;
                                point_finded = true;
                                y = list2[i].J;
                                LastPart(list1, list2);
                                break;

                            }
                            else if (cMap[list2[i].I + 1, list2[i].J] == empty)
                            {
                                cMap[list2[i].I + 1, list2[i].J] = k;
                                list2.Add(new Vector(list2[i].I + 1, list2[i].J));
                                rect[list2[i].I + 1, list2[i].J].Text = k.ToString();
                            }


                        }

                        if (list2[i].J - 1 >= 0 && !(k > size * size))
                        {
                            if (Map[list2[i].I, list2[i].J - 1] == p)
                            {
                                rect[list2[i].I, list2[i].J - 1].Background = Brushes.Yellow;
                                x = list2[i].I;
                                point_finded = true;
                                y = list2[i].J - 1;
                                LastPart(list1, list2);
                                break;

                            }
                            else if (cMap[list2[i].I, list2[i].J - 1] == empty)
                            {
                                cMap[list2[i].I, list2[i].J - 1] = k;
                                list2.Add(new Vector(list2[i].I, list2[i].J - 1));
                                rect[list2[i].I, list2[i].J - 1].Text = k.ToString();
                            }

                        }

                        if (list2[i].J + 1 < size && !(k > size * size))
                        {
                            if (Map[list2[i].I, list2[i].J + 1] == p)
                            {
                                rect[list2[i].I, list2[i].J + 1].Background = Brushes.Yellow;
                                x = list2[i].I;
                                point_finded = true;
                                y = list2[i].J + 1;
                                LastPart(list1, list2);
                                break;
                            }
                            else if (cMap[list2[i].I, list2[i].J + 1] == empty)
                            {
                                cMap[list2[i].I, list2[i].J + 1] = k;
                                list2.Add(new Vector(list2[i].I, list2[i].J + 1));
                                rect[list2[i].I, list2[i].J + 1].Text = k.ToString();
                            }

                        }

                    }

                    if (ashxati)
                    {
                        list1.RemoveRange(0, newcount1);
                        list2.RemoveRange(0, newcount2);
                        k++;

                        oldcount1 = newcount1;
                        oldcount2 = newcount2;
                        if (k > size * size)
                        {
                            hraman.Text = "Elq chka";
                            k = 1;
                            elqChka = true;
                            add = false;
                        }
                        //else
                        //    add = false;
                    }

                }

                LastPart(list1, list2);
                ashxati = true;
                add = true;
            }
            else
                hraman.Text = "Click Show";


        }

        public void Show_Wave(int stp1i, int stp1j, int stp2i, int stp2j)
        {
            if (waves && add)
            {
                color1 = new SolidColorBrush(Color.FromRgb(Convert.ToByte(rd1.Next(0, 255)),
                                                            Convert.ToByte(rd1.Next(0, 255)),
                                                            Convert.ToByte(rd1.Next(0, 255))));
                x2 = x;
                y2 = y;
                while (path_lengt > 0)
                {
                    path_X[path_lengt] = x;
                    path_Y[path_lengt] = y;
                    path_lengt--;
                    for (int i = 0; i < 4; ++i)
                    {
                        int iy = y + dy[i];

                        int ix = x + dx[i];

                        if (iy >= 0 && iy < size && ix >= 0 && ix < size && cMap[ix, iy] == path_lengt && cMap[ix, iy] != wall)
                        {
                            x += dx[i];
                            y += dy[i];
                            break;
                        }
                    }
                }
                path_X[0] = stp2i;
                path_Y[0] = stp2j;

                for (int i = k; i >= 0; i--)
                {
                    rect[path_X[i], path_Y[i]].Background = color1;
                    rect[path_X[i], path_Y[i]].Text = null;
                    cMap[path_X[i], path_Y[i]] = wall;
                    Walls.Add(new Vector(path_X[i], path_Y[i]));
                }

                path_lengt = k;

                for (int i = 0; i <= k; i++)
                {
                    path_X[i] = 0;
                    path_Y[i] = 0;
                }
                while (path_lengt > 0)
                {
                    path_X[path_lengt] = x2;
                    path_Y[path_lengt] = y2;
                    path_lengt--;
                    for (int i = 0; i < 4; ++i)
                    {
                        int iy = y2 + dy[i];
                        int ix = x2 + dx[i];

                        if (iy >= 0 && iy < size && ix >= 0 && ix < size && cMap[ix, iy] == -path_lengt)
                        {
                            x2 += dx[i];
                            y2 += dy[i];
                            break;
                        }
                    }
                }
                path_X[0] = stp1i;
                path_Y[0] = stp1j;

                for (int i = k; i >= 0; i--)
                {
                    rect[path_X[i], path_Y[i]].Background = color1;
                    rect[path_X[i], path_Y[i]].Text = null;
                    cMap[path_X[i], path_Y[i]] = wall;
                    Walls.Add(new Vector(path_X[i], path_Y[i]));
                }

                k = 1;

                waves = false;
                Clear_count();
                finded = true;
                hraman.Text = "Click Generate";
            }
            else
            {
                hraman.Text = "Click Generate";
            }
        }
    }
}
