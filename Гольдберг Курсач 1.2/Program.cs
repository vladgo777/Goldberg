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
        static void print1Mas(int[] matrix, int m)
        {
            for (int i = 0; i < m; i++)
            {
                Console.Write(matrix[i] + " ");
            }
        }

        static void printListArray(List<int[]> Generation1 )
        {
            for (int i = 0; i < Generation1.Count; i++)
            {
                Console.Write((i+1) + ".");
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

        static int [] GenomCritical(int[,] matrix)
        
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
            int num = 0; 
            var rnd = new Random();
            for (int i = 0; i < individ.Length; i++)
            {
                position = indexMas[i];
                //individ[i] = line256[position] / 2;
                num = rnd.Next(line256[position] - step, line256[position]);
                individ[i] = num;                
            }
            return individ;
        }

        static int[] GenomRandom(int[,] matrix)
        {
            int[] individ = new int[matrix.GetLength(0)];
            var rnd = new Random();
            for (int i = 0; i < individ.Length; i++)
            {
                individ[i] = rnd.Next(0, 256);
            }
                return individ;
        }

        static int[] genom2;
        static List<int[]> GenerationRandom( int count_indiv, int[,] matrix)
        {
            List<int[]> Generation2 = new List<int[]>();
            for (int i = 0; i < count_indiv; i++)
            {
                genom2 = GenomRandom(matrix); //геном
                Generation2.Add(genom2);
            }
            return Generation2;
        }

        static int[] genom1;
        static List<int[]> GenerationCritical(int count_indiv,int[,] matrix) 
        {
            List<int[]> Generation1 = new List<int[]>();
            for (int i = 0; i < count_indiv; i++)
            {
                 genom1 = GenomCritical(matrix); //геном
                Generation1.Add(genom1);
            }
            for (int i = 1; i < count_indiv; i += 2)
            {
                sortUbav(matrix);
                 genom1 = GenomCritical(matrix); //геном
                Generation1[i] = genom1;
            }
            for (int i = 1; i < count_indiv; i += 3)
            {
                sortVoz(matrix);
                 genom1 = GenomCritical(matrix); //геном
                Generation1[i] = genom1;
            }
            return Generation1;
        }

        static void Crossover(int[] individCross1, int[] individCross2, int m, int P_cross, int position1, int position2, int P_mutat)
        {
            var rnd2 = new Random();
            int n2 = rnd2.Next(0, 100);
            if (n2 >= 0 && n2 <= P_cross)
            {
                var rnd1 = new Random();
                int n1 = rnd1.Next(1, m - 1);
                int[] part = new int[m - n1];
                int j_part = 0;
                //Console.WriteLine(); Console.WriteLine("n1 ");
                //Console.WriteLine(n1 + " ");
                for (int i = 0; i < m; i++)
                {
                    if ((i == n1) || (i > n1))
                    {
                        part[j_part] = individCross1[i];
                        individCross1[i] = individCross2[i];
                        j_part++;
                    }
                }
                j_part = 0;
                for (int i = 0; i < m; i++)
                {
                    if ((i == n1) || (i > n1))
                    {
                        individCross2[i] = part[j_part];
                        j_part++;
                    }
                }
                Console.WriteLine("         | Кроссовер между " + (position1 + 1) + " особью и " + (position2 + 1) + " особью ВЫПОЛНЯЕТСЯ|");
                //Console.WriteLine(); Console.WriteLine("1^ ");
                //print1Mas(individCross1, individCross1.Length);
                //Console.WriteLine(); Console.WriteLine("2^ ");
                //print1Mas(individCross2, individCross2.Length);


                //мутация
                Mutation(individCross1, individCross2, m, P_mutat, position1, position2);
            }
            else
            {
                Console.WriteLine("         | Кроссовер не выполняется |");
            }
        }
        public static string IntToString(int value, char[] baseChars)
        {
            string result = string.Empty;
            int targetBase = baseChars.Length;

            do
            {
                result = baseChars[value % targetBase] + result;
                value = value / targetBase;
            }
            while (value > 0);

            return result;
        }
        static void Mutation(int[] individCross1, int[] individCross2, int m, int P_mutat, int position1, int position2)
        {
            var rnd3 = new Random();
            int n3 = rnd3.Next(0, 100);

            //int[] individCross1_binary = new int[individCross1.Length];
            //int[] individCross2_binary = new int[individCross2.Length];

            List<string> individCross1_binary = new List<string>();
            List<string> individCross2_binary = new List<string>();
            if (n3 >= 0 && n3 <= P_mutat)
            {
                int[] binaryMas = new int[individCross1.Length];
                for (int i = 0; i < individCross1.Length; i++)
                {

                    string binary = Convert.ToString(individCross1[i], 2);
                    
                    individCross1_binary.Add((binary));
                }

                Console.WriteLine("             | Мутация между " + (position1 + 1) + " особью и " + (position2 + 1) + " особью ВЫПОЛНЯЕТСЯ|");
                for (int i = 0; i < individCross1_binary.Count; i++)
                {
                    Console.Write((i + 1) + ".");
                    Console.Write("(");
                    Console.Write(string.Join(" ", individCross1_binary[i]));
                    Console.WriteLine(")");
                }
                //Console.WriteLine("Двоичное представление");
                //    print1Mas(individCross1_binary, individCross1_binary.Length);
            }
            else
            {
                Console.WriteLine("             | Мутация не выполняется |");
            }
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
            Console.WriteLine("Вероятность кроссовера (%)");//maxc
            int P_cross = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Вероятность мутации (%)");
            int P_mutat = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Число поколений с неизменным лучшим решением? (%)");
            //int best = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine();

            //одномерный массив заданий
            int[] tasksMas = new int[m];
            var rnd = new Random();
            for (int i = 0; i < m; i++)
            {
                int nn = rnd.Next(t1, t2);
                tasksMas[i] = nn;
            }
            Console.WriteLine("Матрица загрузок");
            print1Mas(tasksMas, m);

            //формирование матрицы загрузок для особи
            int[,] matrix = new int[m, n];
            IndvidMatrixGen(matrix, tasksMas, m, n);

            //формирование поколения критического пути
            List<int[]> Generation1 = new List<int[]>();
            Generation1 = GenerationCritical(count_indiv, matrix);

            //формирование поколения рандом
            List<int[]> Generation2 = new List<int[]>();
            Generation2 = GenerationRandom(count_indiv, matrix);

            //основная часть Критического пути
            Console.WriteLine();
            int[] individCross1 = Generation1[0];
            int position1 = 0;
            var rnd1 = new Random();
            int position2;
            Console.WriteLine();
            Console.WriteLine("Начальное поколение (Критического пути)");
            printListArray(Generation1); 
            for (int i = 0; i< count_indiv; i++)
            {
                position2 = rnd.Next(0, count_indiv - 1);
                while (position2 == position1)
                {
                    position2 = rnd.Next(0, count_indiv-1);
                }
                int[] individCross2 = Generation1[position2];
                Console.WriteLine(); 
                //Console.WriteLine("1^ ");
                //print1Mas(individCross1, individCross1.Length);
                //Console.WriteLine(); Console.WriteLine("2^ "); Console.WriteLine();
                //print1Mas(individCross2, individCross2.Length);
                Console.WriteLine("    | Выбраны " + (position1 + 1) + " и " + (position2 + 1) + " особи |");
                
                //кроссовер
                Crossover(individCross1, individCross2, m, P_cross, position1, position2,  P_mutat);
                //Console.WriteLine(); Console.WriteLine("11^ ");
                //print1Mas(individCross1, individCross1.Length);
                //Console.WriteLine(); Console.WriteLine("22^ "); Console.WriteLine();
                //print1Mas(individCross2, individCross2.Length);

                if (i != (count_indiv - 1))
                {
                    position1++;
                    individCross1 = Generation1[position1];
                }
            }
        }
    }
}
