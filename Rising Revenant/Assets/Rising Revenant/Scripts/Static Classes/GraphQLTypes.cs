using UnityEngine;

public static class GraphQLTypes
{
    public class ReinforcementTradeResponse
    {
        public ModelContainer<ReinforcementTradeModel> reinforcementTradeModels { get; set; }
    }

    public class OutpostTradeResponse
    {
        public ModelContainer<OutpostTradeModel> outpostTradeModels { get; set; }
    }

    public class GameStateResponse
    {
        public ModelContainer<GameStateModel> gameStateModels { get; set; }
    }

    public class ModelContainer<T>
    {
        public Edge<T>[] Edges { get; set; }
    }

    public class Edge<T>
    {
        public Node<T> Node { get; set; }
    }

    public class Node<T>
    {
        public Entity<T> Entity { get; set; }
    }

    public class Entity<T>
    {
        public string[] Keys { get; set; }
        public T[] Models { get; set; }
    }

    public class ReinforcementTradeModel : BaseModel
    {
        public string TtradeId { get; set; }
        public string seller { get; set; }
        public string price { get; set; }
        public string status { get; set; }
        public string offer { get; set; }
    }

    public class OutpostTradeModel : BaseModel
    {
        public string tradeId { get; set; }
        public string tradeType { get; set; }
        public string buyer { get; set; }
        public string price { get; set; }
        public string seller { get; set; }
        public string status { get; set; }
        public Vec2 offer { get; set; }
    }

    public class GameStateModel : BaseModel
    {
        public string gameId { get; set; }
    }

    public class Vec2
    {
        public string x { get; set; }
        public string y { get; set; }
    }

    public class BaseModel
    {
        public string __typename { get; set; }
        public string gameId { get; set; }
    }
}
