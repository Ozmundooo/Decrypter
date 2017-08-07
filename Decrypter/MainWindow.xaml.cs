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

namespace Decrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String input;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            TextBox encrypted = new TextBox();
            TextBox key = new TextBox();
            key.MaxLength = 7;
            encrypted.MaxLength = 64;
            String encrypt = EncryptText();
            String alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            char[] alphaArray = alpha.ToCharArray();
            char[] inputArray = encrypt.ToCharArray();
            int seed = 1649662736;;
            int[] tranSwitcher = tranPattern(encrypt, seed);
            char[,] input2D = transpostionStep(tranSwitcher, inputArray);
            char[] input1D = convert2D(input2D);
            char[] letterSwitcher = subPattern(alphaArray, seed);
            char[] outputArray = subStep(input1D, letterSwitcher, alpha);
            String outputString = convertString(outputArray);
            decrypted.Content = outputString;
        }

        private  String EncryptText()
        {
            input = encrypted.Text;
            return input;
        }

        private  int KeyText()
        {
            int seed;
            input = key.Text;
            seed = input.GetHashCode();
            return seed; // if prob look into this
        }

        private int[] tranPattern(String input, int hash)
        {
            int[] patternArray = { 0, 1, 2, 3, 4, 5, 6, 7 };
            int l = patternArray.Length;
            int swap, swap_1, temp;
            Random random = new Random(hash);
            for (int i = 0; i < 100; i++)
            {
                swap = random.Next(0,l);
                swap_1 = random.Next(0,l);
                temp = patternArray[swap];
                patternArray[swap] = patternArray[swap_1];
                patternArray[swap_1] = temp;
            }
            return patternArray;
        }

        private static char[,] transpostionStep(int[] tranSwitcher, char[] input)
        {
            int columns = tranSwitcher.Length;
            int rows = input.Length / columns;
            char[,] tempInput2d = new char[rows, columns];
            char[,] Dec_Input2d = new char[rows, columns];
            char[] dec_Input1d = new char[rows * columns];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    tempInput2d[i,j] = input[((i * rows) + j)];
                    Dec_Input2d[j,tranSwitcher[i]] = tempInput2d[i,j];
                }
            }
            return Dec_Input2d;
        }

        private static char[] convert2D(char[,] input2D)
        {
            int rows = input2D.GetLength(0);
            int columns = input2D.GetLength(1);
            char[] inputArray1D = new char[rows * columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    inputArray1D[(i * rows) + j] = input2D[i,j];
                }
            }
            return inputArray1D;
        }

        private static char[] subPattern(char[] key, int hash)
        {
            char[] result = key;
            int l = key.Length;
            char temp;
            Random random = new Random(hash);
            int swap, swap_1;
            for (int i = 0; i < 100; i++)
            {
                swap = random.Next(0,l);
                swap_1 = random.Next(0,l);
                temp = result[swap];
                result[swap] = result[swap_1];
                result[swap_1] = temp;
            }
            return result;
        }

        private static char[] subStep(char[] inputArray, char[] letterSwitcher, String alpha)
        {
            char[] tempArray = new char[inputArray.Length];
            char[] alphaArray = new char[alpha.Length];
            alphaArray = alpha.ToCharArray();
            for (int i = 0; i < inputArray.Length; i++)
            {
                for (int j = 0; j < alphaArray.Length; j++)
                {
                    if (inputArray[i] == letterSwitcher[j])
                    {
                        tempArray[i] = alphaArray[j];
                    }
                }
            }
            return tempArray;
        }

        private static String convertString(char[] outputArray)
        {
            String output = "";
            for (int i = 0; i < outputArray.Length; i++)
            {
                output += outputArray[i];
            }
            return output;
        }


    }
}
