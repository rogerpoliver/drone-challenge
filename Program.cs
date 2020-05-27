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
            
            //Variáveis do plano cartesiano
            int x = 0;
            int y = 0;

            // Realiza validações no input
            if (IsNullOrEmpty(input)
                || StartsWithNumbers(input)
                || StartsWithBlankSpaces(input)
                || StartsWithCanceledCommand(input)
                || HasInvalidCommands(input)
                || HasInvalidCanceledCommands(input)
                || HasMoreXThanCommands(input))
            {
                //Retorna o resultado de erro
                return "(999, 999)";
            }

            //Garante que o Input String sempre será maiusculo
            input = input.ToUpper();

            //Remove os comandos cancelados
            input = RemoveCanceledCommands(input);
            //Processa os comandos compostos e retorna uma lista de tuplas com os valores
            var commandList = ProcessCommands(input);

            // Verifica se não há overflow na lista de comandos
            if (Overflow(commandList))
            {
                //Retorna o resultado de erro
                return "(999, 999)";
            }

            // Executa os comandos para cada item da lista de comandos.
            foreach (var command in commandList)
            {
                int c = 0;
                do
                {
                    if (command.Item1.Equals("N")) y++;
                    if (command.Item1.Equals("S")) y--;
                    if (command.Item1.Equals("L")) x++;
                    if (command.Item1.Equals("O")) x--;
                    c++;
                } while (c < command.Item2);
            }

            //Retorna o resultado final
            return $"({x}, {y})";
        }

        /// <summary>
        /// Método que verifica se os comandos iniciam com numerais
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se os comandos iniciam com numerais</returns>
        private static bool StartsWithNumbers(string input)
        {
            if (Regex.IsMatch(input, "^[0-9]"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método que verifica se existem comandos em branco
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se existem comandos em branco</returns>
        private static bool StartsWithBlankSpaces(string input)
        {
            if (Regex.IsMatch(input, "^[ ]"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método que verifica se existem comandos vazios ou null
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se existem comandos vazios ou null</returns>
        private static bool IsNullOrEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método que verifica se existem comandos inválidos
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se existem comandos inválidos</returns>
        private static bool HasInvalidCommands(string input)
        {
            if (Regex.IsMatch(input, "[^NSLOX0-9]", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método que verifica se existem comandos de cancelamento inválidos
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se existem comandos inválidos</returns>
        private static bool HasInvalidCanceledCommands(string input)
        {
            if (Regex.IsMatch(input, "[X][0-9]{1,}", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método que verifica o primeiro comando é um comando para cancelamento
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se existe um comando de cancelamento no inicio do input</returns>
        private static bool StartsWithCanceledCommand(string input)
        {
            if(Regex.IsMatch(input, "^[X]", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Método que remove os comandos que foram cancelados
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna o input sem os comandos que foram cancelados</returns>
        private static string RemoveCanceledCommands(string input)
        {
            string regexPattern = "[NSLO][0-9]{0,}[X]";
            while (Regex.IsMatch(input, regexPattern, RegexOptions.IgnoreCase))
            {
                input = Regex.Replace(input, regexPattern, "");
            }
            return input;
        }

        /// <summary>
        /// Método que processa a string input e retorna os comandos do drone através de uma lista de tuplas
        /// </summary>
        /// <param name="input">String de comandos</param>
        /// <returns>Retorna uma Lista de Tuplas</returns>
        private static List<Tuple<string, int>> ProcessCommands(string input)
        {
            //Transforma a String Input em uma Lista com comandos compostos
            MatchCollection matchList = Regex.Matches(input, "[NSLO][0-9]{0,}", RegexOptions.IgnoreCase);
            var list = matchList.Cast<Match>().Select(match => match.Value).ToList();

            //Instancia o dicionario onde serão atribuídos os comandos
            var commands = new List<Tuple<string, int>>();

            //Para cada execução, transforma o comando composto em uma chave e um valor
            foreach (var item in list)
            {
                //Coleta a direção e quantidade de comandos através de Regex
                Match matchDirection = Regex.Match(item, "[NSLO]", RegexOptions.IgnoreCase);
                Match matchCount = Regex.Match(item, "[0-9]{1,}", RegexOptions.IgnoreCase);

                //Convete os comandos para String e Int.
                //Utiliza TryParse para converter a quantidade, se não existir, define como zero.
                int commandValue;
                string commandKey = matchDirection.ToString();
                int.TryParse(matchCount.Value, out commandValue);

                //Inclui os comandos na lista de tuplas
                commands.Add(Tuple.Create(commandKey, commandValue));
            }
            return commands;
        }

        /// <summary>
        /// Método que verifica que a soma de algum comando gera overflow
        /// </summary>
        /// <param name="commands">Lista de tuplas com comandos</param>
        /// <returns>Retorna true se for overflow</returns>
        private static bool Overflow(List<Tuple<string, int>> commands)
        {
            List<char> directions = new List<char> { 'N', 'S', 'L', 'O' };

            foreach (var direction in directions)
            {
                var overflow = commands.Where(c => c.Item1 == $"{direction}").Sum(x => x.Item2);

                if(overflow >= 2147483647)
                {
                    return true;                    
                }              
            }         
            return false;
        }

        /// <summary>
        /// Método que valida se o input possui mais comandos de cancelamento do que direção
        /// </summary>
        /// <param name="input">Input de comandos</param>
        /// <returns>Retorna true se existem mais cancelamentos que comandos</returns>
        private static bool HasMoreXThanCommands(string input)
        {
            MatchCollection xMatches = Regex.Matches(input, "[X]", RegexOptions.IgnoreCase);
            MatchCollection nsloMatches = Regex.Matches(input, "[NSLO]", RegexOptions.IgnoreCase);

            if(xMatches.Count > nsloMatches.Count)
            {
                return true;
            }
            return false;
        }
    }
}
