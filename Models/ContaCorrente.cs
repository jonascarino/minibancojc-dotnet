using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaCorrente : Conta
    {
        public decimal LimiteChequeEspecial { get; }

        public ContaCorrente(int numero, string titular, decimal saldoInicial, decimal limite) 
            : base(numero, titular, saldoInicial)
        {
            LimiteChequeEspecial = limite;
        }

        public override void Sacar(decimal valor)
        {
            if (Saldo + LimiteChequeEspecial < valor)
                throw new SaldoInsuficienteException("Saldo e limite de cheque especial insuficientes.");

            Saldo -= valor;
            RegistrarTransacao("Saque em conta corrente", "Saque", valor);
        }

        public override void AplicarFechamentoMensal()
        {
            Saldo -= 10.00m; 
            RegistrarTransacao("Tarifa de manutenção do banco", "Tarifa", 10.00m);
        }
    }
}