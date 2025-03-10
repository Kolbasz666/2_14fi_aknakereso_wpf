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
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        void Start()
        {
            Random r = new Random();
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    Label oneLabel = new Label();
                    sajt.Children.Add(oneLabel);
                    oneLabel.Content = "";
                    Grid.SetRow(oneLabel, row);
                    Grid.SetColumn(oneLabel, col);
                }
            }
            int count = 0;
            while (count < 10)
            {
                int row = r.Next(0, 6);
                int col = r.Next(0, 6);
                //lekérjük a grid elemeit egy collection-be, ezt a típust kiírja amikor rávisszük az egeret a . children-re
                UIElementCollection labels = sajt.Children;
                //ugyanaz mint alatta kommentben, csak másként

                labels.Cast<Label>().ToList().ForEach(item =>
                {
                    //ha a sor és az oszlop jó, illetve még nincs akna
                    if (Grid.GetColumn(item) == col && Grid.GetRow(item) == row && item.Content != "Akna")
                    {
                        item.Content = "Akna";
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
            if (oneLabel.Content == "Akna")
            {
                MessageBox.Show("Felrobbantál! :(");
            }
            else
            {
                oneLabel.Background = new SolidColorBrush(Colors.LightGreen);
            }
        }
        void RightClickEvent(object s, EventArgs e)
        {

        }
    }
}
