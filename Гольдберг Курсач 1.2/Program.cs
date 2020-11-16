using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Гольдберг_Курсач_1._2
{
    
    class Program
    {
        public class Generation
        {

           

        }
        static void print2Mas(int[,] matrix, int n, int m)//вывод двумерной матрицы
        {

            Console.WriteLine("");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine("");
            }
        }
        static void print1Mas(int[] matrix, int n)
        {
            Console.WriteLine("");
            for (int i = 0; i < n; i++)
            {
                Console.Write(matrix[i] + " ");
            }
        }

        static int[] result(int[,] matrix)
        
        {
            int t = 0;
            int[] res = new int[matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                res[i] = matrix[i, 0];
                t++;
            }
            double n = (double)(matrix.GetLength(0)) / (matrix.GetLength(1));
            n = Math.Ceiling(n);
            int m = (int)n;
            List <int> indexMas = new List <int>();
            for (int i = 0; i< matrix.GetLength(1); i++)
            {
                indexMas.Add(i);
            }
            for (int i = 0; i < matrix.GetLength(0) - matrix.GetLength(1); i++)
            {
                int min = res.Min();
                int index = Array.FindIndex(res, delegate (int p) { return p == min; });
                indexMas.Add(index);
                res[index] = res[index] + matrix[t, 0];
                t++;
            }

            return res;
        }

        

        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество заданий (m)");
            int m = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите количество процессоров (n)");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите диапозон загрузок T1 и T2");
            int t1 = int.Parse(Console.ReadLine());
            int t2 = int.Parse(Console.ReadLine());
            //Console.WriteLine("Вероятность кроссовера (%)");//maxc
            //int P_cross = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Вероятность мутации (%)");
            //int P_mutat = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Число поколений с неизменным лучшим решением? (%)");
            //int best = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine();



            //массив заданий
            int[] tasksMas = new int[m];
            var rnd = new Random();
            for (int i = 0; i < m; i++)
            {
                int nn = rnd.Next(t1, t2);
                tasksMas[i] = nn;
            }



            //формирование особи критического пути

            int[,] matrix = new int [m,n];
            for (int i = 0; i<m; i++)
            {
                int nn = tasksMas[i];
                for(int j = 0; j < n; j++)
                {
                    matrix[i, j] = nn;
                }
            }

            int[] line256 = new int[n];



            /////////////////////
            Console.WriteLine("Матрица загрузок");
            print1Mas(tasksMas, n);
            Console.WriteLine();
            print2Mas(matrix, n, m);



            int[] matrix1 = result(matrix); //массив загрузки
            Console.WriteLine();
            Console.WriteLine("Матрица загрузок случайная: " + matrix1.Max());

            ////////////////////



        }
    }
}
