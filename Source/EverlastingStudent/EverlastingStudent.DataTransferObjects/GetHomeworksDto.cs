namespace EverlastingStudent.DataTransferObjects
{
    using AutoMapper;

    using EverlastingStudent.Common.Infrastructure.Automapper;
    using EverlastingStudent.Models;

    public class GetHomeworksDto : IMapFrom<StudentHomework>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int EnergyCost { get; set; }

        public double KnowledgeGain { get; set; }

        public double ExperienceGain { get; set; }

        public int SolveDurabationInMinutes { get; set; }

        public bool IsSolved { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<StudentHomework, GetHomeworksDto>()
                .ForMember(m => m.Title, opt => opt.MapFrom(e => e.Homework.Title));
        }
    }
}
