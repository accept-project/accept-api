using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Repository.PostEdit
{
    internal static class ProjectQuestionRepository
    {

        public static IEnumerable<ProjectQuestion> GetAll()
        {
            return new RepositoryBase<ProjectQuestion>().Select();
        }

        public static ProjectQuestion Insert(ProjectQuestion record)
        {
            return new RepositoryBase<ProjectQuestion>().Create(record);
        }


        public static ProjectQuestion GetProjectQuestion(int projectQuestionId)
        {
            return new RepositoryBase<ProjectQuestion>().Select(a => a.Id == projectQuestionId).FirstOrDefault();

        }

        public static ProjectQuestion UpdateProjectQuestion(ProjectQuestion projectQuestion)
        {
            return new RepositoryBase<ProjectQuestion>().Update(projectQuestion);
        }


            
    
    }
}
