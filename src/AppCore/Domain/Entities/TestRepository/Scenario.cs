﻿using System.Collections.Generic;
using System.Linq;

namespace AppCore.Domain.Entities.TestRepository
{
    public class Scenario : TaggableEntity
    {
        public static Scenario Factory(string description, Project project, Feature feature)
        {
            if (project == null)
            {
                return null;
            }
            var scenario = new Scenario(description, project);
            if (feature != null)
            {
                scenario.Feature = feature;
            }
            return scenario;
        }

        private Scenario()
        {}

        private Scenario(string Description, Project project)
        {
            this.Description = Description;
            Project = project;
        }
        
        public Project Project { get; set; }
        public Feature Feature { get; set; }
        public string Description { get; private set; }
        public List<Step> StepsList { get; private set; } = new List<Step>();
        public void UpdateInfo(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new System.ArgumentException($"'{nameof(description)}' cannot be null or empty", nameof(description));
            }
            this.Description = description;
        }

        /// <summary>
        /// replace current list with new list
        /// </summary>
        /// <param name="steps">list of steps</param>
        public void AddSteps(List<Step> steps)
        {
            StepsList = steps;
        }
    }
}
