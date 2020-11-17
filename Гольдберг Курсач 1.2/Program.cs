using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Гольдберг_Курсач_1._2
{
    
    class Program
    {

        static int[,] sortUbav(int[,] mat)
        {

            var arr = mat.Cast<int>().OrderByDescending(a => a).ToArray();

            int c = 0;
            for (int j = 0; j < mat.GetLength(0); j++)
            {
                for (int k = 0; k < mat.GetLength(1); k++)
                {
                    mat[j, k] = arr[c];
                    c++;
                }
            }
            return mat;
        }
        static int[,] sortVoz(int[,] mat)
        {

            var arr = mat.Cast<int>().OrderBy(a => a).ToArray();

            int c = 0;
            for (int j = 0; j < mat.GetLength(0); j++)
            {
                for (int k = 0; k < mat.GetLength(1); k++)
                {
                    mat[j, k] = arr[c];
                    c++;
                }
            }
            return mat;
        }
        static void print2Mas(int[,] matrix, int n, int m)//вывод двумерной матрицы
        {
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
            for (int i = 0; i < n; i++)
            {
                Console.Write(matrix[i] + " ");
            }
        }

        static void printListArray(List<int[]> Generation1 )
        {
            Console.WriteLine("");
            for (int i = 0; i < Generation1.Count; i++)
            {
                Console.Write("(");
                Console.Write(string.Join(" ", Generation1[i]));
                Console.WriteLine(")");
            }
        }

        static int [,] IndvidMatrixGen(int[,] matrix, int[] tasksMas, int m , int n)
        {
            for (int i = 0; i < m; i++)
            {
                int nn = tasksMas[i];
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = nn;
                }
            }
            return matrix;
        }

        static int [] CriticalCreating(int[,] matrix)
        
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
                indexMas.Add(index);//массив позиций в матрице
                res[index] = res[index] + matrix[t, 0];//Tmax = return res;
                t++;
            }

            //перенос значений в геном
            int[] line256 = new int[matrix.GetLength(1)];
            int step = 256 / matrix.GetLength(1);
            int point = 0;
            for (int i = 0; i< line256.Length; i++)
            {
                point = point + step;
                line256[i] = point;
            }

            int[] individ = new int[matrix.GetLength(0)];

            int position = 0;
            int num = 0; var rnd = new Random();
            for (int i = 0; i < individ.Length; i++)
            {
                position = indexMas[i];
                //individ[i] = line256[position] / 2;
                num = rnd.Next(line256[position] - step, line256[position]);
                individ[i] = num;                
            }
            return individ;
        }

        static List<int[]> GenerationCritical(List<int[]> Generation1,int count_indiv,int[,] matrix,int m,int n, int[] tasksMas) 
        {
            for (int i = 0; i < count_indiv; i++)
            {
                IndvidMatrixGen(matrix, tasksMas, m, n);
                int[] genom1 = CriticalCreating(matrix); //геном
                Generation1.Add(genom1);
            }
            Console.WriteLine();
            for (int i = 1; i < count_indiv; i += 2)
            {
                IndvidMatrixGen(matrix, tasksMas, m, n);
                sortUbav(matrix);
                int[] genom1 = CriticalCreating(matrix); //геном
                Generation1[i] = genom1;
            }
            Console.WriteLine();
            for (int i = 1; i < count_indiv; i += 3)
            {
                IndvidMatrixGen(matrix, tasksMas, m, n);
                sortVoz(matrix);
                int[] genom1 = CriticalCreating(matrix); //геном
                Generation1[i] = genom1;
            }
            return Generation1;
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
            Console.WriteLine("Сколько особей в популяции?");
            int count_indiv = Convert.ToInt32(Console.ReadLine());
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

            Console.WriteLine("Матрица загрузок");
            print1Mas(tasksMas, m);

            //формирование поколения критического пути
            //формирование особи критического пути
            int[,] matrix = new int[m, n];
            List<int[]> Generation1 = new List<int[]>();
            Generation1 = GenerationCritical(Generation1, count_indiv, matrix, m,n, tasksMas);
           
            Console.WriteLine();
            printListArray(Generation1);
        }
    }
}
