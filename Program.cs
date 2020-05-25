using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Algorithm.Logic
{

    public class Program
    {
        /// <summary>
        /// PROBLEMA:
        /// 
        /// Implementar um algoritmo para o controle de posição de um drone num plano cartesiano (X, Y).
        /// 
        /// O ponto inicial do drone é "(0, 0)" para cada execução do método Evaluate ao ser executado cada teste unitário.
        /// 
        /// A string de entrada pode conter os seguintes caracteres N, S, L, e O representando Norte, Sul, Leste e Oeste respectivamente.
        /// Estes catacteres podem estar presentes aleatóriamente na string de entrada.
        /// Uma string de entrada "NNNLLL" irá resultar em uma posição final "(3, 3)", assim como uma string "NLNLNL" irá resultar em "(3, 3)".
        /// 
        /// Caso o caracter X esteja presente, o mesmo irá cancelar a operação anterior. 
        /// Caso houver mais de um caracter X consecutivo, o mesmo cancelará mais de uma ação na quantidade em que o X estiver presente.
        /// Uma string de entrada "NNNXLLLXX" irá resultar em uma posição final "(1, 2)" pois a string poderia ser simplificada para "NNL".
        /// 
        /// Além disso, um número pode estar presente após o caracter da operação, representando o "passo" que a operação deve acumular.
		/// Este número deve estar compreendido entre 1 e 2147483647.
		/// Deve-se observar que a operação "X" não suporta opção de "passo" e deve ser considerado inválido. Uma string de entrada "NNX2" deve ser considerada inválida.
        /// Uma string de entrada "N123LSX" irá resultar em uma posição final "(1, 123)" pois a string pode ser simplificada para "N123L"
        /// Uma string de entrada "NLS3X" irá resultar em uma posição final "(1, 1)" pois a string pode ser siplificada para "NL".
        /// 
        /// Caso a string de entrada seja inválida ou tenha algum outro problema, o resultado deve ser "(999, 999)".
        /// 
        /// OBSERVAÇÕES:
        /// Realizar uma implementação com padrões de código para ambiente de "produção". 
        /// Comentar o código explicando o que for relevânte para a solução do problema.
        /// Adicionar testes unitários para alcançar uma cobertura de testes relevânte.
        /// </summary>
        /// <param name="input">String no padrão "N1N2S3S4L5L6O7O8X"</param>
        /// <returns>String representando o ponto cartesiano após a execução dos comandos (X, Y)</returns>
        public static string Evaluate(string input)
        {          

            if(StartsWithNumbers(input) || StartsWithBlankSpaces(input) || IsNullOrEmpty(input))
            {
                return "(999, 999)";
            }

            input = RemoveCanceledCommands(input);

            var comandos = ProcessCommands(input);

            //Verica cada item do Array de Chars e realiza o comando
        

            return $"(x, y)";
        }

        public static bool StartsWithNumbers(string input)
        {
            if(Regex.IsMatch(input, "^[0-9]"))
            {
                return true;
            }
            return false;
        }

        public static bool StartsWithBlankSpaces(string input)
        {
            if (Regex.IsMatch(input, "^[ ]"))
            {
                return true;
            }
            return false;
        }

        public static bool IsNullOrEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }
            return false;
        }

        public static string RemoveCanceledCommands(string input)
        {
            string regexPattern = "[NSLO][0-9]{0,}[X]";
            input = Regex.Replace(input, regexPattern, "");
            return input;
        }

        public static string ProcessCommands(string input)
        {
            string newInput = input;
            string regexPattern = "[NSLO][0-9]{0,}";

            Dictionary<string, int> commands = new Dictionary<string, int>();

            while (Regex.IsMatch(newInput, regexPattern))
            {              
                Match commandString = Regex.Match(newInput, regexPattern);

                var dicKey = Regex.Match(commandString.Value, "[NSLO]");
                var dicValue = Regex.Match(commandString.Value, "[0-9]{1,}");

                commands.Add(dicKey.ToString(), Convert.ToInt32(dicValue));
            }

            return newInput;

        }


    }
}
