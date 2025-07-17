using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Services
{
    public interface IProjectService
    {
        ResultViewModel<List<ProjectViewModel>> GetAll(string search = "");
        ResultViewModel<List<ProjectViewModel>> GetAll(string search = "", int page = 0, int size = 3);
        ResultViewModel<ProjectViewModel> GetById(int id);
        ResultViewModel<int> Insert(CreateProjectInputModel projectInputModel);
        ResultViewModel<int> Update(int id, UpdateProjectInputModel projectInputModel);
        ResultViewModel Delete(int id);
        ResultViewModel Start(int id);
        ResultViewModel Finish(int id);
        ResultViewModel<int> InsertComment(int id, CreateCommentInputModel createCommentInputModel);
    }
}
