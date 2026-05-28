namespace Proge.BlazorApp
{
    public class Result
    {
        public Dictionary<string, List<string>> Errors { get; set; }

        public Result()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public bool HasError => Errors.Count > 0;

        public void AddError(string propertyName, string errorMessage)
        {
            if (!Errors.ContainsKey(propertyName))
                Errors.Add(propertyName, new List<string>());
            Errors[propertyName].Add(errorMessage);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }
    }
}