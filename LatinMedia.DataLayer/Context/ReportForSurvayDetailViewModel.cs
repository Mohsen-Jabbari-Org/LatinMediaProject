using System.ComponentModel.DataAnnotations;

namespace LatinMedia.DataLayer.Context
{
    public class ReportForSurvayDetailViewModel
    {
        [Key]
        public int TeacherId { get; set; }
        public string TeacherFullName { get; set; }
        public int Poll1_1 { get; set; }
        public int Poll1_2 { get; set; }
        public int Poll1_3 { get; set; }
        public int Poll1_4 { get; set; }
        public int Poll2_1 { get; set; }
        public int Poll2_2 { get; set; }
        public int Poll2_3 { get; set; }
        public int Poll2_4 { get; set; }
        public int Poll3_1 { get; set; }
        public int Poll3_2 { get; set; }
        public int Poll3_3 { get; set; }
        public int Poll3_4 { get; set; }
        public int Poll4_1 { get; set; }
        public int Poll4_2 { get; set; }
        public int Poll4_3 { get; set; }
        public int Poll4_4 { get; set; }
        public int Poll5_1 { get; set; }
        public int Poll5_2 { get; set; }
        public int Poll5_3 { get; set; }
        public int Poll5_4 { get; set; }
        public int Poll6_1 { get; set; }
        public int Poll6_2 { get; set; }
        public int Poll6_3 { get; set; }
        public int Poll6_4 { get; set; }
        public int UserCount { get; set; }
    }

    public class ReportForSurvayDetailTeachersViewModel
    {
        [Key]
        public int AcademyId { get; set; }
        public string AcademyFullName { get; set; }
        public int Poll1_1 { get; set; }
        public int Poll1_2 { get; set; }
        public int Poll1_3 { get; set; }
        public int Poll1_4 { get; set; }
        public int Poll2_1 { get; set; }
        public int Poll2_2 { get; set; }
        public int Poll2_3 { get; set; }
        public int Poll2_4 { get; set; }
        public int Poll3_1 { get; set; }
        public int Poll3_2 { get; set; }
        public int Poll3_3 { get; set; }
        public int Poll3_4 { get; set; }
        public int Poll4_1 { get; set; }
        public int Poll4_2 { get; set; }
        public int Poll4_3 { get; set; }
        public int Poll4_4 { get; set; }
        public int Poll5_1 { get; set; }
        public int Poll5_2 { get; set; }
        public int Poll5_3 { get; set; }
        public int Poll5_4 { get; set; }
        public int Poll6_1 { get; set; }
        public int Poll6_2 { get; set; }
        public int Poll6_3 { get; set; }
        public int Poll6_4 { get; set; }
        public int UserCount { get; set; }
    }

    public class AcademiesTeacher
    {
        [Key]
        public int AcademyId { get; set; }
    }

    public class CommentsTeacher
    {
        [Key]
        public int SurvayId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Age { get; set; }
        public string AcademyFullName { get; set; }
        public string Comment { get; set; }
    }
}