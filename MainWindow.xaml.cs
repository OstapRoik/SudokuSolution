using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using System.Diagnostics;

using MyProjects;

namespace SudokuSolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int LevelSize = 9;
        private readonly int UserFontSize = 22;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            for (int i = 0; i < LevelSize; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1.0, GridUnitType.Star);
                Grid.RowDefinitions.Add(row);

                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1.0, GridUnitType.Star);
                Grid.ColumnDefinitions.Add(col);
            }

            int Size = LevelSize * LevelSize;
            Label[] labels = new Label[Size];
            for (int i = 0, j = 0; i < Size; i++)
            {
                labels[i] = new Label();
                labels[i].Name = "label" + i.ToString();
                labels[i].VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                labels[i].HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                labels[i].FontSize = UserFontSize;
                Grid.Children.Add(labels[i]);
                Grid.SetColumn(labels[i], i % LevelSize);
                Grid.SetRow(labels[i], j);
                if (i % LevelSize == LevelSize - 1 && i != 0)
                    j++;
                labels[i].MouseEnter += Label_MouseEnter;
                labels[i].MouseLeave += Label_MouseLeave;
                labels[i].MouseDown += Label_MouseDown;

                int SqrtLevel = (int)Math.Sqrt(LevelSize);
                int PowLevel = SqrtLevel * SqrtLevel * SqrtLevel;
                labels[i].BorderBrush = Brushes.Black;
                labels[i].BorderThickness = new Thickness(
                    i % SqrtLevel == 0 ? 1 : 0,
                    ((i % PowLevel) >= 0 && (i % PowLevel) < LevelSize) ? 1 : 0,
                    (i + 1) % SqrtLevel == 0 ? 1 : 0,
                    ((i % PowLevel) >= (PowLevel - LevelSize) && (i % PowLevel) < PowLevel) ? 1 : 0
                    );
            }
        }

        private void Label_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (((Label)sender).Background != Brushes.DarkGray)
                ((Label)sender).Background = Brushes.LightGray;
        }

        private void Label_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (((Label)sender).Background != Brushes.DarkGray)
                ((Label)sender).Background = Brushes.Gray;
        }

        private void Label_MouseDown(object sender, RoutedEventArgs e)
        {
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    ((Label)item).Background = Brushes.Gray;
                }
            }

            ((Label)sender).Background = Brushes.DarkGray;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int SelectLabel = 0;
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    if (((Label)item).Background == Brushes.DarkGray)
                        break;
                    SelectLabel++;
                }
            }
            if (SelectLabel == LevelSize * LevelSize)
                return;
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    if (((Label)item).Name == "label" + SelectLabel.ToString())
                    {
                        ((Label)item).Content = ((Button)sender).Content;
                        break;
                    }
                }
            }
        }

        private void EraseButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectLabel = 0;
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    if (((Label)item).Background == Brushes.DarkGray)
                        break;
                    SelectLabel++;
                }
            }
            if (SelectLabel == LevelSize * LevelSize)
                return;
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    if (((Label)item).Name == "label" + SelectLabel.ToString())
                    {
                        ((Label)item).Content = null;
                        break;
                    }
                }
            }
        }

        private void EraseGridButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Grid.Children)
            {
                if(item is Label)
                {
                    ((Label)item).Content = null;
                }
            }
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            int[,] inArray = GetMap();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            int[,] outArray = Answer.Sudoku(inArray);
            sw.Stop();

            LabelTime.Content = sw.ElapsedMilliseconds + "ms";

            PrintMap(outArray);
        }

        private int[,] GetMap()
        {
            int[,] inArray = new int[LevelSize, LevelSize];
            int i = 0, j = 0;
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    if (((Label)item).Content != null)
                        inArray[i, j] = int.Parse(((Label)item).Content.ToString());
                    else
                        inArray[i, j] = 0;
                    j++;
                    if (j == LevelSize)
                    {
                        j = 0;
                        i++;
                    }
                }
            }
            return inArray;
        }

        private void PrintMap(int[,] data)
        {
            int i = 0, j = 0;
            if (data == null)
                return;
            foreach (var item in Grid.Children)
            {
                if (item is Label)
                {
                    ((Label)item).Content = data[i, j];
                    j++;
                    if (j == LevelSize)
                    {
                        j = 0;
                        i++;
                    }
                }
            }
        }
    }
}
