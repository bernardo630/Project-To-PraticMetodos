# AdvancedCalculatorProject

Este projecto apresenta uma implementação completa e avançada de uma calculadora científica em C#, incluindo operações básicas, cálculo avançado de derivadas, integrais, estatísticas descritivas, manipulação de matrizes, soluções numéricas de equações e geração de sequências matemáticas. O código foi estruturado para servir como referência de boas práticas, uso de genéricos, padrões de design, programação funcional, recursão, cálculos assíncronos e algoritmos numéricos.

---

## Visão Geral

O **AdvancedCalculatorProject** foi desenvolvido com o objetivo de demonstrar conceitos avançados da linguagem C#, incluindo:

* Programação orientada a interfaces
* Genéricos e métodos genéricos com múltiplos parâmetros tipo
* Operações matemáticas avançadas
* Recursividade
* Manipulação de matrizes com operações sobrecarregadas
* Estatísticas descritivas completas
* Algoritmos numéricos (derivadas, integrais e resolução de equações)
* Programação assíncrona com `async` e `await`
* Extensão de funcionalidades através de métodos de extensão
* Estrutura modular e orientada a boas práticas

---

## Funcionalidades Principais

### Operações Básicas

* Adição
* Subtração
* Multiplicação
* Divisão
* Potência
* Logaritmo

Todas as operações implementadas através de `ICalculatorOperation<T>` permitindo flexibilidade e expansão futura.

---

### Operações Matemáticas Avançadas

* Cálculo numérico de derivadas
* Integração usando a Regra de Simpson
* Resolução de equações usando Newton-Raphson
* Cálculo de raízes de polinômios (suporte total para equações quadráticas)
* Geração de sequências com filtros dinâmicos
* Cálculo assíncrono para operações demoradas

---

### Estatísticas Descritivas

A classe `DescriptiveStatistics` fornece:

* Média
* Mediana
* Desvio padrão
* Mínimo
* Máximo
* Contagem

Com suporte usando métodos de extensão adicionais, como `Median`, `StandardDeviation` e `MovingAverage`.

---

### Operações com Matrizes

A classe genérica `Matrix<T>` oferece:

* Criação de matrizes dinâmicas
* Indexação personalizada
* Soma e subtração usando sobrecarga de operadores
* Multiplicação de matrizes
* Validação automática de dimensões
* Impressão formatada via `PrintMatrix()`

---

### Sequências Numéricas

O projeto inclui:

* Geração de sequências personalizadas
* Filtros funcionais configuráveis
* Sequência de Fibonacci utilizando `BigInteger`

---

### Programação Assíncrona

O método `CalculateAsync` permite executar cálculos demorados em segundo plano usando `Task.Run`.

---

## Estrutura Geral do Projeto

* `ICalculatorOperation<T>`
  Interface para operações matemáticas básicas.

* `IAdvancedMathOperations`
  Interface que define operações matemáticas mais complexas.

* `Matrix<T>`
  Estrutura genérica para manipulação de matrizes.

* `CalculatorExtensions`
  Métodos de extensão para funcionalidades adicionais.

* `AdvancedCalculator`
  Implementação principal da lógica da calculadora.

* `DescriptiveStatistics`
  Estrutura auxiliar para resultados estatísticos.

* `Program`
  Classe principal contendo demonstrações completas de todas as funcionalidades.

---

## Demonstração

O método `Main` demonstra:

1. Operações básicas e transformações matemáticas
2. Derivadas, integrais e resolução numérica de equações
3. Multiplicação de matrizes
4. Geração de sequências filtradas
5. Cálculos assíncronos
6. Estatísticas descritivas
7. Sequência de Fibonacci

---

## Requisitos

* .NET 6.0 ou superior
* Sistema operacional Windows, Linux ou macOS
* Qualquer IDE compatível com C#, como:

  * Visual Studio
  * Rider
  * VS Code (com extensão C#)

---

## Como Executar

Clone o repositório:

```bash
git clone https://github.com/bernardo630/Project-To-PraticMetodos.git
```

Entre no diretório:

```bash
cd Project-To-PraticMetodos
```

Compile:

```bash
dotnet build
```

Execute:

```bash
dotnet run
```

---

## Objetivos Educacionais

Este projeto serve como base para estudos nos seguintes temas:

* Estruturas de dados matemáticas
* Algoritmos numéricos
* Boas práticas em C#
* Design orientado a interfaces
* Manipulação de matrizes
* Funções e expressões lambda
* Padrões funcionais
* Recursividade
* Operações em larga escala usando `BigInteger`
* Programação assíncrona e paralelismo

---

## Possíveis Extensões Futuras

* Implementação de matrizes esparsas
* Cálculo simbólico
* Sistema de plugin para novas operações
* Suporte para gráficos e visualizações
* Implementação de métodos numéricos avançados (Runge-Kutta, interpolação, regressão linear)

---
