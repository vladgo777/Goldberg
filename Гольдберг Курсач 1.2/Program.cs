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
        static int[] line256;
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
            line256 = new int[matrix.GetLength(1)];
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

        static void Crossover(int[] individCross1, int[] individCross2, int m, int P_cross, int position1, int position2, int P_mutat, int n, int []tasksMas, List<int[]> Generation1)
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
                Console.WriteLine("         | Кроссовер между " + (position1 + 1) + " особью и " + (position2 + 1) + " особью в точке "+ n1 + " ВЫПОЛНЯЕТСЯ|");
                //Console.WriteLine(); Console.WriteLine("1^ ");
                //print1Mas(individCross1, individCross1.Length);
                //Console.WriteLine(); Console.WriteLine("2^ ");
                //print1Mas(individCross2, individCross2.Length);

                //мутация
                Mutation(individCross1, individCross2, m, P_mutat, position1, position2);
                //определение лучшей особи
                Console.WriteLine("                     | Сравнение потомков |");
                int[] individCrossBestChild = new int[individCross1.Length];
                individCrossBestChild = BestIndivid(individCross1, individCross2, n, tasksMas);

                //замена главной особи с лучшим ребенком
                Console.WriteLine("                     | Сравнение родителя и лучшего потомка |");
                int[] individCrossBest = new int[individCross1.Length];
                individCrossBest = BestIndivid(individCross1, individCrossBestChild, n, tasksMas);

                if (!individCrossBest.SequenceEqual(individCross1))
                {
                    Console.WriteLine("                     | Потомок лучше. Замена произведена |");
                    Generation1[position1] = individCrossBest;
                }
                else
                {
                    Console.WriteLine("                     | Родитель лучше. Замена не произведена |");
                }


                //Generation1[position2]

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

        static List<string> ChooseElement(List<string>  individCross1_binary, int position1)
        {
            int element = 0;
            var rnd4 = new Random();
            element = rnd4.Next(0, individCross1_binary.Count);

            Console.WriteLine("             | Для мутации " + (position1 + 1) + " особи выбрано " + (element + 1) + " звено|");

            string elemMas = individCross1_binary[element];
            char[] charElemMas = elemMas.ToCharArray();
            var rnd5 = new Random();
            int n4 = rnd5.Next(0, charElemMas.Length);
            //int n5 = rnd5.Next(0, charElemMas.Length);
            //while (n5 == n4)
            //{
            //    n5 = rnd5.Next(0, charElemMas.Length);
            //}
            for (int i = 0; i < charElemMas.Length; i++)
            {
                //if ((i == n4) || (i == n5))
                if ((i == n4) )
                {
                    if (charElemMas[i] == '1')
                    {
                        charElemMas[i] = '0';
                    }
                    else
                    {
                        charElemMas[i] = '1';
                    }
                }
            }
            
        
            elemMas = new string(charElemMas);
            individCross1_binary[element] = elemMas;


            return individCross1_binary;
        }
        static void Mutation(int[] individCross1, int[] individCross2, int m, int P_mutat, int position1, int position2)
        {
            var rnd3 = new Random();
            int n3 = rnd3.Next(0, 100);
            List<string> individCross1_binary = new List<string>();
            List<string> individCross2_binary = new List<string>();
            if (n3 >= 0 && n3 <= P_mutat)
            {
                for (int i = 0; i < individCross1.Length; i++)
                {
                    string binary = Convert.ToString(individCross1[i], 2);
                    individCross1_binary.Add((binary));
                }
                for (int i = 0; i < individCross2.Length; i++)
                {
                    string binary2 = Convert.ToString(individCross2[i], 2);
                    individCross2_binary.Add((binary2));
                }

                Console.WriteLine("             | Мутация для " + (position1 + 1) + " особи и " + (position2 + 1) + " особи ВЫПОЛНЯЕТСЯ|");
               
                //мутация каждого ребенка
                individCross1_binary = ChooseElement(individCross1_binary, position1);
                individCross2_binary = ChooseElement(individCross2_binary, position2);

                //print1Mas(individCross1, individCross1.Length);
                //Console.WriteLine();
                //print1Mas(individCross2, individCross2.Length);
                for (int i = 0; i < individCross1_binary.Count; i++)
                {
                    string binary = individCross1_binary[i];
                    int binaryInt = Convert.ToInt32(binary, 2); 
                    string binary2 = Convert.ToString(binaryInt, 10);
                    int binary2Int = Convert.ToInt32(binary2, 10);
                    individCross1[i] = binary2Int;
                }
                for (int i = 0; i < individCross2_binary.Count; i++)
                {
                    string binary = individCross2_binary[i];
                    int binaryInt = Convert.ToInt32(binary, 2);
                    string binary2 = Convert.ToString(binaryInt, 10);
                    int binary2Int = Convert.ToInt32(binary2, 10);
                    individCross2[i] = binary2Int;
                }

                //Console.WriteLine();
                //print1Mas(individCross1, individCross1.Length);
                //Console.WriteLine();
                //print1Mas(individCross2, individCross2.Length);
            }
            else
            {
                Console.WriteLine("             | Мутация не выполняется |");
            }
        }

        static int TmaxFunc(int[] individCross1Main, int n, int[] tasksMas)
        {
            int[] posMas = new int[n];
            for (int q = 0; q < posMas.Length; q++)
            {
                posMas[q] = 0;
            }
            int[] line256Local = new int[line256.Length + 1];
            line256Local[0] = 0;
            int line256I = 0;
            for (int qq = 1; qq < line256Local.Length; qq++)
            {
                line256Local[qq] = line256[line256I];
                line256I++;
            }
           
            for (int j = 0; j < individCross1Main.Length; j++)
            {
                for (int pos = 0; pos < line256Local.Length - 1; pos++)
                {
                    if (individCross1Main[j] >= line256Local[pos] && individCross1Main[j] <= line256Local[pos + 1])
                    {
                        posMas[pos] = posMas[pos] + tasksMas[j];
                        break;
                    }
                }
            }
            int Tmax = posMas.Max();
            return Tmax;
            
        }

        static int [] BestIndivid(int[] individCross1, int[] individCross2, int n, int[] tasksMas)
        {
            int Tmax1 = 0;
            int Tmax2 = 0;
            Tmax1 = TmaxFunc(individCross1, n, tasksMas);
            Tmax2 = TmaxFunc(individCross2, n, tasksMas);

            if (Tmax1 <= Tmax2)
            {
                Console.WriteLine("                     | 1я особь лучше |");
                return individCross1;
            }
            else
            {
                Console.WriteLine("                     | 2я особь лучше|");
                return individCross2;
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
            Console.WriteLine("Число поколений с неизменно лучшим решением? (%)");
            int best = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("-----------------------------------------");

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
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Начальное поколение (Критического пути)");
            printListArray(Generation1);

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();
            int bestCrit = 0;
            int[] bestCritPrevious = Generation1[0];
            int counterCrit = 0;
            int bestNum = 0;
            while (bestCrit != best)
            {
                int[] individCross1Main = Generation1[0];
                int position1 = 0;
                var rnd1 = new Random();
                int position2;
                for (int i = 0; i < count_indiv; i++)
                {
                    position2 = rnd.Next(0, count_indiv - 1);
                    while (position2 == position1)
                    {
                        position2 = rnd.Next(0, count_indiv - 1);
                    }
                    int[] individCross2 = Generation1[position2];
                    Console.WriteLine("    | Выбраны " + (position1 + 1) + " и " + (position2 + 1) + " особи |");

                    //кроссовер
                    Crossover(individCross1Main, individCross2, m, P_cross, position1, position2, P_mutat, n, tasksMas, Generation1);

                    //---------------------------------------
                    //выбор следущей левой особи для сравнения
                    if (i != (count_indiv - 1))
                    {
                        position1++;
                        individCross1Main = Generation1[position1];
                    }
                }
                //поиск лучшей особи в популяции
                int[] bestGenom = Generation1[0];

                Console.WriteLine();
                Console.WriteLine("    Выбор лучшей особи в поколении");
                for (int i = 1; i < Generation1.Count; i++)
                {
                    int[] Genom2 = Generation1[i];
                    bestGenom = BestIndivid(bestGenom, Genom2, n, tasksMas);
                }

                for (int i = 0; i < Generation1.Count; i++)
                {
                    if (bestGenom.SequenceEqual(Generation1[i]))
                    {
                        bestNum = i;
                    }

                }
                Console.WriteLine("    " + bestNum + "я особь - Лучшая особь в популяции ");

                //подсчет лучшей особи
                if (bestGenom.SequenceEqual(bestCritPrevious))
                {
                    bestCrit++;
                }
                else
                {
                    bestCrit = 0;
                }
                bestCritPrevious = bestGenom;
                counterCrit++;
                Console.WriteLine();
                Console.WriteLine("    -------------------------------------");
                Console.WriteLine("    Конец " + counterCrit + "го жизненного цикла");
                Console.WriteLine("    " + bestNum + "я особь - Лучшая особь в популяции ");
                Console.WriteLine("    -------------------------------------");
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine( bestNum + "я особь - Лучшая особь"); 
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Финальное поколение (Критического пути)");
            printListArray(Generation1);
            Console.WriteLine("-----------------------------------------");
        }
    }
}
