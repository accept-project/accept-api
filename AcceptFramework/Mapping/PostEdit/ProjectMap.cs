using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using AcceptFramework.Domain.PostEdit;

namespace AcceptFramework.Mapping.PostEdit
{
    public class ProjectMap:ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(e => e.Id);
            Map(e => e.Name).Length(250);
            Map(e => e.Status);           
            References(e => e.SourceLanguage).Not.LazyLoad().Column("SourceLanguageId");
            References(e => e.TargetLanguage).Not.LazyLoad().Column("TargetLanguageID");
            //should be of value 1 when monolingual(no source is shown) and of value 2 when bilingual(source is shown).
            Map(e => e.InterfaceConfiguration);
            Map(e => e.ProjectOwner);
            Map(e => e.CreationDate);
            Map(e => e.EmailBodyMessage).Length(2500);
            //should be of value 1 when to display translation options and of value 2 when not to show them.
            Map(e => e.TranslationOptions);
            Map(e => e.SurveyLink).Length(2500);
            Map(e => e.ProjectOrganization);
            //should be of value 0 when the source cannot be consulted and of value 1 when the source can be displayed.
            Map(e => e.CustomInterfaceConfiguration);
            //is it an external project?
            Map(e => e.External).Column("IsExternal");
            //project admin token is used as a validation parameter within api calls.
            Map(e => e.AdminToken).Column("AdminToken").Length(250);
            Map(e => e.MaxThreshold);
            Map(e => e.MaxIdleThreshold);
            //collaborative revision, multiple users are working over an unique project task revision.
            Map(e => e.IsSingleRevision);
            Map(e => e.ResetBlockTimeStampWhenSameUser).Column("ResetTimeStamp");
            //should be of value 0 if is disabled and 1 if enabled.
            Map(e => e.ParaphrasingMode);
            //should be of value 0 if is disabled and 1 if enabled.
            Map(e => e.InteractiveCheck);
            //can keep any related metadata.
            Map(e => e.InteractiveCheckMetadata).Length(2500);
            //can keep any related metadata.
            Map(e => e.ParaphrasingMetadata).Length(2500);
            
            //project domain.
            References(e => e.ProjectDomain).Not.LazyLoad().Column("DomainId");

            //project documents.
            HasManyToMany(e => e.ProjectDocuments).Cascade.All().
            LazyLoad().
            AsSet().
            Table("ProjectDocuments").
            ParentKeyColumn("ProjectID").
            ChildKeyColumn("DocumentID");

            //project questions.
            HasManyToMany(e => e.ProjectQuestion).Cascade.AllDeleteOrphan().
            Not.LazyLoad().
            AsSet().
            Table("ProjectQuestions").
            ParentKeyColumn("ProjectID").
            ChildKeyColumn("QuestionID");

            //project options.
            HasManyToMany(e => e.ProjectEditOptions).Cascade.AllDeleteOrphan().
            Not.LazyLoad().
            AsSet().
            Table("ProjectPostEditOptions").
            ParentKeyColumn("ProjectID").
            ChildKeyColumn("PostEditOptionID");
       
        }            
    }
}
