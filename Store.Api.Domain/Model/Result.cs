namespace Store.Api.Domain.Model
{
    public class Result
    {
        public bool IsSuccess
        {
            get
            {
                if (Errors != null)
                {
                    return !Errors.Any();
                }

                return false;
            }
        }

        public bool HasWarnings
        {
            get
            {
                if (Warnings != null)
                {
                    return Warnings.Any();
                }

                return false;
            }
        }

        public List<string> Errors { get; set; }

        public List<string> Warnings { get; set; }

        public Result()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            Errors.AddRange(errors);
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public void AddWarnings(IEnumerable<string> warnings)
        {
            Warnings.AddRange(warnings);
        }

        public void Merge(Result result)
        {
            Errors.AddRange(result.Errors);
            Warnings.AddRange(result.Warnings);
        }

        public string JoinErrors(string delimiter = ",")
        {
            return string.Join(delimiter, Errors);
        }

        public string JoinWarnings(string delimiter = ",")
        {
            return string.Join(delimiter, Warnings);
        }
    }
}
