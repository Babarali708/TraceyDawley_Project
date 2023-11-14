using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraceyDawley.Helping_Classes
{
    public class DtoCollection
    {
        public class UserDto
        {
            public string Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? MiddleName { get; set; }
            public string? Contact { get; set; }
            public string? Email { get; set; }
            public string? encEmail { get; set; }
            public int? Role { get; set; }

        }
        public class BrokerDto
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
        }        
        public class BusinessCenterDto
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
        public class DepositTypeDto
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
        public class GroupDto
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
        public class InvoiceTypeDto
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
        public class InterventionTypeDto
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
        }
        public class SurveyResponseDataDto
        {
            public int? Id { get; set; }

            public string? Question1 { get; set; }
            public string? Question2 { get; set; }
            public string? Question3 { get; set; }

            public string? Question4 { get; set; }

            public string? Question5 { get; set; }

            public string? Question6 { get; set; }

            public string? Question7 { get; set; }

            public string? Question8 { get; set; }
            public string? UserName { get; set; }
            public int? UserId { get; set; }
        }

        public class SurveyResponse
        {
            public string? UserName { get; set; }
            public string? UserEmail { get; set; }
            public string? Question1 { get; set; }
            public string? Question2 { get; set; }
            public string? Question3 { get; set; }

            public string? Question4 { get; set; }

            public string? Question5 { get; set; }

            public string? Question6 { get; set; }

            public string? Question7 { get; set; }

            public string? Question8 { get; set; }
            
        }

        public class UserData
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
        }
        public class Survey
        {
            public UserData? User { get; set; }
            public SurveyResponse? SurveyResponse { get; set; }
        }


    }
}
