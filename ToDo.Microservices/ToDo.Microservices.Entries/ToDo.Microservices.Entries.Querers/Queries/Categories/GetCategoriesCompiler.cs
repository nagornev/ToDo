namespace ToDo.Microservices.Entries.Querers.Queries.Categories
{
    public class GetCategoriesCompiler : QueryCompiler
    {
        public GetCategoriesCompiler(string url)
            : base(url)
        {
        }

        protected override void SetMethod(MethodCompiler compiler)
        {
            compiler.Set(HttpMethod.Get);
        }

        protected override void SetUrl(UrlCompiler compiler)
        {
            compiler.Set(Url);
        }
    }
}
