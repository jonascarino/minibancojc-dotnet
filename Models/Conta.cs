using System;
using System.Collections.Generic;

namespace MiniBanco.ConsoleApp.Models
{
    public abstract class Conta
    {
        public int Numero { get; }
        public string Titular { get; }
        public decimal Saldo { get; protected set; } 
        public List<Transacao> Extrato { get; }

        public Conta(int numero, string titular, decimal saldoInicial)
        {
            this.Numero = numero;
            this.Titular = titular;
            this.Saldo = saldoInicial;
            this.Extrato = new List<Transacao>();
        }

        public void RegistrarTransacao(string descricao, string tipo, decimal valor)
        {
            Transacao t = new Transacao(descricao, tipo, valor);
            Extrato.Add(t);
        }

        public void Depositar(decimal valor, string descricao)
        {
            if (valor <= 0) throw new ArgumentException("O valor do depósito deve ser maior que zero.");
            Saldo += valor;
            RegistrarTransacao(descricao, "Deposito", valor);
        }

        public virtual void Sacar(decimal valor)
        {
            if (valor <= 0) throw new ArgumentException("O valor do saque deve ser maior que zero.");
            Saldo -= valor;
            RegistrarTransacao("Saque em espécie", "Saque", valor);
        }

        public abstract void AplicarFechamentoMensal(); 
    }
}