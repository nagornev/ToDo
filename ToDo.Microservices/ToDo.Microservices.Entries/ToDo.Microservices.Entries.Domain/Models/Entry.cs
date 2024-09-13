using System;
using ToDo.Domain.Results;


namespace ToDo.Microservices.Entries.Domain.Models
{
    public class Entry
    {
        private Entry(Guid id,
                      Guid categoryId,
                      string text,
                      DateTime? deadline,
                      bool completed)
        {
            Id = id;
            CategoryId = categoryId;
            Text = text;
            Deadline = deadline;
            Completed = completed;
        }

        public Guid Id { get; private set; }

        public Guid CategoryId { get; private set; }

        public string Text { get; private set; }

        public DateTime? Deadline { get; private set; }

        public bool Completed { get; private set; }

        public static Entry Constructor(Guid id,
                                        Guid categoryId,
                                        string text,
                                        DateTime? deadline,
                                        bool completed)
        {


            return new Entry(id, categoryId, text, deadline, completed);
        }

        //    public static Entry Create(Guid id,
        //                               Guid categoryId,
        //                               string text,
        //                               DateTime? deadline,
        //                               bool completed)
        //    {
        //        if (id == Guid.Empty)
        //            return Result<Entry>.Failure(Errors.IsNull("The entry id can not be empty"));

        //        if (categoryId == Guid.Empty)
        //            return Result<Entry>.Failure(Errors.IsNull("The category id can not be empty"));

        //        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
        //            return Result<Entry>.Failure(Errors.IsNull("The entry text can not be null or empty."));

        //        if(deadline != null && deadline < DateTime.Now)
        //            return Result<Entry>.Failure(Errors.IsInvalidArgument("The deadline can not bee"))
        //    }
    }
    
}
