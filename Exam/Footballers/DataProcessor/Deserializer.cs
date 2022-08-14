using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ImportDto;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCoachesWithFootballersDto[]), xmlRoot);

            using StringReader reader = new StringReader(xmlString);
            ImportCoachesWithFootballersDto[] coachesDto = (ImportCoachesWithFootballersDto[])xmlSerializer.Deserialize(reader);

            ICollection<Coach> validCoaches = new List<Coach>();

            foreach (var coachDto in coachesDto)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                Coach coachToAdd = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (var footballer in coachDto.Footballers)
                {
                    if (!IsValid(footballer))
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    bool bestSkillTypeEnumValid = Enum.TryParse(typeof(BestSkillType), footballer.BestSkillType, out object bestSkillTypeObj);
                    if (!bestSkillTypeEnumValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    bool positionTypeEnumValid = Enum.TryParse(typeof(PositionType), footballer.PositionType, out object positionTypeObj);
                    if (!positionTypeEnumValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    bool contractStartDateValid = DateTime.TryParseExact(
                        footballer.ContractStartDate, "dd/mm/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var contractStartDate);
                    if (!contractStartDateValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    bool contractEndDateValid = DateTime.TryParseExact(
                        footballer.ContractEndDate, "dd/mm/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var contractEndDate);
                    if (!contractEndDateValid)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    if (contractStartDate > contractEndDate)
                    {
                        sb.AppendLine("Invalid data!");
                        continue;
                    }

                    Footballer footballerToAdd = new Footballer()
                    {
                        Name = footballer.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = (BestSkillType)bestSkillTypeObj,
                        PositionType = (PositionType)positionTypeObj
                    };
                    coachToAdd.Footballers.Add(footballerToAdd);
                }

                validCoaches.Add(coachToAdd);
                sb.AppendLine($"Successfully imported coach - {coachToAdd.Name} with {coachToAdd.Footballers.Count} footballers.");
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
       
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTeamDto[] teamDtos =
                JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            ICollection<Team> validTeams = new List<Team>();

            foreach (var teamDto in teamDtos)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies
                };

                
                foreach (int footballerId in teamDto.Footballers.Distinct())
                {
                    Footballer footballer = context.Footballers.Find(footballerId);
                    if (footballer == null)
                    {
                        sb.AppendLine($"Invalid Data");
                        continue;
                    }

                    TeamFootballer tf = new TeamFootballer() {TeamId = team.Id, FootballerId = footballerId};
                    team.TeamsFootballers.Add(tf);
                }


                context.Teams.Add(team);
                context.SaveChanges();
                sb.AppendLine($"Successfully imported team - {team.Name} with {team.TeamsFootballers.Count} footballers.");
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
