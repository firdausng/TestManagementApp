using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Services.TestRepository.Queries.GetFeature
{
    public class GetFeatureDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
