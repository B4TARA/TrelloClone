﻿using EmailService;
using Microsoft.EntityFrameworkCore;
using TrelloClone.Data;
using TrelloClone.Data.Repositories;
using TrelloClone.Services;

var optionsBuilder = new DbContextOptionsBuilder<TrelloCloneDbContext>();
var options = optionsBuilder
        .UseNpgsql("Host=localhost;Port=5432;Database=TrelloClone;Username=postgres;Password=12345")
        .Options;

TrelloCloneDbContext _db = new TrelloCloneDbContext(options);
RepositoryManager _repository = new RepositoryManager(_db);

EmailConfiguration _configuration = new EmailConfiguration();
_configuration.Port = 25;
_configuration.SmtpServer = "LDGate.mtb.minsk.by";
_configuration.From = "KOPSender";

EmailSender _emailSender = new EmailSender(_configuration);
UserService userService = new UserService(_repository, _emailSender);

var path = "C:\\Users\\tomchikadm\\Documents\\GitHub\\TrelloClone\\TrelloClone-master\\files\\SMART-задачи_список_сотрудников.xlsx";
//var path = "C:\\Users\\evgen\\OneDrive\\Документы\\GitHub\\TrelloClone\\TrelloClone-master\\files\\SMART-задачи_список_сотрудников.xlsx";

Task.Run(async () =>
{
    try
    {
        var exportResponse = userService.ExportUsers(path);
        if (exportResponse.StatusCode != TrelloClone.Models.Enum.StatusCodes.OK)
        {
            Console.WriteLine("Unable to export data from " + path + " : " + exportResponse.Description);
            return;
        }

        var importResponse = await userService.ImportUsers(exportResponse.Data);
        if (importResponse.StatusCode != TrelloClone.Models.Enum.StatusCodes.OK)
        {
            Console.WriteLine("Unable to import data : " + importResponse.Description);
            return;
        }
    }

    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return;
    }
}).GetAwaiter().GetResult();

Console.WriteLine("End of Import");