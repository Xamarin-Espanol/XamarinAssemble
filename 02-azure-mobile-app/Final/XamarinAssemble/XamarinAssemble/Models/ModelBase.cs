using System;

namespace XamarinAssemble.Models
{
    public class ModelBase 
    {
        public ModelBase()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDateTime = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
