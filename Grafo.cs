public class Grafo
{
    private Dictionary<string, Dictionary<string, int>> _grafo = new Dictionary<string, Dictionary<string, int>>();

    public void AdicionarAresta(string no1, string no2, int distancia)
    {
        if (!_grafo.ContainsKey(no1))
            _grafo[no1] = new Dictionary<string, int>();

        if (!_grafo.ContainsKey(no2))
            _grafo[no2] = new Dictionary<string, int>();

        _grafo[no1][no2] = distancia;
        _grafo[no2][no1] = distancia; // Grafo não direcionado
    }

    public Dictionary<string, Tuple<int, List<string>>> Dijkstra(string inicio)
    {
        var distancias = new Dictionary<string, Tuple<int, List<string>>>();
        var filaPrioridade = new SortedSet<Tuple<int, string>>();

        foreach (var no in _grafo.Keys)
            distancias[no] = no == inicio ? new Tuple<int, List<string>>(0, new List<string> { inicio }) : new Tuple<int, List<string>>(int.MaxValue, new List<string>());

        filaPrioridade.Add(new Tuple<int, string>(0, inicio));

        while (filaPrioridade.Count > 0)
        {
            var minNo = filaPrioridade.Min;
            filaPrioridade.Remove(minNo);

            var noAtual = minNo.Item2;
            var distanciaAtual = minNo.Item1;

            foreach (var vizinho in _grafo[noAtual])
            {
                var novaDistancia = distanciaAtual + vizinho.Value;
                if (novaDistancia < distancias[vizinho.Key].Item1)
                {
                    filaPrioridade.RemoveWhere(t => t.Item2 == vizinho.Key);
                    distancias[vizinho.Key] = new Tuple<int, List<string>>(novaDistancia, new List<string>(distancias[noAtual].Item2) { vizinho.Key });
                    filaPrioridade.Add(new Tuple<int, string>(novaDistancia, vizinho.Key));
                }
            }
        }

        return distancias;
    }

    public static void CalcularDistancia(Grafo grafo)
    {
        Console.Write("Digite o ponto de partida: ");
        string inicio = Console.ReadLine();

        Console.Write("Digite o ponto de chegada: ");
        string destino = Console.ReadLine();

        Dictionary<string, Tuple<int, List<string>>> result = grafo.Dijkstra(inicio);

        if (result.ContainsKey(destino))
        {
            int distancia = result[destino].Item1;
            List<string> caminho = result[destino].Item2;

            Console.WriteLine($"A distância entre {inicio} e {destino} é de {distancia} unidades.");
            Console.WriteLine("Caminho percorrido:");
            Console.WriteLine(string.Join(" -> ", caminho));
        }
        else
        {
            Console.WriteLine("Não foi possível calcular a distância entre os pontos fornecidos.");
        }
    }
}
