using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdvancedCalculatorProject
{
    public interface ICalculatorOperation<T>
    {
        T Execute(T a, T b);
        string OperationName { get; }
    }

    public interface IAdvancedMathOperations
    {
        double CalculateDerivative(Func<double, double> function, double x);
        double CalculateIntegral(Func<double, double> function, double a, double b, int steps = 1000);
        Complex[] FindPolynomialRoots(double[] coefficients);
        Matrix<double> MultiplyMatrices(Matrix<double> a, Matrix<double> b);
    }

    public class Matrix<T> where T : struct, IComparable<T>
    {
        private readonly T[,] _data;
        public int Rows { get; }
        public int Columns { get; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _data = new T[rows, columns];
        }

        public Matrix(T[,] data)
        {
            Rows = data.GetLength(0);
            Columns = data.GetLength(1);
            _data = (T[,])data.Clone();
        }

        public T this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b) => a.Add(b);
        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b) => a.Subtract(b);

        public Matrix<T> Add(Matrix<T> other)
        {
            ValidateDimensions(other);
            var result = new Matrix<T>(Rows, Columns);
            
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    dynamic a = _data[i, j];
                    dynamic b = other[i, j];
                    result[i, j] = a + b;
                }
            }
            return result;
        }

        public Matrix<T> Subtract(Matrix<T> other)
        {
            ValidateDimensions(other);
            var result = new Matrix<T>(Rows, Columns);
            
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    dynamic a = _data[i, j];
                    dynamic b = other[i, j];
                    result[i, j] = a - b;
                }
            }
            return result;
        }

        private void ValidateDimensions(Matrix<T> other)
        {
            if (Rows != other.Rows || Columns != other.Columns)
                throw new InvalidOperationException("Matrix dimensions must match");
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write($"{_data[i, j]:F2}\t");
                }
                Console.WriteLine();
            }
        }
    }

    // Classes de operações implementadas fora da classe principal
    public class AdditionOperation : ICalculatorOperation<double>
    {
        public double Execute(double a, double b) => a + b;
        public string OperationName => "Addition";
    }

    public class SubtractionOperation : ICalculatorOperation<double>
    {
        public double Execute(double a, double b) => a - b;
        public string OperationName => "Subtraction";
    }

    public class MultiplicationOperation : ICalculatorOperation<double>
    {
        public double Execute(double a, double b) => a * b;
        public string OperationName => "Multiplication";
    }

    public class DivisionOperation : ICalculatorOperation<double>
    {
        public double Execute(double a, double b)
        {
            if (Math.Abs(b) < double.Epsilon) 
                throw new DivideByZeroException("Cannot divide by zero");
            return a / b;
        }
        public string OperationName => "Division";
    }

    public class PowerOperation : ICalculatorOperation<double>
    {
        public double Execute(double a, double b) => Math.Pow(a, b);
        public string OperationName => "Power";
    }

    public class LogarithmOperation : ICalculatorOperation<double>
    {
        public double Execute(double a, double b)
        {
            if (a <= 0 || b <= 0 || Math.Abs(b - 1) < double.Epsilon)
                throw new ArgumentException("Invalid arguments for logarithm");
            return Math.Log(a, b);
        }
        public string OperationName => "Logarithm";
    }

    // Classe de extensões em nível superior
    public static class CalculatorExtensions
    {
        public static double StandardDeviation(this AdvancedCalculator calculator, IEnumerable<double> values)
        {
            var list = values.ToList();
            if (list.Count < 2)
                throw new ArgumentException("At least two values are required for standard deviation");

            double mean = list.Average();
            double sumSquares = list.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(sumSquares / (list.Count - 1));
        }

        public static IEnumerable<double> MovingAverage(this AdvancedCalculator calculator, 
            IEnumerable<double> values, int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentException("Window size must be positive");

            var queue = new Queue<double>();
            
            foreach (double value in values)
            {
                queue.Enqueue(value);
                if (queue.Count > windowSize)
                    queue.Dequeue();
                
                if (queue.Count == windowSize)
                    yield return queue.Average();
            }
        }

        public static double Median(this AdvancedCalculator calculator, IEnumerable<double> values)
        {
            var sorted = values.OrderBy(x => x).ToArray();
            int count = sorted.Length;
            
            if (count == 0)
                throw new ArgumentException("Collection cannot be empty");

            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            else
                return sorted[count / 2];
        }
    }

    public class AdvancedCalculator : IAdvancedMathOperations
    {
        private readonly Dictionary<string, ICalculatorOperation<double>> _operations;

        public AdvancedCalculator()
        {
            _operations = new Dictionary<string, ICalculatorOperation<double>>
            {
                ["+"] = new AdditionOperation(),
                ["-"] = new SubtractionOperation(),
                ["*"] = new MultiplicationOperation(),
                ["/"] = new DivisionOperation(),
                ["^"] = new PowerOperation(),
                ["log"] = new LogarithmOperation()
            };
        }

        // Método genérico para operações básicas
        public TResult PerformOperation<TResult>(Func<TResult> operation, string operationName = "")
        {
            try
            {
                Console.WriteLine($"Executing operation: {operationName}");
                var result = operation();
                Console.WriteLine($"Operation completed successfully. Result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {operationName}: {ex.Message}");
                throw;
            }
        }

        // Método com múltiplos parâmetros genéricos
        public TResult TransformData<TInput, TResult>(
            TInput input, 
            Func<TInput, TResult> transformer,
            Predicate<TInput> validator = null)
        {
            if (validator != null && !validator(input))
                throw new ArgumentException("Input validation failed");

            return transformer(input);
        }

        // Cálculo de derivada numérica
        public double CalculateDerivative(Func<double, double> function, double x)
        {
            return PerformOperation(() =>
            {
                const double h = 1e-8;
                return (function(x + h) - function(x - h)) / (2 * h);
            }, "Derivative Calculation");
        }

        // Integração numérica usando regra de Simpson
        public double CalculateIntegral(Func<double, double> function, double a, double b, int steps = 1000)
        {
            return PerformOperation(() =>
            {
                if (steps % 2 != 0) steps++;
                
                double h = (b - a) / steps;
                double sum = function(a) + function(b);

                for (int i = 1; i < steps; i++)
                {
                    double x = a + i * h;
                    sum += function(x) * (i % 2 == 0 ? 2 : 4);
                }

                return sum * h / 3;
            }, "Integral Calculation");
        }

        // Encontrar raízes de polinômio
        public Complex[] FindPolynomialRoots(double[] coefficients)
        {
            return PerformOperation(() =>
            {
                if (coefficients == null || coefficients.Length == 0)
                    throw new ArgumentException("Coefficients cannot be null or empty");

                int degree = coefficients.Length - 1;
                var roots = new List<Complex>();

                // Para equação quadrática
                if (degree == 2)
                {
                    double a = coefficients[0];
                    double b = coefficients[1];
                    double c = coefficients[2];
                    double discriminant = b * b - 4 * a * c;

                    if (discriminant >= 0)
                    {
                        roots.Add(new Complex((-b + Math.Sqrt(discriminant)) / (2 * a), 0));
                        roots.Add(new Complex((-b - Math.Sqrt(discriminant)) / (2 * a), 0));
                    }
                    else
                    {
                        roots.Add(new Complex(-b / (2 * a), Math.Sqrt(-discriminant) / (2 * a)));
                        roots.Add(new Complex(-b / (2 * a), -Math.Sqrt(-discriminant) / (2 * a)));
                    }
                }
                else
                {
                    // Método numérico simples para outras equações
                    for (int i = 0; i < degree; i++)
                    {
                        double guess = i - degree / 2.0;
                        try
                        {
                            double root = SolveEquation(x => 
                                coefficients.Select((coef, idx) => coef * Math.Pow(x, degree - idx)).Sum(), 
                                guess);
                            roots.Add(new Complex(root, 0));
                        }
                        catch
                        {
                            // Ignora raízes não encontradas
                        }
                    }
                }

                return roots.ToArray();
            }, "Polynomial Roots Calculation");
        }

        // Multiplicação de matrizes
        public Matrix<double> MultiplyMatrices(Matrix<double> a, Matrix<double> b)
        {
            return PerformOperation(() =>
            {
                if (a.Columns != b.Rows)
                    throw new InvalidOperationException("Matrix dimensions are incompatible for multiplication");

                var result = new Matrix<double>(a.Rows, b.Columns);

                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < b.Columns; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < a.Columns; k++)
                        {
                            sum += a[i, k] * b[k, j];
                        }
                        result[i, j] = sum;
                    }
                }

                return result;
            }, "Matrix Multiplication");
        }

        // Método recursivo para cálculo de fatorial
        public BigInteger Factorial(int n)
        {
            if (n < 0) 
                throw new ArgumentException("Factorial is not defined for negative numbers");
            
            return n == 0 ? BigInteger.One : n * Factorial(n - 1);
        }

        // Método com expressão lambda complexa
        public IEnumerable<double> GenerateSequence(
            double start, 
            Func<double, double> generator, 
            int count,
            Func<double, bool> filter = null)
        {
            return Enumerable.Range(0, count)
                .Select(i => generator(start + i))
                .Where(x => filter == null || filter(x))
                .ToList();
        }

        // Método assíncrono
        public async Task<double> CalculateAsync(Func<double> calculation)
        {
            return await Task.Run(() =>
            {
                Console.WriteLine("Starting async calculation...");
                var result = calculation();
                Console.WriteLine("Async calculation completed");
                return result;
            });
        }

        // Método com parâmetros opcionais e nomeados
        public double SolveEquation(
            Func<double, double> equation,
            double initialGuess = 0,
            double tolerance = 1e-10,
            int maxIterations = 1000)
        {
            return PerformOperation(() =>
            {
                double x = initialGuess;
                for (int i = 0; i < maxIterations; i++)
                {
                    double fx = equation(x);
                    double derivative = CalculateDerivative(equation, x);
                    
                    if (Math.Abs(derivative) < tolerance)
                        throw new InvalidOperationException("Derivative too small");
                    
                    double xNew = x - fx / derivative;
                    
                    if (Math.Abs(xNew - x) < tolerance)
                        return xNew;
                    
                    x = xNew;
                }
                
                throw new InvalidOperationException("Maximum iterations reached");
            }, "Equation Solving");
        }

        // Método para calcular Fibonacci
        public IEnumerable<BigInteger> FibonacciSequence(int count)
        {
            return PerformOperation(() =>
            {
                if (count <= 0)
                    throw new ArgumentException("Count must be positive");

                return GenerateFibonacciSequence(count);
            }, "Fibonacci Sequence");

            IEnumerable<BigInteger> GenerateFibonacciSequence(int n)
            {
                BigInteger a = 0, b = 1;
                for (int i = 0; i < n; i++)
                {
                    yield return a;
                    BigInteger temp = a;
                    a = b;
                    b = temp + b;
                }
            }
        }

        // Método para estatísticas descritivas
        public DescriptiveStatistics CalculateDescriptiveStatistics(IEnumerable<double> data)
        {
            return PerformOperation(() =>
            {
                var list = data.ToList();
                if (!list.Any())
                    throw new ArgumentException("Data cannot be empty");

                return new DescriptiveStatistics
                {
                    Mean = list.Average(),
                    Median = this.Median(list),
                    StandardDeviation = this.StandardDeviation(list),
                    Min = list.Min(),
                    Max = list.Max(),
                    Count = list.Count
                };
            }, "Descriptive Statistics");
        }
    }

    // Classe para estatísticas
    public class DescriptiveStatistics
    {
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public int Count { get; set; }

        public void Print()
        {
            Console.WriteLine($"Mean: {Mean:F4}");
            Console.WriteLine($"Median: {Median:F4}");
            Console.WriteLine($"Standard Deviation: {StandardDeviation:F4}");
            Console.WriteLine($"Min: {Min:F4}");
            Console.WriteLine($"Max: {Max:F4}");
            Console.WriteLine($"Count: {Count}");
        }
    }

    // Classe principal para demonstração
    class Program
    {
        static async Task Main(string[] args)
        {
            var calculator = new AdvancedCalculator();
            
            DemonstrateBasicOperations(calculator);
            DemonstrateAdvancedCalculations(calculator);
            DemonstrateMatrixOperations(calculator);
            DemonstrateSequenceGeneration(calculator);
            await DemonstrateAsyncOperations(calculator);
            DemonstrateStatistics(calculator);
            DemonstrateFibonacci(calculator);
        }

        static void DemonstrateBasicOperations(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== BASIC OPERATIONS ===");
            
            var result1 = calculator.PerformOperation(() => 
                calculator.TransformData(5.0, x => x * x + 2 * x + 1));
            Console.WriteLine($"Quadratic transform: {result1}");

            var result2 = calculator.PerformOperation(() => calculator.Factorial(20));
            Console.WriteLine($"Factorial of 20: {result2}");

            Console.WriteLine();
        }

        static void DemonstrateAdvancedCalculations(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== ADVANCED CALCULATIONS ===");
            
            // Derivada
            double derivative = calculator.CalculateDerivative(x => x * x * x + 2 * x, 2);
            Console.WriteLine($"Derivative at x=2: {derivative:F4}");

            // Integral
            double integral = calculator.CalculateIntegral(x => x * x, 0, 1);
            Console.WriteLine($"Integral of x² from 0 to 1: {integral:F4}");

            // Resolver equação
            double root = calculator.SolveEquation(x => x * x - 4, initialGuess: 1);
            Console.WriteLine($"Root of x² - 4 = 0: {root:F4}");

            // Raízes de polinômio
            var roots = calculator.FindPolynomialRoots(new double[] { 1, -3, 2 }); // x² - 3x + 2
            Console.WriteLine("Polynomial roots:");
            foreach (var rootComplex in roots)
            {
                Console.WriteLine($"  {rootComplex}");
            }

            Console.WriteLine();
        }

        static void DemonstrateMatrixOperations(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== MATRIX OPERATIONS ===");
            
            var matrixA = new Matrix<double>(new double[,] { { 1, 2 }, { 3, 4 } });
            var matrixB = new Matrix<double>(new double[,] { { 5, 6 }, { 7, 8 } });

            var product = calculator.MultiplyMatrices(matrixA, matrixB);
            Console.WriteLine("Matrix multiplication result:");
            product.PrintMatrix();

            Console.WriteLine();
        }

        static void DemonstrateSequenceGeneration(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== SEQUENCE GENERATION ===");
            
            var sequence = calculator.GenerateSequence(1, x => x * x, 10, x => x % 2 == 0);
            Console.WriteLine("Filtered squares: " + string.Join(", ", sequence));

            var numbers = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var movingAvg = calculator.MovingAverage(numbers, 3);
            Console.WriteLine("Moving average: " + string.Join(", ", movingAvg));

            Console.WriteLine();
        }

        static async Task DemonstrateAsyncOperations(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== ASYNC OPERATIONS ===");
            
            var asyncResult = await calculator.CalculateAsync(() => 
            {
                System.Threading.Thread.Sleep(1000); // Simula trabalho pesado
                return Math.PI * Math.Pow(10, 2);
            });
            
            Console.WriteLine($"Async area calculation: {asyncResult:F4}");
            Console.WriteLine();
        }

        static void DemonstrateStatistics(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== STATISTICS ===");
            
            var data = new List<double> { 1.2, 2.3, 3.4, 4.5, 5.6, 6.7, 7.8, 8.9, 9.0, 10.1 };
            var stats = calculator.CalculateDescriptiveStatistics(data);
            stats.Print();

            Console.WriteLine();
        }

        static void DemonstrateFibonacci(AdvancedCalculator calculator)
        {
            Console.WriteLine("=== FIBONACCI SEQUENCE ===");
            
            var fibonacci = calculator.FibonacciSequence(15);
            Console.WriteLine("First 15 Fibonacci numbers: " + string.Join(", ", fibonacci));

            Console.WriteLine();
        }
    }
}