namespace bazy_danych.Models
{
    public class PoliczStudentowRequest
    {
        public int MinLiczbaOcen { get; set; }
    }

    public class PoliczStudentowResponse
    {
        public int LiczbaStudentow { get; set; }
    }
}
