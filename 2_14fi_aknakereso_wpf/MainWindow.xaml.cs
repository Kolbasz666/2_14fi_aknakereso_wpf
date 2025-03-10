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
        int size = 16;
        int bomb = 160;
        public MainWindow()
        {
            InitializeComponent();
            Start();
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
                    Label oneLabel = new Label();
                    oneLabel.MouseLeftButtonUp += LeftClickEvent;
                    oneLabel.MouseRightButtonUp += RightClickEvent;
                    sajt.Children.Add(oneLabel);
                    //azért tag, mert ez nem jelenik meg a UI-on
                    oneLabel.Tag = "";
                    Grid.SetRow(oneLabel, row);
                    Grid.SetColumn(oneLabel, col);
                }
            }
            int count = 0;
            while (count < bomb)
            {
                int row = r.Next(0, size);
                int col = r.Next(0, size);
                //lekérjük a grid elemeit egy collection-be, ezt a típust kiírja amikor rávisszük az egeret a . children-re
                UIElementCollection labels = sajt.Children;
                //ugyanaz mint alatta kommentben, csak másként

                labels.Cast<Label>().ToList().ForEach(item =>
                {
                    //ha a sor és az oszlop jó, illetve még nincs akna
                    if (Grid.GetColumn(item) == col && Grid.GetRow(item) == row && item.Tag != "Akna")
                    {
                        item.Content = "Akna";
                        item.Tag = "Akna";
                        count++;
                    }
                });

                /*foreach (UIElement item in labels)
                {
                    Label oneLabel = item as Label;
                    if (Grid.GetColumn(oneLabel) == col && Grid.GetRow(oneLabel) == row && oneLabel.Content != "Akna") {
                        oneLabel.Content = "Akna";
                        count++;
                    }
                }*/
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

            UIElementCollection labels = sajt.Children;

            labels.Cast<Label>().ToList().ForEach(item =>
            {
                if (row + 1 < size)
                {
                    if (Grid.GetColumn(item) == col && Grid.GetRow(item) == row + 1 && item.Tag != "Akna")
                    {
                        if ((item.Background as SolidColorBrush).Color != Colors.LightGreen)
                        {
                            SearchLabels(item);
                        }
                    }
                }
                if (row - 1 >= 0)
                {
                    if (Grid.GetColumn(item) == col && Grid.GetRow(item) == row - 1 && item.Tag != "Akna")
                    {
                        if ((item.Background as SolidColorBrush).Color != Colors.LightGreen)
                        {
                            SearchLabels(item);
                        }
                    }
                }
                if (col + 1 < size)
                {
                    if (Grid.GetColumn(item) == col + 1 && Grid.GetRow(item) == row && item.Tag != "Akna")
                    {
                        if ((item.Background as SolidColorBrush).Color != Colors.LightGreen)
                        {
                            SearchLabels(item);
                        }
                    }
                }
                if (col - 1 >= 0)
                {
                    if (Grid.GetColumn(item) == col - 1 && Grid.GetRow(item) == row && item.Tag != "Akna")
                    {
                        if ((item.Background as SolidColorBrush).Color != Colors.LightGreen)
                        {
                            SearchLabels(item);
                        }
                    }
                }

            });



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
            else
            {
                oneLabel.Background = new SolidColorBrush(Colors.LightBlue);
                oneLabel.MouseLeftButtonUp -= LeftClickEvent;
            }


        }
    }
}
