namespace ToDo.Microservices.Entries.Querers.Queries.Categories
{
    public class GetCategoryCompiler : QueryCompiler
    {
        private Guid _id;

        public GetCategoryCompiler(string url, Guid id)
            : base(url)
        {
            _id = id;
        }

        protected override void SetMethod(MethodCompiler compiler)
        {
            compiler.Set(HttpMethod.Get);
        }

        protected override void SetUrl(UrlCompiler compiler)
        {
            compiler.Set($"{Url}/{_id}");
        }
    }
}
