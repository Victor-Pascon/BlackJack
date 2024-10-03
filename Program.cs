using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    //Definindo a classe Carta, que representa uma carta do baralho.
    public class Cartas
    {
        //Propriedades para somente leitura para Naipe das cartas (EX: Copas, Ouros)
        public string Naipe { get; }

        //Propriedades para somente leitura para Sequencia das cartas (EX: 2, 3, J, Ace)
        public string Sequencia { get; }

        //Metodo Construtor da classe CARTAS que recebera o valor de (SEQUENCIA) e o (NAIPE) da carta.
        public Cartas(string naipe, string sequencia)
        {
            Sequencia = sequencia;
            Naipe = naipe;
        }

        //Metodo que ira retornar os valores númericos das cartas.
        public int ObterValor()
        {
            //Tentar converter SEQUENCIA para um número inteiro
            if (int.TryParse(Sequencia, out int number))
            {
                return number; //Se for um número entre (2-10), retorna esse número.
            }

            else if (Sequencia == "Ace")
            {
                return 11; //Ás valem 11 inicialmente
            }

            else
            {
                return 10; //Cartas J, Q, K. Valem 10.
            }

        }

        public override string ToString()
        {
            return $"{Sequencia} de {Naipe}";
        }
    }

    //Define a classe Baralho, que ira representar o baralho do jogo.
    public class Baralho
    {
        //Lista interna de cartas no baralho
        private List<Cartas> cartas;

        //Arrays estáticos que define as Sequencias (Valores) e Naipes das cartas.
        private static readonly string[] Naipe = { "Copas", "Ouros", "Espadas", "Paus" };
        private static readonly string[] Sequencia = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "Ace" };
        
        // Instancia RAMDOM para gerar um número aleatorio durante o embaralhamento do baralho.
        private Random ramdom = new Random();

        // Construtor da classe Baralho que inicializa o baralho com 52 castas.
        public Baralho()
        {
            cartas = new List<Cartas>(); // Inicia a lista de cartas.

            // Loop através de cada Naipe
            foreach (var naipe in Naipe)
            {
                 //Adicionar uma nova carta ao baralho com o valor e naipe atuais
                 foreach (var  sequencia in Sequencia)
                {
                    cartas.Add(new Cartas(naipe, sequencia));
                }
            }
        }

        //Metodo para embaralhar o baralho usando o algoritimo de Fisher-Yates
        public void embaralhar()
        {
            // Percorre o baralho de traz para frente
            for (int i = cartas.Count - 1; i > 0; i--)
            {
                //Seleciona um indice aleatorio entre 0 e i
                int j = ramdom.Next(i + 1);

                // Troca as cartas nos indices i e j
                var temp = cartas[i];
                cartas[i] = cartas[j];
                cartas[j] = temp;
            }
        }

        //Método para distribuir uma carta do topo do baralho
        public Cartas distribuir()
        {
            if (cartas.Count == 0)
            {
                throw new InvalidOperationException("### O Baralho Acabou! ###");
            }

            // Selecionar a primeira carta do baralho 
            var carta = cartas[0];

            //Remove a carta do baralho
            cartas.RemoveAt(0);

            // Retorna a carta distribuida
            return carta;
        }
    }

    // Define a classe Mao, que representa a mão do jogador ou Dealer
    public class Mao
    {
        //Lista publica de cartas na mão
        public List<Cartas> Cartas { get; }

        // Construir classe Mao que inicializa a lista de cartas
        public Mao()
        {
            Cartas = new List<Cartas>();
        }

        //Metodo para adicionar uma carta
        public void AdicionarCarta(Cartas cartas)
        {
            Cartas.Add(cartas);
        }

       // Metodo para calcular o valor total da mão, considerando o Aces
        public int RetornarValorTotal()
        {
            int Total = 0;     // Valor total da mão
            int TotalAce = 0; // Número de Aces na mão

            // Intera atraves de todas as cartas na mão
            foreach (var carta in Cartas)
            {
                Total += carta.ObterValor(); // Adiciona o valor da carta ao total

                //Se a carta for um Ace, incrementa o contador de Aces
                if (carta.Sequencia == "Ace")
                {
                    TotalAce++;
                }
            }

            // Ajusta o valor dos Aces de 11 para 1 se o total exceder 21
            while (Total > 21 && TotalAce > 0)
            {
                Total -= 10; //Reduz o total em 10 (equivalente a mudar um Aces de 11 para 1.
                TotalAce--; // Diminui o número de Aces que podem ser ajustados
            }

            return Total; // Retorna o valor total ajustado da mao

        }

        // Metodo que retorna uma representação em string das cartas na mão.
        public override string ToString()
        {
            return string.Join(", ", Cartas);
        }
    }

    // Define a classe Jogo, que comtem a lógica principal do Jogo
    public class Jogo
    {
        //Metodo Main, ponto de entrada do programa
        public static void Main()
        {
            // Exibir mensagem de boas-vindas
            Console.WriteLine("======================= Bem-Vindo ao BlackJack ===================");
            Console.WriteLine("----------------------- A Sorte esta Lançada! --------------------");
            Console.WriteLine("");
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=- Turno do Jogador -=-=-=-=-=-=-=-=-=-=-=-");

            // Criar e embaralhar o baralho
            Baralho baralho = new Baralho();
            baralho.embaralhar();

            // Criar as mãos para o jogador e o Dealer
            Mao MaoJogador = new Mao();
            Mao MaoDealer = new Mao();

            // Distribuir duas cartas para o jogador
            MaoJogador.AdicionarCarta(baralho.distribuir());
            MaoJogador.AdicionarCarta(baralho.distribuir());

            // Distribuir duas cartas para o Dealer
            MaoDealer.AdicionarCarta(baralho.distribuir());
            MaoDealer.AdicionarCarta(baralho.distribuir());

            // Variaveis de Controle do jogo
            bool TurnoJogador = true; // Indica se é a vez do jogador
            bool GameOver = false; //Indica se o jogo terminou

            // Loop principal do turno do jogador
            while (TurnoJogador && !GameOver)
            {
                // Exibe a mão do jogador e o valor total
                Console.WriteLine($"\nVocê tem em sua mão as cartas: {MaoJogador}. Total: {MaoJogador.RetornarValorTotal()}");

                // Exibir apenas a primeira carta do Dealer (a segunda permanece oculta)
                Console.WriteLine($"A carta visivel na mão do Dealer é: {MaoDealer.Cartas[0]}");
                Console.WriteLine("");
                Console.WriteLine("------------------------------------------------------------------");

                // Verificar se o jogador tem BlackJack (21 com as duas primeiras cartas)
                if (MaoJogador.RetornarValorTotal() == 21)
                {
                    Console.WriteLine("Caramba! Parabéns você fez BlackJack, você ganhou!!!!!!!");
                    Console.WriteLine("");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>> Muito Obrigado Por Jogar! <<<<<<<<<<<<<<<<<<<<<<");
                    GameOver = true;
                    break; // Encerra o loop
                }

                // Solicitar ao jogador que escolha entre "pegar" ou "passar"
                Console.WriteLine("");
                Console.Write("Você deseja [P]-> Pegar uma carta ou [S] <- Passar?: ");
                string input = Console.ReadLine().ToLower();  // Lê a entrada do usuário e converte para letra minúscula

                if (input == "p" || input == "pegar")
                {
                    // O jogador escolhe Pegar: recebendo assim uma nova carta
                    Cartas NovaCarta = baralho.distribuir();
                    Console.WriteLine($"Você comprou uma carta, e carta recebida foi: {NovaCarta}");
                    MaoJogador.AdicionarCarta(NovaCarta);
                    Console.WriteLine("");
                    Console.WriteLine("------------------------------------------------------------------");

                    // Verificar se o jogador estourou (Valor maior que 21)
                    if (MaoJogador.RetornarValorTotal() > 21)
                    {
                        Console.WriteLine($"Você tem em sua mão: {MaoJogador}. Totalizando: {MaoJogador.RetornarValorTotal()}");
                        Console.WriteLine("");
                        Console.WriteLine("Eita rapaz... Você estourou, não foi dessa vez, você perdeu!");
                        Console.WriteLine("");
                        Console.WriteLine(">>>>>>>>>>>>>>>>>> Muito Obrigado Por Jogar! <<<<<<<<<<<<<<<<<<<<<<");
                        GameOver = true;
                        break; //Encerra o jogo
                    }
                }
                else if (input == "s" || input == "passar")
                {
                    // O jogador escolhe Passar, terminando o turno do jogador
                    TurnoJogador = false;
                }
                else
                {
                    // Entrada inválida: Informa o usuário e repete o loop
                    Console.WriteLine("Você digitou algo aleatorio nada haver, entenda, digite [P] para Pegar carta e [S] para passar");
                }
            }

            // Turno do Dealer, somente acontece após o turno do jogador
            if (!GameOver)
            {
                Console.WriteLine("");
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=- Turno do Dealer -=-=-=-=-=-=-=-=-=-=-=-");
                Console.WriteLine("");
                Console.WriteLine($"A Mão do Dealer é: {MaoDealer}. Totalizando: {MaoDealer.RetornarValorTotal()}");

                // Dealer deve continuar recebendo até atingir ao menos 17 pontos
                while (MaoDealer.RetornarValorTotal() < 18)
                {
                    // Dealer recebe uma nova carta
                    Cartas NovaCarta = baralho.distribuir();
                    Console.WriteLine($"O Dealer recebeu a carta: {NovaCarta}");
                    MaoDealer.AdicionarCarta(NovaCarta);
                    Console.WriteLine("");
                    Console.WriteLine($"A mão do Dealer é: {MaoDealer}. Totalizando: {MaoDealer.RetornarValorTotal()}");
                    Console.WriteLine("------------------------------------------------------------------");
                    Console.WriteLine("");
                }

                // Verificar se o Dealer estourou
                if (MaoDealer.RetornarValorTotal() > 21)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Mais que coisa em, o Dealer estourou! Você ganhou campeão!");
                    Console.WriteLine("");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>> Muito Obrigado Por Jogar! <<<<<<<<<<<<<<<<<<<<<<");
                    Console.WriteLine("");
                    Console.WriteLine("Pressione Enter para encerrar o jogo.");
                    Console.ReadLine(); // Aguarda o usuário pressionar Enter
                    return; // Encerra o jogo
                }

                // Calcula os valores finais das mãos do Jogador e do Dealer
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("");
                int TotalPontosJogador = MaoJogador.RetornarValorTotal();
                int TotalPontosDealer = MaoDealer.RetornarValorTotal();

                // Exibir a pontuação final
                Console.WriteLine($"A sua pontuação foi: {TotalPontosJogador}");
                Console.WriteLine($"A pontuação do Dealer foi: {TotalPontosDealer}");

                // Determina o vencedor da partida
                if (TotalPontosJogador > TotalPontosDealer)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Aeeeeeee! Você ganhou, meus parabéns!!!!!!!");
                    Console.WriteLine("");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>> Muito Obrigado Por Jogar! <<<<<<<<<<<<<<<<<<<<<<");
                    Console.WriteLine("");
                    Console.WriteLine("Pressione Enter para encerrar o jogo.");
                    Console.ReadLine(); // Aguarda o usuário pressionar Enter
                }
                else if (TotalPontosJogador < TotalPontosDealer)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Eita nois... o Dealer ganhou, infelizmente você perdeu!");
                    Console.WriteLine("");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>> Muito Obrigado Por Jogar! <<<<<<<<<<<<<<<<<<<<<<");
                    Console.WriteLine("");
                    Console.WriteLine("Pressione Enter para encerrar o jogo.");
                    Console.ReadLine(); // Aguarda o usuário pressionar Enter
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Uia rapaz, pelo jeito temos um EMPATE!");
                    Console.WriteLine("");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>> Muito Obrigado Por Jogar! <<<<<<<<<<<<<<<<<<<<<<");
                    Console.WriteLine("");
                    Console.WriteLine("Pressione Enter para encerrar o jogo.");
                    Console.ReadLine(); // Aguarda o usuário pressionar Enter
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Pressione Enter para encerrar o jogo.");
            Console.ReadLine(); // Aguarda o usuário pressionar Enter
        }
    }
}
