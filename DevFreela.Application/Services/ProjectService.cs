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
    internal class ProjectService : IProjectService
    {
        private readonly DevFreelaDbContext _context;

        public ProjectService(DevFreelaDbContext context)
        {
            _context = context;
        }

        // Implementation of the methods defined in the interface

        public ResultViewModel<List<ProjectViewModel>> GetAll(string search = "")
        {
            var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Where(p => !p.IsDeleted)
                .ToList();

            var model = projects
                .Select(ProjectViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<ProjectViewModel>>.Success(model);
        }
        public ResultViewModel<List<ProjectViewModel>> GetAll(string search = "", int page = 0, int size = 3)
        {
            var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search)))
                .Skip(page * size)
                .Take(size)
                .ToList();

            var model = projects
                .Select(ProjectViewModel.FromEntity)
                .ToList();

            return ResultViewModel<List<ProjectViewModel>>.Success(model);
        }
        public ResultViewModel<ProjectViewModel> GetById(int id)
        {
            var project = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.Id == id);

            if (project is null)
            {
                return ResultViewModel<ProjectViewModel>.Failure("Project not exist.");
            }
            if (project.IsDeleted)
            {
                return ResultViewModel<ProjectViewModel>.Failure("Project is deleted.");
            }

            var model = ProjectViewModel.FromEntity(project);

            return ResultViewModel<ProjectViewModel>.Success(model);
        }
        public ResultViewModel<int> Insert(CreateProjectInputModel model)
        {
            if (model.Title.Length > 50)
            {
                return ResultViewModel<int>.Failure("50 characters maximum for the title.");
            }

            var project = model.ToEntity();

            _context.Projects.Add(project);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(project.Id);
        }
        public ResultViewModel<int> Update(int id, UpdateProjectInputModel model)
        {
            model.IdProject = id;

            if (model.Description.Length > 200)
            {
                return ResultViewModel<int>.Failure("200 characters maximum for description.");
            }

            var project = _context.Projects.SingleOrDefault(p => p.Id == model.IdProject);

            if (project is null)
            {
                return ResultViewModel<int>.Failure("Project not exist.");
            }

            project.Update(model.Title, model.Description, model.TotalCost);

            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(project.Id);
        }
        public ResultViewModel Delete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
            {
                return ResultViewModel.Failure("Project not exist.");
            }

            project.SetAsDeleted();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }
        public ResultViewModel Start(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
            {
                return ResultViewModel.Failure("Project not exist.");
            }

            project.Start();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }
        public ResultViewModel Cancel(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
            {
                return ResultViewModel.Failure("Project not exist.");
            }

            project.Cancel();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }
        public ResultViewModel Complete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
            {
                return ResultViewModel.Failure("Project not exist.");
            }

            project.Complete();
            _context.Projects.Update(project);
            _context.SaveChanges();

            return ResultViewModel.Success();
        }
        public ResultViewModel<int> InsertComment(int id, CreateCommentInputModel createComment)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == createComment.IdProject);

            if (project is null)
            {
                return ResultViewModel<int>.Failure("Project not exist.");
            }

            var comment = new ProjectComment(createComment.Content, createComment.IdProject, createComment.IdUser);

            _context.ProjectComments.Add(comment);
            _context.SaveChanges();

            return ResultViewModel<int>.Success(comment.Id);
        }
    }
}
