using System;
using MiniBanco.ConsoleApp.Models;

namespace MiniBanco.ConsoleApp.Services
{
    public class MotorAnalise
    {
        public void ProcessarEExibirRelatorio(Conta conta)
        {
            int pontuacaoObtida = 0;
            int totalTransacoes = 0;

            foreach (var t in conta.Extrato)
            {
                totalTransacoes++;
                if (t.Tipo == "Deposito") pontuacaoObtida += 10;
                else if (t.Tipo == "Saque") pontuacaoObtida += 5;
                else if (t.Tipo == "Tarifa") pontuacaoObtida += 0;
            }

            int pontuacaoMaxima = totalTransacoes * 10;
            double percentualSaude = 0;

            if (pontuacaoMaxima > 0)
                percentualSaude = ((double)pontuacaoObtida / pontuacaoMaxima) * 100;

            string classificacao = "";
            string acaoCorporativa = "";

            if (percentualSaude >= 90)
            {
                classificacao = "[ CONTA SAUDÁVEL ]";
                acaoCorporativa = "Oferecer investimentos e avaliar aumento de limite.";
            }
            else if (percentualSaude >= 60)
            {
                classificacao = "[ CONTA EM ATENÇÃO ]";
                acaoCorporativa = "Contato da gerência para orientação financeira.";
            }
            else
            {
                classificacao = "[ CONTA EM RISCO ]";
                acaoCorporativa = "Bloquear aumento de limite e abrir analise de credito.";
            }

            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine($"> DADOS DA CONTA: {conta.GetType().Name} | Número: {conta.Numero} | Titular: {conta.Titular}");
            Console.WriteLine($"  - Saldo Atual: R$ {conta.Saldo}");
            Console.WriteLine($"> MOVIMENTAÇÕES REGISTRADAS ({totalTransacoes} TRANSAÇÕES):");

            foreach (var t in conta.Extrato)
            {
                int pts = t.Tipo == "Deposito" ? 10 : (t.Tipo == "Saque" ? 5 : 0);
                Console.WriteLine($"  [{t.Tipo}] {t.Descricao,-25} -> R$ {t.Valor} ({pts} pts)");
            }

            Console.WriteLine($"> ÍNDICE DE SAÚDE: {percentualSaude:F1}% | Classificação: {classificacao}");
            Console.WriteLine($"> RECOMENDAÇÃO DA GERÊNCIA: {acaoCorporativa}");
        }
    }
}