# MiniBanco.ConsoleApp - Motor de Gestão de Contas Bancárias

Este projeto consiste em uma **Engine (Motor de Processamento) Back-End** desenvolvida em C# via Console Application para o ecossistema .NET Core. O objetivo do sistema é simular a gestão de contas bancárias de uma fintech, aplicando regras de negócios complexas como fechamentos mensais automatizados, cálculo do Índice de Saúde Financeira de contas e geração de relatórios de recomendações críticas para a equipe de gerência de relacionamento.

---

## 📽️ Demonstração e Apresentação em Vídeo

O vídeo explicativo detalhando o funcionamento prático no terminal, arquitetura de software adotada e decisões de Orientação a Objetos está disponível em:
👉 **[Clique aqui para assistir ao vídeo de apresentação](https://youtu.be/R-mCNPR8vWI)**

---

## 🛠️ Como Executar o Projeto (Passo a Passo)

### Pré-requisitos
* Ter o **.NET SDK 8.0** (ou superior) instalado em sua máquina.

### Execução do Sistema
1. Abra o terminal do seu sistema operacional na pasta raiz do projeto (`minibancojs-dotnet`).
2. Execute o comando de inicialização automática do .NET:
   ```bash
   dotnet run
   ```

---

## 🧠 Regras de Negócio e Engenharia Adotada

### 📊 Cálculo do Índice de Saúde da Conta
O cálculo de saúde utiliza uma tabela de pontuação com pesos estritos baseados na natureza regulatória de cada movimentação:
* **Depósito:** 10 pontos (Entrada saudável de capital)
* **Saque:** 5 pontos (Saída de fluxo que exige acompanhamento)
* **Tarifa:** 0 pontos (Despesa acessória que compromete a integridade da conta)

A fórmula matemática para a extração do percentual de integridade é:
Percentual (%) = (Pontuação Obtida / Pontuação Máxima Possível) × 100

**Por que essa regra foi adotada?** 
A abordagem visa isolar contas com excesso de cobranças de tarifas ou saques desmedidos. Para evitar o bug clássico de divisão inteira (*truncamento para zero*), foi aplicado o operador de `cast (double)` explícito antes do cálculo do quociente. Contas novas sem transações registradas possuem tratamento de contingência evitando erros de `DivideByZeroException`.

### 🔍 Critérios para Recomendações à Gerência
O relatório percorre o extrato aplicando uma triagem categórica clássica:
1. **Itens Críticos (Tarifas):** Alerta a gerência sobre a necessidade de isenções tarifárias ou migração para pacotes mais econômicos para reter o cliente.
2. **Itens de Atenção (Saques):** Caso a conta seja uma *Conta Corrente* operando com o saldo negativo, o motor detecta o uso do limite do cheque especial e emite um gatilho de contato urgente para renegociação de crédito.

---

## 🏗️ Conceitos de POO Aplicados no Código

* **Encapsulamento:** A propriedade `Saldo` dentro da classe base `Conta` possui modificador `{ get; protected set; }`. Isso blinda o estado financeiro contra manipulações externas indevidas, permitindo alteração exclusiva por métodos de negócios autorizados (`Depositar` e `Sacar`).
* **Herança:** As subclasses `ContaCorrente`, `ContaPoupanca` e `ContaSalario` herdam propriedades centrais da classe abstrata `Conta`, reaproveitando a assinatura estrutural básica e estendendo atributos próprios de cada nicho (como taxas e limites).
* **Polimorfismo:** Demonstrado extensivamente na chamada do método `AplicarFechamentoMensal()`. O loop principal itera sobre uma coleção genérica `List<Conta>` e executa o fechamento personalizado de cada subclasse em tempo de execução sem necessitar realizar conversões de tipo ou condicionais complexas.
* **Varredura sem LINQ:** Conforme restrições estritas do projeto, todas as buscas manuais de contas e filtragens de pendências críticas foram orquestradas por laços `foreach` e condicionais tradicionais, consolidando a lógica estrutural pura.

---

## 🤖 Uso de Inteligência Artificial (IA)

Ferramentas de IA generativa foram adotadas exclusivamente como parceiras de code-review durante a fase de concepção da arquitetura das classes de modelo.
* **O que foi validado:** Foi verificado minuciosamente se a IA não introduziria cláusulas de sintaxe baseadas em LINQ na camada de filtragem de relatórios. O algoritmo gerado foi inspecionado linha por linha para assegurar o correto lançamento das exceções personalizadas no escopo correto dos métodos polimórficos de saque.

---

## 🔀 Organização de Branches e Histórico Git

O projeto seguiu o fluxo de trabalho profissional baseado em branches de isolamento para manter a estabilidade da ramificação de produção:
* `main`: Branch de entrega final, estável e testada.
* `feature/motor-e-ajustes`: Branch de desenvolvimento paralelo onde as validações da classe de transação, exceções customizadas e o motor analítico foram integrados antes do merge final.