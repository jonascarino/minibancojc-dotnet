//Console.WriteLine("Hello, World!");
using System;
using System.Collections.Generic;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Services;

namespace MiniBanco.ConsoleApp
{
    class Program
    {
        private static List<Conta> _contas = new List<Conta>();
        private static MotorAnalise _motorAnalise = new MotorAnalise();

        static void Main(string[] args)
        {
            bool executar = true;

            while (executar)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("====================================================================");
                    Console.WriteLine("                  MINIBANCO .NET - MENU PRINCIPAL                   ");
                    Console.WriteLine("====================================================================");
                    Console.WriteLine("1 - Abrir Nova Conta");
                    Console.WriteLine("2 - Registrar Movimentação (Depósito / Saque)");
                    Console.WriteLine("3 - Aplicar Fechamento Mensal");
                    Console.WriteLine("4 - Exibir Relatório das Contas");
                    Console.WriteLine("0 - Sair");
                    Console.WriteLine("--------------------------------------------------------------------");
                    Console.Write("Escolha uma opção: ");
                    
                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            MenuAbrirConta();
                            break;
                        case "2":
                            MenuRegistrarMovimentacao();
                            break;
                        case "3":
                            MenuFechamentoMensal();
                            break;
                        case "4":
                            MenuExibirRelatorio();
                            break;
                        case "0":
                            executar = false;
                            Console.WriteLine("\nSistema encerrado. Obrigado por utilizar o MiniBanco .NET!");
                            break;
                        default:
                            Console.WriteLine("\n⚠️ Opção inválida! Escolha um número de 0 a 4.");
                            AguardarTecla();
                            break;
                    }
                }
                catch (SaldoInsuficienteException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n❌ ERRO DE SALDO: {ex.Message}");
                    Console.ResetColor();
                    AguardarTecla();
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n❌ ERRO DE ARGUMENTO: {ex.Message}");
                    Console.ResetColor();
                    AguardarTecla();
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ ERRO DE FORMATO: Entrada inválida. Esperava-se um valor numérico.");
                    Console.ResetColor();
                    AguardarTecla();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n❌ ERRO INESPERADO: {ex.Message}");
                    Console.ResetColor();
                    AguardarTecla();
                }
            }
        }

        private static void MenuAbrirConta()
        {
            Console.Clear();
            Console.WriteLine(">>> ABRIR NOVA CONTA <<<");
            Console.WriteLine("1 - Conta Corrente");
            Console.WriteLine("2 - Conta Poupança");
            Console.WriteLine("3 - Conta Salário");
            Console.Write("Selecione o tipo de conta: ");
            string tipo = Console.ReadLine();

            if (tipo != "1" && tipo != "2" && tipo != "3")
            {
                throw new ArgumentException("Tipo de conta inválido.");
            }

            Console.Write("Digite o número da conta (inteiro): ");
            int numero = int.Parse(Console.ReadLine());

            foreach (var c in _contas)
            {
                if (c.Numero == numero)
                    throw new ArgumentException($"Já existe uma conta registrada com o número {numero}.");
            }

            Console.Write("Digite o nome do titular: ");
            string titular = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(titular))
                throw new ArgumentException("O nome do titular não pode ser vazio.");

            Console.Write("Digite o saldo inicial (R$): ");
            decimal saldoInicial = decimal.Parse(Console.ReadLine());

            Conta novaConta = null;

            if (tipo == "1")
            {
                Console.Write("Digite o limite do cheque especial (R$): ");
                decimal limite = decimal.Parse(Console.ReadLine());
                novaConta = new ContaCorrente(numero, titular, saldoInicial, limite);
            }
            else if (tipo == "2")
            {
                Console.Write("Digite a taxa de rendimento mensal (%): ");
                double taxa = double.Parse(Console.ReadLine());
                Console.Write("Digite o dia do aniversário da conta: ");
                int aniversario = int.Parse(Console.ReadLine());
                novaConta = new ContaPoupanca(numero, titular, saldoInicial, taxa, aniversario);
            }
            else if (tipo == "3")
            {
                Console.Write("Digite o nome da empresa empregadora: ");
                string empregador = Console.ReadLine();
                Console.Write("Digite o limite de saques gratuitos no mês: ");
                int limiteSaques = int.Parse(Console.ReadLine());
                novaConta = new ContaSalario(numero, titular, saldoInicial, empregador, limiteSaques);
            }

            _contas.Add(novaConta);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Conta aberta com sucesso!");
            Console.ResetColor();
            AguardarTecla();
        }

        private static void MenuRegistrarMovimentacao()
        {
            Console.Clear();
            Console.WriteLine(">>> REGISTRAR MOVIMENTAÇÃO <<<");
            Console.Write("Digite o número da conta: ");
            int numero = int.Parse(Console.ReadLine());

            Conta conta = BuscarContaManual(numero);

            if (conta == null)
            {
                Console.WriteLine("\n⚠️ Conta não localizada no sistema.");
                AguardarTecla();
                return;
            }

            Console.WriteLine($"Conta localizada! Titular: {conta.Titular} | Saldo: R$ {conta.Saldo}");
            Console.WriteLine("1 - Depósito");
            Console.WriteLine("2 - Saque");
            Console.Write("Escolha a operação: ");
            string operacao = Console.ReadLine();

            Console.Write("Digite o valor da operação (R$): ");
            decimal valor = decimal.Parse(Console.ReadLine());

            if (operacao == "1")
            {
                Console.Write("Digite uma descrição para o depósito: ");
                string desc = Console.ReadLine();
                conta.Depositar(valor, string.IsNullOrWhiteSpace(desc) ? "Depósito em dinheiro" : desc);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ Depósito realizado com sucesso!");
                Console.ResetColor();
            }
            else if (operacao == "2")
            {
                conta.Sacar(valor);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ Saque realizado com sucesso!");
                Console.ResetColor();
            }
            else
            {
                throw new ArgumentException("Operação inválida escolhida.");
            }

            AguardarTecla();
        }

        private static void MenuFechamentoMensal()
        {
            Console.Clear();
            Console.WriteLine(">>> APLICANDO FECHAMENTO MENSAL <<<");

            if (_contas.Count == 0)
            {
                Console.WriteLine("\n⚠️ Nenhuma conta ativa para aplicar o fechamento.");
                AguardarTecla();
                return;
            }

            foreach (var conta in _contas)
            {
                conta.AplicarFechamentoMensal();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Fechamento mensal aplicado com sucesso a todas as contas!");
            Console.ResetColor();
            AguardarTecla();
        }

        private static void MenuExibirRelatorio()
        {
            Console.Clear();
            if (_contas.Count == 0)
            {
                Console.WriteLine("====================================================================");
                Console.WriteLine("            MINIBANCO .NET - RELATÓRIO GERAL DE CONTAS              ");
                Console.WriteLine("====================================================================");
                Console.WriteLine("\n⚠️ Nenhuma conta aberta até o momento.");
                Console.WriteLine("====================================================================");
                AguardarTecla();
                return;
            }

            Console.WriteLine("====================================================================");
