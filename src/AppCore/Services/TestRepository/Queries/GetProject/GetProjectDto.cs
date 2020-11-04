using System;

namespace AppCore.Services.TestRepository.Queries
{
    public class GetProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
