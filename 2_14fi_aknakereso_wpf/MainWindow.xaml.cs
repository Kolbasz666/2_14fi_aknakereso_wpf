using System;
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

namespace _2_14fi_aknakereso_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int size = 9;
        int bomb = 10;
        int[,] matrix;
        Label[,] labels;

        //ezek az elvégezhető műveletek, listában, mindkét koordinátára
        readonly List<int[]> operations = new List<int[]>() {
                            new int[2] { -1, -1 }, new int[2] { -1, 0 }, new int[2] { -1, 1 },
                            new int[2] { 0, -1 },                        new int[2] { 0, 1 },
                            new int[2] { 1, -1 },  new int[2] { 1, 0 },  new int[2] { 1, 1 }};
        public MainWindow()
        {
            matrix = new int[size, size];
            labels = new Label[size, size];
            InitializeComponent();
            Start();
        }
        //ellenőrzi milyen lépéseket tudunk végrehajtani
        bool[] stepCheck(int row, int col)
        {
            //a nyolc érték reprezentálja a nyolc elvégezhető műveletet egy elemből: fel-le, jobbra-balra, átlók
            bool[] temp = new bool[8] { true, true, true, true, true, true, true, true };

            //nulladik sor: szélső érték
            if (row == 0)
            {
                temp[0] = false;
                temp[1] = false;
                temp[2] = false;
            }
            //utolsó sor: szélső érték 
            else if (row == size - 1)
            {
                temp[5] = false;
                temp[6] = false;
                temp[7] = false;
            }
            //nulladik oszlop: szélső érték
            if (col == 0)
            {
                temp[0] = false;
                temp[3] = false;
                temp[5] = false;
            }
            //utolsó oszlop: szélső érték
            else if (col == size - 1)
            {
                temp[2] = false;
                temp[4] = false;
                temp[7] = false;
            }

            return temp;
        }

        void Start()
        {
            for (int i = 0; i < size; i++)
            {
                RowDefinition oneRow = new RowDefinition();
                ColumnDefinition oneCol = new ColumnDefinition();
                sajt.ColumnDefinitions.Add(oneCol);
                sajt.RowDefinitions.Add(oneRow);
            }
            Random r = new Random();
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    labels[row, col] = new Label();
                    labels[row, col].MouseLeftButtonUp += LeftClickEvent;
                    labels[row, col].MouseRightButtonUp += RightClickEvent;
                    sajt.Children.Add(labels[row, col]);
                    //azért tag, mert ez nem jelenik meg a UI-on
                    labels[row, col].Tag = "";
                    Grid.SetRow(labels[row, col], row);
                    Grid.SetColumn(labels[row, col], col);
                }
            }
            int count = 0;
            while (count < bomb)
            {
                int row = r.Next(0, size);
                int col = r.Next(0, size);
                //lekérjük a grid elemeit egy collection-be, ezt a típust kiírja amikor rávisszük az egeret a . children-re
                UIElementCollection labels = sajt.Children;

                labels.Cast<Label>().ToList().ForEach(item =>
                {
                    //ha a sor és az oszlop jó, illetve még nincs akna
                    if (Grid.GetColumn(item) == col && Grid.GetRow(item) == row && item.Tag != "Akna")
                    {
                        item.Content = "Akna";
                        item.Tag = "Akna";
                        matrix[row, col] = -1;
                        count++;

                        bool[] temp = stepCheck(row, col);

                        for (int i = 0; i < 8; i++)
                        {
                            if (temp[i])
                            {
                                int teeemp = matrix[row + operations[i][0], col + operations[i][1]];
                                if (teeemp >= 0)
                                {
                                    matrix[row + operations[i][0], col + operations[i][1]]++;
                                }
                            }
                        }
                    }
                });
            }
        }



        void LeftClickEvent(object s, EventArgs e)
        {
            Label oneLabel = s as Label;
            if (oneLabel.Tag == "Akna")
            {
                oneLabel.Background = new SolidColorBrush(Colors.IndianRed);
                MessageBox.Show("Felrobbantál! :(");
                this.Close();
            }
            else
            {
                //oneLabel.Background = new SolidColorBrush(Colors.LightGreen);
                SearchLabels(oneLabel);
            }
        }
        void SearchLabels(Label originalLabel)
        {
            int row = Grid.GetRow(originalLabel);
            int col = Grid.GetColumn(originalLabel);

            if (originalLabel.Tag == "Akna")
            {
                return;
            }

            originalLabel.Background = new SolidColorBrush(Colors.LightGreen);
            if (matrix[row, col] > 0)
            {
                originalLabel.Content = matrix[row, col];
                return;
            }

            bool[] temp = stepCheck(row, col);

            for (int i = 0; i < 8; i++)
            {
                if (temp[i])
                {
                    int tempRow = row + operations[i][0];
                    int tempCol = col + operations[i][1];
                    if ((labels[tempRow, tempCol].Background as SolidColorBrush).Color != Colors.LightGreen)
                    {
                        SearchLabels(labels[tempRow, tempCol]);
                    }
                }
            }
        }
        void RightClickEvent(object s, EventArgs e)
        {
            Label oneLabel = s as Label;
            //if (((SolidColorBrush)oneLabel.Background).Color == Colors.LightBlue)
            if ((oneLabel.Background as SolidColorBrush).Color == Colors.LightBlue)
            {
                oneLabel.Background = new SolidColorBrush(Colors.White);
                oneLabel.MouseLeftButtonUp += LeftClickEvent;
            }
            else if ((oneLabel.Background as SolidColorBrush).Color != Colors.LightGreen)
            {
                oneLabel.Background = new SolidColorBrush(Colors.LightBlue);
                oneLabel.MouseLeftButtonUp -= LeftClickEvent;
            }


        }
    }
}
