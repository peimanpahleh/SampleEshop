namespace Orders.Infrastructure.Persistence.Mongo;

public static class MongoQueryByPage
{
    public static async Task<(
    long totalItems,
    int totalPages,
    IReadOnlyList<TDocument> data)>
    AggregateByPage<TDocument>(
    this IMongoCollection<TDocument> collection,
    FilterDefinition<TDocument> filterDefinition,
    SortDefinition<TDocument> sortDefinition,
    int page,
    int pageSize)
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 5;

        var countFacet = AggregateFacet.Create("count",
            PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
            {
                        PipelineStageDefinitionBuilder.Count<TDocument>()
            }));

        var dataFacet = AggregateFacet.Create("data",
            PipelineDefinition<TDocument, TDocument>.Create(new[]
            {
                        PipelineStageDefinitionBuilder.Sort(sortDefinition),
                        PipelineStageDefinitionBuilder.Skip<TDocument>((page - 1) * pageSize),
                        PipelineStageDefinitionBuilder.Limit<TDocument>(pageSize),
            }));


        var aggregation = await collection.Aggregate()
            .Match(filterDefinition)
            .Facet(countFacet, dataFacet)
            .ToListAsync();

        var count = aggregation.First()
            .Facets.First(x => x.Name == "count")
            .Output<AggregateCountResult>()
            ?.FirstOrDefault()
            ?.Count;

        if (count == null)
            return (0, 0, new List<TDocument>());

        var totalPages = (int)Math.Ceiling((double)count / pageSize);

        var data = aggregation.First()
            .Facets.First(x => x.Name == "data")
            .Output<TDocument>();

        return (count.Value, totalPages, data);
    }
}
