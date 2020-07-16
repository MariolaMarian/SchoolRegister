using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(IMapper mapper, ApplicationDbContext dbContext, UserManager<User> userManager) : base(dbContext, mapper) { }
        public GroupVM GetGroup(Expression<Func<Group, bool>> filterPredicate)
        {
            if (filterPredicate == null)
            {
                throw new ArgumentNullException($"Predicate is null");
            }
            var groupEntity = _dbContext.Groups.FirstOrDefault(filterPredicate);
            var groupVM = _mapper.Map<GroupVM>(groupEntity);
            return groupVM;
        }

        public IEnumerable<GroupVM> GetGroups(Expression<Func<Group, bool>> filterPredicate = null)
        {
            var groupEntities = _dbContext.Groups.AsQueryable();
            if (filterPredicate != null)
            {
                groupEntities = groupEntities.Where(filterPredicate);
            }
            var groupsVM = _mapper.Map<IEnumerable<GroupVM>>(groupEntities.ToList());
            return groupsVM;
        }
        
        public GroupVM CreateOrUpdateGroup(GroupForCreateOrUpdateDTO groupForCreateOrUpdateDTO)
        {
            if(groupForCreateOrUpdateDTO == null)
            {
                throw new ArgumentNullException($"DTO is null");
            }
            var groupEntity = _mapper.Map<Group>(groupForCreateOrUpdateDTO);
            if(groupForCreateOrUpdateDTO.Id == null || groupForCreateOrUpdateDTO.Id == 0)
            {
                _dbContext.Groups.Add(groupEntity);
            }
            else
            {
                _dbContext.Groups.Update(groupEntity);
            }
            try
            {
                if (_dbContext.SaveChanges() < 1)
                    return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            var groupVM = _mapper.Map<GroupVM>(groupEntity);
            return groupVM;
        }

    }
}
