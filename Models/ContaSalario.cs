using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaSalario : Conta
    {
        public string Empregador { get; }
        public int LimiteSaquesMensais { get; }
        private int _saquesRealizadosNoMes;

        public ContaSalario(int numero, string titular, decimal saldoInicial, string empregador, int limiteSaques) 
            : base(numero, titular, saldoInicial)
        {
            Empregador = empregador;
            LimiteSaquesMensais = limiteSaques;
            _saquesRealizadosNoMes = 0;
        }

        public override void Sacar(decimal valor)
        {
            decimal custoTransacao = 0;
            
            if (_saquesRealizadosNoMes >= LimiteSaquesMensais)
                custoTransacao = 5.00m;

            if (Saldo < (valor + custoTransacao))
                throw new SaldoInsuficienteException("Saldo insuficiente para saque com tarifa de excesso.");

            Saldo -= valor;
            RegistrarTransacao("Saque em espécie", "Saque", valor);
            _saquesRealizadosNoMes++;

            if (custoTransacao > 0)
            {
                Saldo -= custoTransacao;
                RegistrarTransacao("Tarifa de saque excedente", "Tarifa", custoTransacao);
            }
        }

        public override void AplicarFechamentoMensal()
        {
            _saquesRealizadosNoMes = 0; 
        }
    }
}