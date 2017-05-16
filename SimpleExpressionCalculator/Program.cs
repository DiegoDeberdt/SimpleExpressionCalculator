using System;

namespace Deberdt.Yarp.SimpleExpressionCalculator
{    
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        public Program()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "Simple Calculator";
        }

        public void Run()
        {
            string expression = null;
            var parser = new ExpressionParser();

            Console.Write("Calc> ");
            while (!String.IsNullOrWhiteSpace(expression = Console.ReadLine()))
            {                
                try
                {                                                          
                    var tokens = parser.Parse(expression);
                    int result = Interpret(tokens);
                    Console.WriteLine(result);
                }
                catch (InvalidInputException e1)
                {                    
                    Console.WriteLine(expression);
                    Console.WriteLine("^".PadLeft(e1.Position + 1));
                    Console.WriteLine($"Error: unrecognized character '{e1.Input}' at position {e1.Position + 1}");
                }
                catch (InvalidOperationException e2)
                {
                    Console.WriteLine($"Error: not a valid expression! Details: {e2.Message}");
                }
                Console.Write("Calc> ");
            }
        }

        private int Interpret(Token<TokenType>[] tokens)
        {
            // Operator precedence is left to right (* or / do not receive precedence)
            int result = 0;
            for (int i = 0; i < tokens.Length;)
            {
                if (i == 0)
                {
                    result = Int32.Parse(tokens[i++].Value);
                    continue;
                }

                var operation = tokens[i++];
                int right = Int32.Parse(tokens[i++].Value);

                if (operation.Value == "+") result += right;
                else if (operation.Value == "-") result -= right;
                else if (operation.Value == "/") result /= right;
                else if (operation.Value == "*") result *= right;
            }
            return result;
        }
    }
}
