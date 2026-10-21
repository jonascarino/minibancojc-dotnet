namespace MiniBanco.ConsoleApp.Models
{
    public class ContaPoupanca : Conta
    {
        public double TaxaRendimentoMensal { get; }
        public int DiaAniversario { get; }

        public ContaPoupanca(int numero, string titular, decimal saldoInicial, double taxa, int aniversario) 
            : base(numero, titular, saldoInicial)
        {
            TaxaRendimentoMensal = taxa;
            DiaAniversario = aniversario;
        }

        public override void AplicarFechamentoMensal()
        {
            decimal rendimento = Saldo * (decimal)(TaxaRendimentoMensal / 100.0);
            Saldo += rendimento;
            RegistrarTransacao("Rendimento mensal poupança", "Deposito", rendimento);
        }
    }
}