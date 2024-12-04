using System;
using System.IO;

class ProgramaIMC
{
    struct Pessoa
    {
        public string Nome;
        public int Idade;
        public double Peso;
        public double Altura;
        public string Sexo;
    }

    static double CalcularIMC(double peso, double altura)
    {
        return peso / (altura * altura);
    }

    static string ClassificarIMC(double imc)
    {
        if (imc < 18.5) return "Abaixo do peso";
        if (imc >= 18.5 && imc < 24.9) return "Peso normal";
        if (imc >= 25 && imc < 29.9) return "Sobrepeso";
        return "Obesidade";
    }

    static double EstimarGorduraCorporal(double imc, int idade, string sexo)
    {
        double fatorSexo = sexo.ToLower() == "masculino" ? 1 : 0;
        return (1.20 * imc) + (0.23 * idade) - (10.8 * fatorSexo) - 5.4;
    }

    static void SugerirDietaEAtividade(double imc, string objetivo)
    {
        double caloriasRecomendadas = 2000;
        double carbCal, protCal, gordCal;
        double carbGramas, protGramas, gordGramas;

        Console.WriteLine("\n--- Recomendações para seu objetivo ---");

        switch (objetivo.ToLower())
        {
            case "ganhar massa":
                caloriasRecomendadas = 2800;
                carbCal = caloriasRecomendadas * 0.50;
                protCal = caloriasRecomendadas * 0.30;
                gordCal = caloriasRecomendadas * 0.20;
                Console.WriteLine("- Foco em treinos de força e consumo elevado de proteínas.");
                break;

            case "perder gordura":
                caloriasRecomendadas = 1600;
                carbCal = caloriasRecomendadas * 0.35;
                protCal = caloriasRecomendadas * 0.40;
                gordCal = caloriasRecomendadas * 0.25;
                Console.WriteLine("- Priorize exercícios aeróbicos e um déficit calórico.");
                break;

            case "manter saudável":
            default:
                caloriasRecomendadas = 2000;
                carbCal = caloriasRecomendadas * 0.50;
                protCal = caloriasRecomendadas * 0.25;
                gordCal = caloriasRecomendadas * 0.25;
                Console.WriteLine("- Mantenha exercícios regulares e uma alimentação equilibrada.");
                break;
        }

        carbGramas = carbCal / 4;
        protGramas = protCal / 4;
        gordGramas = gordCal / 9;

        Console.WriteLine("\nSugestão de dieta:");
        Console.WriteLine($"Carboidratos: {carbGramas:F2}g");
        Console.WriteLine($"Proteínas: {protGramas:F2}g");
        Console.WriteLine($"Gorduras: {gordGramas:F2}g");
    }

    static void SalvarDados(Pessoa pessoa, double imc, string classificacao, double gorduraCorporal, string objetivo)
    {
        string fileName = $"{pessoa.Nome}.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Nome: {pessoa.Nome}");
                writer.WriteLine($"Idade: {pessoa.Idade}");
                writer.WriteLine($"Sexo: {pessoa.Sexo}");
                writer.WriteLine($"Peso: {pessoa.Peso} kg");
                writer.WriteLine($"Altura: {pessoa.Altura} m");
                writer.WriteLine($"IMC: {imc:F2}");
                writer.WriteLine($"Classificação do IMC: {classificacao}");
                writer.WriteLine($"Porcentagem de Gordura Corporal: {gorduraCorporal:F2}%");
                writer.WriteLine($"Objetivo: {objetivo}");
            }
            Console.WriteLine($"\nAs informações foram salvas no arquivo \"{fileName}\".");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar o arquivo: {ex.Message}");
        }
    }

    static void CarregarDados(string nome)
    {
        string fileName = $"{nome}.txt";
        if (File.Exists(fileName))
        {
            Console.WriteLine($"\n--- Informações de {nome} ---");
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()!) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        else
        {
            Console.WriteLine("\nArquivo não encontrado! Certifique-se de ter salvo as informações anteriormente.");
        }
    }

    static void Main(string[] args)
    {
        Pessoa pessoa;

        Console.Write("Digite seu nome: ");
        pessoa.Nome = Console.ReadLine()!;

        Console.Write("Digite sua idade: ");
        pessoa.Idade = int.Parse(Console.ReadLine()!);

        Console.Write("Digite seu sexo (Masculino/Feminino): ");
        pessoa.Sexo = Console.ReadLine()!;

        Console.Write("Digite seu peso (kg): ");
        pessoa.Peso = double.Parse(Console.ReadLine()!);

        Console.Write("Digite sua altura (m): ");
        pessoa.Altura = double.Parse(Console.ReadLine()!);

        double imc = CalcularIMC(pessoa.Peso, pessoa.Altura);
        string classificacao = ClassificarIMC(imc);
        double gorduraCorporal = EstimarGorduraCorporal(imc, pessoa.Idade, pessoa.Sexo);

        Console.WriteLine($"\n{pessoa.Nome}, seu IMC é: {imc:F2}");
        Console.WriteLine($"Classificação do IMC: {classificacao}");
        Console.WriteLine($"Porcentagem de Gordura Corporal Estimada: {gorduraCorporal:F2}%");

        Console.WriteLine("\nQual é o seu objetivo?");
        Console.WriteLine("1. Ganhar massa");
        Console.WriteLine("2. Perder gordura");
        Console.WriteLine("3. Manter-se saudável");
        Console.Write("Digite a opção (1, 2 ou 3): ");

        string escolha = Console.ReadLine()!;
        string objetivo = escolha switch
        {
            "1" => "ganhar massa",
            "2" => "perder gordura",
            "3" => "manter saudável",
            _ => "manter saudável"
        };

        SugerirDietaEAtividade(imc, objetivo);

        Console.Write("\nDeseja salvar essas informações? (Sim/Não): ");
        string salvar = Console.ReadLine()!.ToLower();

        if (salvar == "sim")
        {
            SalvarDados(pessoa, imc, classificacao, gorduraCorporal, objetivo);
        }

        Console.Write("\nDeseja carregar dados salvos? Digite 'Load + Nome' ou 'Não': ");
        string carregar = Console.ReadLine()!;
        if (carregar.StartsWith("Load", StringComparison.OrdinalIgnoreCase))
        {
            string nome = carregar.Substring(5).Trim();
            CarregarDados(nome);
        }

        Console.WriteLine("\nPrograma encerrado. Até mais!");
    }
}
